from __future__ import print_function
import os
import sys
import urllib
import urllib2
import httplib
import arcpy
import json
import re
from arcpy import env


class ServiceException(Exception):
    pass


class AgsAdmin(object):
    def __init__(self, server, port, token=None):
        self.server = server
        self.port = port
        self.token = token
        
    @classmethod
    def connectWithToken(self, server, port, token):
        return AgsAdmin(server, port, token)

    @classmethod
    def connectWithoutToken(self, server, port, adminUser, adminPass, expiration=60):
        token = self.getToken(server, port, adminUser, adminPass, expiration=expiration)
        return AgsAdmin(server, port, token)

    @staticmethod
    def getToken(server, port, adminUser, adminPass, expiration=60):
        """Get a token required for Admin changes"""
        
        query_dict = {'username':   adminUser,
                      'password':   adminPass,
                      'expiration': str(expiration),
                      'client':     'requestip'}
 
        query_string = urllib.urlencode(query_dict)
        url = "http://{}:{}/arcgis/admin/generateToken".format(server, port)
    
        token = json.loads(urllib.urlopen(url + "?f=json", query_string).read())
        
        try:
            return token["token"]
        except KeyError:
            raise ServiceException("No token returned. Check credientials.")

    def stopStartDeleteService(self, command, servicename, folder=None):  
        """
        Function to stop, start or delete a service.
        Requires token, server, and port.
        command = Stop|Start|Delete
        serviceList = List of services. A service must be in the <name>.<type> notation
        """

        if folder:
            if folder.endswith("/"):
                pass
            else:
                folder = folder + "/"
        else:
            folder = ""
        
        service = urllib.quote(servicename.encode('utf8'))
        op_service_url = "http://{0}:{1}/arcgis/admin/services/{2}{3}/{4}?token={5}&f=json".format(self.server,
                                                                                                   self.port,
                                                                                                   folder,
                                                                                                   service,
                                                                                                   command,
                                                                                                   self.token)        
        status = urllib2.urlopen(op_service_url, ' ').read()
            
        if not 'success' in status:
            raise ServiceException("Could not {0} service {1} successfully.".format(command, servicename))
        else:
            return 0

    def stopService(self, servicename):
        return self.stopStartDeleteService("Stop", servicename, folder=None)

    def startService(self, servicename):
        return self.stopStartDeleteService("Start", servicename, folder=None)

    def deleteService(self, servicename):
        return self.stopStartDeleteService("Delete", servicename, folder=None)

    def servicesInFolder(self, foldername, namefilter=None):
        """
        """

        # test if name filter is valid regex
        if namefilter:
            try:
                re.compile(namefilter)
            except re.error:
                raise re.error("Specified namefilter argument must be a vaild regex. Aborting.")

        listofservices = []
        folderURL = "/arcgis/admin/services/" + foldername

        # This request only needs the token and the response formatting parameter 
        params = urllib.urlencode({'token': self.token, 'f': 'json'})
        
        headers = {"Content-type": "application/x-www-form-urlencoded", "Accept": "text/plain"}
        
        # Connect to URL and post parameters    
        httpConn = httplib.HTTPConnection(self.server, self.port)
        httpConn.request("POST", folderURL, params, headers)

        # Read response
        response = httpConn.getresponse()
        if (response.status != 200):
            httpConn.close()
            raise ServiceException("Could not read folder information.")
        else:
            data = response.read()
            
            # Check that data returned is not an error object
            if not assertJsonSuccess(data):          
                raise ServiceException("Error when reading folder information. " + str(data))

            # Deserialize response into Python object
            dataObj = json.loads(data)
            httpConn.close()
            
            for item in dataObj['services']:
                # if namefilter, check to see if name matches; if not, skip to next item
                if namefilter:
                    if not re.search(namefilter, item['serviceName']):
                        continue
                listofservices.append(item['serviceName'] + "." + item['type'])

        return listofservices

    def stopStartDeleteAllServicesInFolder(self, command, foldername, namefilter=None):
        """
        """

        errorcount = 0
        listofservices = self.servicesInFolder(foldername, namefilter=namefilter)

        if not listofservices:
            raise ServiceException("No services were found in the folder {0}.".format(foldername))

        for service in listofservices:
            try:
                self.stopStartDeleteService(command, service, foldername)
            except ServiceException as e:
                print(e)
                print("Failed to {0} service {1}.".format(command.lower(), service))
                errorcount += 1

        return errorcount

    def stopAllServicesInFolder(self, foldername, namefilter=None):
        return self.stopStartDeleteAllServicesInFolder("Stop", foldername, namefilter=namefilter)

    def startAllServicesInFolder(self, foldername, namefilter=None):
        return self.stopStartDeleteAllServicesInFolder("Start", foldername, namefilter=namefilter)

    def deleteAllServicesInFolder(self, foldername, namefilter=None):
        return self.stopStartDeleteAllServicesInFolder("Delete", foldername, namefilter=namefilter)


# A function that checks that the input JSON object 
#  is not an error object.
def assertJsonSuccess(data):
    obj = json.loads(data)
    if 'status' in obj and obj['status'] == "error":
        print("Error: JSON object returns an error. " + str(obj))
        return False
    else:
        return True
        

if __name__ == "__main__":
    sys.exit(main())

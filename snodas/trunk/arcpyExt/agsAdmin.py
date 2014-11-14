from __future__ import print_function
import os
import sys
import urllib
import urllib2
import arcpy
import json
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

    def stopStartDeleteService(self, command, servicename):  
        """
        Function to stop, start or delete a service.
        Requires token, server, and port.
        command = Stop|Start|Delete
        serviceList = List of services. A service must be in the <name>.<type> notation
        """
        
        service = urllib.quote(servicename.encode('utf8'))
        op_service_url = "http://{0}:{1}/arcgis/admin/services/{2}/{3}?token={4}&f=json".format(self.server,
                                                                                                self.port,
                                                                                                service,
                                                                                                command,
                                                                                                self.token)        
        status = urllib2.urlopen(op_service_url, ' ').read()
            
        if not 'success' in status:
            raise ServiceException("Could not {0} service {1} successfully.".format(command, servicename))
        else:
            return 0

    def stopService(self, servicename):
        return self.stopStartDeleteService("Stop", servicename)

    def startService(self, servicename):
        return self.stopStartDeleteService("Start", servicename)

    def deleteService(self, servicename):
        return self.stopStartDeleteService("Delete", servicename)

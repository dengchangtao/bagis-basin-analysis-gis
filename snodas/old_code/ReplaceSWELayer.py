import os, sys, urllib, urllib2, arcpy, json
#from arcpy import env

imgWorkspace = "C:/GIS/GIS_Data/SNODAS/SWE/" #needs a backslash at the end
SNODAS_SourceDataFile = "C:/inetpub/wwwroot/SNODAS/SNODASWebService_SourceData.txt"
SNODAS_ServiceName = "SNODAS/SWE_SNODAS.MapServer"
GIS_Server = "atlas.geog.pdx.edu"
Server_Port = "6080"

#env.workspace = imgWorkspace
sourceFilename = arcpy.GetParameterAsText(0)
sourceFilepathname = imgWorkspace + sourceFilename
#sourceFilename = imgWorkspace + str(sys.argv[1])


def gentoken(server, port, adminUser, adminPass, expiration=60):
    #Re-usable function to get a token required for Admin changes
    
    query_dict = {'username':   adminUser,
                  'password':   adminPass,
                  'expiration': str(expiration),
                  'client':     'requestip'}
    
    query_string = urllib.urlencode(query_dict)
    url = "http://{}:{}/arcgis/admin/generateToken".format(server, port)
    
    token = json.loads(urllib.urlopen(url + "?f=json", query_string).read())
        
    if "token" not in token:
        arcpy.AddError(token['messages'])
        quit()
    else:
        return token['token']


def stopStartServices(server, port, adminUser, adminPass, stopStart, serviceList, token=None):  
    ''' Function to stop, start or delete a service.
    Requires Admin user/password, as well as server and port (necessary to construct token if one does not exist).
    stopStart = Stop|Start|Delete
    serviceList = List of services. A service must be in the <name>.<type> notation
    If a token exists, you can pass one in for use.  
    '''    
    
    # Get and set the token
    if token is None:       
        token = gentoken(server, port, adminUser, adminPass)
    
    # Getting services from tool validation creates a semicolon delimited list that needs to be broken up
    services = serviceList.split(';')
    
    #modify the services(s)
    returnValue = True
    for service in services:        
        service = urllib.quote(service.encode('utf8'))
        op_service_url = "http://{}:{}/arcgis/admin/services/{}/{}?token={}&f=json".format(server, port, service, stopStart, token)        
        status = urllib2.urlopen(op_service_url, ' ').read()
        
        if 'success' in status:
            arcpy.AddMessage(str(service) + " === " + str(stopStart))
        else:
            arcpy.AddWarning(status)
            returnValue = False
    
    return returnValue


def get_targetname():
    layerName = ""
    page = urllib2.urlopen("http://atlas.geog.pdx.edu/arcgis/rest/services/SNODAS/SWE_SNODAS/MapServer")
    text = page.read().decode("utf8")
    position_begin = text.find("<b>Layers: </b>")
    position_end = text.find("<b>Description: </b>")
    layerString = text[position_begin:position_end]
    position_begin = layerString.find("/MapServer/0")
    position_end = layerString.find(".img</a>")
    layerName = layerString[position_begin + 14:position_end + 4] #get the layer name without extension name
    return layerName

    
def remove_file(filename):
    try:
        arcpy.Delete_management(filename)
        #os.remove(filename)
        return 1
    except OSError, e:
        arcpy.AddMessage("Failed to delete the target file: " + filename)
        return 0


def copy_file(fromname, toname):
    try:
        arcpy.Copy_management(fromname, toname)
        #shutil.copyfile(fromname, toname)
        return 1
    except OSError, e:
        arcpy.AddMessage("Failed to copy the source file to the target file.")
        return 0


def update_sourcedatadefinition(filepath, message):
    try:
        sdfile = open(filepath,'w')
        sdfile.write(str(message))
        sdfile.close()
        return 1
    except OSError, e:
        arcpy.AddMessage("Failed to update the definition log.")
        return 0


if os.path.isfile(sourceFilepathname):
    #arcpy.AddMessage("ImageWorkspace: " + imgWorkspace)
    targetFilename = get_targetname()
    targetFilepathname = imgWorkspace + targetFilename

    #Stop the web service on the server
    #stopStartServices(server, port, adminUser, adminPass, stopStart, serviceList)
    success = stopStartServices(GIS_Server, Server_Port, "AtlasAdmin", "CSARAtlas459", "Stop", SNODAS_ServiceName)

    if success:
        if remove_file(targetFilepathname) == 1:
            if copy_file(sourceFilepathname, targetFilepathname) == 1:
                if update_sourcedatadefinition(SNODAS_SourceDataFile, sourceFilename) == 1:
                    arcpy.AddMessage(targetFilename + " is replaced by " + sourceFilename + ".")
                    stopStartServices(GIS_Server, Server_Port, "AtlasAdmin", "CSARAtlas459", "Start", SNODAS_ServiceName)
    else:
        arcpy.AddMessage("Unable to stop the web service to update the source data!")
else:
    arcpy.AddMessage("Specified file doesn't exist!")
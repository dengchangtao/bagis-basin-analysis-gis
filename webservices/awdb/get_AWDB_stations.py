# --------------------------------------------------------------------
# NAME:     get_AWDB_stations.py
# AUTHOR:   Jarrett Keifer
# DATE:     11/13/2014
#
# DESC:     Downloads SNOTEL, Snow Course, and USGS stations from the
#           NRCS Air-Water Database, reprojects them, writes them to
#           an SDE database, then publishes them to a web service.
#
# USAGE:    The script requires no arguments, but some key constants,
#           such as output locations and network codes to download,
#           are grouped at the beginning of this file.
#
#           Run with 64-bit python install due to problem reading SDE.
# --------------------------------------------------------------------

# some import statements in functions to speed process generation
from __future__ import print_function
from suds.client import Client
from urllib2 import URLError
from multiprocessing import Process, Lock, Queue
import traceback
import os
import logging
from login_settings import ADMIN_USER, ADMIN_PSWD


# --------------------------------------
#            GLOBAL CONSTANTS
# --------------------------------------

# output locations
#TEMP_WORKSPACE = r"C:\Users\phoetrymaster\Documents\Testing\AWDB"  #?
ARCHIVE_WORKSPACE = r"C:\inetpub\ftproot\AWDB\Stations"  # zip each shapefile here for FTP access
TEMP_WORKSPACE = r"C:\GIS\GIS_Data\NRCS\AWDB\Stations"  # location for intermediate files
TEMP_GDB_NAME = "awdb_temp"  # Name of temp GDB

# sde target database -- for WFSs
SDE_WORKSPACE = r"C:\GIS\_SDEConnections\atlasprod_awdb.sde"  # SDE database connection
SDE_DATABASE = "atlasprod"
SDE_USERNAME = "sde_awdb"
FDS_SDE_PREFIX = SDE_DATABASE + "." + SDE_USERNAME + "."

# name of output feature dataset
FDS_NAME = "AWDB"  # set to None to write to DB root, not dataset

# url of the NRCS AWDB SOAP WDSL (defines the web API connection)
WDSL = "http://www.wcc.nrcs.usda.gov/awdbWebService/services?WSDL"

# NRCS AWDB network codes to download
NETWORKS = ["SNTL", "SNOW", "USGS", "COOP", "SCAN", "SNTLT", "OTHER", "BOR",
            "MPRC", "MSNT"]
#NETWORKS = ["SNOW"]


# dictionaries of the station fields
# these fields are required, and are geometry, so do not need to be aded to the output FC
REQUIRED_FIELDS = [{"field_name": "elevation"},
                  {"field_name": "latitude"},
                  {"field_name": "longitude"}
                  ]

# these fields will be added to the output FC, but can be null
FIELDS = [{"field_name": "actonId",             "field_type": "TEXT", "field_length": 20},  # 0
          {"field_name": "beginDate",           "field_type": "DATE"},                      # 1
          {"field_name": "countyName",          "field_type": "TEXT", "field_length": 25},  # 2
          {"field_name": "endDate",             "field_type": "DATE"},                      # 3
          {"field_name": "fipsCountryCd",       "field_type": "TEXT", "field_length": 5},   # 4
          {"field_name": "fipsCountyCd",        "field_type": "SHORT"},                     # 5
          {"field_name": "fipsStateNumber",     "field_type": "SHORT"},                     # 6
          {"field_name": "huc",                 "field_type": "TEXT", "field_length": 20},  # 7
          {"field_name": "hud",                 "field_type": "TEXT", "field_length": 20},  # 8
          {"field_name": "name",                "field_type": "TEXT", "field_length": 100}, # 9
          {"field_name": "shefId",              "field_type": "TEXT", "field_length": 20},  # 10
          {"field_name": "stationDataTimeZone", "field_type": "DOUBLE"},                    # 11
          {"field_name": "stationTimeZone",     "field_type": "DOUBLE"},                    # 12
          {"field_name": "stationTriplet",      "field_type": "TEXT", "field_length": 50},  # 13
          {"field_name": "elevation",           "field_type": "DOUBLE"},                    # 14
          ]


# number of attempts to retry getting station
RETRY_COUNT = 2

# message inserted into queue to signal end or logging message
QUEUE_DONE = "DONE"
MESSAGE_CODE = "MSG"

# number of worker processes to get station records (max number of processes)
WORKER_PROCESSES = 10

# maximum number of records per metadata request (larger is faster but more likely to timeout)
MAX_REQUEST = 250


# ArcGIS Server settings
SERVER_ADDRESS = "atlas.geog.pdx.edu"  # PSU server hosting AWDB services
SERVER_PORT = "6080"

# WFS service settings
# name of the folders containing all the AWDB web services
WFS_SERVICE_FOLDERS = ["AWDB", "AWDB_ALL", "AWDB_ACTIVE", "AWDB_INACTIVE"]

# USGS metadata retrival constants
NEW_FIELDS = [("basinarea", "DOUBLE"), ("USGS_ID", "TEXT"), ("USGSname", "TEXT")]  # fields to add: tuple of name and data type
STATION_ID_FIELD = FIELDS[13]["field_name"]  # the stationTriplet field name is defined in FIELDS above
USGS_URL = "http://waterservices.usgs.gov/nwis/site/"  # URL of the USGS site information REST service

# logging constants
FULL_LOGFILE = os.path.join(TEMP_WORKSPACE, "AWDB_LOG.txt")  # contains debug info
SUMMARY_LOGFILE = os.path.join(TEMP_WORKSPACE, "AWDB_SUMMARY.txt")  # records processed OK/Failed only
__DEBUG__ = False  # true outputs debug-level messages to stderr


# -------------------------------
#             LOGGING
# -------------------------------

# LOGGING IS SETUP AFTER THE MAIN CHECK BECAUSE OF MULTIPROCESSING


# -------------------------------
#          ERROR CLASSES
# -------------------------------

class SDEError(Exception):
    pass


# -------------------------------
#            FUNCTIONS
# -------------------------------

def recursive_asdict(d):
    """
    Convert Suds object into serializable format (dictonary).

    Requires: d -- the input suds object

    Returns:  out -- a dictionary representation od d
    """

    from suds.sudsobject import asdict

    out = {}
    for key, value in asdict(d).iteritems():
        if hasattr(value, '__keylist__'):
            out[key] = recursive_asdict(value)
        elif isinstance(value, list):
            out[key] = []
            for item in value:
                if hasattr(item, '__keylist__'):
                    out[key].append(recursive_asdict(item))
                else:
                    out[key].append(item)
        else:
            out[key] = value

    return out


def grouper(iterable, n, fillvalue=None):
    """
    Takes an iterable and splits it into pieces of n length.
    The last piece will be filled to fit the desired length with
    whatever is specified by the fillvalue argument.

    Requires: iterable -- the iterable to be split
              n -- length of each piece made from iterable

    Optional: fillvalue -- value to use to make last piece length = n
                           If fillvalue is None, last piece will not be filled
                           DEFAULT: None

    Returns:  pieces -- a list of the pieces made from iterable
                        each piece is a list

    Example:  grouper([0, 1, 2, 3, 4, 5, 6, 7], 5) returns [[0, 1, 2, 3, 4], [5, 6, 7]]
              grouper([0, 1, 2, 3, 4, 5, 6, 7], 6, 'f') returns [[0, 1, 2, 3, 4, 5], [6, 7, 'f', 'f', 'f', 'f']]
    """

    from itertools import izip_longest

    args = [iter(iterable)] * n
    pieces = list(izip_longest(*args, fillvalue=fillvalue))

    for i, piece in enumerate(pieces):
        pieces[i] = list(piece)

    # remove any null values from station group
    if fillvalue is None:
        pieces[len(pieces)-1] = [x for x in pieces[len(pieces)-1] if x is not None]

    return pieces


def get_multiple_stations_thread(stations, outQueue, queueLock, recursiveCall=0):
    """
    Gets the metadata for a list of stations from the AWDB web service. Designed to be
    run as a worker process as part of a multiprocessed solution to get stations.
    Called from get_stations().

    Required: stations -- a list of stations to retrieve by station triplet
              outQueue -- the queue object into which retrieved stations should be placed
                          the queue is read by the main process and records are inserted
                          into an FC as they are received
              queueLock -- the lock object used to prevent collisions when writing to the queue

    Optional: recursiveCall -- the number of times the process should recursively call
                               itself to retry any stations that were not retrieved
                               successfully.
                               DEFAULT is 0, so stations are not retried by default.

    Returns:  Error code
    """

    data = None

    try:
        client = Client(WDSL)  # connect to the service definition
        data = client.service.getStationMetadataMultiple(stationTriplets=stations)
    except Exception as e:
        with queueLock:
            outQueue.put((MESSAGE_CODE, 15, e))
            outQueue.put((MESSAGE_CODE, 15, traceback.format_exc()))
    except URLError as e:
        if "Errno 10060" in e:  # this is a connection error -- time out or no response
            with queueLock:
                outQueue.put((MESSAGE_CODE, 15, e))
                outQueue.put((MESSAGE_CODE, 15, "Error connecting to server. Retrying..."))
        else:
            with queueLock:
                outQueue.put((MESSAGE_CODE, 15, e))
                outQueue.put((MESSAGE_CODE, 15, traceback.format_exc()))

    if data:
        for station in data:
            station = validate_station_data(recursive_asdict(station))

            if station:
                with queueLock:
                    outQueue.put(station)
                stations.remove(station["stationTriplet"])
            else:
                pass

    if len(stations) and recursiveCall:
        with queueLock:
            outQueue.put((MESSAGE_CODE, 15, "Some stations were not successfully retrieved; retrying..."))
        recursiveCall -= 1
        get_multiple_stations_thread(stations, outQueue, queueLock, recursiveCall=recursiveCall)
    elif len(stations):
        with queueLock:
            outQueue.put((MESSAGE_CODE, 15, "Stations could not be successfully retrieved:\n{0}".format(stations)))

    return 0


def get_stations(stationIDs, stationQueue):
    """
    This function calls get_multiple_stations_thread with groups of stations to be
    retrieved as multiple worker processes. It controls how many worker processes
    are running at any given time, keeping that number at or below the WORKER_PROCESSES
    constant.

    Requires: stationIDs -- the complete list of stations to retrieve, by station triplet
              stationQueue -- the queue object that the worker process will write station
                              records to

    Returns:  None
    """

    import time

    queueLock = Lock()
    processes = []  # to track running processes
    for stationgroup in grouper(stationIDs, MAX_REQUEST):  #split list into 1000-station chunks to avoid timeouts
        getProcess = Process(target=get_multiple_stations_thread, args=(stationgroup, stationQueue, queueLock), kwargs={"recursiveCall": RETRY_COUNT})  # get metadata for group
        getProcess.daemon = True
        getProcess.start()
        processes.append(getProcess)

        # if max number of processes, wait until one closes before starting another
        while len(processes) == WORKER_PROCESSES:
            for p in processes:
                if not p.is_alive():
                    # p.is_alive() will be false if process closes
                    processes.remove(p)
            time.sleep(0.5)

    # join remaining child processes to prevent done message until all records are returned
    for p in processes:
        p.join()

    stationQueue.put(QUEUE_DONE)


def validate_station_data(station):
    """
    Checks a station's data to ensure all required fields are present,
    and inserts a null value into any non-required fields.

    Requires: station -- the station data returned from the server as a dict

    Returns:  station -- the validated station object

    Error condition: if a required field is missing, will return False
    """

    # test to ensure all required fields have a value
    for field in REQUIRED_FIELDS:
        try:
            station[field["field_name"]]
        except:
            return False

    # test all non-required fields for a value; insert null if missing
    for field in FIELDS:
        try:
            station[field["field_name"]]
        except KeyError:
            station[field["field_name"]] = None

    return station


def get_network_stations(networkCode, fc_name, spatialref, workspace="in_memory"):
    """
    Queries the AWDB to get a list of all stations by station triplet in the network
    specified. Then spawns get_stations() as a child process. get_stations retrieves
    the metadata of the stations, the records of which are placed into the stationQueue.
    As station records are returned, this function reads the records from the queue
    and writes them as features in a feature class created in the specified workspace.

    Requires: networkCode -- the code of the network to retrieve, i.e. SNTL
              fc_name -- the name of the fc to create in the specified workspace
              spatialref -- the spatial reference object to use for the fc

    Optional: workspace -- the workspace in which to create the fc
                           DEFAULT: "in_memory", the ArcGIS RAM workspace

    Returns: fc -- the result object from the CreateFeatureClass function
    """

    from arcpy import AddField_management, CreateFeatureclass_management
    from arcpy.da import InsertCursor

    LOGGER.info("\nGetting stations in the {0} network...".format(networkCode))
    client = Client(WDSL)  # connect to the service definition
    stationIDs = client.service.getStations(networkCds=networkCode)  # get list of station IDs in network
    numberofstations = len(stationIDs)
    LOGGER.log(15, "Found {0} stations in {1} network.".format(numberofstations, networkCode))

    stationQueue = Queue()  # to pass back results from thread

    # create process to get station data
    getStationProcess = Process(target=get_stations, args=(stationIDs, stationQueue))
    getStationProcess.start()  # start thread execution

    LOGGER.info("Creating feature class in memory...")
    fc = CreateFeatureclass_management(workspace, fc_name, "POINT", has_z="ENABLED", spatial_reference=spatialref)

    LOGGER.info("Adding attribute fields to feature class...")
    for field in FIELDS:
        AddField_management(fc, **field)

    # tuple of fields to access with the insert cursor
    fieldsToAccess = (FIELDS[0]["field_name"],
                      FIELDS[1]["field_name"],
                      FIELDS[2]["field_name"],
                      "SHAPE@Z",
                      FIELDS[3]["field_name"],
                      FIELDS[4]["field_name"],
                      FIELDS[5]["field_name"],
                      FIELDS[6]["field_name"],
                      FIELDS[8]["field_name"],
                      "SHAPE@Y",
                      "SHAPE@X",
                      FIELDS[9]["field_name"],
                      FIELDS[10]["field_name"],
                      FIELDS[11]["field_name"],
                      FIELDS[12]["field_name"],
                      FIELDS[13]["field_name"],
                      FIELDS[14]["field_name"],
                      )

    countInserted = 0

    LOGGER.info("Writing stations to FC as data are returned from server...")

    # insert cursor to add records to fc
    with InsertCursor(fc, fieldsToAccess) as cursor:
        while True:
            station = stationQueue.get()  # get station data from queue

            if station == QUEUE_DONE:
                break

            try:
                if station[0] == MESSAGE_CODE:
                    LOGGER.log(station[1], station[2])
                    continue
            except KeyError:
                pass

            stationIDs.remove(station["stationTriplet"])

            cursor.insertRow((station[FIELDS[0]["field_name"]],
                              station[FIELDS[1]["field_name"]],
                              station[FIELDS[2]["field_name"]],
                              station["elevation"],
                              station[FIELDS[3]["field_name"]],
                              station[FIELDS[4]["field_name"]],
                              station[FIELDS[5]["field_name"]],
                              station[FIELDS[6]["field_name"]],
                              station[FIELDS[8]["field_name"]],
                              station["latitude"],
                              station["longitude"],
                              station[FIELDS[9]["field_name"]],
                              station[FIELDS[10]["field_name"]],
                              station[FIELDS[11]["field_name"]],
                              station[FIELDS[12]["field_name"]],
                              station[FIELDS[13]["field_name"]],
                              station[FIELDS[14]["field_name"]],
                              ))
            countInserted += 1

    LOGGER.info("Successfully inserted {0} of {1} records into {2}.".format(countInserted, numberofstations, fc_name))

    if countInserted != numberofstations:
        raise Exception("ERROR: Failed to get all stations for unknown reason.")

    return fc


def archive_GDB_FC(fc, outdir):
    """
    Copies an input FC from a geodatabase into a temp folder in shapefile format.
    Creates a zip file in the outdir, and copies the shapefile files into that
    zip archive. Deletes the temp folder after archiving.

    Requires: fc -- the feature class in a GDB to archive as a zip
              outdir -- the output location of the zip file

    Returns:  zippath -- the path to the created zip archive
    """

    import zipfile
    import glob
    from datetime import datetime
    from arcpy import CopyFeatures_management
    from tempfile import mkdtemp
    from shutil import rmtree

    fc_name = os.path.basename(fc)
    today = "_" + datetime.today().strftime("%m-%d-%Y")

    tempfolder = mkdtemp()

    CopyFeatures_management(fc, tempfolder)

    filelist = glob.glob(os.path.join(tempfolder, fc_name + ".*"))

    zippath = os.path.join(ARCHIVE_WORKSPACE, fc_name + today + ".zip")

    with zipfile.ZipFile(zippath, mode='w', compression=zipfile.ZIP_DEFLATED) as zfile:
        for f in filelist:
            if not f.upper().endswith('.LOCK'):
                newfile = os.path.splitext(os.path.basename(f))[0] + today + os.path.splitext(f)[1]
                zfile.write(f, newfile)

    rmtree(tempfolder)

    return zippath


def replace_wfs_data(newdata, target_workspace):
    """
    Copy the new FC into the target workspace.

    Requires: newdata -- the FC to be copied
              target_workspace -- the location into which the data will be copied

    Returns:  None
    """
    from arcpy import CopyFeatures_management
    from arcpy import env
    env.overwriteOutput = True
    CopyFeatures_management(newdata, os.path.join(target_workspace, os.path.splitext(os.path.basename(newdata))[0]))


def update_all_wfs(fcstoupdate, target_workspace, servicefoldernames):
    """
    Stops all web services in a folder on the server to remove all data locks.
    For all new data, write that data to SDE, replacing the old web service data.
    On any exception or if successful, re-start all services in the folder.

    Requires: fcstoupdate -- a list of the new FCs to overwrite the data in SDE
              target_worksapce -- the SDE FDS where the web service data lives
              servicefoldername -- the name of the folder of AWDB web services
                                   e.g. "AWDB"

    Returns:  errorcount -- the number of FCs that were not copied successfully
    """

    from arcpyExt import agsAdmin

    if not fcstoupdate:
        raise Exception("No FCs to update.")

    errorcount = 0
    errors = 0

    try:
        print("Connecting to server admin and stopping all services in {0} folder(s)...".format(servicefoldernames))
        agsconnection = agsAdmin.AgsAdmin.connectWithoutToken(SERVER_ADDRESS, SERVER_PORT, ADMIN_USER, ADMIN_PSWD)
        for servicefoldername in servicefoldernames:
            errors += agsconnection.stopAllServicesInFolder(servicefoldername)
    except Exception as e:
        LOGGER.log(15, e)
        LOGGER.log(15, traceback.format_exc())
        raise Exception("Could not stop the WFS services in {0}.".format(servicefoldername))
        return
    else:
        if errors:
            for servicefoldername in servicefoldernames:
                agsconnection.startAllServicesInFolder(servicefoldername)
            raise Exception("Failed to stop all services in {0}. Tried to restart then aborted.".format(servicefoldername))

    for newdata in fcstoupdate:
        try:
            replace_wfs_data(newdata, target_workspace)
        except Exception as e:
            LOGGER.log(15, e)
            LOGGER.log(15, traceback.format_exc())
            errorcount += 1

    if errorcount:
        LOGGER.error("Failed to update one or more wfs data sources.")

    LOGGER.info("Restarting stoppped services...")
    try:
        for servicefoldername in servicefoldernames:
            agsconnection.startAllServicesInFolder(servicefoldername)
    except Exception as e:
        LOGGER.log(15, e)
        LOGGER.log(15, traceback.format_exc())
        errorcount += 1

    return errorcount


def validateSDE(sdeconnection):
    """
    Checks to see if the SDE connection file exists.

    Requires: sdeconnection -- the path to a SDE connection file

    Returns: True if the connection exists
    """
    from arcpy import Exists

    if not Exists(sdeconnection):
        raise SDEError("SDE connection invalid or inaccessible.")

    return True


def validateFDS(sdeconnection, fds, spatialref="#"):
    """
    Checks to see if an FDS exists, and creates it if not.

    Requires: sdeconnection -- the path to a SDE connection file
              fds -- the name of the FDS

    Optional: spatialref -- the spatialref to use for the FDS
                            DEFAULT: the ArcGIS default

    Returns:  True if successful
    """
    from arcpy import Exists, CreateFeatureDataset_management

    if not Exists(os.path.join(sdeconnection, fds)):
        CreateFeatureDataset_management(sdeconnection, fds, spatialref)

    return True


def create_temp_workspace(directory, name, is_gdb=True):
    """
    Creates a temp workspace for processing. If is_gdb, will create a GDB.
    Else a folder will be created.

    Required: directory -- the directory in which to create the temp GDB
              name -- the name of the temp GDB

    Optional: is_gdb -- whether or not to create a GDB. Default is True.

    Returns:  path -- the full path to the temp workspace
    """
    import shutil
    from arcpy import CreateFileGDB_management

    LOGGER.info("Creating temp workspace {0} in {1}...".format(name, directory))

    path = os.path.join(directory, name)

    if is_gdb:
        LOGGER.log(15, "Workspace will be format GDB.")
        path = path + ".gdb"

    if os.path.isdir(path):
        LOGGER.log(15, "Temp workspace already exists; removing...")
        shutil.rmtree(path)

    if is_gdb:
        CreateFileGDB_management(directory, name)
    else:
        os.mkdir(path)

    return path


def get_USGS_metadata(usgs_fc):
    """
    Access the USGS site information REST API to get the basin area
    for all applicable sites. Adds the basinarea field to the FC and
    writes the data returned from the REST serivce.

    Required: usgs_fc -- the feature class of records from the AWDB

    Returns:  None
    """

    import urllib
    import urllib2
    import gzip
    from re import search
    from arcpy import ListFields, AddField_management
    from arcpy.da import SearchCursor, UpdateCursor
    from StringIO import StringIO

    # check for area field and add if missing
    fields = ListFields(usgs_fc)

    for fieldname, datatype in NEW_FIELDS:
        for field in fields:
            #print(field.name)
            if field.name == fieldname:
                break
        else:
            AddField_management(usgs_fc, fieldname, datatype)

    # get a list of station IDs in the FC
    stationIDs = []
    with SearchCursor(usgs_fc, STATION_ID_FIELD) as cursor:
        for row in cursor:
            sid = row[0].split(":")[0]
            if len(sid) >= 8 and not search('[a-zA-Z]', sid):  # valid USGS station IDs are between 8 and 15 char and are numerical
                stationIDs.append(sid)

    #print(len(stationIDs))

    # setup and get the HTTP request
    request = urllib2.Request(USGS_URL, urllib.urlencode({"format": "rdb",  # get the data in USGS rdb format
                                                          "sites": ",".join(stationIDs),  # the site IDs to get, separated by commas
                                                          "siteOutput": "expanded",  # expanded output includes basin area
                                                          #"modifiedSince": "P" + str(SCRIPT_FREQUENCY) + "D"  # only get records modified since last run
                                                          }))
    request.add_header('Accept-encoding', 'gzip')  # allow gzipped response
    response = urllib2.urlopen(request)

    # check to see if response is gzipped and decompress if yes
    if response.info().get('Content-Encoding') == 'gzip':
        buf = StringIO(response.read())
        data = gzip.GzipFile(fileobj=buf)
    else:
        data = response

    # parse the response and create a dictionary keyed on the station ID
    stationAreas = {}
    for line in data.readlines():
        if line.startswith("USGS"):
            line = line.split("\t")  # data elements in line (station record) are separated by tabs
            stationAreas[line[1]] = (line[29], line[1], line[2])  # the 2nd element is the station ID, 3rd is the name, and the 30th is the area
                                                                  # order in the tuple is important, so data is entered into the correct fields in the table

    #print(len(stationAreas))

    # write the response data to the FC
    fieldsToAccess = [STATION_ID_FIELD] + [name for name, datatype in NEW_FIELDS]
    with UpdateCursor(usgs_fc, fieldsToAccess) as cursor:
        for row in cursor:
            stationid = row[0].split(":")[0]

            try:
                row[1] = float(stationAreas[stationid][0])  # row[1] is area
            except KeyError:
                # in case no record was returned for ID
                continue  # skip to next record
            except ValueError:
                # in case area returned is ""
                pass

            try:
                row[2] = stationAreas[stationid][1]  # row[2] is the USGS station ID
            except ValueError:
                # in case ID returned is ""
                pass

            try:
                row[3] = stationAreas[stationid][2]  # row[3] is the USGS station name
            except ValueError:
                # in case name returned is ""
                pass

            # no exception so data valid, update row
            cursor.updateRow(row)


def write_to_summary_log(message):
    """
    Write a message string to the summary logfile.

    Requires: message -- the message string to write to the log

    Returns:  None
    """
    with open(SUMMARY_LOGFILE, 'a') as summary:
        summary.write(message + "\n")


# ----------------------------
#             MAIN
# ----------------------------

def main():
    from datetime import datetime
    import shutil
    import arcpy
    from arcpy import env

    start = datetime.now()
    LOGGER.log(15, "\n\n--------------------------------------------------------------\n")
    LOGGER.log(15, "Started at {0}.".format(start))

    # create spatial ref objects
    unprjSR = arcpy.SpatialReference(4326)  # spatial ref WKID 4326: WGS 1984 GCS
    prjSR   = arcpy.SpatialReference(102039)  # spatial ref WKID 102039: USA_Contiguous_Albers_Equal_Area_Conic_USGS_version

    # ensure SDE connection is valid
    try:
        LOGGER.log(15, "Validating DB connection...")
        validateSDE(SDE_WORKSPACE)
        target_workspace = SDE_WORKSPACE
    except Exception as e:
        LOGGER.log(15, e)
        LOGGER.log(15, traceback.format_exc())
        LOGGER.error("Fatal Error: DB connection not valid. Aborting...")
        return 101

    # if using a feature dataset, ensure exists
    if FDS_NAME:
        target_workspace = os.path.join(SDE_WORKSPACE, FDS_SDE_PREFIX + FDS_NAME)
        try:
            LOGGER.log(15, "Validating feature dataset in DB...")
            validateFDS(SDE_WORKSPACE, FDS_NAME, spatialref=prjSR)
        except Exception as e:
            LOGGER.log(15, e)
            LOGGER.log(15, traceback.format_exc())
            LOGGER.error("Fatal Error: Feature Dataset not valid. Aborting...")
            return 102

    # arcpy variable to allow overwriting existing files
    env.overwriteOutput = True

    # create temp processing workspace
    templocation = create_temp_workspace(TEMP_WORKSPACE, TEMP_GDB_NAME)
    LOGGER.log(15, "Temporary location is {0}.".format(templocation))

    wfsupdatelist = []  # list of wfs data to update
    archiveerror = 0

    # process stations in each network
    for network in NETWORKS:
        fc = None
        fc_name = "stations_" + network
        try_count = 0
        archiveerror += 1  # add error to be removed after successful execution

        while try_count <= RETRY_COUNT:
            try:
                try_count += 1
                fc = get_network_stations(network, fc_name, unprjSR)
            except Exception as e:
                LOGGER.log(15, e)
                LOGGER.log(15, traceback.format_exc())
                LOGGER.warning("\nError. Retrying...\n")
            else:
                break

        if fc:
            LOGGER.info("Reprojecting station data and writing to output...")
        else:
            LOGGER.error("ERROR: Failed to retrieve all stations from {0}. Skipping to next network...".format(network))
            write_to_summary_log("{}: stations_{} processing FAILED".format(datetime.now(), network))
            continue

        if network == "USGS":
            LOGGER.info("USGS data requires area from USGS web service. Retreiving...")
            try:
                get_USGS_metadata(fc)
            except Exception as e:
                LOGGER.log(15, e)
                LOGGER.log(15, traceback.format_exc())
                LOGGER.error("Failed to retreive the USGS area data. Could not continue.")
                write_to_summary_log("{}: stations_{} processing FAILED".format(datetime.now(), network))
                continue

        try:
            projectedfc = arcpy.Project_management(fc, os.path.join(templocation, fc_name), prjSR)  # from unprjSR to prjSR
        except Exception as e:
            LOGGER.log(15, e)
            LOGGER.log(15, traceback.format_exc())
            LOGGER.error("Failed to reproject the data. Could not continue.")
            write_to_summary_log("{}: stations_{} processing FAILED".format(datetime.now(), network))
            continue

        write_to_summary_log("{}: stations_{} processed OK".format(datetime.now(), network))

        # remove in_memory temp FC
        try:
            arcpy.Delete_management(fc)
        except:
            pass

        try:
            LOGGER.info("Archiving the data to the FTP site...")
            archive_GDB_FC(projectedfc.getOutput(0), ARCHIVE_WORKSPACE)
            write_to_summary_log("{}: stations_{} archived OK".format(datetime.now(), network))
        except Exception as e:
            LOGGER.log(15, e)
            LOGGER.log(15, traceback.format_exc())
            LOGGER.error("Failed to archive the data.")
        else:
            archiveerror -= 1  # exectued successfully, so no error

        LOGGER.info("Adding data to WFS update list...")
        wfsupdatelist.append(projectedfc.getOutput(0))

        # end processing of network

    if wfsupdatelist:
        LOGGER.info("\nUpdating WFSs in update list...")
        try:
            updateerrors = update_all_wfs(wfsupdatelist, target_workspace, WFS_SERVICE_FOLDERS)
            if updateerrors:
                LOGGER.error("\nUpdating failed with errors. Aborting...")
                return 201
        except Exception as e:
            LOGGER.log(15, e)
            LOGGER.log(15, traceback.format_exc())
            LOGGER.error("\nFailed to update the web services. Aborting...")
            return 200
    else:
        LOGGER.error("\nNo web services to update. Aborting...")
        return 300

    if archiveerror:
        LOGGER.info("\nOne or more FCs were not successfully archived. Script is aborting before temp data is removed...")
        write_to_summary_log("{}: {} web services FAILED".format(datetime.now(), archiveerror))
    else:
        LOGGER.info("\nAll processes exectued successfully.")
        write_to_summary_log("{}: all web services updated OK".format(datetime.now()))
        try:
            LOGGER.info("Removing temp data...")
            shutil.rmtree(templocation)
        except Exception as e:
            LOGGER.log(15, e)
            LOGGER.log(15, traceback.format_exc())
            LOGGER.error("Could not remove temp data.")

    end = datetime.now()
    LOGGER.info("\nTime finished: {0}.".format(end))
    LOGGER.info("Time elapsed: {0}.".format(end-start))

    return 0


# --------------------------------
#            MAIN CHECK
# --------------------------------

# WARNING: the main is ABSOULTELY NECESSARY on Windows when using multiprocessing.
# FAILURE TO USE THE MAIN CHECK WILL FILL ALL RAM AND CRASH THE ENTIRE SYSTEM.

if __name__ == '__main__':
    import sys

    # -------------------------------
    #             LOGGING
    # -------------------------------

    # in case user calls file with debugging disabled override __DEBUG__ setting
    if not __debug__:
        __DEBUG__ = False

    # DEBUG level is 15; using logging.DEBUG writes SUDS to log

    # setup basic logger for file
    LOGGER = logging.getLogger()
    LOGGER.setLevel(15)

    # to write logging at debug level to file
    fl = logging.FileHandler(FULL_LOGFILE)

    # to write logging at specified level to stderr
    pr = logging.StreamHandler()

    # set stderr level
    if __DEBUG__:
        pr.setLevel(15)
    else:
        pr.setLevel(logging.INFO)

    # add handlers to logger
    LOGGER.addHandler(fl)
    LOGGER.addHandler(pr)

    # -------------------------------

    # call main
    sys.exit(main())

# --------------------------------------------------------------------
#
# NAME:     CSAR_SWE_Prep.py
# AUTHOR:   Jarrett Keifer
# DATE:     11/05/2014 -- Refactored from original SNODAS processing script
#
# DESC:     Checks for SNODAS files in the SOURCE_WORKSPACE. If found,
#           process any SWE files. This involves extracting, removing
#           negative values, and reprojecting. The process raster is then
#           zipped in the FTP_WORKSPACE, moved to the TARGET_WORKSPACE,
#           and added to the web services.
#
# USAGE:    The script requires no arguments, but many key constants
#           such as workspace locations and server settings can be
#           found at the beginning of this script.
#
# REVISION HISTORY:
#           11/06/2014: Fixed logging issues and errors JK
#
# --------------------------------------------------------------------

# some statements in functions to speed import
from __future__ import print_function
import os
import re
import shutil
import glob
import logging
import arcpy
from arcpy import env
from arcpy.sa import *
from datetime import datetime
import traceback


# ---------------- TODO ITEMS -----------------

#  0) None currently


# ---------------- CONSTANTS -----------------

# workspaces
SOURCE_FILE_EXTS = [".grz", "tar.gz"]
SOURCE_WORKSPACE = r"C:\inetpub\ftproot\SNODAS"
TARGET_WORKSPACE = r"C:\GIS\GIS_Data\SNODAS\SWE"
ARCHIVE_WORKSPACE = r"C:\GIS\GIS_Data\SNODAS\Archived"
FTP_WORKSPACE = r"C:\inetpub\ftproot\SNODAS_SWE"
TEMP_WORKSPACE = r"C:\GIS\GIS_Data\SNODAS\TEMPPROCESSING"

# mosaic dataset consts
MOSAIC_GDB = r"C:\Users\jkeifer\Documents\MosaicTesting\MosaicTest.gdb"
MOSAIC_DATASET_NAME = "SWE_test2"
MOSAICDS = os.path.join(MOSAIC_GDB, MOSAIC_DATASET_NAME)
MOSAIC_FIELDS = ("Name", "Time")

# ArcGIS Server settings
SERVER_ADDRESS = "atlas.geog.pdx.edu"
SERVER_PORT = "6080"
ADMIN_USER = "AtlasAdmin"
ADMIN_PSWD = "CSARAtlas459"

# image service settings
IMAGE_SERVICE = "SWE_test/SWE_test2.ImageServer"

# logging
AGS_DYNAMIC_WORKSPACE = "SWE_SNODAS"
SNODAS_RASTER_LIST = r"C:\inetpub\wwwroot\SNODAS\SNODAS_SWE_list.txt"
SNODAS_LOGFILE = r"C:\GIS\GIS_Data\SNODAS\SNODAS_SWE_Processed.txt"
FULL_LOGFILE = os.path.join(TEMP_WORKSPACE, "SNODAS_PROCESSING_LOG.txt")
__DEBUG__ = False  # true outputs debug-level messages to stderr

# unique code in SWE grz file names
SWE_FILE_CODE = "v11034tS"

# spatial ref consts
UNPROJECTED_CRS = arcpy.SpatialReference(4326)  # spatial ref WKID 4326: WGS 1984 GCS
PROJECTED_CRS   = arcpy.SpatialReference(102039)  # spatial ref WKID 102039: USA_Contiguous_Albers_Equal_Area_Conic_USGS_version
DATUM_TRANSFORM = "WGS_1984_(ITRF00)_To_NAD_1983"

# hdr file replacment string
HDRFILE_STRING = "byteorder M\n\
                  layout bil\n\
                  nbands 1\n\
                  nbits 16\n\
                  ncols 6935\n\
                  nrows 3351\n\
                  ulxmap -124.729583333331703\n\
                  ulymap 52.871249516804028\n\
                  xdim 0.00833333333\n\
                  ydim 0.00833333333\n"


# ----------------- LOGGING ------------------

# in case user calls file with debugging disabled override __DEBUG__ setting
if not __debug__:
    __DEBUG__ = False

# setup basic logger for file
logger = logging.getLogger()
logger.setLevel(logging.DEBUG)

# to write logging at debug level to file
fl = logging.FileHandler(FULL_LOGFILE)

# to write logging at specified level to stderr
pr = logging.StreamHandler()

# set stderr level
if __DEBUG__:
    pr.setLevel(logging.DEBUG)
else:
    pr.setLevel(logging.INFO)

# add handlers to logger
logger.addHandler(fl)
logger.addHandler(pr)


# ----------------- CLASSES ------------------

class GeoprocessingError(Exception):
    """Basic error class for geoprocessing error exceptions"""
    pass


# ---------------- FUNCTIONS -----------------

def untar(filepath, outfoldername='.', compression='r', deletesource=False):
    """
    Given an input tar archive filepath, extracts the files.

    Required: filepath -- the path to the tar archive

    Optional: outfoldername -- the output directory for the files; DEFAULT is directory with tar archive
              compression -- the type of compression used in the archive; DEFAULT is 'r'; use "r:gz" for gzipped archives
              deletesource -- a boolean argument determining whether to remove the archive after extraction; DEFAULT is false

    Output:   filelist -- the list of all extract files
    """
    import tarfile

    with tarfile.open(filepath, compression) as tfile:
        filelist = tfile.getnames()
        tfile.extractall(path=outfoldername)

    if deletesource:
        try:
            os.remove(filepath)
        except:
            raise Exception("Could not delete tar archive {0}.".format(filepath))

    return filelist

   
def get_date_of_SWE_data(instring):
    """
    Given a SWE filename as a string, return SWE_YYYYMMDDHH.

    Required: instring -- the SWE filename as a string

    Output:   datestring -- the date parsed from the filename, prepended with "SWE_"
    """
    datestring = "SWE_" + instring[instring.find("TTNATS") + 6:instring.find("TTNATS") + 16]
    # TTNA - a detail of snow modeling operations, is always TTNA
    # TS - Time-stamp, leading characters for 8 digits (YYYYMMDDHH) time-stamp
    return datestring


def writelog(SWEFilename, Message, replacement=False):
    """
    Write a message to the SWE logfile

    Requires: SWEFilename -- the filepath to the SWE logfile
              Message -- the message to append to the line in the logfile

    Optional: replacement -- a boolean flag as to whether a file is replacement data or not

    Output:   None
    """
    if replacement:
        replacestring = "REPLACEMENT_DATA:"
    else:
        replacestring = ""
    
    with open(SNODAS_LOGFILE, "a") as logfile:
        #write ArcGIS Server Dynamic Workspace name, dataset name, datestamp, timestamp to log file
        logfile.write(AGS_DYNAMIC_WORKSPACE + "," + str(SWEFilename)+ "," + datetime.now().strftime("%Y%m%d,%H:%M:%S") + "," + replacestring + Message + "\n")


def add_to_SWE_list(SWEFilename):
    """
    Write a message to the SWE list of available rasters

    Requires: SWEFilename -- the filepath to the SWE raster list
              Message -- the message to append to the line in the raster list

    Output:   None
    """
    
    with open(SNODAS_RASTER_LIST, "a") as logfile:
        #write ArcGIS Server Dynamic Workspace name, dataset name, datestamp, timestamp to log file
        logfile.write("\n" + AGS_DYNAMIC_WORKSPACE + "," + str(SWEFilename)+ "," + datetime.now().strftime("%Y%m%d,%H:%M:%S"))


def replace_hdr_file(hdrfile):
    """
    Replace the .hdr file for a .bil raster with the correct data for Arc processing

    Required: hdrfile -- filepath for .hdr file to replace/create

    Output:   None
    """
    with open(hdrfile, 'w') as o:
        o.write(HDRFILE_STRING)


def remove_negative_values(infile, outfile):
    """
    Removes the 2's-complement input .bil raster negative values, leaving only values from 0-32767

    Requires: infile -- path to input raster
              outfile -- path for output raster to be created

    Output:   outfile -- returns the outfile filepath for convenience
    """
    #temp = "in_memory/temp"  # FOR UNSIGNED INT
    
    # Check out the ArcGIS Spatial Analyst extension license
    arcpy.CheckOutExtension("Spatial")

    # convert raster data from signed to unsigned integer
    outCon = Con(Raster(infile) >= 32768, Raster(infile) - 65536, Raster(infile))
    outCon2 = Con(outCon < 0, 0, outCon) # replace negative values with 0

    # save the output to disk
    # NOTE: IN 32-BIT SIGNED INT FORMAT
    outCon2.save(outfile)  # COMMENT OUR THIS LIKE FOR UNSIGNED INT

    # save the output to memory 
    #outCon2.save(temp)  # FOR UNSIGNED INT

    # TO GET UNSIGNED INT FORMAT, USE THE FOLLOWING LINE AND MAKE REQUSITE CHANGES TO FUNCTION
    #arcpy.CopyRaster_management(temp, outfile, pixel_type="16_BIT_UNSIGNED", nodata_value=32767)
    
    return outfile
    

def zip_img_to_ftp(imgfile, ftp_folder):
    """
    Zips an input img file and all anciliary files into an archive in the FTP workspace

    Requires: imgfile -- the path to the img file to be compressed
              ftp_folder -- the folder where the archive will be written

    Output:   zippath -- the path the created zip archive
    """
    imgfilelist = glob.glob(os.path.splitext(imgfile)[0] + "*")
    filename = os.path.basename(imgfile)
    
    # the zipped files are stored in YYYYMM subfolders of the tofolder
    outfolder = os.path.join(ftp_folder, filename[4:10])  # get year and month for folder name
    
    if not os.path.isdir(outfolder):
        os.mkdir(outfolder)

    zippath = zip_files(imgfilelist, os.path.join(outfolder, os.path.splitext(filename)[0] + ".zip"))
                    
    return zippath


def zip_files(infiles, zippath, deletesource=False):
    """
    Zips a list of files to a zip archive specified by the zippath

    Required: infiles -- a llist of files to add to the archive
              zippath -- the path of the zip archive that will be created

    Optional: deletesource -- a boolean flag to control whether the infiles are removed after archiving;
                              DEFAULT is False and the files will not be deleted

    Output:   zippath -- the path of the created zip archive, for convenience
    """
    import zipfile
    
    with zipfile.ZipFile(zippath, mode = "w", compression=zipfile.ZIP_DEFLATED) as zipf:
        for afile in infiles:
            zipf.write(afile, arcname=os.path.basename(afile))

            if deletesource:
                try:
                    os.remove(afile)
                except Exception as e:
                    logger.debug(e)

    return zippath


def create_temp_folder(workspace, foldername):
    """
    Creates a temp folder in which an SWE file will be processed

    Required: workspace -- the directory in which to create the temp folder
              foldername -- the name of the temp folder (should be the SWE name)

    Output:   directory -- the full path to the temp folder
    """
    directory = os.path.join(workspace, foldername)

    if os.path.isdir(directory):
        shutil.rmtree(directory)
        
    os.mkdir(directory)

    return directory


def archive_sourcefile(filepath):
    """
    Moves a processed source file to the source file archive

    Required: filepath -- the path of the file to be moved

    Output:   None
    """
    archivefile = os.path.join(ARCHIVE_WORKSPACE, os.path.basename(filepath))
    
    if os.path.exists(archivefile):
        os.remove(archivefile)

    shutil.move(filepath, ARCHIVE_WORKSPACE)


def OLDMETHOD_update_webservice_sourcedata(sourceimgfile):
    """
    Uses the old shared geoprocessing service to update the SWE MapService

    Required: sourceimgfile -- the path of the img file to be used in the MapService

    Output:   None
    """
    import time
    
    arcpy.ImportToolbox("http://atlas.geog.pdx.edu:6080/arcgis/services;SNODAS_Utilities/ReplaceSWELayer", "ReplaceSWELayer")
    result = arcpy.ReplaceSWELayer_ReplaceSWELayer(sourceimgfile)
    while result.status < 4:
        time.sleep(0.2)
    print(sourceimgfile + " is set as the source data of SNODAS web service.")


def strip_raster_date_from_name(name):
    """
    Get a datetime object from the YYYY, MM, and DD in the SWE name

    Required: name -- the SWE name, as returned by get_date_of_SWE_data()

    Output:   a datetime object
    """
    return datetime(int(name[4:8]), int(name[8:10]), int(name[10:12]))


def add_to_mosaic_dataset(imgfile):
    """
    Adds a raster to the global Mosaic Dataset and updates the time field with the date of the raster

    Required: imgfile -- the path to the raster to be added

    Output:   None
    """
    arcpy.AddRastersToMosaicDataset_management(MOSAICDS,
                                               "Raster Dataset",
                                               imgfile,
                                               duplicate_items_action="OVERWRITE_DUPLICATES",
                                               build_pyramids=True,
                                               calculate_statistics=True,
                                               )
    with arcpy.da.UpdateCursor(MOSAICDS, MOSAIC_FIELDS) as cursor:
        for row in cursor: 
            if row[0] == os.path.splitext(os.path.basename(imgfile))[0]:
                row[1] = strip_raster_date_from_name(row[0])
            cursor.updateRow(row)


def update_image_service():
    """
    Calculates the statistics on the mosaic dataset, then updates the image service
    by restarting the service

    Required: None (all arguments are global constants)

    Output:   None
    """
    from arcpyExt import agsAdmin

    # update mosaic statistics
    #arcpy.CalculateStatistics_management(MOSAICDS)

    # restart the service to make changes take effect
    try:
        agsconnection = agsAdmin.AgsAdmin.connectWithoutToken(SERVER_ADDRESS, SERVER_PORT, ADMIN_USER, ADMIN_PSWD)
        agsconnection.stopService(IMAGE_SERVICE)
        agsconnection.startService(IMAGE_SERVICE)
    except Exception as e:
        logging.debug(e)
        raise Exception("Could not successfully update the image service {0}.".format(IMAGE_SERVICE))    
    

# ------------------- MAIN ---------------------
    
def main():

    env.overwriteOutput = True

    updates = 0

    filelist = []
    for ext in SOURCE_FILE_EXTS:
        filelist += glob.glob(os.path.join(SOURCE_WORKSPACE, "*" + ext))

    logger.debug("\n----------------------------------------------------------------------------------------------\n")
    logger.debug("{0} -- Processing files in {1}.".format(datetime.now().strftime("%Y-%m-%d"),
                                                          SOURCE_WORKSPACE))
    logger.info("Found {0} files to process.".format(len(filelist)))

    for f in filelist:
        logger.info("\nProcessing {0}...".format(f))
        errormsg = None
        replaced = False
        fullfilename = os.path.basename(f)
        filename, originalext = os.path.splitext(fullfilename)

        if filename.endswith(".tar"):
            originalext += os.path.splitext(filename)[1]
            filename = os.path.splitext(filename)[0]

        if glob.glob(os.path.join(ARCHIVE_WORKSPACE, fullfilename)):
            # TODO: implement whatever needs to happen if file is already in archive.
            # assume that files are revisions; overwrite all old files.
            logger.info("File already processed; overwriting.")
            replaced = True
            #continue

        if SWE_FILE_CODE in f:
            swe_name = get_date_of_SWE_data(f)
            projectedname =  swe_name + ".img"
            outfile = os.path.join(TARGET_WORKSPACE, projectedname)

            # create a folder in the temp workspace for this data
            try:
                logger.info("Creating temp processing folder...")
                tempfolder = create_temp_folder(TEMP_WORKSPACE, swe_name)
            except Exception as e:
                logger.debug(e)
                logger.debug(traceback.format_exc())
                errormsg = "Failed to create a temp processing folder."
                logger.error("File conversion stopped. Status: " + errormsg)
                writelog(projectedname, errormsg, replaced)
                continue

            # extract files from .grz archive
            try:
                logger.info("Extracting compressed files to temp folder...")
                untar(f, outfoldername=tempfolder, compression='r:gz')
            except Exception as e:
                logger.debug(e)
                logger.debug(traceback.format_exc())
                errormsg = "Failed to extract the compressed source files."
                logger.error("File conversion stopped. Status: " + errormsg)
                writelog(projectedname, errormsg, replaced)
                continue

            # change ext on .dat file to .bil
            try:
                logger.info("Changing dat file extension to bil...")
                datfile = glob.glob(os.path.join(tempfolder, filename + ".dat"))
                bilfile = re.sub(r".dat", ".bil", datfile[0])
                os.rename(datfile[0], bilfile)
            except Exception as e:
                logger.debug(e)
                logger.debug(traceback.format_exc())
                errormsg = "Failed to change the file extension to bil."
                logger.error("File conversion stopped. Status: " + errormsg)
                writelog(projectedname, errormsg, replaced)
                continue

            # replace header file to allow processing with ArcGIS
            hdrfile = re.sub(r".dat", ".hdr", datfile[0])
            
            try:
                logger.info("Rewriting raster header file...")
                replace_hdr_file(hdrfile)
            except Exception as e:
                logger.debug(e)
                logger.debug(traceback.format_exc())
                errormsg = "Failed to write necessary data to raster header file."
                logger.error("File conversion stopped. Status: " + errormsg)
                writelog(projectedname, errormsg, replaced)
                continue
            
            # convert .bil values to unsigned integer range
            try:
                logger.info("Converting image values to unsigned integer range...")
                unsignedraster = os.path.join(tempfolder, "unsigned.img")
                remove_negative_values(bilfile, unsignedraster)
            except Exception as e:
                logger.debug(e)
                logger.debug(traceback.format_exc())
                logger.debug(arcpy.GetMessages())
                errormsg = "Failed to convert .bil file to unsigned integer range."
                logger.error("File conversion stopped. Status: " + errormsg)
                writelog(projectedname, errormsg, replaced)
                continue

            # define CRS of converted file
            try:
                logger.info("Defining the image's initial CRS...")
                arcpy.DefineProjection_management(unsignedraster, UNPROJECTED_CRS)
            except Exception as e:
                logger.debug(e)
                logger.debug(traceback.format_exc())
                logger.debug(arcpy.GetMessages())
                errormsg = "Failed to define the projection for the .bil file."
                logger.error("File conversion stopped. Status: " + errormsg)
                writelog(projectedname, errormsg, replaced)
                continue

            # reproject raster to new CRS
            try:
                logger.info("Reprojecting image to new CRS...")
                projectedraster = arcpy.ProjectRaster_management(unsignedraster,
                                                                 os.path.join(tempfolder, projectedname),
                                                                 PROJECTED_CRS,
                                                                 "BILINEAR",
                                                                 "#",
                                                                 DATUM_TRANSFORM)
            except Exception as e:
                logger.debug(e)
                logger.debug(traceback.format_exc())
                logger.debug(arcpy.GetMessages())
                errormsg = "Failed to reproject the raster to new CRS."
                logger.error("File conversion stopped. Status: " + errormsg)
                writelog(projectedname, errormsg, replaced)
                continue

            # copy file from temp to target workspace
            try:
                logger.info("Copying img file to target workspace...")
                arcpy.Copy_management(projectedraster, outfile)
            except Exception as e:
                logger.debug(e)
                logger.debug(traceback.format_exc())
                logger.debug(arcpy.GetMessages())
                errormsg = "Failed to copy img file to target workspace."
                logger.error("File conversion stopped. Status: " + errormsg)
                writelog(projectedname, errormsg, replaced)
                continue

            # write file entry to log as successful
            writelog(projectedname, "OK", replaced)

            # MOVE INTO FUNCTION TO ALLOW RETURN VALUE FROM ARCHIVE AND UPDATE STEPS TO PREVENT REMOVING TEMP FOLDER IF PROBLEM
            # compress files into zip archive in ftp folder
            try:
                logger.info("Compressing projected image to folder in FTP workspace...")
                zip_img_to_ftp(outfile, FTP_WORKSPACE)
            except Exception as e:
                logger.debug(e)
                logger.debug(traceback.format_exc())
                errormsg = "Failed to compress img files to FTP folder."
                logger.error("Error: " + errormsg)
                pass

            # update the web service
            try:
                logger.info("Updating the web service and mosaic...")
                if not replaced:
                    # do not want to update MapService or raster list if replacement data
                    add_to_SWE_list(projectedname)
                    OLDMETHOD_update_webservice_sourcedata(outfile)
                    pass
                add_to_mosaic_dataset(outfile)
                updates += 1
            except Exception as e:
                logger.debug(e)
                logger.debug(traceback.format_exc())
                logger.debug(arcpy.GetMessages())
                errormsg = "Failed to update the web service."
                logger.error("File conversion stopped. Status: " + errormsg)
                continue

            # remove temp processing folder
            try:
                logger.info("File processed successfully.")
                logger.info("Removing the temp processing folder...")
                shutil.rmtree(tempfolder)
            except Exception as e:
                logger.debug(e)
                logger.debug(traceback.format_exc())
                errormsg = "Failed to remove temp processing folder."
                logger.error("Error: " + errormsg)
                pass

        # move files to archive workspace
        try:
            logger.info("Moving files from source to archive workspace...")
            archive_sourcefile(f)
        except Exception as e:
            logger.debug(e)
            logger.debug(traceback.format_exc())
            errormsg = "Failed to move the source files from the source directory to the archive directory."
            logger.error("Error: " + errormsg)
            pass

    if updates:
        try:
            logger.info("\nRestarting the image web service to make updates available...")
            update_image_service()
            pass
        except Exception as e:
            logger.debug(e)
            logger.debug(traceback.format_exc())
            errormsg = "Failed to stop or start the image service for updates."
            logger.error("Error: " + errormsg)
    
    return 0


# ----------------- MAIN CHECK ------------------

if __name__ == '__main__':
    import sys
    
    sys.exit(main())

import os, sys, tarfile, glob, zipfile, gzip, shutil, datetime, arcpy, traceback, tempfile, time
from arcpy.sa import *
# http://docs.python.org/2/library/index.html 

sourceWorkspace = r"Y:\Development\CSAR\SNODAS\Scripts\NewSource\SOURCE/"
targetWorkspace = r"Y:\Development\CSAR\SNODAS\Scripts\NewSource\TARGET/"
archiveWorkspace = r"Y:\Development\CSAR\SNODAS\Scripts\NewSource\ARCHIVE/"
ftpWorkspace = r"Y:\Development\CSAR\SNODAS\Scripts\NewSource\FTP/"
temp_dir = r"Y:\Development\CSAR\SNODAS\Scripts\NewSource\TEMP/"

AGS_DYNAMIC_WORKSPACE = "SWE_SNODAS"
SNODAS_LOGFILE = r"Y:\Development\CSAR\SNODAS\Scripts\NewSource\SNODAS_SWE_list.txt"

def rename_file_extension_in_folder(foldername, oldext, newext):
    fileList = glob.glob(foldername + "*." + oldext)
    oldextlen = len(oldext)
    fileno = len(fileList)
    
    if fileno > 0:
        for afile in fileList:
            newfile = afile[ :oldextlen * -1] + newext
            os.rename(afile, newfile)
            #print(afile + " ===> " + newfile)

    print (str(fileno) + " files were renamed.")
    return fileno;


def unzip_SWE_gz_to_dat_file(infoldername, outfoldername, deleteSource = True):
    fileList = glob.glob(infoldername + "*v11034tS*.tar.gz")
    # v1 - operational snow model output
    # 1034 - snow water equivalent
    # tS__ - snow pack data is the integral through all the layers of the snow pack
    fileno = len(fileList)

    if fileno > 0:
        for afile in fileList:
            tarfilename = afile[ :-6] + "tar"
            
            #extract tar from gz
            f = gzip.open(afile, 'rb')
            outf = open(tarfilename, 'wb')
            outf.writelines(f.read())
            f.close()
            outf.close()
            
            #extract all files from tar file
            tarf = tarfile.open(tarfilename, "r")

            tarfilelist = tarf.getmembers()
            
            for subfile in tarfilelist:
                tarf.extract(subfile, outfoldername)
                
            tarf.close()
            
            #delete tar file
            os.remove(tarfilename)
            if deleteSource:
                os.remove(afile)
                
            #print(afile + " ===> decompressed")

    print (str(fileno) + " files were decompressed.")
    return fileno


def remove_files_in_folder(folderpath, filetype):   
    fileList = glob.glob(folderpath + "*." + filetype)
    fileno = len(fileList)
    
    if fileno > 0:
        for afile in fileList:
            os.remove(afile)
            
    return fileno


def move_files_in_folder(fromfolder, tofolder, filetype):   
    fileList = glob.glob(fromfolder + "*." + filetype)
    fileno = len(fileList)
    
    if fileno > 0:
        for afile in fileList:
            shutil.move(afile, tofolder)

    print (str(fileno) + " files were archived.")            
    return fileno
   
   
def get_date_of_SWE_data(instring):
    datestring = "SWE_" + instring[instring.find("TTNATS") + 6:instring.find("TTNATS") + 16]
    # TTNA - a detail of snow modeling operations, is always TTNA
    # TS - Time-stamp, leading characters for 8 digits (YYYYMMDDHH) time-stamp
    return datestring


def writelog(logFilename, SWEFilename, OtherMessage):
#this function is called at the end of the geoprocess function

    #prepare text message for data logging
    now = datetime.datetime.now()
    #set date and time formats
    ymd = now.strftime("%Y%m%d")
    hms = now.strftime("%H:%M:%S")
    
    # write the text string to the log file
    with open(logFilename, "a") as logfile:
        #write ArcGIS Server Dynamic Workspace name, dataset name, datestamp, timestamp to log file
        logfile.write(AGS_DYNAMIC_WORKSPACE + "," + str(SWEFilename)+ "," + str(ymd) + "," + str(hms) + "," + OtherMessage + "\n")

#check if temp and target folders exist and cleanup the temp folder
def initialize_workspace(outFolderpath):
    try:
#         if os.path.isdir(tempFolderpath): #if temp folder exists, clean up the folder
#             shutil.rmtree(tempFolderpath)
#     
#         os.mkdir(tempFolderpath)
    
        if os.path.isdir(outFolderpath) == False:
            os.mkdir(outFolderpath)
        
        return True
        
    except:
        return False


def copy_img_to_ftp(fromfolder, ftp_folder, filename):
    outFolderpath = ftp_folder + filename[4:10]
    
    if os.path.isdir(outFolderpath) == False:
        os.mkdir(outFolderpath)

    try:    
        arcpy.Copy_management(fromfolder + filename, outFolderpath + "/" + filename)
        print (filename + " was copied to " + outFolderpath + ".")
    except:
        print ("Error! Unable to copy " + filename + " to the ftp folder!")

    
def geoprocess(inFilepathname, outPath, outFilename):
    # Overwrite pre-existing files
    arcpy.env.overwriteOutput = True

    SNODAS_errorMessage = ""
    inFilepath, inFilename = os.path.split(inFilepathname)
    
    # define file names of temp layers
    unsignedRaster = inFilepath + "/unsigned"
    projectedName = "albers"
    projectedRaster = inFilepath + "/" + projectedName    
    
    #prepare the header file
    hdrFilepathname = inFilepathname[:-3] + "Hdr"
    
    o = open(hdrFilepathname,'w')
    o.write("byteorder M\n")
    o.write("layout bil\n")
    o.write("nbands 1\n")
    o.write("nbits 16\n")
    o.write("ncols 6935\n")
    o.write("nrows 3351\n")
    o.write("ulxmap -124.729583333331703\n")
    o.write("ulymap 52.871249516804028\n")
    o.write("xdim 0.00833333333\n")
    o.write("ydim 0.00833333333\n")
    o.close()
    
    
    # CONVERT .BIL FILE FROM SIGNED TO UNSIGNED INTEGER
    print ("CONVERT .BIL FILE FROM SIGNED TO UNSIGNED INTEGER")
    try:
        # Check out the ArcGIS Spatial Analyst extension license
        arcpy.CheckOutExtension("Spatial")

        # Execute Con to convert raster data from signed integer to unsigned integer
        outCon = Con(Raster(inFilepathname) >= 32768, Raster(inFilepathname) - 65536, Raster(inFilepathname))
        # replace negative values with 0"        
        outCon2 = Con(outCon < 0, 0, outCon)

        # Save the output
        outCon2.save(unsignedRaster)
        
    except arcpy.ExecuteError: 
        # Get the tool error messages 
        SNODAS_errorMessage = SNODAS_errorMessage + arcpy.GetMessages() + ";" 
        print SNODAS_errorMessage

    except:
        # Get the traceback object
        tb = sys.exc_info()[2]
        tbinfo = traceback.format_tb(tb)[0]
        # Concatenate information together concerning the error into a message string
        pymsg = tbinfo + ";" + str(sys.exc_info()[1])
        SNODAS_errorMessage = SNODAS_errorMessage + "Failed at CONVERT SIGNED TO UNSIGNED;" 
        print pymsg
        
        
    # DEFINE PROJECTION FOR CONVERTED FILE
    print ("DEFINE PROJECTION FOR CONVERTED FILE")
    try:
        #coordinateSystem = "C:\Program Files (x86)\ArcGIS\Desktop10.0\Coordinate Systems\Geographic Coordinate Systems\World\WGS 1984.prj"
        coordinateSystem = "GEOGCS['GCS_WGS_1984',DATUM['D_WGS_1984',SPHEROID['WGS_1984' \
        ,6378137.0,298.257223563]],PRIMEM['Greenwich',0.0],UNIT['Degree',0.0174532925199433]]"
        arcpy.DefineProjection_management(unsignedRaster, coordinateSystem)

    except:
        # Get the tool error messages 
        SNODAS_errorMessage = SNODAS_errorMessage + "Failed at DEFINE PROJECTION;" 
        print arcpy.GetMessages()
                
                
    # REPROJECT CONVERTED FILE FROM WGS84 TO ALBERS EQUAL AREA CONIC USGS
    print ("REPROJECT CONVERTED FILE FROM WGS84 TO ALBERS EQUAL AREA CONIC USGS")
    try:
        #arcpy.ProjectRaster_management(inProj, outProj, "USA Contiguous Albers Equal Area Conic USGS.prj" \
        #,"BILINEAR", "#","WGS_1984_(ITRF00)_To_NAD_1983", "#", "#")
        arcpy.ProjectRaster_management(unsignedRaster, projectedRaster,"PROJCS['USA_Contiguous_Albers_Equal_Area_Conic_USGS_version' \
        ,GEOGCS['GCS_North_American_1983',DATUM['D_North_American_1983',SPHEROID['GRS_1980',6378137.0,298.257222101]] \
        ,PRIMEM['Greenwich',0.0],UNIT['Degree',0.0174532925199433]],PROJECTION['Albers'],PARAMETER['False_Easting',0.0] \
        ,PARAMETER['False_Northing',0.0],PARAMETER['Central_Meridian',-96.0],PARAMETER['Standard_Parallel_1',29.5] \
        ,PARAMETER['Standard_Parallel_2',45.5],PARAMETER['Latitude_Of_Origin',23.0],UNIT['Meter',1.0]]", "BILINEAR" \
        ,"#", "WGS_1984_(ITRF00)_To_NAD_1983","#", "#")
        arcpy.Delete_management(unsignedRaster)
        
    except:
        # Get the tool error messages 
        SNODAS_errorMessage = SNODAS_errorMessage + "Failed at REPROJECTION;" 
        print arcpy.GetMessages()


    # CONVERT GRID FILE TO IMAGINE .IMG FORAMT
    print "CONVERT REPROJECTED FILE TO IMAGINE .IMG FORAMT"
    try:
        arcpy.RasterToOtherFormat_conversion(projectedRaster, outPath, "IMAGINE Image")
        arcpy.Delete_management(projectedRaster)
        arcpy.Rename_management(outPath + projectedName + ".img", outPath + outFilename)
        
    except:
        # Get the tool error messages 
        SNODAS_errorMessage = SNODAS_errorMessage + "Failed at CONVERT TO .IMG FORMAT;" 
        print arcpy.GetMessages() 
        pass
    
    if SNODAS_errorMessage == "":
        SNODAS_errorMessage = "OK"
        
    writelog(SNODAS_LOGFILE, outFilename, SNODAS_errorMessage);
    print "File conversion stopped. Status: " + SNODAS_errorMessage


def zip_imgfiles(imgfilepathname, tofolder, deleteSource = True):
    imgfilelist = glob.glob(imgfilepathname[:-3] + "*")
    filename = os.path.basename(imgfilepathname)[:-4]
    toname = filename + ".zip"
    
    #the zipped files are stored in YYYYMM subfolders of the tofolder
    outFolderpath = tofolder + filename[4:10]
    
    if os.path.isdir(outFolderpath) == False:
        os.mkdir(outFolderpath)
    
    zipfilename = outFolderpath + "/" + toname
    
    zipf = zipfile.ZipFile(zipfilename, mode = "w", compression = zipfile.ZIP_DEFLATED)

    try:
        for afile in imgfilelist:
            zipf.write(afile, arcname = os.path.basename(afile))
            if deleteSource:
                os.remove(afile)
            
    finally:
        zipf.close()

    print imgfilepathname + " was added to " + zipfilename + "."


#call asynchronous GP service
def update_webservice_sourcedata(sourceimgfile):
    arcpy.ImportToolbox("http://atlas.geog.pdx.edu:6080/arcgis/services;SNODAS_Utilities/ReplaceSWELayer", "ReplaceSWELayer")
    result = arcpy.ReplaceSWELayer_ReplaceSWELayer(sourceimgfile)
    while result.status < 4:
        time.sleep(0.2)
    print sourceimgfile + " is set as the source data of SNODAS web service."
    

def main():
    if initialize_workspace(targetWorkspace):
        try: 
            #temp_dir = tempfile.mkdtemp() + "/"
            print "temp_dir folder is: " + temp_dir
            rename_file_extension_in_folder(sourceWorkspace, "grz", "tar.gz");
        
            unzip_SWE_gz_to_dat_file(sourceWorkspace, temp_dir, False)
            grzfileno = rename_file_extension_in_folder(temp_dir, "dat", "bil")
            
            move_files_in_folder(sourceWorkspace, archiveWorkspace, "tar.gz")
                
            if grzfileno > 0:                
                #Process all the dat/hdr files in the tempWorkspace one by one
                fileList = glob.glob(temp_dir + "*.bil")
                fileno = len(fileList)
                
                if fileno > 0:
                    for afile in fileList:
                        outFilename = get_date_of_SWE_data(afile) + ".img"
                        print "\n" + outFilename + " ======================"
                        geoprocess(afile, targetWorkspace, outFilename)
                        
                        #copy the img data to the ftp server
                        #copy_img_to_ftp(targetWorkspace, ftpWorkspace, outFilename)
                        #zip the img data to the ftp server
                        zip_imgfiles(targetWorkspace + outFilename, ftpWorkspace, False)
                        
                    print ("\n" + str(fileno) + " files were converted to .img format.")
                    
                    #set the last converted SWE layer as the source data of SNODA web service
                    #update_webservice_sourcedata(outFilename)
              
                else:
                    print ("\nNo file was processed.")
            else:
                print ("\nNo file was processed.")
            
        finally:
            #try:
            #    shutil.rmtree(temp_dir)
            #except OSError, e:
            #    if e.errno != 2: # code 2 - no such file or directory
            #        raise
            pass
            
    else:
        print ("\nUnable to initialize the temporary and the target workspaces.")
        
main()

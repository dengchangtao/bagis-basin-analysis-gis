import os, glob, gzip, zipfile

sourceWorkspace = "C:/inetpub/ftproot/SNODAS/"
targetWorkspace = "C:/GIS/GIS_Data/SNODAS/SWE/"
archiveWorkspace = "C:/GIS/GIS_Data/SNODAS/Archived/"
ftpWorkspace = "C:/inetpub/ftproot/SNODAS_SWE/"

AGS_DYNAMIC_WORKSPACE = "SWE_SNODAS"
SNODAS_LOGFILE = "C:/inetpub/wwwroot/SNODAS/SNODAS_SWE_list.txt"


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
    

#This function is for server servicing use only, not for daily data processing
def zip_imgfiles_on_ftp_server(ftpfolder, deleteSource = True):
    containerlist = glob.glob(ftpfolder + "201*")

    for aFolder in containerlist:
        print aFolder
        filelist = glob.glob(aFolder + "/*.img")

        for aFile in filelist:
            print aFile
            zip_imgfiles(aFile, ftpfolder, deleteSource)
            
        print str(len(filelist)) + " .img files compressed in folder: " + aFolder + "."

    
def main():
    zip_imgfiles_on_ftp_server(ftpWorkspace, True)


main()
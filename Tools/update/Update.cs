using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Ionic.Zip;

namespace Update
{
    public class Update
    {
        private static string tempZipFile = string.Empty;
        private static string newRelaseDir = string.Empty;

        public static void Main(string[] args)
        {
            string name = GetZipFile();
            if (!name.Equals(string.Empty))
            {
                if (Unzip(name))
                {
                    CopyConfigs(name);
                    CopyLibrary(name);
                }
            }
            DisplayInfo();
        }

        #region GetZipFile
        public static string GetZipFile()
        {
            string downloadURI = GetDownloadLink();
            if (downloadURI.Equals(string.Empty))
                return string.Empty;
            Console.WriteLine("Using download link " + downloadURI);

            string newReleaseName = string.Empty;
            if (!CheckUpdateNeeded(downloadURI, out newReleaseName))
            {
                Console.WriteLine("Your release is already up to date.");
                return string.Empty;
            }
            Console.WriteLine("New release available. Updating...");
            GetIt(downloadURI);

            return newReleaseName;
        }

        private static string GetDownloadLink()
        {
            WebRequest webReq = HttpWebRequest.Create("http://metaverseink.com/download");
            webReq.Method = "GET";

            HttpWebResponse webResp;
            try
            {
                webResp = (HttpWebResponse)webReq.GetResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine("* Unable to contact Metaverse Ink for updates. Please try again later. Reason: " + e.Message);
                return string.Empty;
            }

            if (webResp.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("* Bad response (" + webResp.StatusCode + ") from Metaverse Ink. Please try updating later.");
                return string.Empty;
            }

            Stream receiveStream = webResp.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string response = readStream.ReadToEnd();

            if (webResp != null) webResp.Close();
            if (receiveStream != null) receiveStream.Close();
            if (readStream != null) readStream.Close();

            // This is very fragile.
            string[] parts = response.Split(new char[] { '@' });
            string uri = string.Empty;
            if (parts.Length == 3)
            {
                uri = parts[1];
            }
            else
            {
                Console.WriteLine("* Unable to find download link. Please try again later.");
            }
            return uri;
        }

        private static bool CheckUpdateNeeded(string downloadURI, out string newName)
        {
            newName = string.Empty;
            string thisFolder = Directory.GetCurrentDirectory();
            string[] parts = thisFolder.Split(new char[] {Path.DirectorySeparatorChar});
            string currentRelease = parts[parts.Length - 2];
            string zipfile = Path.GetFileNameWithoutExtension(downloadURI);
            Console.WriteLine("Your current release is " + currentRelease + ". Available release is " + zipfile);
            if (zipfile == currentRelease)
                return false;

            newName = zipfile;
            return true;
        }

        private static void GetIt(string downloadURI)
        {
            WebRequest webReq = HttpWebRequest.Create(downloadURI);
            webReq.Method = "GET";
            HttpWebResponse webResp;
            try
            {
                webResp = (HttpWebResponse)webReq.GetResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine("* Unable to download new release. Please try again later. (Reason: " + e.Message + ")");
                return;
            }
            if (webResp.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("* Bad response (" + webResp.StatusCode + ") from " + downloadURI + ". Please try updating later.");
                return;
            }

            tempZipFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(downloadURI));
            Stream localStream = File.Create(tempZipFile);

            Stream receiveStream = null;

            Console.WriteLine("Downloading new release from " + downloadURI + " to " + tempZipFile + ". This may take a few minutes.");
            try
            {
                receiveStream = webResp.GetResponseStream();

                byte[] buffer = new byte[1024];
                int bytesRead;
                int buffers = 0;

                // Simple do/while loop to read from stream until
                // no bytes are returned
                do
                {
                    // Read data (up to 1k) from the stream
                    bytesRead = receiveStream.Read(buffer, 0, buffer.Length);

                    // Write the data to the local file
                    localStream.Write(buffer, 0, bytesRead);

                    // Increment total bytes processed
                    buffers++;

                    //Console.Write(" " + bytesProcessed + " ");
                    if ((buffers % 100) == 0) 
                        Console.Write(".");
                } while (bytesRead > 0);
                Console.WriteLine(":-)");
            }
            catch (Exception e)
            {
                Console.WriteLine("* Unable to download file " + downloadURI + ". Please try updating later. Reason: " + e.Message);
            }
            finally
            {
                if (webResp != null) webResp.Close();
                if (receiveStream != null) receiveStream.Close();
                if (localStream != null) localStream.Close();
            }

        }

        #endregion GetZipFile

        #region Unzip

        public static bool Unzip(string newFolderName)
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                // we're in .....top/thisrelease/bin
                // ../..
                DirectoryInfo dirInfo = Directory.GetParent(Directory.GetParent(currentDirectory).FullName);
                newRelaseDir = Path.Combine(dirInfo.FullName, newFolderName);

                using (ZipFile zip = ZipFile.Read(tempZipFile))
                {
                    Console.WriteLine("Extracting new release to " + dirInfo.FullName);
                    foreach (ZipEntry entry in zip)
                    {
                        //Console.WriteLine("Extracting ... " + entry.FileName);
                        entry.Extract(dirInfo.FullName, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine("* Unable to unzip. Please try updating later. (Reason: " + e.Message + ")");
            }
            return false;
        }

        #endregion Unzip

        #region Copy stuff
        public static void CopyConfigs(string newReleaseName)
        {
            Console.WriteLine("Migrating previous configuration data");
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                DirectoryInfo releasesRoot = Directory.GetParent(Directory.GetParent(currentDirectory).FullName);
                string newReleaseDirBinFullPath = Path.Combine(Path.Combine(releasesRoot.FullName, newReleaseName), "bin");

                string oldfile = Path.Combine(Path.Combine(currentDirectory, "config-include"), "MyWorld.ini");
                string newfile = Path.Combine(Path.Combine(newReleaseDirBinFullPath, "config-include"), "MyWorld.ini");
                File.Copy(oldfile, newfile);
                oldfile = Path.Combine(Path.Combine(currentDirectory, "Regions"), "RegionConfig.ini");
                newfile = Path.Combine(Path.Combine(newReleaseDirBinFullPath, "Regions"), "RegionConfig.ini");
                File.Copy(oldfile, newfile);
            }
            catch (Exception e)
            {
                Console.WriteLine("* Unable to migrate configuration data. (Reason: " + e.Message + ")");
            }
        }

        public static void CopyLibrary(string newReleaseName)
        {
            Console.WriteLine("Migrating previous Library data");
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                DirectoryInfo releasesRoot = Directory.GetParent(Directory.GetParent(currentDirectory).FullName);
                string newReleaseDirBinFullPath = Path.Combine(Path.Combine(releasesRoot.FullName, newReleaseName), "bin");

                string oldLibrary = Path.Combine(currentDirectory, "Library");
                string newLibrary = Path.Combine(newReleaseDirBinFullPath, "Library");
                string[] oldIars = Directory.GetFiles(oldLibrary, "*.iar");
                string[] newIars = Directory.GetFiles(newLibrary, "*.iar");
                if (oldIars != null)
                {
                    foreach (string fileName in oldIars)
                    {
                        if (NotIn(newIars, Path.GetFileName(fileName)))
                        {
                            File.Copy(fileName, Path.Combine(newLibrary, Path.GetFileName(fileName)));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("* Unable to migrate Library data. (Reason: " + e.Message + ")");
            }
        }

        private static bool NotIn(string[] files, string fileName)
        {
            if (files != null)
            {
                foreach (string name in files)
                {
                    if (Path.GetFileName(name) == fileName)
                        return false;
                }
            }
            return true;
        }

        #endregion

        #region DisplayInfo

        public static void DisplayInfo()
        {
            if (!newRelaseDir.Equals(string.Empty))
            {
                Console.WriteLine("*********************************************************************");
                Console.WriteLine("Your installation has been updated. The new release is in");
                Console.WriteLine(" >> " + newRelaseDir + " <<");
                Console.WriteLine("Your regions have been migrated.");
                Console.WriteLine("Please read the RELEASENOTES to know what changed.");
                Console.WriteLine("If you made changes to OpenSim.ini, please copy that file over\n or change the new one.");
                Console.WriteLine("*********************************************************************");
            }
            Console.WriteLine("<Press return to exit>");
            Console.ReadLine();
        }

        #endregion
    }
}

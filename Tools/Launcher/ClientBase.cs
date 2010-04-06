using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Win32;

namespace Launcher
{
    public abstract class ClientBase
    {
        protected string target = String.Empty;

        protected string viewerFilename = String.Empty;
        protected string viewerFlags = String.Empty;
        protected string viewerWorkingDirectory = String.Empty;

        protected Process viewerProcess = null;
        protected readonly ProcessStartInfo viewerPSI = new ProcessStartInfo();
        protected string loginUri;

        protected ClientBase()
        {            
            if (ProcessArguments())
            {
                if (TryGetSecondLife(ref viewerFilename, ref viewerWorkingDirectory,
                                           ref viewerFlags))
                {
                    viewerPSI.FileName = viewerFilename;
                    viewerPSI.WorkingDirectory = viewerWorkingDirectory;

                    string viewerArgs = BuildViewerArgs();

                    viewerPSI.Arguments = viewerArgs;
                    viewerPSI.UseShellExecute = true;
                    viewerPSI.WindowStyle = ProcessWindowStyle.Maximized;

                    viewerProcess = Process.Start(viewerPSI);
                }
                else
                {
                    NoViewerError();
                }
            }
        }

        private string BuilrUrlArg()
        {
            string targetFlag;

            if (String.IsNullOrEmpty(target) || target == "/")
            {
                targetFlag = String.Empty;
            }
            else
            {
                targetFlag = string.Format(" -url secondlife://{0}", target);
            }

            return targetFlag;
        }

        protected virtual string BuildViewerArgs()
        {
            string args = BuildMultipleArg() + BuildLoginUriArg() + BuildLoginArg() + " " + viewerFlags + BuilrUrlArg();
            return args;
        }

        protected abstract void NoViewerError();

        public static bool TryGetSecondLife(ref string filename, ref string workingDirectory, ref string flags)
        {
            // Pre-viewer 2.0
            if( TryGetSecondLife(@"SOFTWARE\Linden Research, Inc.\SecondLife", ref filename, ref workingDirectory, ref flags) )
            {
                return true;
            }

            // Beta versions
            if (TryGetSecondLife(@"SOFTWARE\Linden Research, Inc.\SecondLifeBetaViewer", ref filename, ref workingDirectory, ref flags))
            {
                return true;
            }

            // Beta versions
            if (TryGetSecondLife(@"SOFTWARE\Linden Research, Inc.\SecondLifeViewer2", ref filename, ref workingDirectory, ref flags))
            {
                return true;
            }

            return false;
        }

        private static bool TryGetSecondLife(string keyName, ref string filename, ref string workingDirectory, ref string flags)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName);

            if (key == null)
            {
                return false;
            }
            else
            {
                Object exe = key.GetValue("Exe");
                flags = key.GetValue("Flags").ToString();
                workingDirectory = key.GetValue("").ToString();
                filename = workingDirectory + "\\" + exe;

                Registry.LocalMachine.Flush();
                Registry.LocalMachine.Close();

                return true;
            }
        }

        protected bool ProcessArguments()
        {
            bool launch = false;
            string[] args = Environment.GetCommandLineArgs();

            foreach (string arg in args)
            {
                if (arg.ToLower() == "-register")
                {
                    Register();                    
                }

                launch = ProcessArgument(arg, launch);
            }

            return launch;
        }

        protected abstract bool ProcessArgument(string arg, bool launch);

        protected void RegisterMoniker(string moniker)
        {
            RegistryKey regHKLM = Registry.ClassesRoot.CreateSubKey(moniker);

            regHKLM.SetValue("", "URL:" + moniker);
            regHKLM.SetValue("URL Protocol", "");
            RegistryKey regHKLMicon = regHKLM.CreateSubKey("DefaultIcon");
            string ex = Path.Combine( GetAppDir(), GetAppExe());
            regHKLMicon.SetValue("", ex);
            RegistryKey regShell = regHKLM.CreateSubKey("shell");
            RegistryKey regopen = regShell.CreateSubKey("open");
            RegistryKey regcom = regopen.CreateSubKey("command");
            regcom.SetValue("", ex + " %1");

            Console.WriteLine("Registered " + moniker + ":// url moniker.");
        }

        protected abstract string GetAppExe();

        protected abstract string GetAppDir();

        protected abstract void Register();

        protected string BuildLoginUriArg()
        {
            return " -loginuri " + loginUri;
        }

        protected abstract string BuildLoginArg();

        protected string BuildMultipleArg()
        {
            return " -multiple";
        }

        protected void InstallViewer()
        {
            //var downloadViewer = new DownloadViewer("http://instructions/");
            //downloadViewer.Show();

            //Action performStep = () => downloadViewer.Invoke(
            //                               delegate { 
            //                                   downloadViewer.progressBar1.PerformStep();
            //                               });

            DownloadViewer.InvokeDelegate performStep = delegate { };

            string path = Path.Combine( Path.GetTempPath(), "SL_Setup.exe");

            // string downloadUrl = "http://download-secondlife-com.s3.amazonaws.com/Second_Life_1-19-1-4_Setup.exe";

            string downloadUrl = "http://get.secondlife.com/";

            int bytes = DownloadFile(downloadUrl, path, performStep);

            // downloadViewer.Close();                                                               

            if (bytes > 0)
            {
                Process slInstallProcess = null;
                var slInstallPsi = new ProcessStartInfo();
                slInstallPsi.FileName = Path.GetFileName(path);
                slInstallPsi.WorkingDirectory = Path.GetDirectoryName(path);

                slInstallProcess = Process.Start(slInstallPsi);
                if (slInstallProcess != null)
                {
                    slInstallProcess.WaitForExit();
                }
            }
        }

        internal int DownloadFile(String remoteFilename,
                             String localFilename, DownloadViewer.InvokeDelegate performStep)
        {
            // Function will return the number of bytes processed
            // to the caller. Initialize to 0 here.
            var bytesProcessed = 0;

            // Assign values to these objects here so that they can
            // be referenced in the finally block
            Stream remoteStream = null;
            Stream localStream = null;
            WebResponse response = null;

            // Use a try/catch/finally block as both the WebRequest and Stream
            // classes throw exceptions upon error
            try
            {
                // Create a request for the specified remote file name
                var request = (HttpWebRequest)WebRequest.Create(remoteFilename);

                //request.AllowAutoRedirect = false;
                request.Referer = "http://www.sunet.se/";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 6.0)";

                // Send the request to the server and retrieve the
                // WebResponse object
                response = request.GetResponse();
                if (response != null)
                {
                    // Once the WebResponse object has been retrieved,
                    // get the stream object associated with the response's data
                    remoteStream = response.GetResponseStream();

                    // Create the local file
                    localStream = File.Create(localFilename);

                    // Allocate a 16k buffer
                    var buffer = new byte[16 * 1024];
                    int bytesRead;

                    // Simple do/while loop to read from stream until
                    // no bytes are returned
                    do
                    {
                        // Read data (up to 1k) from the stream
                        bytesRead = remoteStream.Read(buffer, 0, buffer.Length);

                        // Write the data to the local file
                        localStream.Write(buffer, 0, bytesRead);

                        // Increment total bytes processed
                        bytesProcessed += bytesRead;

                    } while ((bytesRead > 0) && (!CancelDownloadViewer));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (response != null) response.Close();
                if (remoteStream != null) remoteStream.Close();
                if (localStream != null) localStream.Close();
            }

            return bytesProcessed;
        }

        protected abstract bool CancelDownloadViewer { get; }
    }
}

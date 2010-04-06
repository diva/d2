using System;
using System.IO;
using System.Windows.Forms;

namespace Launcher
{
    class Program : ClientBase
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var program = new Program();            
        }

        protected override void NoViewerError()
        {
            if( DialogResult.Yes == MessageBox.Show("No compatible viewer installed. Do you want me to download and install the latest SL viewer?", "No Viewer Installed!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Stop))
            {
                InstallViewer();
            }            
        }

        protected override bool ProcessArgument(string arg, bool launch)
        {
            // hgtp://host:port/
            if (arg.ToLower().StartsWith("hgtp://"))
            {
                Uri hgtpUri = new Uri(arg);

                this.loginUri = "http://" + hgtpUri.GetComponents(UriComponents.HostAndPort | UriComponents.PathAndQuery, UriFormat.Unescaped);

                return true;
            }

            return false;
        }
     
        protected override string GetAppDir()
        {
            return Application.StartupPath;
        }

        protected override string GetAppExe()
        {
            return Path.GetFileName(Application.ExecutablePath);
        }
        protected override void Register()
        {
            this.RegisterMoniker("hgtp");
        }

        protected override string BuildLoginArg()
        {
            return String.Empty;
            // return string.Format(" -login {0} {1} {2}", "first", "last", "password");
        }

        protected override bool CancelDownloadViewer
        {
            get { return false; }
        }
    }
}

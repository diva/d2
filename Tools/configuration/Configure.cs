using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Text;
using OpenMetaverse;

namespace MetaverseInk.Configuration
{
    public class Configure
    {
        private static string worldName = "My World";
        private static string dbPasswd = "secret";
        private static string adminFirst = "Wifi";
        private static string adminLast = "Admin";
        private static string adminPasswd = "secret";
        private static string adminEmail = "admin@localhost";
        private static string ipAddress = "127.0.0.1";
        private static string platform = "1"; // 1 for .NET; 2 for mono
        private static int baseLocationX = 0, baseLocationY = 0;
        private static bool confirmationRequired = false;
        private static bool myWorldReconfig = false;
        private static string gmailAccount = string.Empty;
        private static string gmailPasswd = string.Empty;

        private enum RegionConfigStatus : uint
        {
            OK = 0,
            NeedsCreation = 1,
            NeedsEditing = 2
        }

        public static void Main(string[] args)
        {
            GetUserInput();
            ConfigureRegions();
            ConfigureMyWorld();
            Notify();
            DisplayInfo();
        }

        private static void GetUserInput()
        {
            Console.Write("Name of your world: ");
            worldName = Console.ReadLine();
            if (worldName == string.Empty)
                worldName = "My World";
            else
                worldName = worldName.Trim();

            Console.Write("MySql database password for opensim account: ");
            dbPasswd = Console.ReadLine();

            Console.Write("Your external domain name (preferred) or IP address: ");
            ipAddress = Console.ReadLine();
            if (ipAddress == string.Empty)
                ipAddress = "127.0.0.1";

            Console.Write("This installation is going to run on \n [1] .NET/Windows \n [2] *ix/Mono \nChoose 1 or 2 [1]: ");
            platform = Console.ReadLine();
            if (platform == string.Empty)
                platform = "1";
            platform = platform.Trim();

            Console.WriteLine("\nThe next questions are for configuring Wifi,\nthe web application where your users can register.\n");
            Console.Write("Wifi Admin first name [Wifi]: ");
            string input = Console.ReadLine();
            if (input != string.Empty)
                adminFirst = input;

            Console.Write("Wifi Admin last name [Admin]: ");
            input = Console.ReadLine();
            if (input != string.Empty)
                adminLast = input;

            Console.Write("Wifi Admin password [secret]: ");
            input = Console.ReadLine();
            if (input != string.Empty)
                adminPasswd = input;

            Console.Write("Wifi Admin email [admin@localhost]: ");
            input = Console.ReadLine();
            if (input != string.Empty)
                adminEmail = input;

            Console.Write("User account creation [o]pen or [c]ontrolled [o]: ");
            string conf = Console.ReadLine();
            if (conf != string.Empty && conf[0] == 'c')
                    confirmationRequired = true;

            Console.WriteLine("Wifi sends email notifications via gmail, by default.");
            Console.Write("Gmail user name [none]: ");
            input = Console.ReadLine();
            if (input != string.Empty)
                gmailAccount = input;
            
            Console.Write("Gmail password [none]: ");
            input = Console.ReadLine();
            if (input != string.Empty)
                gmailPasswd = input;

            Console.WriteLine("");
        }

        private static RegionConfigStatus CheckRegionConfig()
        {
            if (File.Exists("Regions/RegionConfig.ini"))
            {
                using (TextReader tr = new StreamReader("Regions/RegionConfig.ini"))
                {
                    string line;
                    while ((line = tr.ReadLine()) != null)
                    {
                        if (line.Contains("MasterAvatar"))
                            return RegionConfigStatus.NeedsEditing;
                    }
                }
                return RegionConfigStatus.OK;
            }

            return RegionConfigStatus.NeedsCreation;
        }

        private static void ConfigureRegions()
        {
            RegionConfigStatus status = CheckRegionConfig();

            if (status == RegionConfigStatus.OK)
            {
                Console.WriteLine("Your regions have been preserved."); 
                return;
            }

            if (status == RegionConfigStatus.NeedsEditing)
            {
                Console.WriteLine("*** Warning: Master Avatar is obsolete.\nPlease edit file Regions/RegionConfig.ini and delete all references to MasterAvatar.");
                return;
            }

            // else RegionConfigStatus.NeedsCreation
            int count = 0;
            try
            {
                using (TextReader tr = new StreamReader("Regions/RegionConfig.ini.example"))
                {
                    using (TextWriter tw = new StreamWriter("Regions/RegionConfig.ini"))
                    {
                        string line;
                        while ((line = tr.ReadLine()) != null)
                        {
                            if (line.Contains("My World"))
                                line = line.Replace("My World", worldName);
                            if (line.Contains("RegionUUID"))
                                line = "RegionUUID = \"" + UUID.Random() + "\"";
                            if (line.Contains("Location"))
                            {
                                if (count == 0)
                                {
                                    Random rand = new Random();
                                    baseLocationX = rand.Next(5000 - 2048, 5000 + 2048);
                                    baseLocationY = rand.Next(5000 - 2048, 5000 + 2048);
                                    line = "Location = \"" + baseLocationX + "," + baseLocationY + "\"";
                                }
                                else if (count == 1)
                                    line = "Location = \"" + baseLocationX + "," + (baseLocationY + 1) + "\"";
                                else if (count == 2)
                                    line = "Location = \"" + (baseLocationX + 1) + "," + baseLocationY + "\"";
                                else if (count == 3)
                                    line = "Location = \"" + (baseLocationX + 1) + "," + (baseLocationY + 1) + "\"";
                                count++;
                            }
                            if (line.Contains("SYSTEMIP"))
                                line = line.Replace("SYSTEMIP", ipAddress);
                            tw.WriteLine(line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error configuring RegionConfig " + e.Message);
                return;
            }
            Console.WriteLine("Your regions have been successfully configured.");
        }

        private static void CheckMyWorldConfig()
        {
            if (File.Exists("config-include/MyWorld.ini"))
            {
                try
                {
                    File.Move("config-include/MyWorld.ini", "config-include/MyWorld.ini.old");
                }
                catch
                {
                    // ignore and proceed
                }

                myWorldReconfig = true;
            }
        }

        private static void ConfigureMyWorld()
        {
            CheckMyWorldConfig();

            try
            {
                using (TextReader tr = new StreamReader("config-include/MyWorld.ini.example"))
                {
                    using (TextWriter tw = new StreamWriter("config-include/MyWorld.ini"))
                    {
                        string line;
                        while ((line = tr.ReadLine()) != null)
                        {
                            if (line.Contains("Password"))
                                line = line.Replace("***", dbPasswd);
                            if (line.Contains("127.0.0.1"))
                                line = line.Replace("127.0.0.1", ipAddress);
                            if (line.Contains("async_call_method") && platform.Equals("1"))
                                line = line.Replace("SmartThreadPool", "UnsafeQueueUserWorkItem");
                            if (line.Contains("use_async_when_possible") && platform.Equals("1"))
                                line = line.Replace("false", "true");
                            if (line.Contains("welcome_message"))
                                line = line.Replace("Your World", worldName);
                            if (line.Contains("DefaultRegion"))
                            {
                                string defRegionName = "Region_" + worldName.Replace(' ', '_') + "_1";
                                line = line.Replace("Region_My_World_1", defRegionName);
                            }
                            if (line.Contains("gridname") || line.Contains("GridName"))
                                line = line.Replace("My World", worldName);
                            if (line.Contains("gridnick"))
                                line = line.Replace("hippogrid", worldName.ToLower().Replace(' ', '_'));
                            if (line.Contains("WelcomeMessage"))
                                line = line.Replace("Welcome!", "Welcome to " + worldName + "!");
                            if (line.Contains("AccountConfirmationRequired") && confirmationRequired)
                                line = line.Replace("false", "true");

                            if (line.Contains("AdminFirst"))
                                line = line.Replace("Wifi", adminFirst);
                            if (line.Contains("AdminLast"))
                                line = line.Replace("Administrator", adminLast);
                            if (line.Contains("AdminPassword"))
                                line = line.Replace("secret", adminPasswd);
                            if (line.Contains("AdminEmail"))
                                line = line.Replace("admin@localhost", adminEmail);

                            if (line.Contains("SmtpUsername"))
                                line = line.Replace("your_email", gmailAccount);
                            if (line.Contains("SmtpPassword"))
                                line = line.Replace("secret", gmailPasswd);

                            tw.WriteLine(line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error configuring MyWorld " + e.Message);
                return;
            }
            Console.WriteLine("Your World has been successfully configured");
        }

        private static void Notify()
        {
            WebRequest webReq = HttpWebRequest.Create("http://metaverseink.com/cgi-bin/divadistribution.py?host=" + ipAddress);
            webReq.Method = "GET";

            HttpWebResponse webResp;
            try
            {
                webResp = (HttpWebResponse)webReq.GetResponse();
            }
            catch 
            {
                return ;
            }

            if (webResp.StatusCode != HttpStatusCode.OK)
            {
                return ;
            }

            Stream receiveStream = webResp.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string response = readStream.ReadToEnd();

            if (webResp != null) webResp.Close();
            if (receiveStream != null) receiveStream.Close();
            if (readStream != null) readStream.Close();

        }

        private static void DisplayInfo()
        {
            Console.WriteLine("\n***************************************************");
            Console.WriteLine("Your world is " + worldName);
            Console.WriteLine("Your loginuri is http://" + ipAddress + ":9000");
            Console.WriteLine("Your Wifi app is http://" + ipAddress + ":9000/wifi");
            Console.WriteLine("You admin account for Wifi is:");
            Console.WriteLine("  username: " + adminFirst + " " + adminLast);
            Console.WriteLine("  passwd:   " + adminPasswd +"\n");
            if (gmailAccount == string.Empty)
                Console.WriteLine("Remember to set the Smtp Account for email notifications");
            else
                Console.WriteLine("Your users get email notifications from " + gmailAccount + "@gmail.com");
            if (myWorldReconfig)
                Console.WriteLine("\nNOTE: config-include/MyWorld.ini has been reconfigured.\nPrevious configuration: config-include/MyWorld.ini.old.\nPlease revise the new configuration.\n");
            else
                Console.WriteLine("Your world's configuration is config-include/MyWorld.ini.\nPlease revise it.");
            Console.WriteLine("***************************************************\n");
            Console.Write("<Press enter to exit>");
            Console.ReadLine();
        }
    }
}

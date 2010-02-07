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
        private static string masterPasswd = "secret";
        private static string ipAddress = "127.0.0.1";
        private static string platform = "1"; // 1 for .NET; 2 for mono
        private static int baseLocationX = 0, baseLocationY = 0;

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

            Console.Write("MySql database password for opensim account: ");
            dbPasswd = Console.ReadLine();

            Console.Write("Master Avatar password (default " + dbPasswd + "): ");
            masterPasswd = Console.ReadLine();
            if (masterPasswd == string.Empty)
                masterPasswd = dbPasswd;

            Console.Write("Your IP address or domain name (default 127.0.0.1 not reacheable from outside): ");
            ipAddress = Console.ReadLine();
            if (ipAddress == string.Empty)
                ipAddress = "127.0.0.1";

            Console.Write("This installation is going to run on \n (1) .NET/Windows \n (2) *ix/Mono \nType 1 or 2 (default 1): ");
            platform = Console.ReadLine();
            if (platform == string.Empty)
                platform = "1";
            platform = platform.Trim();

        }

        private static void ConfigureRegions()
        {
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
                            if (line.Contains("Password"))
                                line = line.Replace("***", masterPasswd);
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
            Console.WriteLine("Your regions have been successfully configured");
        }

        private static void ConfigureMyWorld()
        {
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
                            if (line.Contains("default_location_x"))
                                line = line.Replace("5000", baseLocationX.ToString());
                            if (line.Contains("default_location_y"))
                                line = line.Replace("5000", baseLocationY.ToString());
                            if (line.Contains("async_call_method") && platform.Equals("1"))
                                line = line.Replace("SmartThreadPool", "UnsafeQueueUserWorkItem");
                            if (line.Contains("use_async_when_possible") && platform.Equals("1"))
                                line = line.Replace("false", "true");
                            if (line.Contains("welcome_message"))
                                line = line.Replace("Your World", worldName);

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
            Console.WriteLine("The owner/god account is Master Avatar with password " + masterPasswd);
            Console.WriteLine("Your loginuri is http://" + ipAddress + ":9000");
            Console.WriteLine("***************************************************\n");
            Console.Write("<Press enter to exit>");
            Console.ReadLine();
        }
    }
}

using Leaf.xNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using Console = Colorful.Console;

namespace UrlTok_Fixed
{
    internal class Functions
    {
        public static void Discord()
        {
            ConsoleKey keyInfo;

            Console.WriteLine("Make sure to join the Discord server for help/inquiries and updates.");
            Console.WriteLine("Either press \"D\" on your keyboard or copy paste this url 'https://discord.gg/S2G6QkZRqM'");
            Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100);

            do
            {
                keyInfo = Console.ReadKey(true).Key;

                switch (keyInfo)
                {
                    case ConsoleKey.D:
                        System.Diagnostics.Process.Start("https://discord.gg/S2G6QkZRqM");
                        break;

                    case ConsoleKey.Escape:
                        Console.Clear();
                        Interface.Menu();
                        Interface.Options();
                        break;

                    default:
                        Console.Write("Press ESC to return to the menu!\r");
                        break;
                }
            } while (keyInfo != ConsoleKey.Escape);
        }

        public static void Download(string rawUrl, string authorID, string userId, string tdir, string fdir, string dlbool)
        {
            using (var Client = new WebClient())
            {
                try
                {
                    if (dlbool == "true")
                    {
                        Client.DownloadFile(rawUrl, tdir);
                    }
                    else if (dlbool == "false")
                    {
                        Client.DownloadFile(rawUrl, fdir);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Console.WriteLine($"Done downloading video: {userId} by {authorID}");
                return;
            }
        }

        public static void downloadManually()
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Manually"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Manually");

            string dirDate = DateTime.Now.ToString("MM-dd-yyyy");

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + $"\\Manually\\{dirDate}"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + $"\\Manually\\{dirDate}");

            dynamic configFile = JsonConvert.DeserializeObject(File.ReadAllText("ttConfig.json"));

            string boolNewDir = configFile["directory_create"];
            string boolLogTxt = configFile["save_raw"];
            string showStats = configFile["show_stats"];
            boolNewDir = boolNewDir.ToLower();
            boolLogTxt = boolLogTxt.ToLower();
            showStats = showStats.ToLower();

            if (boolNewDir == "true")
            {
                Console.WriteLine("Creating folders foreach creator is turned ON");
            }
            else
            {
                Console.WriteLine("Creating folders foreach creator is turned OFF.");
            };

            if (boolLogTxt == "true")
            {
                Console.WriteLine("Saving raw download urls into txt is turned ON");
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + $"\\Manually\\{dirDate}\\{dirDate}-raw.txt"))
                {
                    var x = File.Create(AppDomain.CurrentDomain.BaseDirectory + $"\\Manually\\{dirDate}\\{dirDate}-raw.txt");
                    x.Close();
                }
            }
            else
            {
                Console.WriteLine("Saving raw download urls into txt is turned OFF");
            }

            if (showStats == "true")
            {
                Console.WriteLine("Showing statistics every video is turned ON");
            }
            else
            {
                Console.WriteLine("Showing statistics every video is turned OFF");
            }

            using (HttpRequest hr = new HttpRequest())
            {
                Console.Write("Insert video url/id: ");
                string videoUrl = Console.ReadLine();
                if (showStats == "true") { Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100); }
                string jsonResults = string.Empty;

                // Split video ID here..

                hr.AllowAutoRedirect = false;
                hr.Proxy = null;
                hr.UserAgentRandomize();

                // Make payload
                string payLoad = string.Concat(new string[]
                {
                    "url=",
                    videoUrl
                });
                // Current Api
                string currAPI()
                {
                    return "https://api.tikmate.app/api/lookup";
                }

                try
                {
                    jsonResults = hr.Post(currAPI(), payLoad, "application/x-www-form-urlencoded; charset=UTF-8").ToString();
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                    if (error.Contains("403"))
                    {
                        Console.WriteLine("[!] Error 403 Forbidden, retrying this request.");

                        jsonResults = hr.Post(currAPI(), payLoad, "application/x-www-form-urlencoded; charset=UTF-8").ToString();
                    }
                }

                //Console.WriteLine(jsonResults); => Log Complete json

                try
                {
                    JObject jobj = JObject.Parse(jsonResults);
                    string downloadToken = jobj.SelectToken("token").ToString(); // EX: qsGP2O5XfbxKOD0XwZoMz7jaCu2emlAtYgDC7YJnGsnintJ9OsgoR2-8ejiTcYU4T32FOgxCOcyjWgPJ
                    string userID = jobj.SelectToken("id").ToString(); // EX: 7102114283504389382
                    string commentCount = jobj.SelectToken("comment_count").ToString(); // EX: 136
                    string creationDate = jobj.SelectToken("create_time").ToString(); // EX: May 27, 2022
                    string authorID = jobj.SelectToken("author_id").ToString(); // EX: rezpects
                    string shareCount = jobj.SelectToken("share_count").ToString(); // EX: 747
                    string likeCount = jobj.SelectToken("like_count").ToString(); // EX: 87362

                    string downloadVideo = string.Concat(new string[]
                    {
                        "https://tikmate.app/download/",
                        downloadToken,
                        "/",
                        userID,
                        ".mp4?hd=1"
                    });

                    if (showStats == "true") // Config: Show stats
                    {
                        Console.WriteLine($"Token: {downloadToken}");
                        Console.WriteLine($"ID: {userID}");
                        Console.WriteLine($"Comments: {commentCount}");
                        Console.WriteLine($"Author: {authorID}");
                        Console.WriteLine($"Shares: {shareCount}");
                        Console.WriteLine($"Likes: {likeCount}");
                    }

                    string trueDownloadLOC = string.Empty;
                    string falseDownloadLOC = string.Empty;

                    if (boolNewDir == "true") // Config: New directory
                    {
                        trueDownloadLOC = AppDomain.CurrentDomain.BaseDirectory + $"\\Manually\\{dirDate}\\{authorID}\\{authorID}-{userID}.mp4".ToString();
                        if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + $"\\Manually\\{dirDate}\\{authorID}"))
                            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + $"\\Manually\\{dirDate}\\{authorID}");
                    }
                    else
                    {
                        falseDownloadLOC = AppDomain.CurrentDomain.BaseDirectory + $"\\Manually\\{dirDate}\\{authorID}-{userID}.mp4".ToString();
                    }

                    if (boolLogTxt == "true")
                    {
                        //File.WriteAllText(downloadVideo, AppDomain.CurrentDomain.BaseDirectory + $"\\Manually\\{dirDate}\\{dirDate}-raw.txt"); // Write raw download into txt
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + $"\\Manually\\{dirDate}\\{dirDate}-raw.txt",
                            downloadVideo + Environment.NewLine);
                    }

                    Download(downloadVideo, authorID, userID, trueDownloadLOC, falseDownloadLOC, boolNewDir); // Redirect void to download
                    Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100);
                    Keyreader.escapeKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.ToString()}");
                }
            }
        }

        public static void Github()
        {
            ConsoleKey keyInfo;
            Console.WriteLine("Make sure to star the project & for issues open an issue or join the Discord server.");
            Console.WriteLine("Either press \"G\" on your keyboard or copy paste this url 'https://github.com/Sat178/UrlTok'");
            Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100);

            do
            {
                keyInfo = Console.ReadKey(true).Key;

                switch (keyInfo)
                {
                    case ConsoleKey.G:
                        System.Diagnostics.Process.Start("https://github.com/Sat178/UrlTok");
                        break;

                    case ConsoleKey.Escape:
                        Console.Clear();
                        Interface.Menu();
                        Interface.Options();
                        break;

                    default:
                        Console.Write("Press ESC to return to the menu!\r");
                        break;
                }
            } while (keyInfo != ConsoleKey.Escape);
        }

        public static void ProxyScraper()
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Proxies"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Proxies");

            string[] dirNames = { "Https", "Socks4", "Socks5", "All" };

            foreach (string obj in dirNames)
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{obj}"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{obj}");
                }
            }

            // Https/Socks4/Socks5/All

            dynamic configFile = JsonConvert.DeserializeObject(File.ReadAllText("ttConfig.json"));
            string proxyType = configFile["proxies"];
            Console.WriteLine($"Going to scrape {proxyType} proxies!\n");

            // Open web

            WebClient wc = new WebClient();

            //Api Arrays

            List<string> rawProxies = new List<string>();
            List<string> FixedProxies = new List<string>();
            List<string> FinalProxies = new List<string>();

            int apiCounter = 0;

            string[] httpsAPI = {
                "https://api.proxyscrape.com/v2/?request=displayproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all",
                "https://www.proxy-list.download/api/v1/get?type=http",
                "https://raw.githubusercontent.com/ShiftyTR/Proxy-List/master/http.txt",
                "https://raw.githubusercontent.com/ShiftyTR/Proxy-List/master/https.txt"};

            string[] socks4API = {
                "https://api.proxyscrape.com/v2/?request=displayproxies&protocol=socks4",
                "https://www.proxy-list.download/api/v1/get?type=socks4",
                "https://raw.githubusercontent.com/ShiftyTR/Proxy-List/master/socks4.txt" };

            string[] socks5API = {
                "https://api.proxyscrape.com/v2/?request=displayproxies&protocol=socks5",
                "https://www.proxy-list.download/api/v1/get?type=socks5",
                "https://raw.githubusercontent.com/ShiftyTR/Proxy-List/master/socks5.txt" };

            string[] allAPI = {
                "https://api.proxyscrape.com/v2/?request=displayproxies&protocol=socks4",
                "https://www.proxy-list.download/api/v1/get?type=socks4",
                "https://raw.githubusercontent.com/ShiftyTR/Proxy-List/master/socks4.txt",
                "https://api.proxyscrape.com/v2/?request=displayproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all",
                "https://www.proxy-list.download/api/v1/get?type=http",
                "https://raw.githubusercontent.com/ShiftyTR/Proxy-List/master/http.txt",
                "https://raw.githubusercontent.com/ShiftyTR/Proxy-List/master/https.txt",
                "https://api.proxyscrape.com/v2/?request=displayproxies&protocol=socks4",
                "https://www.proxy-list.download/api/v1/get?type=socks4",
                "https://raw.githubusercontent.com/ShiftyTR/Proxy-List/master/socks4.txt" };

            switch (proxyType)
            {
                case "Https":

                    string timeStamp = DateTime.Now.ToString("HH-mm-ss");
                    var tempHttp = File.Create(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\Https-temp.txt"); // Create temporary file.
                    tempHttp.Close(); // Close file.

                    foreach (string api in httpsAPI) // Foreach api in array
                    {
                        try
                        {
                            apiCounter++;
                            var scrapedProxy = wc.DownloadString(api);
                            //Console.WriteLine(scrapedProxy);
                            //Console.Write($"{scrapedProxy}\r");
                            Console.WriteLine($"Scraped API #{apiCounter}");
                            scrapedProxy.Replace(" ", string.Empty);
                            rawProxies.Add(scrapedProxy);
                        }
                        catch (Exception ex)
                        {
                            string error = ex.ToString();
                            if (error.Contains("404")) ;
                            Console.WriteLine($"404 Page not found, Dead API. (Api #{apiCounter})", Color.Red);
                            continue;
                        }
                    }

                    File.WriteAllLines("Https-temp.txt", rawProxies);
                    string[] dupsProxies = File.ReadAllLines("Https-temp.txt");
                    List<string> dupsFinalProxies = dupsProxies.ToList();
                    dupsFinalProxies.Sort();
                    dupsFinalProxies.Distinct();
                    foreach (string dup in dupsFinalProxies.ToList())
                    {
                        if (dup == "")
                            dupsFinalProxies.Remove(dup);
                    }

                    Console.WriteLine($"Done scraping {dupsFinalProxies.Count} {configFile["proxies"]} proxies!", Color.Red);
                    File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\Https-temp.txt", dupsFinalProxies);
                    File.Move(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\Https-temp.txt", AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\{dupsFinalProxies.Count}-{configFile["proxies"]}-{timeStamp}.txt");
                    Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100);
                    Keyreader.escapeKey();

                    break;

                case "Socks4":

                    string s4timeStamp = DateTime.Now.ToString("HH-mm-ss");
                    var s4tempHttp = File.Create(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\Socks4-temp.txt"); // Create temporary file.
                    s4tempHttp.Close(); // Close file.

                    foreach (string api in socks4API) // Foreach api in array
                    {
                        try
                        {
                            apiCounter++;
                            var scrapedProxy = wc.DownloadString(api);
                            //Console.WriteLine(scrapedProxy);
                            //Console.Write($"{scrapedProxy}\r");
                            Console.WriteLine($"Scraped API #{apiCounter}");
                            scrapedProxy.Replace(" ", string.Empty);
                            rawProxies.Add(scrapedProxy);
                        }
                        catch (Exception ex)
                        {
                            string error = ex.ToString();
                            if (error.Contains("404")) ;
                            Console.WriteLine($"404 Page not found, Dead API. (Api #{apiCounter})", Color.Red);
                            continue;
                        }
                    }

                    File.WriteAllLines("Socks4-temp.txt", rawProxies);
                    string[] s4dupsProxies = File.ReadAllLines("Socks4-temp.txt");
                    List<string> s4dupsFinalProxies = s4dupsProxies.ToList();
                    s4dupsFinalProxies.Sort();
                    s4dupsFinalProxies.Distinct();
                    foreach (string dup in s4dupsFinalProxies.ToList())
                    {
                        if (dup == "")
                            s4dupsFinalProxies.Remove(dup);
                    }

                    Console.WriteLine($"Done scraping {s4dupsFinalProxies.Count} {configFile["proxies"]} proxies!", Color.Red);
                    File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\Socks4-temp.txt", s4dupsFinalProxies);
                    File.Move(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\Socks4-temp.txt", AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\{s4dupsFinalProxies.Count}-{configFile["proxies"]}-{s4timeStamp}.txt");
                    Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100);
                    Keyreader.escapeKey();

                    break;

                case "Socks5":

                    string s5timeStamp = DateTime.Now.ToString("HH-mm-ss");
                    var s5tempHttp = File.Create(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\Socks5-temp.txt"); // Create temporary file.
                    s5tempHttp.Close(); // Close file.

                    foreach (string api in socks5API) // Foreach api in array
                    {
                        try
                        {
                            apiCounter++;
                            var scrapedProxy = wc.DownloadString(api);
                            //Console.WriteLine(scrapedProxy);
                            //Console.Write($"{scrapedProxy}\r");
                            Console.WriteLine($"Scraped API #{apiCounter}");
                            scrapedProxy.Replace(" ", string.Empty);
                            rawProxies.Add(scrapedProxy);
                        }
                        catch (Exception ex)
                        {
                            string error = ex.ToString();
                            if (error.Contains("404")) ;
                            Console.WriteLine($"404 Page not found, Dead API. (Api #{apiCounter})", Color.Red);
                            continue;
                        }
                    }

                    File.WriteAllLines("Socks5-temp.txt", rawProxies);
                    string[] s5dupsProxies = File.ReadAllLines("Socks5-temp.txt");
                    List<string> s5dupsFinalProxies = s5dupsProxies.ToList();
                    s5dupsFinalProxies.Sort();
                    s5dupsFinalProxies.Distinct();
                    foreach (string dup in s5dupsFinalProxies.ToList())
                    {
                        if (dup == "")
                            s5dupsFinalProxies.Remove(dup);
                    }

                    Console.WriteLine($"Done scraping {s5dupsFinalProxies.Count} {configFile["proxies"]} proxies!", Color.Red);
                    File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\Socks5-temp.txt", s5dupsFinalProxies);
                    File.Move(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\Socks5-temp.txt", AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\{s5dupsFinalProxies.Count}-{configFile["proxies"]}-{s5timeStamp}.txt");
                    Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100);
                    Keyreader.escapeKey();

                    break;

                case "All":

                    string alltimeStamp = DateTime.Now.ToString("HH-mm-ss");
                    var alltempHttp = File.Create(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\Socks5-temp.txt"); // Create temporary file.
                    alltempHttp.Close(); // Close file.

                    foreach (string api in allAPI) // Foreach api in array
                    {
                        try
                        {
                            apiCounter++;
                            var scrapedProxy = wc.DownloadString(api);
                            //Console.WriteLine(scrapedProxy);
                            //Console.Write($"{scrapedProxy}\r");
                            Console.WriteLine($"Scraped API #{apiCounter}");
                            scrapedProxy.Replace(" ", string.Empty);
                            rawProxies.Add(scrapedProxy);
                        }
                        catch (Exception ex)
                        {
                            string error = ex.ToString();
                            if (error.Contains("404")) ;
                            Console.WriteLine($"404 Page not found, Dead API. (Api #{apiCounter})", Color.Red);
                            continue;
                        }
                    }

                    File.WriteAllLines("All-temp.txt", rawProxies);
                    string[] alldupsProxies = File.ReadAllLines("All-temp.txt");
                    List<string> alldupsFinalProxies = alldupsProxies.ToList();
                    alldupsFinalProxies.Sort();
                    alldupsFinalProxies.Distinct();
                    foreach (string dup in alldupsFinalProxies.ToList())
                    {
                        if (dup == "")
                            alldupsFinalProxies.Remove(dup);
                    }

                    Console.WriteLine($"Done scraping {alldupsFinalProxies.Count} {configFile["proxies"]} proxies!", Color.Red);
                    File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\All-temp.txt", alldupsFinalProxies);
                    File.Move(AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\All-temp.txt", AppDomain.CurrentDomain.BaseDirectory + $"\\Proxies\\{proxyType}\\{alldupsFinalProxies.Count}-{configFile["proxies"]}-{alltimeStamp}.txt");
                    Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100);
                    Keyreader.escapeKey();
                    break;
            }
        }
    }
}
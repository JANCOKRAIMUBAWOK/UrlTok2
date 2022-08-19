﻿using Leaf.xNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Console = Colorful.Console;

namespace UrlTok_Fixed
{
    internal class UrlTok
    {
        public static class Vars
        {
            public static List<string> dupedTikTokUrls = new List<string>();
            public static List<string> cleanedTikTokUrls = new List<string>();
            public static int qCounter = 0;
            public static string plRequest = string.Empty;
            public static string downloadToken = string.Empty;
            public static string userID = string.Empty;
            public static string commentCount = string.Empty;
            public static string creationDate = string.Empty;
            public static string authorID = string.Empty;
            public static string shareCount = string.Empty;
            public static string likeCount = string.Empty;
            public static string boolNewDir = string.Empty;
            public static string boolLogTxt = string.Empty;
            public static string showStats = string.Empty;
            public static string dirDate = string.Empty;
            public static string trueDownloadLOC = string.Empty;
            public static string falseDownloadLOC = string.Empty;
            public static string downloadVideo = string.Empty;
            public static int progressCount = 0;
            public static int ADM = 0;
            public static int Errors = 0;
            public static int Aux_ADM = 0;
        }

        public static void DPM_Calculator()
        {
            Task.Factory.StartNew(delegate ()
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                while (true)
                {
                    Console.Title = string.Format("UrlTok - Downloaded: {0}/{1} - Errors: {2} - Average downloads per minute: {3}", new object[]
                    {
                        Vars.progressCount,
                        Vars.cleanedTikTokUrls.Count<String>(),
                        Vars.Errors,
                        Vars.Aux_ADM= Vars.ADM * 60,
                    });
                    Vars.Aux_ADM = 0;
                    Vars.ADM = 0;
                    Thread.Sleep(1000);
                }
            });
        }

        public static void Loader()
        {
            DPM_Calculator();
            Vars.dirDate = DateTime.Now.ToString("MM-dd-yyyy");

            dynamic configFile = JsonConvert.DeserializeObject(File.ReadAllText("ttConfig.json"));
            Vars.boolNewDir = configFile["directory_create"];
            Vars.boolLogTxt = configFile["save_raw"];
            Vars.showStats = configFile["show_stats"];
            Vars.boolNewDir = Vars.boolNewDir.ToLower();
            Vars.boolLogTxt = Vars.boolLogTxt.ToLower();
            Vars.showStats = Vars.showStats.ToLower();

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + $"\\Automatically\\{Vars.dirDate}"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + $"\\Automatically\\{Vars.dirDate}");

            Console.WriteLine("Open your file including the urls!");
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    var Urlqueue = File.ReadAllLines(filePath);

                    foreach (string url in Urlqueue) { Vars.dupedTikTokUrls.Add(url); }
                    Vars.cleanedTikTokUrls = Vars.dupedTikTokUrls.Distinct().ToList();
                    Console.WriteLine($"Found {Vars.cleanedTikTokUrls.Count<string>()} valid TikTok urls.");
                    Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100);
                    foreach (string url in Vars.cleanedTikTokUrls.ToList())
                    {
                        Vars.qCounter++;
                        Downloader(url, Vars.qCounter);
                    }
                }
            }
        }

        public static void Downloader(string downloadurl, int queueNumber)
        {
            dynamic configFile = JsonConvert.DeserializeObject(File.ReadAllText("ttConfig.json"));
            using (HttpRequest hr = new HttpRequest())
            {
                hr.AllowAutoRedirect = false;
                hr.Proxy = null;
                hr.UserAgentRandomize(); // Avoids status code 403

                string payLoad = string.Concat(new string[]
                {
                    "url=",
                    downloadurl
                });

                string currAPI()
                {
                    return "https://api.tikmate.app/api/lookup";
                }

                try
                {
                    Vars.plRequest = hr.Post(currAPI(), payLoad, "application/x-www-form-urlencoded; charset=UTF-8").ToString();
                }
                catch (Exception ex)
                {
                    Vars.Errors++;
                    string error = ex.ToString();
                    if (error.Contains("403"))
                    {
                        Console.WriteLine("[!] Error 403 Forbidden, retrying this request.");
                        Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100);

                        // Adding back to begin of list!!
                        Vars.cleanedTikTokUrls.Insert(0, downloadurl);
                        Downloader(downloadurl, queueNumber);
                    }
                    else if (error.Contains("404"))
                    {
                        Console.WriteLine("[!] Error 404 Not found, skipping this request.");
                        Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100);
                        Vars.cleanedTikTokUrls.Remove(downloadurl);
                        return; // Skipping this url
                        
                    }
                    else
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                try
                {
                    JObject jobj = JObject.Parse(Vars.plRequest);

                    Vars.downloadToken = jobj.SelectToken("token").ToString(); // EX: qsGP2O5XfbxKOD0XwZoMz7jaCu2emlAtYgDC7YJnGsnintJ9OsgoR2-8ejiTcYU4T32FOgxCOcyjWgPJ
                    Vars.userID = jobj.SelectToken("id").ToString(); // EX: 7102114283504389382
                    Vars.commentCount = jobj.SelectToken("comment_count").ToString(); // EX: 136
                    Vars.creationDate = jobj.SelectToken("create_time").ToString(); // EX: May 27, 2022
                    Vars.authorID = jobj.SelectToken("author_id").ToString(); // EX: rezpects
                    Vars.shareCount = jobj.SelectToken("share_count").ToString(); // EX: 747
                    Vars.likeCount = jobj.SelectToken("like_count").ToString(); // EX: 87362

                    Vars.downloadVideo = string.Concat(new string[]
                    {
                        "https://tikmate.app/download/",
                        Vars.downloadToken,
                        "/",
                        Vars.userID,
                        ".mp4?hd=1"
                    });

                    if (Vars.showStats == "true") // Config: Show stats
                    {
                        Console.WriteLine($"Token: {Vars.downloadToken}");
                        Console.WriteLine($"ID: {Vars.userID}");
                        Console.WriteLine($"Comments: {Vars.commentCount}");
                        Console.WriteLine($"Author: {Vars.authorID}");
                        Console.WriteLine($"Shares: {Vars.shareCount}");
                        Console.WriteLine($"Likes: {Vars.likeCount}");
                        Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100);
                    }

                    Vars.trueDownloadLOC = string.Empty;
                    Vars.falseDownloadLOC = string.Empty;

                    if (Vars.boolNewDir == "true") // Config: New directory
                    {
                        Vars.trueDownloadLOC = AppDomain.CurrentDomain.BaseDirectory + $"\\Automatically\\{Vars.dirDate}\\{Vars.authorID}\\{Vars.authorID}-{Vars.userID}.mp4".ToString();
                        if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + $"\\Automatically\\{Vars.dirDate}\\{Vars.authorID}"))
                            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + $"\\Automatically\\{Vars.dirDate}\\{Vars.authorID}");
                        Task.Run(() => Webc(Vars.downloadVideo, Vars.trueDownloadLOC, Vars.downloadToken));
                    }
                    else
                    {
                        Vars.falseDownloadLOC = AppDomain.CurrentDomain.BaseDirectory + $"\\Automatically\\{Vars.dirDate}\\{Vars.authorID}-{Vars.userID}.mp4".ToString();
                        Task.Run(() => Webc(Vars.downloadVideo, Vars.trueDownloadLOC, Vars.downloadToken));
                    }

                    if (Vars.boolLogTxt == "true")
                    {
                        //File.WriteAllText(downloadVideo, AppDomain.CurrentDomain.BaseDirectory + $"\\Manually\\{dirDate}\\{dirDate}-raw.txt"); // Write raw download into txt
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + $"\\Automatically\\{Vars.dirDate}\\{Vars.dirDate}-raw.txt",
                            Vars.downloadVideo + Environment.NewLine);
                    }
                }
                catch (Exception ex)
                {
                    Vars.Errors++;
                    Console.WriteLine($"{ex.ToString()}");
                }
            }
        }

        public static async void Webc(string dlurl, string titleName, string token) // downloadVideo, Vars.trueDownloadLOC => queueNumber
        {
            using (WebClient wc = new WebClient())
            {
                var fileName = titleName;

                if (Vars.boolNewDir == "true")
                {
                    wc.DownloadFile(dlurl, fileName);
                }
                else
                {
                    wc.DownloadFile(dlurl, AppDomain.CurrentDomain.BaseDirectory + $"\\Automatically\\{Vars.dirDate}\\{Vars.authorID}-{Vars.userID}.mp4");
                }

                wc.Dispose();
                Interlocked.Increment(ref Vars.progressCount);
                Interlocked.Increment(ref Vars.ADM);
                Console.WriteLine($"Finished downloading Token: {token}!\nProgress: {Vars.progressCount}/{Vars.cleanedTikTokUrls.Count<string>()}");
                Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100);
                if (Vars.progressCount == Vars.cleanedTikTokUrls.Count<string>())
                {
                    Console.WriteLine($"Finished downloading ({Vars.cleanedTikTokUrls.Count<string>()}) all videos!");
                }
                return;
            }
        }
    }
}
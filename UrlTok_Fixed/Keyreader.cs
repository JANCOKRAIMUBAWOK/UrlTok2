using System;

namespace UrlTok_Fixed
{
    internal class Keyreader
    {
        public static void KeyReader()
        {
            ConsoleKey keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true).Key;

                switch (keyInfo)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        Interface.Menu();
                        Console.Title = "UrlTok - Viewing Config - v2.0.0";
                        Config.createConfig();
                        break;

                    case ConsoleKey.D2:
                        Console.Clear();
                        Interface.Menu();
                        Console.Title = "UrlTok - Scraping Proxies - v2.0.0";
                        Functions.ProxyScraper();
                        break;

                    case ConsoleKey.D3:
                        Console.Clear();
                        Interface.Menu();
                        Console.Title = "UrlTok - Downloading Manually- v2.0.0";
                        Functions.downloadManually();
                        break;

                    case ConsoleKey.D4:
                        Console.Clear();
                        Interface.Menu();
                        Console.Title = "UrlTok - Downloading Automatically - v2.0.0";
                        UrlTok.Loader();
                        break;

                    case ConsoleKey.D5:
                        Console.Clear();
                        Interface.Menu();
                        Console.Title = "UrlTok - Discord Invite - v2.0.0";
                        Functions.Discord();
                        break;

                    case ConsoleKey.D6:
                        Console.Clear();
                        Interface.Menu();
                        Console.Title = "UrlTok - Github Repository - v2.0.0";
                        Functions.Github();
                        break;

                    default:
                        break;
                }
            }
            while (keyInfo != ConsoleKey.D1 && keyInfo != ConsoleKey.D2 && keyInfo != ConsoleKey.D3 && keyInfo != ConsoleKey.D4 && keyInfo != ConsoleKey.D5 && keyInfo != ConsoleKey.D6);
        }

        public static void escapeKey()
        {
            ConsoleKey keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true).Key;

                switch (keyInfo)
                {
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
    }
}
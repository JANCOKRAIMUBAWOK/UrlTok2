using System.Drawing;
using Console = Colorful.Console;

namespace UrlTok_Fixed
{
    internal class Interface
    {
        public static class menuStrings
        {
            public static string firstLine = "\n\t\t\t\t /$$   /$$           /$$ /$$$$$$$$        /$$      \n";
            public static string secondLine = "\t\t\t\t| $$  | $$          | $$|__  $$__/       | $$      \n";
            public static string thirdLine = "\t\t\t\t| $$  | $$  /$$$$$$ | $$   | $$  /$$$$$$ | $$   /$$\n";
            public static string fourthLine = "\t\t\t\t| $$  | $$ /$$__  $$| $$   | $$ /$$__  $$| $$  /$$/\n";
            public static string fifthLine = "\t\t\t\t| $$  | $$| $$  \\__/| $$   | $$| $$  \\ $$| $$$$$$/ \n";
            public static string sixthLine = "\t\t\t\t| $$  | $$| $$      | $$   | $$| $$  | $$| $$_  $$ \n";
            public static string seventhLine = "\t\t\t\t| $$  | $$| $$      | $$   | $$| $$  | $$| $$_  $$ \n";
            public static string eigthLine = "\t\t\t\t|  $$$$$$/| $$      | $$   | $$|  $$$$$$/| $$ \\  $$\n";
            public static string ninethLine = "\t\t\t\t \\______/ |__/      |__/   |__/ \\______/ |__/  \\__/\n";
        }

        public static class versionBars
        {
            public static string version = "> 2.0.3";
            public static string githubUrl = "> Github.com/Sat178";
            public static string bars = "========================================================================================================================";
        }

        public static void Menu()
        {
            Console.Title = "UrlTok - Main Menu - v2.0.3";
            Console.WriteWithGradient(menuStrings.firstLine, Color.Aqua, Color.HotPink, 5);
            Console.WriteWithGradient(menuStrings.secondLine, Color.Aqua, Color.HotPink, 5);
            Console.WriteWithGradient(menuStrings.thirdLine, Color.Aqua, Color.HotPink, 5);
            Console.WriteWithGradient(menuStrings.fourthLine, Color.Aqua, Color.HotPink, 5);
            Console.WriteWithGradient(menuStrings.fifthLine, Color.Aqua, Color.HotPink, 5);
            Console.WriteWithGradient(menuStrings.sixthLine, Color.Aqua, Color.HotPink, 5);
            Console.WriteWithGradient(menuStrings.seventhLine, Color.Aqua, Color.HotPink, 5);
            Console.WriteWithGradient(menuStrings.eigthLine, Color.Aqua, Color.HotPink, 5);
            Console.WriteWithGradient(menuStrings.ninethLine, Color.Aqua, Color.HotPink, 5);
            Console.WriteLine(versionBars.version, Color.Aqua);
            Console.WriteLine(versionBars.githubUrl, Color.Pink);
            Console.WriteWithGradient(versionBars.bars, Color.Aqua, Color.HotPink, 100);
        }

        public static void Options()
        {
            Console.WriteLine("[1] Open Config \t [4] Download Automatically");
            Console.WriteLine("[2] Scrape Proxies \t [5] Discord");
            Console.WriteLine("[3] Download Manually \t [6] Github");
            Console.WriteWithGradient(versionBars.bars, Color.Aqua, Color.HotPink, 100); Keyreader.KeyReader();
        }
    }
}
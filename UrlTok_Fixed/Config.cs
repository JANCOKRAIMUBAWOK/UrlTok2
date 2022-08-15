using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using Console = Colorful.Console;

namespace UrlTok_Fixed
{
    internal class Config
    {
        public static void createConfig()
        {
            if (!File.Exists("ttConfig.json"))
            {
                File.Create("ttConfig.json");
                //Console.WriteLine("Created a config file!");
            }
            else
            {
                //Console.WriteLine("File already exists!");
            }

            dynamic configFile = JsonConvert.DeserializeObject(File.ReadAllText("ttConfig.json"));

            Console.WriteLine($"Create new folder foreach creator: {configFile["directory_create"]}"); // True - False
            Console.WriteLine($"Display statistics per video: {configFile["show_stats"]}"); // True - False
            Console.WriteLine($"Save raw download urls into txt: {configFile["save_raw"]}"); // True - False
            Console.WriteLine($"Scape Proxies: {configFile["proxies"]}"); // None - Https - Socks4 - Socks5 - All

            Console.WriteWithGradient(Interface.versionBars.bars, Color.Aqua, Color.HotPink, 100);
            Keyreader.escapeKey();
        }
    }
}
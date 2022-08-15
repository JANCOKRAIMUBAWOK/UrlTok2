using System;

namespace UrlTok_Fixed
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Interface.Menu();
            Interface.Options();
            Console.ReadKey();
        }
    }
}
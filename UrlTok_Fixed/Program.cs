﻿using System;

namespace UrlTok_Fixed
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            UrlTok.CheckVersion();
            UrlTok.configCreate();
            Interface.Menu();
            Interface.Options();
            Console.ReadKey();
        }
    }
}
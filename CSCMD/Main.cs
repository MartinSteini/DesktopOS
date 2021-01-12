using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSCMD
{
    class Main
    {
        private string VersionNumber { get; set; }
        private string stdin { get; set; }
        private string buffer { get; set; }
        private int x { get; set; }
        private int y { get; set; }

        private ProgressBar progressbar;
        private Window window;

        private string[] terminal_filemenu = new string[]
        {
            " File ",
            " Edit ",
            " Application ",
            " Help "
        };

        public Main ()
        {
            this.VersionNumber = "1.0";
            this.x = 1;
            this.y = 1;
        }

        public void run ()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Tools.ShowWindow(Tools.ThisConsole, Tools.MAXIMIZE);
            Mouse.DisableQuickEditMode();

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Console.Title = $"cDOS Operating System v {(this.VersionNumber)}";
            Console.WriteLine("Starting C# Disk Operating System (cDOS) ...");

            this.progressbar = new ProgressBar(38, 23);
            this.progressbar.copysize = 99999999.0f / 2;
            this.progressbar.show();

            Thread.Sleep(100);

            this.cmdline();
        }

        private void cmdline ()
        {

            this.window = new Window(0, 0, 102, 24, "C# DOS -- Terminal", this.terminal_filemenu);
            this.window.show();

            while ( true )
            {
                this.stdin = Tools.getKey();

                switch ( this.stdin )
                {
                    case "Enter":
                        break;
                }
            }

            // this.closeapp();
        }

        private void closeapp()
        {
            Environment.Exit(0);
        }
    }
}

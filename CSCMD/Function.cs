using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
using System.Windows.Input;
using System.IO;
using System.Windows.Forms.VisualStyles;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Reflection.Emit;
using WindowsInput;
using WindowsInput.Native;
using System.Windows;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Services;
using Gma.System.MouseKeyHook;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace CSCMD
{
    class Function
    {
        // DllImports
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
        string fileName,
        [MarshalAs(UnmanagedType.U4)] uint fileAccess,
        [MarshalAs(UnmanagedType.U4)] uint fileShare,
        IntPtr securityAttributes,
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] int flags,
        IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Foreground;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        // variables 
        private string version_majority { get; set; }
        private string version_minority { get; set; }
        public string version_complete { get; set; }
        private DateTime mydatetime { get; set; }
        private string time { get; set; }
        private string parser { get; set; }
        private string title { get; set; }
        private string[] buffer = new string[24];
        private ArrayList EingabeListe = new ArrayList();
        private ConsoleColor defaultbackground { get; set; }
        private ConsoleColor defaultforeground { get; set; }

        private Calc clc = new Calc();
        private InputSimulator im = null;
        private ConsoleColor foreground { get; set; }
        private ConsoleColor background { get; set; }
        private int TerminalInstance { get; set; }

        private string[] windowdata = new string[25];
        public string username { get; set; }

        // coordinates variables for command line
        private int x { get; set; }
        private int y { get; set; }

        private int sc_x { get; set; }
        private int sc_y { get; set; }
        private bool loaded { get; set; }
        private bool loggedin { get; set; }
        public int thicknesslevel { get; set; }

        // string array which holds the entire command list of the parser

        private string[] commands = new string[]
        {
            "/help", "/cls", "/version", "/all","/exit","/gotoxy","/time","/restart","/!shell","/support","/basic","/edit",
            "/ls","/shutdown","/calc","/setup","setuplanguage","/runos","/paint","/cp", "/cd", "/clear","/sudo","/nano",
            "/logout","/create","/terminal","/+","/-","/clock","/logontime", "/weather",
            "set-foreground", "sf", "set-background", "sb", "get-background","get-foreground","get-version","set-cursor", "sc", "get-cursor", "gc",
            "gv", "get-time", "gt", "set-time", "st","/xmas","/snake", "/hny"
        };

        private string[] bashmenubar = new string[]
        {
            "_File",
            "_Edit",
            "_Applications",
            "_Help"
        };

        // commands for simple basic 

        private string[] basiccommands = new string[]
        {
            "/s", "/l", "/exit", "dim", "if", "write", "/exec"
        };

        // string which holds the complete arguments list for commands
        private string[] arguments = new string[]
        {
            "/?", "-?", "?","0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "10", "11", "12", "13", "14", "15", "16", "-default", "-def", "/cmd",
            "/cmdlet", "/alias", "/q", "/Q"
        };

        // filemenu string for edit.app
        private string[] filemenu_editapp = new string[]
        {
            "File", "Edit", "Search", "Application", "-", "Help"
        };

        private string[] filemenu_basicapp = new string[]
        {
            "File", "Edit", "Search", "Application", "Help"
        };

        private Language lng = new Language();
        private Window terminal;
        public Function()
        {
            this.mydatetime = DateTime.Now;
            this.version_majority = "";
            this.version_minority = "(Build 5000)";
            this.version_complete = this.version_majority + "" + this.version_minority;
            this.username = "martin";
            this.parser = $"{this.username}@!desktop:~>";
            this.time = $"{this.mydatetime}";
            this.title = $"!Desktop Terminal Version " + this.version_complete + " release 2020/2021";
            this.defaultbackground = ConsoleColor.DarkBlue;
            this.defaultforeground = ConsoleColor.White;
            this.background = ConsoleColor.DarkBlue;
            this.foreground = ConsoleColor.White;
            this.TerminalInstance++;
            this.thicknesslevel = 3;
        }
        private void setDefaultColors()
        {
            Console.BackgroundColor = this.defaultbackground;
            Console.ForegroundColor = this.defaultforeground;
            // Tools.cursor_state(true);
        }

        private void mainmenu()
        {
            int index = -1;

            Console.Title = this.title;

            string[] menues = new string[]
            {
                "Start !Desktop Terminal    ",
                "Setup !Desktop Terminal    ",
                "-",
                $"!Desktop Applications     {(char)26}",
                "-",
                "About !Desktop Version     ",
                "-",
                "Register Online            ",
                "-",
                "Search for latest Updates  ",
                "-",
                "Quit !Desktop Terminal     "
            };

            // setting up back / foreground for the shell

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Tools.setbackground(ConsoleColor.White);
            Tools.setforeground(ConsoleColor.Black);

            Tools.cursor_state(false);

            MenuBuilder menu = new MenuBuilder(38, 2, menues, "!Desktop Executive Shell");

            menu.shadow = true;

            while (true)
            {
                // index = menu.getValue();
                index = menu.handle();

                if ( index == 7 ) // register online mask 
                {
                    InputBox inputbox = new InputBox(30, 16, 73, 23, 2, "Please input your Informations!");
                    inputbox.show();
                    Console.ReadKey();
                    inputbox.destroy();
                }

                if (index == 3)
                {
                    string[] apps = new string[]
                    {
                        "!Desktop Basic       ",
                        "!Desktop Editor      ",
                        "!Desktop Nano        ",
                        "!Desktop Painter     ",
                        "!Desktop Documenter  ",
                        "!Desktop Calculator  ",
                        "-",
                        $"{(char)27} Back               "
                    };

                    MenuBuilder appmenu = new MenuBuilder(68, 6, apps, "Applications");

                    appmenu.shadow = true;

                    while (true)
                    {
                        // index = appmenu.getValue();
                        index = appmenu.handle();
                        if (index > -1 && index < 4 || index == 5 || index == 7) break;
                    }

                    switch (index)
                    {
                        case 0: // run basic app
                            index = 0;
                            this.basic_app();
                            break;
                        case 1: // run editor app
                            index = 0;
                            this.edit_app();
                            break;
                        case 2: // run nano app 
                            index = 0;
                            this.nano_app();
                            break;
                        case 3: // run the painter app 
                            index = 0;
                            this.paint_app(true);
                            break;
                        case 5: // run calc app
                            index = 0;
                            this.calc_app(true);
                            break;
                        case 7:
                            appmenu.destroyMenu();
                            break;
                    }
                }

                if (index == 5)
                {
                    this.getVersion();
                }

                if (index == 11)
                {
                    MessageBox msgbox = new MessageBox( this.terminal, 30, 16, 75, 23, 2, "Quitting !Desktop Executive Shell?", "^Do you really want to quit ?");
                    //                    index = wnd.MessageBox(30, 18, 73, 23, "Quitting C# Executive Shell ?", "Do you really want to quit ?^^");

                    // index = msgbox.show();
                    index = msgbox.handle();

                    if (index == 2) // NO button is pressed
                    {
                        msgbox.destroy();
                    }

                    if (index == 1)
                    {
                        msgbox.destroy();
                        Thread.Sleep(100);
                        this.closeapp();
                    }
                }

                if (index == 0 || index == 1 || index == 11)
                {
                    break;
                }
            }

            switch (index)
            {
                case 0:
                    this.run();
                    break;
                case 1:
                    this.setup();
                    break;
            }
        }

        // method to determine if the input string will be found in the
        // string array and check for executing
        public ArrayList getCommands(string eingabe)
        {
            ArrayList tmp = new ArrayList();
            int counter = 0;
            int len = eingabe.Length;
            string buffer = "";
            char ch;

            while (counter < len)
            {
                ch = eingabe[counter];

                if (ch == ' ')
                {
                    tmp.Add(buffer);
                    buffer = "";
                }
                else
                {
                    buffer += ch.ToString();
                }
                counter++;
            }
            tmp.Add(buffer);
            return tmp;
        }

        // this method will execute the given input 
        // executes if the command was found , otherwise
        // an error will be displayed
        private void command_execute(ArrayList cmds)
        {
            string command = "";
            string argument = "";
            bool commandfound = false;
            bool argumentfound = false;
            string text = "";

            if (cmds.Count == 1) // single command found and do the logic
            {
                command = (string)cmds[0];

                for (int i = 0; i < commands.Length; i++)
                {
                    if (command == commands[i])
                        commandfound = true;
                }

                if (commandfound == true)
                {
                    Tools.gotoxy(this.parser.Length + 1, this.y - 2);
                    // Tools.setbackground(ConsoleColor.Green);
                    // Console.Write($"executing command : '{command}' ...");
                    // Tools.setbackground(ConsoleColor.DarkBlue);

                    this.y--;

                    if ( command == "/clock" ) // run the clock app 
                    {
                        Clockapp clock = new Clockapp();
                        clock.run();
                    }

                    if ( command == "/hny" )
                    {
                        hny hny = new hny();

                        hny.run();
                    }

                    if ( command == "/snake" )
                    {
                        Console.WriteLine("Starting Snake the Game...");
                        Thread.Sleep(500);
                        SnakeGame snakegame = new SnakeGame();
                        snakegame.run();
                    }

                    if ( command == "/xmas" )
                    {
                        Xmas xmas = new Xmas();
                        xmas.run();
                    }

                    if (command == "/calc")
                    {
                        this.calc_app(true);
                    }

                    if ( command == "/weather" )
                    {
                        Weather weather = new Weather();
                        weather.run();
                    }

                    if (command == "/paint")
                    {
                        this.paint_app(false);
                    }

                    if (command == "/shutdown")
                    {
                        this.closeapp();
                    }

                    if (command == "/ls")
                    {
                        /*
                        Tools.gotoxy(1, this.y);
                        Console.WriteLine("listing apps : ");
                        Tools.gotoxy(1, this.y + 1);
                        Console.WriteLine("edit.app, basic.app");
                        this.y += 2;
                        */
                    }

                    if (command == "/edit")
                    {
                        // this.edit_app();
                    }
                    if (command == "sc" || command == "set-cursor")
                    {
                        Tools.gotoxy(1, this.y);
                        this.errorcolor();
                        Console.WriteLine($"error: missing argument for {command}!");
                        this.y += 2;
                    }

                    if (command == "/runos")
                    {
                        //Tools.gotoxy(1, this.y);
                        //Console.Write("Operating System environment is not implemented yet!");
                        //OS os = new OS();
                        //os.run();
                        //this.y++;
                    }

                    if (command == "/basic")
                    {
                        // this.basic_app();
                    }
                    if (command == "/support")
                    {
                        this.supportedfunctions();
                    }

                    if (command == "/+")
                    {
                        if (this.TerminalInstance >= 1)
                        {
                            text = $"Running instances of Terminal {this.TerminalInstance} . . .^^To add new instances of terminal please use '/terminal'!";

                            Label tmp = new Label(1, this.y, 30, this.y + 1, text, ConsoleColor.White, ConsoleColor.DarkBlue);
                            tmp.show();

                            this.y += 3;
                        }

                        if (this.TerminalInstance < 1)
                        {
                            text = $"Error: There are no running terminals in the background!^Please restart the application to solve this problem!";

                            Label tmp = new Label(1, this.y, 30, this.y + 1, text, ConsoleColor.White, ConsoleColor.DarkBlue);
                            tmp.show();

                            this.y += 2;
                        }
                    }

                    if (command == "/-")
                    {

                    }

                    if ( command == "/cls" || command == "/clear" )
                    {
                        this.refreshshell();
                        this.cmdline();
                    }

                    if (command == "/terminal")
                    {
                        text = "Starting a new instance of terminal . . .";

                        Label tmp = new Label(1, this.y, 30, this.y + 4, text, ConsoleColor.White, ConsoleColor.DarkBlue);
                        tmp.show();
                        this.y += 1;

                    }

                    if (command == "/create")
                    {

                    }

                    if (command == "/logout")
                    {
                        this.loaded = false;
                        this.login();
                        this.run();
                    }

                    if (command == "/nano")
                    {
                        // this.nano_app();
                    }

                    if (command == "/!shell")
                    {
                        //int index = 0;

                        //MessageBox messagebox = new MessageBox(this.terminal, 20, 18, 85, 25, 2, "!Shell is no more supported", "This function will no be longer be supported^^Proceed anyway?");
                        //index = messagebox.handle();

                        //if (index == 1) // yes button was pressed
                        //{
                        //    messagebox.destroy();
                        //    this.mainmenu();
                        //}
                        //else if (index == 2) // no button was pressedd 
                        //{
                        //    messagebox.destroy();
                        //    Tools.cursor_state(true);
                        //}
                    }


                    if (command == "/help")
                    {
                        this.showhelp();
                    }

                    if (command == "/all")
                    {
                        Console.WriteLine("listing commands:\n");
                        bool showcommand = false;
                        bool showcmdlet = false;
                        bool showalias = false;

                        foreach (string s in commands)
                        {
                            if (s.Contains("/") == true)
                            {
                                if (showcommand == false)
                                {
                                    Console.WriteLine("c#-command-line-parser main commands!");
                                    showcommand = true;
                                }
                                if (s != "/all")
                                    Console.WriteLine(s);
                            }

                            if (s.Contains("-") == true)
                            {
                                if (showcmdlet == false)
                                {
                                    Console.WriteLine("\nc#-command-line-parser main cmdlets!");
                                    showcmdlet = true;
                                }
                                Console.WriteLine(s);
                            }
                        }

                        foreach (string s in commands)
                        {
                            if (s.Length < 3)
                            {
                                if (showalias == false)
                                {
                                    Console.WriteLine("\nc#-command-line-parser main alias!");
                                    showalias = true;
                                }
                                Console.WriteLine(s);
                            }
                        }

                        Console.WriteLine("\nfor more details on the command line arguments\nusage : 'command|cmdlet' -?|/?|?");
                    }

                    if (command == "get-foreground")
                    {
                        ConsoleColor tmp = Tools.getConsoleForegroundColor();

                        Console.WriteLine($"Foreground color is {(char)26} {tmp.ToString()}");
                    }

                    if (command == "/setup")
                    {
                        Tools.gotoxy(1, this.y);
                        Console.WriteLine("Running !Desktop Setup ...");
                        Thread.Sleep(300);
                        this.setup();
                    }

                    if (command == "/setuplanguage")
                    {
                        this.languagesetup();
                    }

                    if (command == "get-background")
                    {
                        ConsoleColor tmp = Tools.getConsoleBackgroundColor();

                        Console.WriteLine($"Background color is {(char)26} {tmp.ToString()}");
                    }

                    if (command == "set-foreground" || command == "sf")
                    {
                        Console.WriteLine("available color table (foreground) ...\n");
                        string eingabe = "";
                        int color = 0;

                        for (int i = 0; i < 8; i++)
                        {
                            Console.WriteLine($"{(i + 1)}. {(ConsoleColor)i} \t {(i + 9)}. {(ConsoleColor)i + 8}");
                        }

                        Console.Write("\nrequested color ( return = quit ) : ");

                        eingabe = Console.ReadLine();

                        if (eingabe.Length == 0) // found enter key ....
                        {
                            Console.WriteLine("aborted!");
                        }
                        else
                        {
                            try
                            {
                                color = Int32.Parse(eingabe);

                                if (color > 0 && color < 17)
                                {
                                    Console.WriteLine($"setting foreground color to '{(ConsoleColor)color - 1}'");
                                    // Console.ForegroundColor = (ConsoleColor)color - 1;
                                    this.foreground = (ConsoleColor)color - 1;
                                }
                                else
                                {
                                    Console.WriteLine("error: not supported color!");
                                }
                            }
                            catch (Exception )
                            {
                                Console.WriteLine("internal error");
                            }
                        }
                    }

                    if (command == "set-background" || command == "sb")
                    {
                        Console.WriteLine("available color table (background) ...\n");
                        string eingabe = "";
                        int color = 0;

                        for (int i = 0; i < 8; i++)
                        {
                            Console.WriteLine($"{(i + 1)}. {(ConsoleColor)i} \t {(i + 9)}. {(ConsoleColor)i + 8}");
                        }

                        Console.Write("\nrequested color ( return = quit ) : ");

                        eingabe = Console.ReadLine();

                        if (eingabe.Length == 0) // found enter key ....
                        {
                            Console.WriteLine("aborted!");
                        }
                        else
                        {
                            color = Int32.Parse(eingabe);

                            if (color > 0 && color < 17)
                            {
                                Console.WriteLine($"setting background color to '{(ConsoleColor)color - 1}'");
                                // Console.BackgroundColor = (ConsoleColor)color - 1;
                                this.background = (ConsoleColor)color - 1;
                            }
                            else
                            {
                                Console.WriteLine("error: not supported color!");
                            }
                        }
                    }

                    if (command == "/time" || command == "get-time" || command == "gt")
                    {
                        Tools.gotoxy(1, this.y);
                        DateTime currenttime = DateTime.Now;
                        string tmp = "";

                        tmp = $"Current System time is {currenttime}";
                    
                        Console.WriteLine(tmp);

                        this.y++;
                    }

                    if ( command == "/logontime" )
                    {
                        Tools.gotoxy(1, this.y);
                        Console.WriteLine("");
                        this.y++;
                    }

                    if (command == "/restart")
                    {
                        int index = 0;

                        MessageBox messagebox = new MessageBox( this.terminal, 30, 15, 75, 22, 2,
                            "Restarting !Desktop Terminal ...",
                            "^Do you really want to restart ?^All data will be lost!");

                        // index = messagebox.show();
                        index = messagebox.handle();

                        if (index == 1)
                        {
                            Function tmp = new Function();
                            // tmp.set_fullscreen();
                            tmp.bootscreen();
                        }
                        else
                        {
                            messagebox.destroy();
                        }

                    }

                    if (command == "set-time")
                    {
                        Console.WriteLine("Not supported command yet!");
                        this.y += 1;
                    }

                    if (command == "/version" || command == "get-version" || command == "gv")
                    {
                        this.getVersion();
                    }

                    // Thread.Sleep(delay);
                }
                else
                {
                    string s = "";
                    s += $"error: could not find command '{command}' Type '/all' for all commands!";
                    this.errorcolor();
                    // Console.WriteLine(s + "\ntype '/all' for all commands!");
                    Tools.gotoxy(1, this.y - 1);
                    Console.WriteLine(s);

                    this.setDefaultColors();
                    // this.y += 3;
                }
            }
            else // command found with arguments !
            {
                for (int i = 0; i < commands.Length; i++)
                {
                    command = (string)cmds[0];
                    if (command == this.commands[i])
                        commandfound = true;
                }

                for (int i = 0; i < arguments.Length; i++)
                {
                    argument = (string)cmds[1];
                    if (argument == this.arguments[i])
                        argumentfound = true;
                }

                if (commandfound == true && argumentfound == true)
                {
                    this.y--;

                    if (command == "/exit")
                    {
                        if (argument == "/q" || argument == "/Q")
                        {
                            this.closeapp();
                        }
                    }

                    if (command == "/calc")
                    {
                        if (argument == "/q" || argument == "/Q")
                        {
                            this.calc_app(true);
                        }

                    }

                    if (command == "/!shell")
                    {
                        if (argument == "/q" || argument == "/Q")
                        {
                            // Console.WriteLine("Quick starting CSHELL...");
                        }
                    }

                    if (argument == "-?" || argument == "/?" || argument == "?")
                    {
                        if (command == "/exit")
                        {
                            Tools.gotoxy(1, this.y);
                            Console.WriteLine("Quits the C# Executive Shell environemnt with a standard Messagebox!");
                            Tools.gotoxy(1, this.y + 1);
                            Console.WriteLine("Using argument /q|/Q for quick exit!");
                            this.y += 2;
                        }
                        if (command == "/!shell")
                        {
                            Tools.gotoxy(1, this.y);
                            Console.WriteLine("Starts the C# Executive Shell ( Graphical User Interface )");
                            Tools.gotoxy(1, this.y + 1);
                            Console.WriteLine("This environment is in early stages of development, so it will be buggy!");
                            Tools.gotoxy(1, this.y + 2);
                            Console.WriteLine("Type '/cshell' without any arguments to run it!");
                            this.y += 3;
                        }

                        if (command == "/gotoxy")
                        {
                            Tools.gotoxy(1, this.y);
                            Console.WriteLine("gotoxy -- help");
                            Tools.gotoxy(1, this.y + 1);
                            Console.WriteLine();
                            Tools.gotoxy(1, this.y + 2);
                            Console.WriteLine("replaces the cursor at the given x,y coordinates i.e /gotoxy 1,1");
                            this.y += 3;
                        }
                        if (command == "/all")
                        {
                            Tools.gotoxy(1, this.y);
                            Console.WriteLine("all -- help");
                            Tools.gotoxy(1, this.y + 1);
                            Console.WriteLine("type '/all' /cmd /cmdlet /alias for specified help!");
                            this.y += 2;
                        }

                        if (command == "sc" || command == "set-cursor")
                        {
                            Console.WriteLine("set-curspr | sc -- help");
                            Console.WriteLine();
                            Console.WriteLine("setting cursor state to on(1) or off(0)!");
                        }

                        if (command == "/help")
                        {
                            Console.WriteLine("help -- help");
                        }

                        if (command == "set-foreground" || command == "sf")
                        {
                            Console.WriteLine("set-foreground | sf -- help");
                            Console.WriteLine();

                            for (int i = 0; i < 8; i++)
                            {
                                Console.WriteLine($"{(i + 1)}. {(ConsoleColor)i} \t {(i + 9)}. {(ConsoleColor)i + 8}");
                            }

                            Console.WriteLine("\nchanges the foreground for text inputs!\nargument '-default' or '-def' changes setting back to default");
                        }

                        if (command == "set-background" || command == "sb")
                        {
                            Console.WriteLine("set-background | sb -- help");
                            Console.WriteLine();

                            for (int i = 0; i < 8; i++)
                            {
                                Console.WriteLine($"{(i + 1)}. {(ConsoleColor)i} \t {(i + 9)}. {(ConsoleColor)i + 8}");
                            }

                            Console.WriteLine("\nchanges the background for text inputs!\nargument '-default' or '-def' changes setting back to default");
                        }

                        if (command == "/time")
                        {
                            text = "time -- help^shows the current system time/date on the console screen!^";
                            Label lblcls = new Label(1, this.y, 30, this.y + 4, text, ConsoleColor.White, ConsoleColor.DarkBlue);
                            lblcls.show();
                            this.y += 2;
                        }

                        if (command == "/cls")
                        {
                            text = "cls -- help^^clears the entire screen of the console and returns the cursor to start position!^";
                            Label lblcls = new Label(1, this.y, 30, this.y + 4, text, ConsoleColor.White, ConsoleColor.DarkBlue);
                            lblcls.show();
                            this.y += 3;
                        }

                        if (command == "/calc")
                        {
                            Tools.gotoxy(1, this.y);
                            Console.WriteLine("Starts the standard installed application for number operations!");
                            Tools.gotoxy(1, this.y + 1);
                            Console.WriteLine("/calc | /q|/Q - for quick start");
                            this.y += 2;
                        }

                        if ( command == "/clock" ) 
                        {
                            Tools.gotoxy(1, this.y);
                            Console.WriteLine("Starts the standard installed application for time & date.");
                            Tools.gotoxy(1, this.y + 1);
                            Console.WriteLine("This is the graphical version of '/time'!");
                            Tools.gotoxy(1, this.y + 2);
                            Console.WriteLine("/clock | /q|/Q - for quick start");
                            this.y += 3;
                        }

                        if (command == "/shutdown")
                        {
                            text = "Shutdown -- help" +
                                   "^^" +
                                   "Shuts immediatley the !Desktop OS environment without any prompt down!^";

                            Label lblcls = new Label(1, this.y, 30, this.y + 4, text, ConsoleColor.White, ConsoleColor.DarkBlue);
                            lblcls.show();
                            this.y += 3;
                        }

                        if (command == "/restart")
                        {
                            Console.WriteLine("Restart -- help");
                            Console.WriteLine();
                            Console.WriteLine("Restarts the current session manager and resets the screen to point 0,0!");
                            this.y += 3;
                        }

                    }
                    else if (argument == "-default" || argument == "-def")
                    {
                        if (command == "set-foreground" || command == "sf")
                        {
                            Tools.gotoxy(0, this.y);
                            Console.WriteLine("setting foreground default...");
                            Console.ForegroundColor = ConsoleColor.White;
                            this.y++;
                        }
                        if (command == "set-background" || command == "sb")
                        {
                            Tools.gotoxy(0, this.y);
                            Console.WriteLine("setting background default...");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            this.y++;
                        }

                    }
                    else if (argument == "/cmd")
                    {
                        this.getcommandHelp(commands, 0);
                    }
                    else if (argument == "/cmdlet")
                    {
                        this.getcommandHelp(commands, 1);
                    }
                    else if (argument == "/alias")
                    {
                        this.getcommandHelp(commands, 2);
                    }
                    else
                    {
                        if (command == "set-cursor" || command == "sc")
                        {
                            int state = Int32.Parse(argument);

                            if (state > -1 && state < 2)
                            {
                                Tools.gotoxy(1, this.y);
                                
                                Console.WriteLine("setting cursor mode ...");
                                
                                if (state == 0)
                                {
                                    Tools.gotoxy(1, this.y + 1);
                                    Console.WriteLine("Disabling cursor for text mode!");
                                    Console.CursorVisible = false;
                                }
                                else
                                {
                                    Tools.gotoxy(1, this.y + 1);
                                    Console.WriteLine("Enabling cursor for text mode!");
                                    Console.CursorVisible = true;
                                }

                                this.y += 2;
                            }
                            else
                            {
                                this.errorcolor();
                                Tools.gotoxy(1, this.y);
                                Console.WriteLine("syntax error");
                            }
                        }

                        if (command == "/gotoxy")
                        {
                            int x = Int32.Parse(argument);

                            Tools.gotoxy(x, 1);
                        }

                        if (command == "set-foreground" || command == "sf")
                        {
                            int color = Int32.Parse(argument);

                            color--;

                            if (color > -1 && color < 17)
                            {
                                Console.WriteLine($"setting color to '{(ConsoleColor)color}' ...");
                                // Console.ForegroundColor = (ConsoleColor)color;
                                this.foreground = (ConsoleColor)color;
                            }
                            else
                            {
                                Console.WriteLine("error:\ninvalid type for requested color argument!");
                            }
                        }

                        if (command == "set-background" || command == "sb")
                        {
                            int color = Int32.Parse(argument);
                            color--;
                            if (color > -1 && color < 17)
                            {
                                Console.WriteLine($"setting color to '{(ConsoleColor)color}' ...");
                                // Console.BackgroundColor = (ConsoleColor)color;
                                this.background = (ConsoleColor)color;
                            }
                            else
                            {
                                Console.WriteLine("error:\ninvalid type for requested color argument!");
                            }
                        }
                    }
                }
                else
                {
                    this.errorcolor();
                    Tools.gotoxy(1, this.y);
                    Console.WriteLine("syntax error! please refer to '/all' to see all supported commands!");
                    this.setDefaultColors();
                    this.y++;
                }
            }
        }
        public void bootscreen()
        {

            SetLayout s = new SetLayout();
            s.Set();

            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Tools.ShowWindow(Tools.ThisConsole, Tools.MAXIMIZE);
            Mouse.DisableQuickEditMode();

            try
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                Console.WriteLine("Starting !Desktop OS ...");
                Console.WriteLine();
                Console.Write("initializing mini Kernel ...");
                Thread.Sleep(500);
                Console.WriteLine(" Done!");
                Console.Write("initializing graphical user interface (GUI) ...");
                Thread.Sleep(750);
                Console.WriteLine(" Done!");
                Console.Write("initializing devices ...");
                Thread.Sleep(1250);
                Console.WriteLine(" Done!");
                Console.WriteLine("\nAll operations done!");
                Console.CursorVisible = false;
                Thread.Sleep(1000);
            }

            catch ( Exception )
            {
                Console.WriteLine("\nERROR: Could not initilize !Desktop OS environment.\n\nAny key to abort!");
                Console.ReadKey();
                Environment.Exit(0);
            }

            Console.Clear();
            Tools.cursor_state(false);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            this.login();
            this.run();
        }

        // v3.0 big shell changes with internal window manager etc
        public void run()
        {
            OS os = new OS();
            os.initialize();
        }

        // this method is for refreshing the command line
        private void refreshshell()
        {
            terminal = new Window(0, 0, 103, 26, "!Desktop Terminal", this.bashmenubar);
            
            terminal.showtaskbar = false;
            terminal.shadow = false;
            terminal.showclosebutton = true;
            terminal.showhelpbutton = true;
            terminal.showcontextmenu = false;

            terminal.show();
        }

        // the pure code of using the command line on prompt
        private void cmdline()
        {
            int index = 0;
            int counter = 0;
            int column = 0;

            string[] cmdlinefilemenu = new string[]
            {
                "New Terminal      [Alt+N]",
                "Close Terminal    [Alt+C]",
                "-",
                "Setup Prefences   [Alt+S]",
                "-",
                "Close !Desktop    [Alt+Q]"
            };

            string[] cmdlinehelpmenu = new string[]
            {
                "Info                F1",
                "-",
                "Get Help...           "
            };

            string[] cmdlineapps = new string[]
            {
                "Run Calculator        ",
                "Run Basic             ",
                "Run Clock             ",
                "Run Weather           ",
                "Run App Wizard        ",
                "-",
                "Back to !DesktopOS GUI",
                "-",
                "Settings              "
            };

            string[] cmdlineedit = new string[]
            {
                "Copy Items from Terminal      ",
                "Paste Items into Terminal     ",
                "-",
                "Setup environmental variables "
            };

            MenuBuilder fm;
            MenuBuilder em;
            MenuBuilder am;
            MenuBuilder hm;

            string eingabe = "";
            string key = "";
            string buffer = "";

            this.x = 1;
            this.y = 2;
            int parserlength = this.parser.Length + this.x;
            Console.Title = this.title;

            Tools.gotoxy(0, this.y);
            // ConsoleKeyInfo cki;

            Tools.setbackground(ConsoleColor.Blue);
            Tools.setforeground(ConsoleColor.White);
            string tmp = "";

            Console.CursorVisible = false;

            // main loop 
            Console.BackgroundColor = this.background;
            Console.ForegroundColor = this.foreground;

            while (true)
            {
                Console.BackgroundColor = this.background;
                Console.ForegroundColor = this.foreground;

                Console.SetCursorPosition(this.x, this.y);
                Console.Write(this.parser);

                key = "";
                buffer = "";
                tmp = "";

                while ( key != "Enter" )
                {
                    key = terminal.handle();

                    if ( key == "$close" )
                    {
                        this.loaded = false;

                        key = "AltQ";
                    }

                    if ( key == "$help" )
                    {
                        this.showversion(0);
                    }

                    // setting ranges for x and y coordinates!
                    
                    if (this.y < 2)
                    {
                        this.y = 2;
                    }

                    if (this.y >= 25)
                    {
                        this.y = 25;
                    }

                    if (this.x <= 1)
                    {
                        this.x = 1;
                    }

                    Tools.gotoxy(this.x + this.parser.Length, this.y);

                    index = 0;

                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;

                    if (key.Contains("Alt") == false &&
                          key.Contains("Enter") == false &&
                          key.Contains("Shift") == false &&
                          key.Contains("Backspace") == false &&
                          key.Contains("Arrow") == false &&
                          key.Contains("Tab") == false &&
                          key.Contains("Escape") == false &&
                          key.Contains("F1") == false &&
                          key.Contains("F2") == false &&
                          key.Contains("F4") == false && 
                          key.Contains("F5") == false &&
                          key.Contains("F9") == false && 
                          key.Contains("F7") == false && 
                          key.Contains("F8") == false && 
                          key.Contains("F9") == false && 
                          key.Contains("F10") == false && 
                          key.Contains("F11") == false && 
                          key.Contains("F12") == false && 
                          key.Contains("$help") == false && 
                          key.Contains("$minimize") == false && 
                          key.Contains("$maximize") == false && 
                          key.Contains("$close") == false && 
                          key.Contains("$noevent") == false && 
                          key.Contains("Ctrl") == false &&  
                          key.Contains("$menu0") == false &&
                          key.Contains("$menu1") == false && 
                          key.Contains("$menu2") == false && 
                          key.Contains("$menu3") == false &&
                          key.Contains("wKey") == false 
                          ) 
                    {

                        Tools.gotoxy(this.parser.Length + this.x, this.y);

                        if (key == "Spacebar")
                        {
                            this.x++;
                            buffer += " ";
                            key = "";
                            Console.Write(" ");
                        }

                        if (key != "")
                        {
                            this.x++;
                            buffer += key;
                            Console.Write(key);
                        }
                    }

                    switch (key)
                    {
                        case "UpArrow":
                            this.y--;
                            break;
                        case "LeftArrow":
                            this.x--;
                            break;
                        case "RightArrow":
                            this.x++;
                            break;
                        case "DownArrow":

                            if (column > 0)
                            {
                                this.y++;
                                column--;
                            }

                            break;
                    }

                    if ( key == "F7" ) // call clearing console method
                    {
                        key = "AltC";
                    }

                    if (key == "Backspace" && buffer.Length > 0)
                    {
                        tmp = "";

                        for (int i = 0; i < buffer.Length - 1; i++)
                        {
                            tmp += buffer[i];
                        }

                        buffer = tmp;
                        this.x--;

                        // formatting line
                        for (int x = this.parser.Length + 1; x < 102; x++)
                        {
                            Tools.gotoxy(x, this.y);
                            Console.Write(" ");
                        }

                        // rebuild parser string in console

                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;

                        Tools.gotoxy(1, this.y);
                        Console.Write(this.parser);
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                        // write the new content of buffer at the console
                        Tools.gotoxy(this.parser.Length + 1, this.y);
                        Console.Write(buffer);
                    }

                    if (key == "F9")
                    {
                        Function main = new Function();
                        // main.set_fullscreen();
                        main.bootscreen();
                    }

                    if (key == "F5")
                    {
                        this.refreshshell();
                        this.cmdline();
                    }

                    if (key == "F1")
                    {
                        this.showhelp();
                    }

                    if (key == "AltQ")
                    {
                        MessageBox messagebox = new MessageBox( this.terminal, 33, 14, 73 , 22, 2,
                        "Closing Terminal",
                        "^Do you want to quit terminal?^Back to !DesktopOS GUI interface?^>_^");

                        index = messagebox.handle();

                        if (index == 1)
                        {
                            OS os = new OS();
                            os.initialize();
                        }

                        if (index == 2)
                        {
                            messagebox.destroy();
                            this.terminal.gotFocus();
                        }
                    }

                    if (key == "AltF" || key == "$menu0" )
                    {
                        fm = new MenuBuilder(2, 2, cmdlinefilemenu, "File");

                        if ( key == "AltF" )
                        {
                            fm.is_key = true;
                        }

                        // POINT point = Mouse.getMousePosition();

                        index = fm.handle();

                        if ( index == 10 ) // code for selecitng menus to the right
                        {

                        }

                        if ( index == 20 ) // code for sleczing menus to the left
                        {

                        }

                        if (index == 3) // setup preferences 
                        {
                            this.setup();
                        }

                        if (index == 1) // close terminal 
                        {
                            // code will be written
                        }

                        if (index == 5) // code for quitting the app
                        {
                            MessageBox messagebox = new MessageBox(this.terminal, 33, 14, 73, 22, 2,
                            "Closing Terminal",
                            "^Do you want to quit terminal?^Back to !DesktopOS GUI interface?^>_^");

                            // fm.destroyMenu();

                            this.refreshshell();

                            // index = messagebox.show();
                            index = messagebox.handle();

                            if (index == 1)
                            {
                                OS os = new OS();
                                os.initialize();
                            }

                            if (index == 2)
                            {
                                messagebox.destroy();
                                this.terminal.gotFocus();
                            }
                        }

                        if (index == 0)
                        {
                            this.refreshshell();
                            this.cmdline();
                        }

                        Function.extern_run();
                    }

                    if (key == "AltC")
                    {
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Clear();
                    }

                    if (key == "AltN")
                    {
                        this.refreshshell();
                        this.cmdline();
                    }

                    if ( key == "AltA" || key == "$menu2" )
                    {
                        am = new MenuBuilder(18, 2, cmdlineapps,"Applications");

                        if ( key == "AltA" )
                        {
                            am.is_key = true;
                        }

                        while (true)
                        {
                            // index = am.getValue();
                            
                            index = am.handle();

                            if (index != 99 && index != 100) break;
                        }

                        if (index == -1)
                        {
                            am.destroyMenu();
                        }

                        if (index == 0)
                        {
                            this.calc_app(true);
                        }

                        if ( index == 1 )
                        {
                            // this.basic_app();
                            am.destroyMenu();
                        }

                        if ( index == 2 )
                        {
                            Clockapp clock = new Clockapp();
                            clock.run();
                        }

                        if ( index == 3 ) // run weather app from menu 
                        {
                            Weather w = new Weather();
                            w.run();
                        }

                        if ( index == 6 ) // run !Desktop UI
                        {
                            OS os = new OS();
                            os.initialize();
                        }

                        Function.extern_run();
                    }

                    if (key == "AltE" || key == "$menu1" )
                    {
                        em = new MenuBuilder(10, 2, cmdlineedit, "Edit");

                        if ( key == "AltE" )
                        {
                            em.is_key = true;
                        }

                        while (true)
                        {
                            // index = em.getValue();
                            index = em.handle();

                            if (index != 99 && index != 100) break;
                        }

                        if (index == -1)
                        {
                            em.destroyMenu();
                        }

                        Function.extern_run();
                    }

                    if (key == "F10") // quits the application immediately
                    {
                        this.closeapp();
                    }

                    if (key == "AltH" || key == "$menu3" )
                    {
                        hm = new MenuBuilder(34, 2, cmdlinehelpmenu, "Help");
                        // hm.shadow = false;

                        if ( key == "AltH" )
                        {
                            hm.is_key = true;
                        }

                        while (true)
                        {
                            // index = hm.getValue();
                            index = hm.handle();

                            if (index == -1 || index == 0 || index == 2)
                            {
                                break;
                            }
                        }

                        if (index == -1) // escape code was send
                        {
                            this.refreshshell();
                            // hm.destroyMenu();
                        }

                        if (index == 0) // first item from the menu list has been selected 
                        {

                            MessageBox messagebox = new MessageBox( this.terminal, 30, 14, 75, 22, 0,
                                "About !Desktop Terminal",
                                $"^Version 2020/2021, {(this.version_complete)}^Developed by Martin Steinkasserer^>_^");

                            this.refreshshell();

                            // messagebox.show();
                            messagebox.handle();
                            
                            this.refreshshell();
                            
                            Function.extern_run();
                        }

                        if ( index == 2 ) // help 
                        {
                            this.refreshshell();
                            this.showhelp();
                        }

                        Function.extern_run();
                    }
                }

                // if enter 

                column++;

                if (this.y < 25)
                {
                    this.y++;
                }
                else
                {
                    this.y = 25;
                }

                if (this.buffer.Length < 24)
                {
                    this.buffer[counter++] = buffer;
                }
                else
                {
                    counter = 0;
                }

                eingabe = buffer;

                if (eingabe.Length > 0)
                {
                    this.y++;
                    EingabeListe = this.getCommands(eingabe);
                    this.command_execute(EingabeListe);
                }

                if (eingabe == "/exit")
                {
                    break;
                }

                this.x = 1;

                // this.updatecolrow();
            }

            while (true)
            {
                MessageBox msgbox = new MessageBox( this.terminal, 30, 15, 75, 23, 2, "Quitting !Desktop Terminal ?", "^Do you really want to quit ?^Back to !Desktop OS^>_");
                // index = msgbox.show();
                index = msgbox.handle();

                if (index == 1) // YES
                {
                    // this.closeapp();
                    OS os = new OS();
                    os.initialize();
                }

                if (index == 2) // NO 
                {
                    break;
                }
            }

            if (index == 2)
            {
                this.refreshshell();
                this.cmdline();
            }

        }
        
        // this method will print the current in build supported applications!
        private void supportedfunctions()
        {
            Tools.gotoxy(1, this.y);
            Console.WriteLine($"Current Build : {this.version_complete}");
            Tools.gotoxy(1, this.y + 1);
            Console.WriteLine("!Desktop supports following functions : ");
            Tools.gotoxy(1, this.y + 2);
            Console.WriteLine(" - !Desktop Executive Shell '/!shell'");
            // Console.WriteLine(" - C# OS ( early ALPHA ) '/runos'");
            Tools.gotoxy(1, this.y + 3);
            Console.WriteLine(" - !Desktop Editor '/edit'");
            Tools.gotoxy(1, this.y + 4);
            Console.WriteLine(" - !Desktop Basic '/basic'");
            Tools.gotoxy(1, this.y + 5);
            Console.WriteLine(" - !Desktop Calculator '/calc'");
            Tools.gotoxy(1, this.y + 6);
            Console.WriteLine(" - !Desktop Clock '/clock'");
            //Console.WriteLine("For future release is planned to add console mouse support for really\n16-bit MS-DOS Look & Feel!");
            this.y += 7;
        }

        private void edit_app()
        {
            EditApp editapp = new EditApp();

            editapp.run();
        }

        private void getcommandHelp(string[] list, int argument)
        {
            if (argument == 0)
            {   // "/"
                Console.WriteLine("listing C#SHELL commands:\n");
                foreach (string tmp in list)
                {
                    if (tmp.Contains("/") == true)
                        Console.WriteLine(tmp);
                }
            }
            else if (argument == 1)
            {   // "-"
                Console.WriteLine("listing C#SHELL cmdlets:\n");
                foreach (string tmp in list)
                {
                    if (tmp.Contains("-") == true)
                        Console.WriteLine(tmp);
                }
            }
            else if (argument == 2)
            {   // less than 3 in length
                Console.WriteLine("listing C#SHELL aliases:\n");
                foreach (string tmp in list)
                    if (tmp.Length < 3)
                        Console.WriteLine(tmp);
            }
            else
            {
                Console.WriteLine("error: internal error has occured!");
                Console.WriteLine("please refer to users manual for fixing this problem!");
            }
        }
        public void runapp()
        {
            // Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            // Tools.ShowWindow(Tools.ThisConsole, Tools.MAXIMIZE);
            
            Mouse.DisableQuickEditMode();

            this.mainmenu();
        }
        public void closeapp()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Clear();
            Function.ext_wallpaper(0);

            Console.BackgroundColor = ConsoleColor.White;

            for (int x = 1; x < 104; x++)
            {
                for (int y = 24; y < 27; y++)
                {
                    Tools.gotoxy(x, y);
                    Console.Write(" ");
                    Tools.gotoxy(x, 0);
                    Console.Write(" ");
                    Tools.gotoxy(x, 1);
                    Console.Write(" ");
                    Tools.gotoxy(x, 2);
                    Console.Write(" ");
                }
            }


            Console.BackgroundColor = ConsoleColor.White;
            Tools.gotoxy(0, 2);
            int dx = 0;

            Tools.gotoxy(dx, 1);
            string str = "";
            Console.Write(str);

            Thread.Sleep(100);

            Tools.gotoxy(35, 23);

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            // Console.Write("Closing !Desktop, please wait . . .");

            ProgressBar progress = new ProgressBar(38, 25);
            progress.copysize = 999999999.19f / 7.0;
            progress.caption = " ";
            progress.showpercentage = false;
            progress.background = ConsoleColor.White;
            progress.show();

            Thread.Sleep(100);
            Tools.cursor_state(true);
            Environment.Exit(0);
        }

        private void setup()
        {
            int index = 0;

            Console.Clear();

            string[] menu = new string[]
            {
                "Setup preferred language     ",
                "-",
                "Set up colors for environemnt",
                "-",
                "Back to Mainmenu             "
            };

            MenuBuilder setupmenu = new MenuBuilder(35, 3, menu, "Settings");

            while (true)
            {
                // index = setupmenu.getValue();
                index = setupmenu.handle();

                if (index == 0 || index == 2 || index == 4)
                    break;
            }

            switch (index)
            {
                case 0:
                    this.languagesetup();
                    break;
                case 2:
                    this.colorsetup();
                    break;
                case 4:
                    this.mainmenu();
                    break;
            }
        }

        public void languagesetup()
        {

            int index = 0;
            Console.Clear();

            string[] menu = new string[]
            {
                "English                      ",
                "German                       ",
                "French                       ",
                "Spansih                      ",
                "-",
                "Back to Setup Mainmenu       "
            };

            MenuBuilder languagesettings = new MenuBuilder(35, 3, menu, "Language");

            while (true)
            {
                // index = languagesettings.getValue();
                index = languagesettings.handle();

                if (index > -1 && index < 5)
                {
                    lng.setLanguage(index);
                }

                if (index == 5)
                    break;
            }

            switch (index)
            {
                case 5:
                    this.setup();
                    break;
            }
        }

        public void colorsetup()
        {

            int index = 0;
            Console.Clear();

            string[] menu = new string[]
            {
                "Set foreground color         ",
                "-",
                "Set background color         ",
                "-",
                "Back to Setup Mainmenu       "
            };

            MenuBuilder colorssettings = new MenuBuilder(35, 3, menu, "Setup Colors");

            while (true)
            {
                // index = colorssettings.getValue();
                index = colorssettings.handle();

                if (index > -1 && index < 5)
                    break;
            }

            switch (index)
            {
                case 0:
                    this.setforegroundcolor();
                    break;
                case 2:
                    this.setbackgroundcolor();
                    break;
                case 4:
                    this.setup();
                    break;
            }
        }
        public void setforegroundcolor()
        {

            int index = 0;

            Console.Clear();

            string[] menu = new string[18];

            for (int i = 0; i < 16; i++)
            {
                menu[i] = ((ConsoleColor)i).ToString() + "            ";
            }

            menu[16] = "-";
            menu[17] = "Back                         ";

            MenuBuilder colorssettings = new MenuBuilder(35, 3, menu, "Setup Colors");

            while (true)
            {
                // index = colorssettings.getValue();
                index = colorssettings.handle();

                if (index > -1 && index < 18)
                    break;
            }

            switch (index)
            {
                case 17:
                    this.setup();
                    break;
            }
        }
        public void setbackgroundcolor()
        {

            int index = 0;

            Console.Clear();

            string[] menu = new string[18];

            for (int i = 0; i < 16; i++)
            {
                menu[i] = ((ConsoleColor)i).ToString() + "            ";
            }

            menu[16] = "-";
            menu[17] = "Back                         ";

            MenuBuilder colorssettings = new MenuBuilder(35, 3, menu, "Setup Colors");

            while (true)
            {
                // index = colorssettings.getValue();
                index = colorssettings.handle();


                if (index > -1 && index < 18)
                    break;
            }

            switch (index)
            {
                case 17:
                    this.setup();
                    break;
            }
        }

        private void basic_app()
        {
            BasicApp app = new BasicApp();
            app.run();
        }

        private void errorcolor()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void calc_app(bool qs)
        {

            if (qs == false)
            {
                MessageBox msgbox = new MessageBox( this.terminal , 30, 15, 75, 22, 2,
                    "Running Calculator...", "^Do you want to start Calculator anyway?");
                // int mindex = msgbox.show();
                int mindex = msgbox.handle();

                if (mindex == 1)
                {
                    Tools.setbackground(ConsoleColor.DarkBlue);
                    Tools.setforeground(ConsoleColor.White);

                    Console.Clear();

                    Tools.setbackground(ConsoleColor.DarkBlue);
                    Tools.setforeground(ConsoleColor.White);
                    Console.Clear();

                    clc.run();
                }
                else
                {
                    msgbox.destroy();
                }
            }
            else
            {
                Tools.setbackground(ConsoleColor.DarkBlue);
                Tools.setforeground(ConsoleColor.White);

                Console.Clear();

                /*
                Thread.Sleep(100);

                
                Console.WriteLine($"C# Calculator {this.version_complete} is starting...");
                Console.Title = $"C# Calculator {this.version_complete}";

                ProgressBar loading = new ProgressBar(38, 21);
                loading.copysize = 99999932.0f;
                loading.show();
                */

                Tools.setbackground(ConsoleColor.DarkBlue);
                Tools.setforeground(ConsoleColor.White);
                Console.Clear();

                clc.run();
            }

            Console.CursorVisible = true;
        }

        private void getVersion()
        {
            MessageBox msgbox = new MessageBox( this.terminal, 30, 15, 75, 23, 0, "About !Desktop Terminal", $"^Version {this.version_complete}^Developed by Martin Steinkasserer 2020^>_");
            
            msgbox.handle();
            msgbox.destroy();
            
            this.setDefaultColors();
            Tools.gotoxy(0, this.y + 1);
            
            this.terminal.gotFocus();
        }

        private void paint_app(bool flag)
        {
            Console.WriteLine("Starting Painter...");
            Painter painter = new Painter(0, 0, 102, 24);
            painter.run();
        }

        public static void extern_run()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            Function tmp = new Function();
           
            tmp.refreshshell();
            tmp.cmdline();
        }

        private void nano_app()
        {
            Nano nano = new Nano();
            nano.run();
        }
        private void login()
        {
            if (this.loaded == false && this.loggedin == false )
            {
                SetLayout s = new SetLayout();
                s.Set();
                this.drawasciiart();
            }

            this.loggedin = true;

            string key = "";

            int x1 = 20;
            int y1 = 3;
            int x2 = 82;
            int y2 = 23;
            int tmpx = 0;
            int index = 0;

            if (this.loaded == false)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                this.wallpaper(1); // login
            }

            this.loaded = true;
            
            Window login = new Window(x1, y1, x2, y2, "Welcome to !Desktop OS", null);
            login.showhelpbutton = true;
            login.showcontextmenu = false;
            login.showtaskbar = false;
            login.shadow = false;

            string lbltxt = $"^^"+
                             "$$Username      : [$$]^^" +
                             "$$Password      : [$$]^^^" +
                             " Please type in your user-account and password to proceed!^^" + 
                             " !Desktop OS " + this.version_complete + " is still in development!^" + 
                             " Developed by Martin Steinkasserer since July 2020.^" + 
                             " comes with incremental updates! Developed in Visual C#^" + 
                             " ";

            string lbl2txt = $"$$      !Desktop OS Account Manager^" +
                              ">_^";

            string lblregister = $"^^" +
                                  "$$Username      : [$$]^^" +
                                  "$$Password      : [$$]^^" +
                                  "$$Password      : [$$]^^^" +
                                  "To register with !Desktop OS you've to enter your username^" +
                                  "and your specified password to validate the process!^";

            Button[] buttons = new Button[3];
            Textfield[] txtfields = new Textfield[2];

            MessageBox messagebox = new MessageBox(login, x1 + 5, y2 - 9, x2 - 5, y2 - 1, 2, "Cancel Login process...",
                "^Do you want to cancel !Desktop login process ?^Quit right now...^>_");

            Label label = new Label(x1 + 2, y1 + 4, x2 - 1, y2 - 3, lbltxt,
                ConsoleColor.Black, ConsoleColor.Cyan);

            Label label2 = new Label(x1+1, y1 + 1, x2, y1 + 3, lbl2txt,
                ConsoleColor.White, ConsoleColor.DarkBlue);

            label.delay = 0;

            Label labelhelp = new Label(x1 + 2, y1 + 4, x2 - 1, y2 - 4, lblregister,
                ConsoleColor.Black, ConsoleColor.Cyan);
            labelhelp.delay = 1;

            string[] buttoncaption = new string[]
            {
                "Cancel",
                "Register",
                "Login"
            };
            
            login.show();
            label.show();
            label2.show();

            tmpx = x1 + 12;

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new Button(tmpx, y2 - 3, tmpx + 10, y2 - 3, buttoncaption[i]);
                buttons[i].background = ConsoleColor.White;
                buttons[i].foreground = ConsoleColor.Black;
                // buttons[i].shadow = true;
                tmpx += 14;
                buttons[i].delay *= 2;
                buttons[i].create();
            }

            while ( true )
            {
                key = login.handle(); // if something haopens on the main window screen 

                if ( key == "" ) 
                {
                    foreach ( Button b in buttons ) // otherwise check the inputs for the buttons
                    {
                        key = b.handle();

                        if ( key != "" )
                        {
                            break;
                        }
                    }
                }

                if (key == "AltC" || key == "Cancel" )
                {
                    buttons[0].Click();
                    Thread.Sleep(200);
                    key = "$close";
                }

                if ( key == "F1" || key == "$help" )
                {
                    login.lost_focus();
                    label.show();
                    label2.show();

                    tmpx = x1 + 12;
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i] = new Button(tmpx, y2 - 3, tmpx + 10, y2 - 3, buttoncaption[i]);
                        buttons[i].background = ConsoleColor.White;
                        buttons[i].foreground = ConsoleColor.Black;
                        tmpx += 14;
                        buttons[i].create();
                    }

                    string about = $"!Desktop Version : {this.version_complete}" +
                        "^^by Martin Steinkasserer^>_";

                    login.savedraw(23, 11, 80, 19);

                    MessageBox msgbox = new MessageBox( login, 23, 11, 80, 19, 0, "About !Desktop", about);

                    msgbox.handle();
                    
                    break;
                }

                if ( key == "AltQ" || key == "$close" )
                {
                    login.lost_focus();
                    label.show();
                    label2.show();

                    tmpx = x1 + 12;
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i] = new Button(tmpx, y2 - 3, tmpx + 10, y2 - 3, buttoncaption[i]);
                        buttons[i].background = ConsoleColor.White;
                        buttons[i].foreground = ConsoleColor.Black;
                        tmpx += 14;
                        buttons[i].create();
                    }

                    index = messagebox.handle();

                    if (index == 1)
                    {
                        this.loaded = false;
                        this.login();
                    }
                    else
                    {
                        break;
                    }
                }

                if (key == "AltL" || key == "Login" ) 
                {
                    buttons[2].Click();
                    Thread.Sleep(200);
                    this.loggedin = false;
                    break;
                }

                if (key == "AltR" || key == "Register" )
                {
                    buttons[1].Click();
                    Thread.Sleep(200);
                    labelhelp.show();
                    Console.ReadKey();
                    label.show();
                }
            }

            if ( key == "F1" || key == "$help" )
            {
                this.login();
            }

            if (index == 2)
            {
                this.login();
            }

            this.wallpaper(2);
        }
        private void drawasciiart()
        {
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Clear();

            // set x and y deep
            //int deepx = 5;
            //int deepy = 2;

            // draw the metadata onto the screen in ascii art style

            // this.wallpaper();
            Function.ext_wallpaper(0);


            Console.BackgroundColor = ConsoleColor.White;

            Console.CursorVisible = false;

            for (int x = 1; x < 104; x++)
            {
                for (int y = 24; y < 27; y++)
                {
                    Tools.gotoxy(x, y);
                    Console.Write(" ");
                    Tools.gotoxy(x, 0);
                    Console.Write(" ");
                    Tools.gotoxy(x, 1);
                    Console.Write(" ");
                    Tools.gotoxy(x, 2);
                    Console.Write(" ");

                }
            }

            Console.BackgroundColor = ConsoleColor.White;
            Tools.gotoxy(0, 2);
            int dx = 0;
            // string str = $"Developd by Martin Steinkasserer Version {this.version_complete}";

            // Console.WriteLine($"Developed by Martin Steinkasserer Version {this.version_complete}.2020");

            string str = "";

            dx = (103 - str.Length) / 2;

            Tools.gotoxy(dx, 1);
            Console.Write(str);


            Thread.Sleep(100);

            Tools.gotoxy(35, 23);

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            // Console.Write("Loading !Desktop, please wait . . .");
            
            ProgressBar progress = new ProgressBar(38, 25);
            progress.copysize = 99999931.19f / 3.154;
            progress.caption = " ";
            progress.showpercentage = false;
            progress.background = ConsoleColor.White;
            progress.show();
            Thread.Sleep(300);
        }

        public void drawasciichar(int x, int y, int[] array)
        {
            this.drawasciichar(x, y, 0, 0, false, array);
        }
        public void drawasciichar(int x, int y, int deepx, int deepy, bool shadow, int[] array)
        {
            //CharInfo[] buf = new CharInfo[80 * 25];
            //SmallRect[] rect = new SmallRect[10];
            //int counter = 0;
            //SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            int c = 0;
            int yc = 0;
            int rows = array.Length / 11;

            int thickness = this.thicknesslevel;

            for (int i = 0; i < array.Length; i++)
            {
                if (c < rows)
                {
                    if (array[i] == 1)
                    {
                        for (int j = 0; j < thickness; j++)
                        {
                            Tools.gotoxy(x + c + j, y + yc);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write(" ");
                        }

                        if (shadow == true)
                        {
                            Tools.gotoxy(x + c + deepx, y + yc + deepy);
                            Console.BackgroundColor = ConsoleColor.Black;

                            if (thickness == 1)
                            {
                                Console.Write(" ");
                            }
                            else if (thickness == 2)
                            {
                                Console.Write("  ");
                            }
                            else if (thickness == 3)
                            {
                                Console.Write("   ");
                            }
                        }
                    }
                }
                else
                {
                    c = 0;
                    // x++;
                    yc++;
                }

                if (c == 0)
                {

                    if (array[i] == 1)
                    {
                        for (int j = 0; j < thickness; j++)
                        {
                            Tools.gotoxy(x + c + j, y + yc);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write(" ");
                        }

                        if (shadow == true)
                        {
                            Tools.gotoxy(x + c + deepx, y + yc + deepy);
                            Console.BackgroundColor = ConsoleColor.Black;

                            if (thickness == 1)
                            {
                                Console.Write(" ");
                            }
                            else if (thickness == 2)
                            {
                                Console.Write("  ");
                            }
                            else if (thickness == 3)
                            {
                                Console.Write("   ");
                            }
                        }
                    }
                }
                c++;
            }
        }

        private void showhelp () // help 
        {
            string key = "";
            terminal.lost_focus();

            Window helpwnd = new Window(5, 3, 97, 22, "Welcome to !Desktop Help-Center", null);
            helpwnd.shadow = false;
            helpwnd.showtaskbar = false;
            helpwnd.showcontextmenu = false;
            helpwnd.show();

            string hlpstr = "$$$$!Desktop OS Help Center Version 1.0^" +
                     ">_^" +
                     "^" +
                     "This program is in early stages of development and maybe it will never be finished.^" +
                     "^" +
                     "Developed by Martin Steinkasserer in case of understanding how C# classes will work!^^^" +
                     "!Desktop Build " + this.version_complete + "^^^^^" +
                     "To see all supported commands for !Desktop Terminal type : '/all /cmd'";

            int dx = ((97 - 7) / 2);

            Button button = new Button(dx, 19, dx+10, 19, "OK");

            Label label = new Label(7, 5, 96, 19, hlpstr, ConsoleColor.White, ConsoleColor.DarkBlue);

            label.show();
            button.show();

            // code for the mouse input 
            var handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);

            int mode = 0;
            if (!(NativeMethods.GetConsoleMode(handle, ref mode))) { throw new Win32Exception(); }

            mode |= NativeMethods.ENABLE_MOUSE_INPUT;
            mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;
            mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;

            if (!(NativeMethods.SetConsoleMode(handle, mode))) { throw new Win32Exception(); }
            // var record = new NativeMethods.INPUT_RECORD();
            // uint recordLen = 0;
            
            // end code for mouse input 

            while (true)
            {
                key = helpwnd.handle();

                if ( key == "" )
                {
                    key = button.handle();

                    if ( key != "" )
                    {
                        if ( key == "OK" ) 
                        {
                            key = "Enter";
                        }
                    }
                }

                if ( key == "Enter" || key == "AltO" || key == "O" || key == "o" )
                {
                    button.Click();
                    break;
                }

                if ( key == "$close" )
                {
                    break;
                }
            }

            Thread.Sleep(350);

            Function.extern_run();
        }

        private void showversion(int mode)
        {
            string about = $"!Desktop Version : {this.version_complete}" +
                "^^by Martin Steinkasserer^>_";
            
            MessageBox msgbox = new MessageBox( this.terminal , 23, 11, 80, 19, 0, "About !Desktop", about);

            msgbox.handle();
            
            if (mode == 0)
            {
               this.refreshshell();
               msgbox.destroy();
            }
        }

        public static void ext_wallpaper(int mode)
        {
            Function tmp = new Function();

            tmp.wallpaper(mode);
        }
        public void wallpaper (int mode)
        {
            if ( mode == 0 ) // displays the standard !DesktopOS wallpaper
            {
                ArrayList frame = new ArrayList();
                ArrayList inlay = new ArrayList();
                int counter = 0;
                string buffer = "";

                Console.Clear();
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();

                Console.BackgroundColor = ConsoleColor.White;
                #region iterations
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    frame.Add(buffer);
                    buffer = "";
                }

                buffer = "";

                for (int y = 24; y < 27; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    inlay.Add(buffer);
                    buffer = "";
                }
                #endregion

                counter = 0;

                foreach (string s in frame)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.Write(s);
                    counter++;
                }

                counter = 24;

                foreach (string s in inlay)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.Write(s);
                    counter++;
                }

                // set x and y deep
                int deepx = 5;
                int deepy = 2;


                // draw the metadata onto the screen in ascii art style

                this.drawasciichar(-1, 7, deepx, deepy, true, AsciiArt.asciiexclamationmark);
                this.drawasciichar(9, 7, deepx, deepy, true, AsciiArt.asciiD);
                this.drawasciichar(19, 7, deepx, deepy, true, AsciiArt.asciiE);
                this.drawasciichar(29, 7, deepx, deepy, true, AsciiArt.asciiS);
                this.drawasciichar(39, 7, deepx, deepy, true, AsciiArt.asciiK);
                this.drawasciichar(49, 7, deepx, deepy, true, AsciiArt.asciiT);
                this.drawasciichar(59, 7, deepx, deepy, true, AsciiArt.asciiO);
                this.drawasciichar(69, 7, deepx, deepy, true, AsciiArt.asciiP);
                this.drawasciichar(82, 4, deepx, deepy, true, AsciiArt.asciiO);
                this.drawasciichar(92, 4, deepx, deepy, true, AsciiArt.asciiS);

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (mode == 1) // wallpaper for "Login"
            {
                Console.Clear();
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                // set x and y deep
                int deepx = 5;
                int deepy = 2;

                Console.SetCursorPosition(2, 5);
                Console.WriteLine("!Desktop OS");

                this.drawasciichar(2, 7, deepx, deepy, true, AsciiArt.asciiL);
                this.drawasciichar(12, 7, deepx, deepy, true, AsciiArt.asciiO);
                this.drawasciichar(22, 7, deepx, deepy, true, AsciiArt.asciiG);
                this.drawasciichar(33, 7, deepx, deepy, true, AsciiArt.asciiI);
                this.drawasciichar(43, 7, deepx, deepy, true, AsciiArt.asciiN);
                this.drawasciichar(55, 10, deepx, deepy, true, AsciiArt.asciipoint);
                this.drawasciichar(65, 10, deepx, deepy, true, AsciiArt.asciipoint);
                this.drawasciichar(75, 10, deepx, deepy, true, AsciiArt.asciipoint);

                Console.BackgroundColor = ConsoleColor.White;

                ArrayList frame = new ArrayList();
                ArrayList inlay = new ArrayList();
                int counter = 0;
                string buffer = "";

                for (int y = 0; y < 2; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    frame.Add(buffer);
                    buffer = "";
                }

                buffer = "";

                for (int y = 25; y < 27; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    inlay.Add(buffer);
                    buffer = "";
                }

                counter = 0;

                foreach (string s in frame)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.WriteLine(s);
                    counter++;
                }

                counter = 25;

                foreach (string s in inlay)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.Write(s);
                    counter++;
                }

                string[] msg = new string[]
                {
                  @"Please press ""Control+Alt+Enter"" to login! Or ""Control+Alt+Q"" to quit!",
                  @"Please press ""Control+Alt+T"" for !Desktop Terminal"
                };

                int[] dx = new int[2];

                dx[0] = (103 - msg[1].Length) / 2;
                dx[1] = (103 - msg[0].Length) / 2;
                
                ConsoleKeyInfo cki;
                bool quit = false;

                InputSimulator is_mouse = new InputSimulator();

                Console.ForegroundColor = ConsoleColor.Black;

                Console.SetCursorPosition(dx[0], 25);
                Console.WriteLine(msg[1]);

                Console.SetCursorPosition(dx[1], 1);
                Console.WriteLine(msg[0]);
                
                while (!quit)
                {
                    if ( Console.KeyAvailable == true )
                    {
                        cki = Console.ReadKey(true);

                        // check for ctrl+alt+enter combination and then quit the while
                        if ( (cki.Modifiers & ConsoleModifiers.Control) != 0 && 
                             (cki.Modifiers & ConsoleModifiers.Alt ) != 0 && 
                             cki.Key == ConsoleKey.Enter )
                        {
                            quit = true;
                        }

                        if ( ( cki.Modifiers & ConsoleModifiers.Control) != 0 && 
                             ( cki.Modifiers & ConsoleModifiers.Alt ) != 0 && 
                             cki.Key == ConsoleKey.Q )
                        {
                            this.closeapp();
                        }

                        if ( ( cki.Modifiers & ConsoleModifiers.Control ) != 0 && 
                            ( cki.Modifiers & ConsoleModifiers.Alt ) != 0 && 
                            cki.Key == ConsoleKey.T )
                        {
                            Function.ext_wallpaper(8);
                            Function.extern_run();
                        }
                    }
                }

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.White;
                
                Console.SetCursorPosition(dx[0], 25);
                Console.WriteLine(msg[1]);
                Console.SetCursorPosition(dx[1], 1);
                Console.WriteLine(msg[0]);

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if ( mode == 2 ) // wallpaper for !desktop os blank
            {
                string buffer = "";
                ArrayList frame = new ArrayList();
                int counter = 0;

                for ( int y = 0; y < 27; y++ )
                {
                    for ( int x = 1; x < 104; x++ )
                    {
                        buffer += " ";
                    }
                    frame.Add(buffer);
                    buffer = "";
                }

                buffer = "";
                counter = 0;

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;

                foreach ( string s in frame )
                {
                    Console.SetCursorPosition(0, counter);
                    Console.Write(s);
                    counter++;
                }
            }
            else if ( mode == 3 )  // logout wallpaper
            {
                Console.Clear();
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                // set x and y deep
                int deepx = 5;
                int deepy = 2;

                Console.SetCursorPosition(2, 5);
                Console.WriteLine("!Desktop OS");

                this.drawasciichar(2, 7, deepx, deepy, true, AsciiArt.asciiL);
                this.drawasciichar(12, 7, deepx, deepy, true, AsciiArt.asciiO);
                this.drawasciichar(22, 7, deepx, deepy, true, AsciiArt.asciiG);
                this.drawasciichar(35, 7, deepx, deepy, true, AsciiArt.asciiO);
                this.drawasciichar(45, 7, deepx, deepy, true, AsciiArt.asciiU);
                this.drawasciichar(55, 7, deepx, deepy, true, AsciiArt.asciiT);
                this.drawasciichar(67, 10, deepx, deepy, true, AsciiArt.asciipoint);
                this.drawasciichar(79, 10, deepx, deepy, true, AsciiArt.asciipoint);
                this.drawasciichar(91, 10, deepx, deepy, true, AsciiArt.asciipoint);
                
                Console.BackgroundColor = ConsoleColor.White;
                
                ArrayList frame = new ArrayList();
                ArrayList inlay = new ArrayList();
                int counter = 0;
                string buffer = "";

                for (int y = 0; y < 2; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    frame.Add(buffer);
                    buffer = "";
                }

                buffer = "";

                for (int y = 25; y < 27; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    inlay.Add(buffer);
                    buffer = "";
                }

                counter = 0;

                foreach (string s in frame)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.WriteLine(s);
                    counter++;
                }

                counter = 25;

                foreach (string s in inlay)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.Write(s);
                    counter++;
                }

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if ( mode == 4 )
            {
                Console.Clear();
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                // set x and y deep
                int deepx = 5;
                int deepy = 2;

                Console.SetCursorPosition(2, 5);
                Console.WriteLine("!Desktop OS");

                this.drawasciichar(2, 7, deepx, deepy, true, AsciiArt.asciiS);
                this.drawasciichar(12, 7, deepx, deepy, true, AsciiArt.asciiE);
                this.drawasciichar(22, 7, deepx, deepy, true, AsciiArt.asciiT);
                this.drawasciichar(32, 7, deepx, deepy, true, AsciiArt.asciiT);
                this.drawasciichar(42, 7, deepx, deepy, true, AsciiArt.asciiI);
                this.drawasciichar(52, 7, deepx, deepy, true, AsciiArt.asciiN);
                this.drawasciichar(65, 7, deepx, deepy, true, AsciiArt.asciiG);
                this.drawasciichar(79, 7, deepx, deepy, true, AsciiArt.asciiS);

                Console.BackgroundColor = ConsoleColor.White;

                ArrayList frame = new ArrayList();
                ArrayList inlay = new ArrayList();
                int counter = 0;
                string buffer = "";

                for (int y = 0; y < 2; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    frame.Add(buffer);
                    buffer = "";
                }

                buffer = "";

                for (int y = 25; y < 27; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    inlay.Add(buffer);
                    buffer = "";
                }

                counter = 0;

                foreach (string s in frame)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.WriteLine(s);
                    counter++;
                }

                counter = 25;

                foreach (string s in inlay)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.Write(s);
                    counter++;
                }

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.White;

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if ( mode == 5 ) // wallpaper for "Calculator" app
            {
                Console.Clear();
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                // set x and y deep
                int deepx = 5;
                int deepy = 2;

                Console.SetCursorPosition(2, 5);
                Console.WriteLine("Loading ...");

                this.drawasciichar(2, 7, deepx, deepy, true, AsciiArt.asciiC);
                this.drawasciichar(12, 7, deepx, deepy, true, AsciiArt.asciiA);
                this.drawasciichar(22, 7, deepx, deepy, true, AsciiArt.asciiL);
                this.drawasciichar(32, 7, deepx, deepy, true, AsciiArt.asciiC);
                this.drawasciichar(42, 7, deepx, deepy, true, AsciiArt.asciiU);
                this.drawasciichar(52, 7, deepx, deepy, true, AsciiArt.asciiL);
                this.drawasciichar(62, 7, deepx, deepy, true, AsciiArt.asciiA);
                this.drawasciichar(72, 7, deepx, deepy, true, AsciiArt.asciiT);
                this.drawasciichar(82, 7, deepx, deepy, true, AsciiArt.asciiO);
                this.drawasciichar(92, 7, deepx, deepy, true, AsciiArt.asciiR);

                // this.drawasciichar(99, 7, deepx, deepy, true, AsciiArt.asciiR);


                Console.BackgroundColor = ConsoleColor.White;

                ArrayList frame = new ArrayList();
                ArrayList inlay = new ArrayList();
                int counter = 0;
                string buffer = "";

                for (int y = 0; y < 2; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    frame.Add(buffer);
                    buffer = "";
                }

                buffer = "";

                for (int y = 25; y < 27; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    inlay.Add(buffer);
                    buffer = "";
                }

                counter = 0;

                foreach (string s in frame)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.WriteLine(s);
                    counter++;
                }

                counter = 25;

                foreach (string s in inlay)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.Write(s);
                    counter++;
                }

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.White;

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if ( mode == 6 ) // wallpaper for "Clock" app
            {
                Console.Clear();
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                // set x and y deep
                int deepx = 5;
                int deepy = 2;

                Console.SetCursorPosition(2, 5);
                Console.WriteLine("Loading ...");

                this.drawasciichar(2, 7, deepx, deepy, true, AsciiArt.asciiC);
                this.drawasciichar(12, 7, deepx, deepy, true, AsciiArt.asciiL);
                this.drawasciichar(22, 7, deepx, deepy, true, AsciiArt.asciiO);
                this.drawasciichar(32, 7, deepx, deepy, true, AsciiArt.asciiC);
                this.drawasciichar(42, 7, deepx, deepy, true, AsciiArt.asciiK);

                // this.drawasciichar(99, 7, deepx, deepy, true, AsciiArt.asciiR);


                Console.BackgroundColor = ConsoleColor.White;

                ArrayList frame = new ArrayList();
                ArrayList inlay = new ArrayList();
                int counter = 0;
                string buffer = "";

                for (int y = 0; y < 2; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    frame.Add(buffer);
                    buffer = "";
                }

                buffer = "";

                for (int y = 25; y < 27; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    inlay.Add(buffer);
                    buffer = "";
                }

                counter = 0;

                foreach (string s in frame)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.WriteLine(s);
                    counter++;
                }

                counter = 25;

                foreach (string s in inlay)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.Write(s);
                    counter++;
                }

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.White;

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if ( mode == 7 ) // weather wallpaper
            {
                Console.Clear();
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                // set x and y deep
                int deepx = 5;
                int deepy = 2;

                Console.SetCursorPosition(2, 5);
                Console.WriteLine("Loading ...");

                this.drawasciichar(2, 7, deepx, deepy, true, AsciiArt.asciiW);
                this.drawasciichar(15, 7, deepx, deepy, true, AsciiArt.asciiE);
                this.drawasciichar(25, 7, deepx, deepy, true, AsciiArt.asciiA);
                this.drawasciichar(35, 7, deepx, deepy, true, AsciiArt.asciiT);
                this.drawasciichar(45, 7, deepx, deepy, true, AsciiArt.asciiH);
                this.drawasciichar(55, 7, deepx, deepy, true, AsciiArt.asciiE);
                this.drawasciichar(65, 7, deepx, deepy, true, AsciiArt.asciiR);

                Console.BackgroundColor = ConsoleColor.White;

                ArrayList frame = new ArrayList();
                ArrayList inlay = new ArrayList();
                int counter = 0;
                string buffer = "";

                for (int y = 0; y < 2; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    frame.Add(buffer);
                    buffer = "";
                }

                buffer = "";

                for (int y = 25; y < 27; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    inlay.Add(buffer);
                    buffer = "";
                }

                counter = 0;

                foreach (string s in frame)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.WriteLine(s);
                    counter++;
                }

                counter = 25;

                foreach (string s in inlay)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.Write(s);
                    counter++;
                }

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.White;

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if ( mode == 8 ) // terminal wallpaper
            {
                Console.Clear();
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                // set x and y deep
                int deepx = 5;
                int deepy = 2;

                Console.SetCursorPosition(2, 5);
                Console.WriteLine("Loading ...");

                this.drawasciichar(2, 7, deepx, deepy, true, AsciiArt.asciiT);
                this.drawasciichar(13, 7, deepx, deepy, true, AsciiArt.asciiE);
                this.drawasciichar(23, 7, deepx, deepy, true, AsciiArt.asciiR);
                this.drawasciichar(33, 7, deepx, deepy, true, AsciiArt.asciiM);
                this.drawasciichar(46, 7, deepx, deepy, true, AsciiArt.asciiI);
                this.drawasciichar(54, 7, deepx, deepy, true, AsciiArt.asciiN);
                this.drawasciichar(66, 7, deepx, deepy, true, AsciiArt.asciiA);
                this.drawasciichar(76, 7, deepx, deepy, true, AsciiArt.asciiL);

                // this.drawasciichar(99, 7, deepx, deepy, true, AsciiArt.asciiR);


                Console.BackgroundColor = ConsoleColor.White;

                ArrayList frame = new ArrayList();
                ArrayList inlay = new ArrayList();
                int counter = 0;
                string buffer = "";

                for (int y = 0; y < 2; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    frame.Add(buffer);
                    buffer = "";
                }

                buffer = "";

                for (int y = 25; y < 27; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    inlay.Add(buffer);
                    buffer = "";
                }

                counter = 0;

                foreach (string s in frame)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.WriteLine(s);
                    counter++;
                }

                counter = 25;

                foreach (string s in inlay)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.Write(s);
                    counter++;
                }

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.White;

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if ( mode == 9 ) // error
            {
                Console.Clear();
                Console.CursorVisible = false;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                // set x and y deep
                int deepx = 5;
                int deepy = 2;

                this.drawasciichar(2, 7, deepx, deepy, true, AsciiArt.asciiE);
                this.drawasciichar(13, 7, deepx, deepy, true, AsciiArt.asciiR);
                this.drawasciichar(23, 7, deepx, deepy, true, AsciiArt.asciiR);
                this.drawasciichar(33, 7, deepx, deepy, true, AsciiArt.asciiO);
                this.drawasciichar(43, 7, deepx, deepy, true, AsciiArt.asciiR);

                Console.BackgroundColor = ConsoleColor.White;

                ArrayList frame = new ArrayList();
                ArrayList inlay = new ArrayList();
                int counter = 0;
                string buffer = "";

                for (int y = 0; y < 2; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    frame.Add(buffer);
                    buffer = "";
                }

                buffer = "";

                for (int y = 25; y < 27; y++)
                {
                    for (int x = 1; x < 104; x++)
                    {
                        buffer += " ";
                    }
                    inlay.Add(buffer);
                    buffer = "";
                }

                counter = 0;

                foreach (string s in frame)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.WriteLine(s);
                    counter++;
                }

                counter = 25;

                foreach (string s in inlay)
                {
                    Console.SetCursorPosition(1, counter);
                    Console.Write(s);
                    counter++;
                }
            }

            if ( mode != 0 && mode != 2 )
            {
                Thread.Sleep(350);
            }
        }
        private void sql_connection ()
        {
            /*string host = "127.0.0.1";
            string passwd = "osdesktop$$";
            string db_base = "!desktop_os_db";
            */
        }

        private void sql_disconnection ()
        {

        }

        private void sql_query ( string query ) 
        {
            /*
            if ( query == "" )
            {
                query = "SELECT * FROM users_os";
                string result = "";
            }*/
        }
    }
}

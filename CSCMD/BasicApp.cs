using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Runtime.Remoting.Services;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.CompilerServices;

namespace CSCMD
{
    class BasicApp
    {
        private string[] filemenu_basicapp = new string[]
        {
            "_File", "_Edit", "_Search", "_Application", "_Help"
        };

        private int x { get; set; }
        private int y { get; set; }

        private Window basicwindow = null;

        public BasicApp()
        {
            this.x = 0;
            this.y = 0;
        }

        public void run()
        {
            // ConsoleKeyInfo cki;
            string[] textfield = new string[24];

            string key = "";

            string[] filemenu = new string[]
            {
                "New file...         [Alt+N]",
                "Open file...        [Alt+O]",
                "-",
                "Save file...        [Alt+S]",
                "Save file as ...    [Alt+X]",
                "-",
                "Exit !Desktop Basic [Alt+Q]"
            };

            string[] editmenu = new string[]
            {
                "Search for Keywords        ",
                "Search and Replace Keywords",
                "-",
                "Cut                        ",
                "Paste                      ",
                "-",
                "Placeholder                "
            };

            string[] searchmenu = new string[]
            {
                "Placeholder                 ",
                "Placeholder                 ",
                "Placeholder                 ",
                "Placeholder                 ",
                "Placeholder                 ",
                "-",
                "Placeholder                 "
            };

            string[] appmenu = new string[]
            {
                "Placeholder                 ",
                "Placeholder                 ",
                "Placeholder                 ",
                "Placeholder                 ",
                "Placeholder                 ",
                "-",
                "Placeholder                 "
            };

            string[] helpmenu = new string[]
            {
                "Info about !Desktop Basic [F1]",
                "-",
                "Get Help...                   ",
            };


            Console.Clear();
            Tools.cursor_state(true);
            Console.Clear();
            Thread.Sleep(100);

            Function tmp = new Function();
            /*
            Console.WriteLine($"C# Documenter {tmp.version_complete} is starting...");
            Console.Title = $"C# Documenter {tmp.version_complete}";

            ProgressBar loading = new ProgressBar(38, 21);
            loading.copysize = 99999932.0f;
            loading.show();
            */

            Tools.setbackground(ConsoleColor.DarkBlue);
            Tools.setforeground(ConsoleColor.White);
            Console.Clear();

            int x = 0;
            int y = 0;
            int index = 0;

            Tools.setforeground(ConsoleColor.Black);
            Tools.setbackground(ConsoleColor.White);

            for (x = 0; x < 104; x++)
            {
                Tools.gotoxy(x, 0);
                Console.Write(" ");
                Tools.gotoxy(x, 26);
                Console.Write(" ");
            }

            for (x = 0; x < 27; x++)
            {
                Tools.gotoxy(0, x);
                Console.Write(" ");
                Tools.gotoxy(103, x);
                Console.Write(" ");
            }


            // string appname = "!Desktop Basic 1.3.0a";
            // Tools.gotoxy((100 - appname.Length - 1), 0);
            // Console.Write(appname);

            Tools.setbackground(ConsoleColor.Red);
            Tools.setforeground(ConsoleColor.White);

            /*Tools.gotoxy(100, 0);
            Console.Write(" x ");
            
            */

            x = 1;
            y = 0;


            // this.window1.createWindow(0, 0, 101, 24, "C# Basic 2.0", null);

            /*
            this.toolbarcolor();

            for (int i = 0; i < this.filemenu_basicapp.Length; i++)
            {
                Tools.gotoxy(x + i, 0);
                Console.Write(this.filemenu_basicapp[i]);
                x += this.filemenu_basicapp[i].Length + 2;

                for (int j = i; j < x; j++)
                {
                    Console.Write(" ");
                }
            }

            */

            this.toolbarcolor();

            string s = "";
            const int status_x = 1;
            const int status_y = 26;
            int charcount = 0;
            int wordcount = 0;
            // int lines = 0;

            Tools.gotoxy(status_x, status_y);

            s = "[01:01]";

            Console.Write(s);

            Tools.gotoxy(status_x + 20, status_y);
            Console.Write("                              ");
            Tools.gotoxy(status_x + s.Length + 1, status_y);
            // Console.Write("Counted Characters:" + charcount);
            Console.Write("Characters: {0} Words: {1}", charcount, wordcount);

            x = 1;
            y = 2;
            // int _y = 1;

            this.textfieldcolor();

            string buffer = "";

            string[] array = new string[24];
            bool quit = false;
            // ConsoleKeyInfo cki;

            Function function = new Function();

            // bool quit = false;

            // int ch = 0;

            Window basicwindow = new Window(0, 0, 103, 26, "!Desktop Basic", this.filemenu_basicapp);
            basicwindow.background = ConsoleColor.DarkBlue;
            basicwindow.showmenubutton = true;
            basicwindow.showminimizebutton = true;
            basicwindow.showhelpbutton = true;
            basicwindow.show();

            Tools.cursor_state(true);

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            while (!quit)
            {
                // end of textfield
                Tools.gotoxy(x, y);

                key = Tools.getKey();

                // this will check the returned keystrokes by the user and determine correct output to the screen
                // if those keys were found in the string the corresponding parameters will be deleted
                // only characters will be written to screen, when key != param

                if ( key != "Enter" && key != "Spacebar" &&
                     key != "UpArrow" && key != "DownArrow" &&
                     key != "LeftArrow" && key != "RightArrow" &&
                     key != "Backspace" && key != "Tab" && key != "Delete" &&
                     key.Contains("F1") == false && key.Contains("F2") == false &&
                     key.Contains("F3") == false && key.Contains("F4") == false &&
                     key.Contains("F5") == false && key.Contains("F6") == false &&
                     key.Contains("F7") == false && key.Contains("F8") == false &&
                     key.Contains("F8") == false && key.Contains("F9") == false &&
                     key.Contains("F10") == false && key.Contains("F11") == false &&
                     key.Contains("F12") == false && key.Contains("Alt") == false &&
                     key.Contains("Escape") == false && 
                     key != "ShiftTab" && key != "ShiftF" )
                {
                    charcount++;
                    x++;
                    Console.Write(key);
                }

                if ( key == "ShiftF" ) // draw special character uppercase "F"
                {
                    Console.Write("F");
                    x++;
                }
                
                if (key == "F1") // call the about menu via hotkey function
                {
                    this.callInfoAbout();
                }

                Tools.gotoxy(1, 20);
                Console.Write(buffer);

                // handling modifier commands with specified keys in combination

                if ( key == "AltQ" )
                {

                    MessageBox msgbox = new MessageBox(basicwindow, 30, 10, 70, 17, 2, "About to quit C# Basic...", "Do you really want to quit?^^Quit right now?");

                    index = msgbox.handle();

                    if (index == 1) break;

                    if (index == 2)
                    {
                        msgbox.destroy();
                    }
                }


                if (key == "UpArrow")
                {
                    y--;
                }

                if (key == "LeftArrow")
                {
                    x--;
                }

                if (key == "RightArrow")
                {
                    x++;
                }

                if (key == "Backspace")
                {
                    if (x > 1)
                    {
                        x--;
                        Tools.gotoxy(x, y);
                        Console.Write(" ");
                        charcount--;
                    }
                }

                if (key == "Delete")
                {

                }

                if (key == "Tab")
                {
                    x += 4;
                }

                if ( key == "ShiftTab" ) 
                {
                    x -= 4;
                }

                if (key == "Spacebar")
                {
                    x++;
                    // wordcount++;
                }

                if (y < 1)
                {
                    y = 1;
                }

                if (y > 22)
                {
                    y = 23;
                }

                if (x > 100)
                {
                    x = 101;
                }

                if (x < 1)
                {
                    x = 1;
                }

                this.toolbarcolor();
                Tools.gotoxy(status_x, status_y);
                Console.Write("                              ");
                Tools.gotoxy(status_x, status_y);

                if (x < 10)
                {
                    s = $"[0{y}:0{x}]";
                }
                else
                {
                    s = $"[0{y}:{x}]";
                }

                Console.Write(s);

                Tools.gotoxy(status_x + 20, status_y);
                Console.Write("                    ");
                Tools.gotoxy(status_x + s.Length + 1, status_y);
                Console.Write("Characters: {0} Words: {1}", charcount, wordcount);

                this.textfieldcolor();

                if (key == "Enter")
                {
                    textfield[y] = buffer;

                    buffer = "";

                    y++;
                    x = 1;
                    Tools.gotoxy(status_x, status_y);

                    if (y > 22)
                    {
                        y = 23;
                    }

                    if (y < 10)
                    {
                        s = $"[0{y}:0{x}]";
                    }
                    else
                    {
                        s = $"[{y}:0{x}]";
                    }

                    this.toolbarcolor();

                    Console.Write(s);

                    this.textfieldcolor();


                }

                if (key == "AltF") // Open File menu...
                {
                    MenuBuilder fm = new MenuBuilder(2, 2, filemenu, "File");

                    //while (true)
                    //{
                    //    index = fm.getValue();
                    //    break;
                    //}

                    index = fm.getValue();

                    for (y = 0; y < textfield.Length; y++)
                    {
                        Tools.gotoxy(1, 1 + y);
                        Console.Write(textfield[y]);
                    }

                    if (index == 6)
                    {
                        MessageBox msgbox = new MessageBox( basicwindow, 30, 10, 70, 17, 2, "About to quit C# Basic...", "Do you really want to quit?^^Quit right now?");

                        fm.destroyMenu();

                        this.repaint();

                        index = msgbox.handle();

                        if (index == 1) break;

                        if (index == 2)
                        {
                            msgbox.destroy();
                        }
                    }

                    this.repaint();
                }

                if (key == "AltS")
                {
                    MenuBuilder sm = new MenuBuilder(18, 2, searchmenu, "Search");

                    while (true)
                    {
                        index = sm.getValue();
                        break;
                    }

                    this.repaint();
                }

                if (key == "AltE") // edit menu
                {
                    MenuBuilder em = new MenuBuilder(10, 2, editmenu, "Edit");

                    //while (true)
                    //{
                    //    index = em.getValue();

                    //    break;
                    //}

                    index = em.getValue();

                    this.repaint();
                }

                if (key == "AltA")
                {
                    MenuBuilder am = new MenuBuilder(28, 2, appmenu, "Application");

                    while (true)
                    {
                        index = am.getValue();
                        break;
                    }

                    this.repaint();
                }

                if (key == "AltH") // Open Help Menu
                {
                    MenuBuilder fm = new MenuBuilder(43, 2, helpmenu, "Help");
                    MessageBox msgbox = new MessageBox(basicwindow, 30, 5, 80, 15, 0, "Info about !Desktop Basic", " !Desktop Basic 1.3.0a^ Developed by Martin Steinkasserer^^ Windows Console Application Version^^ early Alpha! Release 2020");

                    while (true)
                    {
                        index = fm.getValue();

                        fm.destroyMenu();

                        if (index == 0)
                        {
                            this.repaint();

                            index = msgbox.handle();

                            if (index == 0) // OK was pressed
                            {
                                msgbox.destroy();
                            }
                        }
                        break;
                    }

                    this.repaint();
                }
            }

            Function main = new Function();

            main.run();

            /*Console.Clear();
            this.textfieldcolor();
            Console.Clear();
            */
        }

        private void repaint()
        {
            Window basicwindow = new Window(0, 0, 103, 26, "!Desktop Basic", this.filemenu_basicapp);
            basicwindow.background = ConsoleColor.DarkBlue;
            basicwindow.show();
        }

        private void toolbarcolor()
        {
            Tools.setbackground(ConsoleColor.White);
            Tools.setforeground(ConsoleColor.Black);
        }

        private void textfieldcolor()
        {
            Tools.setbackground(ConsoleColor.DarkBlue);
            Tools.setforeground(ConsoleColor.White);
        }

        private void callInfoAbout()
        {
            MessageBox msgbox = new MessageBox( basicwindow, 30, 5, 80, 15, 0, "Info about !Desktop Basic", " !Desktop Basic 1.3.0a^ Developed by Martin Steinkasserer^^ Windows Console Application Version^^ early Alpha! Release 2020");

            msgbox.handle();

            msgbox.destroy();
        }
    }
}


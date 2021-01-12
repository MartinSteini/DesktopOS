using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSCMD
{
    class EditApp
    {
        private string[] filemenu_editapp = new string[]
{
            "File", "Edit", "Search", "Application", "Help"
};

        public void run()
        {
            // ConsoleKeyInfo cki;
            string[] textfield = new string[20];
            string key = "";

            string[] filemenu = new string[]
            {
                "New file        ",
                "Open file       ",
                "-",
                "Save file       ",
                "Save file as... ",
                "-",
                "Exit C# Editor  "
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
                "Start C# Basic              ",
                "Placeholder                 ",
                "Placeholder                 ",
                "Placeholder                 ",
                "Placeholder                 ",
                "-",
                "Placeholder                 "
            };

            string[] helpmenu = new string[]
            {
                "Info about C# Basic   ",
                "-",
                "Get Help...           ",
            };

            Console.Clear();
            Tools.setbackground(ConsoleColor.DarkBlue);
            Tools.setforeground(ConsoleColor.White);
            Tools.cursor_state(true);
            Console.Clear();
            Thread.Sleep(100);

            Console.WriteLine("C# Editor 1.0.1.0 is starting...");
            Thread.Sleep(1000);
            Console.Title = "C# Editor (ALPHA-Build) 1.0.1.0";
            Console.Clear();

            int x = 0;
            int y = 0;
            int index = 0;

            Tools.setforeground(ConsoleColor.Black);
            Tools.setbackground(ConsoleColor.White);

            for (x = 0; x < 103; x++)
            {
                Tools.gotoxy(x, 0);
                Console.Write(" ");
                Tools.gotoxy(x, 24);
                Console.Write(" ");
            }

            for (x = 0; x < 24; x++)
            {
                Tools.gotoxy(0, x);
                Console.Write(" ");
                Tools.gotoxy(102, x);
                Console.Write(" ");
            }


            string appname = "C# Editor 1.0";
            Tools.gotoxy((100 - appname.Length - 1), 0);
            Console.Write(appname);

            Tools.setbackground(ConsoleColor.Red);
            Tools.setforeground(ConsoleColor.White);

            Tools.gotoxy(100, 0);
            Console.Write(" x ");


            x = 1;
            y = 0;


            // this.window1.createWindow(0, 0, 101, 24, "C# Basic 2.0", null);

            this.toolbarcolor();

            for (int i = 0; i < this.filemenu_editapp.Length; i++)
            {
                Tools.gotoxy(x + i, 0);
                Console.Write(this.filemenu_editapp[i]);
                x += this.filemenu_editapp[i].Length + 2;

                for (int j = i; j < x; j++)
                {
                    Console.Write(" ");
                }
            }

            this.toolbarcolor();

            string s = "";
            const int status_x = 1;
            const int status_y = 24;
            int wordcount = 0;

            Tools.gotoxy(status_x, status_y);

            s = "[01:01]";

            Console.Write(s);

            Tools.gotoxy(status_x + 20, status_y);
            Console.Write("          ");
            Tools.gotoxy(status_x + s.Length + 1, status_y);
            Console.Write("Counted Words:" + wordcount);


            x = 1;
            y = 1;

            this.textfieldcolor();

            while (true)
            {
                // end of textfield
                Tools.gotoxy(x, y);

                key = Tools.getKey();

                if (key != "Enter" && key != "Spacebar" &&
                     key != "UpArrow" && key != "DownArrow" &&
                     key != "LeftArrow" && key != "RightArrow" &&
                     key != "Backspace")
                {
                    wordcount++;
                    x++;
                }

                if (key == "Backspace")
                {
                    x--;
                    Tools.gotoxy(x, y);
                    Console.Write("");
                    wordcount--;
                }

                if (key == "Spacebar")
                    x++;


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
                Console.Write("               ");
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
                Console.Write("          ");
                Tools.gotoxy(status_x + s.Length + 1, status_y);
                Console.Write("Counted Words:" + wordcount);

                this.textfieldcolor();

                if (key == "Enter")
                {
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
                    MenuBuilder fm = new MenuBuilder(2, 2, filemenu, "");

                    while (true)
                    {
                        index = fm.getValue();
                        break;
                    }

                    if (index == 6)
                    {
                        MessageBox msgbox = new MessageBox(null, 30, 10, 70, 17, 2, "About to quit C# Editor...", "Do you really want to quit?^^Quit right now?");

                        fm.destroyMenu();

                        index = msgbox.handle();

                        if (index == 1) break;

                        if (index == 2)
                        {
                            msgbox.destroy();
                        }
                    }
                }

                if (key == "AltS")
                {
                    MenuBuilder sm = new MenuBuilder(15, 2, searchmenu, "");

                    while (true)
                    {
                        index = sm.getValue();
                        break;
                    }
                }

                if (key == "AltE") // edit menu
                {
                    MenuBuilder em = new MenuBuilder(8, 2, editmenu, "");

                    while (true)
                    {
                        index = em.getValue();

                        break;
                    }
                }

                if (key == "AltA")
                {
                    MenuBuilder am = new MenuBuilder(24, 2, appmenu, "");

                    while (true)
                    {
                        index = am.getValue();
                        break;
                    }
                }

                if (key == "AltH") // Open Help Menu
                {
                    MenuBuilder fm = new MenuBuilder(38, 2, helpmenu, "");
                    MessageBox msgbox = new MessageBox(null, 30, 5, 80, 15, 0, "Info about C# Basic", "C# Basic 1.0(0.4)^Developed by Martin Steinkasserer^^Windows Console Application Version^^early Alpha!");

                    while (true)
                    {
                        index = fm.getValue();

                        fm.destroyMenu();

                        if (index == 0)
                        {
                            index = msgbox.handle();

                            if (index == 0) // OK was pressed
                            {
                                msgbox.destroy();
                            }
                        }
                        break;
                    }
                }
            }

            Console.Clear();
            this.textfieldcolor();
            Console.Clear();
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
    }
}

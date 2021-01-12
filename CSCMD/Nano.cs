using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCMD
{
    class Nano
    {
        // C# based nano editor inspired by Linux
        private bool quit { get; set; }
        private string key { get; set; }
        private Window window;

        private int x { get; set; }

        private int y { get; set; }


        private string[] menu = new string[]
        {
            "_File",
            "_Help"
        };

        private string[] fmenu = new string[]
        {
            "New file ",
            "Open file",
            "-",
            "Save file",
            "-",
            "Quit     "
        };

        private string[] hmenu = new string[]
        {
            "Info       ",
            "-",
            "Get Updates"
        };
        public Nano()
        {
            this.quit = false;
            this.x = 1;
            this.y = 2;
        }

        public void run()
        {
            Console.Clear();

            window = new Window(0, 0, 103, 26, "Nano for !Desktop", menu);
            window.background = ConsoleColor.DarkBlue;
            window.show();

            Tools.cursor_state(true);
            /*
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;*/ 
            Tools.gotoxy(this.x,this.y);

            while ( !quit )
            {
                key = Tools.getKey();

                if ( key == "Enter" )
                {
                    if (this.y < 25)
                    {
                        this.y++;
                        this.x = 1;
                    }

                    Tools.gotoxy(this.x, this.y);
                }

                if ( key == "Spacebar" )
                {
                    this.x++;
                    Tools.gotoxy(this.x, this.y);
                }

                if ( key == "ShiftF" )
                {
                    Tools.gotoxy(this.x, this.y);
                    Console.Write("F");
                    this.x++;
                }

                if ( key == "Backspace" )
                {
                    this.x--;
                    Tools.gotoxy(this.x, this.y);
                }

                if ( key == "Tab" )
                {
                    this.x += 8;
                    Tools.gotoxy(this.x, this.y);
                }

                if ( key == "ShiftTab" )
                {
                    this.x -= 8;
                    Tools.gotoxy(this.x, this.y);
                }

                if ( key != "Enter" && key != "ShiftF" && 
                     key.Contains("Alt") == false && 
                     key.Contains("Space") == false &&
                     key.Contains("Tab") == false && 
                     key.Contains("Back") == false && 
                     key.Contains("Ctrl") == false && 
                     key.Contains("Shift") == false && 
                     key.Contains("Up") == false && 
                     key.Contains("Down") == false && 
                     key.Contains("Left") == false && 
                     key.Contains("Right") == false &&
                     key.Contains("F") == false )
                {
                    Tools.gotoxy(this.x, this.y);
                    Console.Write(key);
                    this.x++;
                }

                if ( key == "F1" )
                {
                    /*
                    MessageBox msgbox = new MessageBox(20, 18, 80, 25, 0, "!Desktop Nano", "This program is in early stages of development^Developed by Martin Steinkasserer^2020");
                    msgbox.show();
                    msgbox.destroy();
                    this.run(); */
                    this.showinfo();
                }

                if ( key == "AltF" ) // did the user pressed alt+f to open file menu ? 
                {
                    int index = 0;

                    MenuBuilder filemenu = new MenuBuilder(2, 3, fmenu, "");

                    while (true)
                    {
                        index = filemenu.getValue();

                        if ( index == 5 || index == 0 )
                        {
                            filemenu.destroyMenu();
                            break;
                        }
                    }

                    if ( index == 0 )
                    {
                        this.newfile();
                    }

                    if ( index == 5 )
                    {
                        index = 0;
                        string quitstr = "Do you really want to quit Nano?^^Data will be lost in case you didn't saved them!";
                        MessageBox messagebox = new MessageBox(this.window, 30, 15, 80, 22, 2, "Do you want to quit Nano?", quitstr);

                        index = messagebox.handle();
                        
                        if ( index == 1 ) 
                            Function.extern_run();

                        if ( index == 2 )
                        {
                            messagebox.destroy();
                        }
                    }

                    filemenu.destroyMenu();
                }

                if ( key == "AltH" ) // did the user pressed alt+h to open help menu ? 
                {
                    int index = 0;

                    MenuBuilder helpmenu = new MenuBuilder(10, 3, hmenu, "");

                    index = helpmenu.getValue();

                    helpmenu.destroyMenu();

                    if  ( index == 0 ) // info was pressed show the current build number of the program
                    {
                        this.showinfo();
                    }
                }

                if ( key == "AltQ" || key == "Escape" ) // if the user hit the escape key the app will close 
                    quit = true;

            }

            Function.extern_run();
        }

        private void showinfo ()
        {
            MessageBox msgbox = new MessageBox( this.window, 20, 15, 80, 23, 0, "Info . . .", "Nano for !Desktop Version 0.1^^Compiled by Martin Steinkasserer^^2020!");

            msgbox.handle();
            msgbox.destroy();
        }

        private void newfile ()
        {
            InputBox inputbox = new InputBox(10, 10, 80, 18, 1, "Filename");

        }
    }
}

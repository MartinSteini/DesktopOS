using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using WindowsInput;
using WindowsInput.Native;

namespace CSCMD
{
    class Calc
    {
        private Button[] number_buttons = new Button[17];

        private Textfield txtfield;
        private Textfield txtfieldresults;
        private int counter { get; set; }

        private string[] math_op_symbols = new string[]
        {
            "*", "+", "-", "/"
        };

        private Window window;
        private int x1 { get; set; }
        private int x2 { get; set; }
        private int y1 { get; set; }
        private int y2 { get; set; }
        private string key { get; set; }
        private string buffer { get; set; }
        private int result { get; set; }
        private int number { get; set; }
        private bool res { get; set; }
        private bool maxed { get; set; }
        private bool minimized { get; set; }
        private int calcmode { get; set; } // variable for mode of calculation...
        private bool isloaded { get; set; }
        private bool isScientist { get; set; }
        // 0 = + , 1 = -, 2 = * , 3 = / 

        private string[] filemenubar = new string[]
        {
            "_File",
            "_Settings",
            "_Help"
        };

        private string[] filemenu = new string[]
        {
            "Start a new instance",
            "-",
            "Exit                "
        };

        private string[] helpmenu = new string[]
        {
            "About         ",
            "-",
            "Get Updates..."
        };

        private string[] settingsmenu = new string[]
        {
            $"Standard        " + (char)27,
            "Scientist        ",
            "-",
            "Options          "
        };

        public Calc()
        {
            this.result = 0;
            this.calcmode = -1;
            this.key = "";
            this.maxed = false;
            this.isloaded = false;
            this.isScientist = false;
        }
        public void run()
        {
            this.repaint();
            this.mainloop();
        }
        private void repaint()
        {
            if (this.isloaded == false)
            {
                Function.ext_wallpaper(5);
                this.isloaded = true;
                Thread.Sleep(50);
                Function.ext_wallpaper(0);
            }

            if ( this.key == "$restore" || this.key == "AltR" ) 
            {
                Function.ext_wallpaper(0);
            }


            if (this.maxed == false)
            {
                this.x1 = 30;
                this.x2 = 80;
                this.y1 = 2;
                this.y2 = 24;
            }
            else if (this.maxed == true)
            {
                this.x1 = 1;
                this.x2 = 103;
                this.y1 = 1;
                this.y2 = 25;
            }

            window = new Window(this.x1, this.y1, this.x2, this.y2, "!Desktop Calculator", this.filemenubar);
            window.showhelpbutton = true;

            if (this.key != "$mhelp" )
            {
                window.show();
            }

            if (this.maxed == false)
            {
                window.showmaximizebutton = true;
            }
            else if (this.maxed == true)
            {
                window.showmaximizedbutton = true;
            }

            window.showminimizebutton = true;
            window.showmenubutton = true;
            window.background = ConsoleColor.White;
            window.shadow = false;
            // window.show();
            window.show();

            // draw the window controls

            if ( this.minimized == false )
            {

                txtfield = new Textfield(x1 + 2, y1 + 3, x2 - 1, y1 + 5, "");
                txtfield.show();
                txtfieldresults = new Textfield(this.x1 + 27, this.y1 + 6, this.x2 - 1, this.y1 + 17, "");
                txtfieldresults.show();

                int numbers = 1;
                int counter = 0;
                int tmpx = x1;
                int tmpy = y1;

                // build number buttons 1-9

                for (int j = 0; j < 9; j++)
                {

                    this.number_buttons[j] = new Button(tmpx + 3, tmpy + 6, tmpx + 7, tmpy + 9, $"{(numbers)}");

                    this.number_buttons[j].background = ConsoleColor.White;
                    this.number_buttons[j].foreground = ConsoleColor.Black;
                    this.number_buttons[j].create();

                    counter++;
                    numbers++;

                    if (counter < 3)
                    {
                        tmpx += 6;
                    }
                    else
                    {
                        tmpx = x1;
                        tmpy += 4;
                        counter = 0;
                    }
                }

                // build number button "0"
                // this.number_buttons[9] = new Button(tmpx + 3, tmpy + 6, tmpx + 19, tmpy + 9, "0");
                this.number_buttons[9] = new Button(tmpx + 9, tmpy + 6, tmpx + 13, tmpy + 9, "0");
                this.number_buttons[9].background = ConsoleColor.White;
                this.number_buttons[9].foreground = ConsoleColor.Black;
                this.number_buttons[9].create();

                // "," button
                this.number_buttons[15] = new Button(tmpx + 3, tmpy + 6, tmpx + 7, tmpy + 9, ",");
                this.number_buttons[15].background = ConsoleColor.White;
                this.number_buttons[15].foreground = ConsoleColor.Black;
                this.number_buttons[15].create();

                // "<-" button
                this.number_buttons[16] = new Button(tmpx + 15, tmpy + 6, tmpx + 19, tmpy + 9, $"{(char)27}");
                this.number_buttons[16].background = ConsoleColor.White;
                this.number_buttons[16].foreground = ConsoleColor.Black;
                this.number_buttons[16].create();

                // build math operation buttons 
                counter = 0;
                tmpx = x1;
                tmpy = y1;

                for (int i = 10; i < 14; i++)
                {
                    this.number_buttons[i] = new Button(tmpx + 21, tmpy + 6, tmpx + 25, tmpy + 9, this.math_op_symbols[counter]);
                    this.number_buttons[i].background = ConsoleColor.White;
                    this.number_buttons[i].foreground = ConsoleColor.Black;
                    this.number_buttons[i].create();
                    counter++;
                    tmpy += 4;
                }

                if (this.maxed == false)
                {
                    tmpx = this.x1 + 27;
                    tmpy = this.y2 - 4;
                }
                else if (this.maxed == true)
                {
                    tmpx = this.x1 + 27;
                    tmpy = this.y2 - 6;
                }
                // build "=" button
                this.number_buttons[14] = new Button(tmpx, tmpy, this.x2 - 1, tmpy + 3, "=");
                this.number_buttons[14].background = ConsoleColor.Green;
                this.number_buttons[14].foreground = ConsoleColor.White;
                this.number_buttons[14].create();

                Tools.cursor_state(false);
            }
        }
        private void mainloop()
        {
            this.buffer = "";
            this.key = "";
            this.number = 0;

            while ( true )
            {
                key = window.handle();

                if (key == "")
                {
                    if (this.minimized == false)
                    {
                        foreach (Button b in this.number_buttons)
                        {
                            key = b.handle();

                            if (key == "")
                            {

                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                if ( this.key == $"{(char)27}" )
                {
                    this.key = "Backspace";
                }

                if ( this.minimized == true )
                {
                    if ( key == "AltF" || key == "AltH" || key == "AltD" )
                    {
                        key = "";
                    }
                }

                if ( this.key == "$close" )
                {
                    window.lost_focus();
                    this.closecalc();
                }

                if ( this.key == "$maximize" || key == "AltU" )
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();

                    this.maxed = true;
                    this.x1 = 1;
                    this.x2 = 103;
                    this.y1 = 1;
                    this.y2 = 25;

                    window.shadow = false;
                    window.showmaximizebutton = false;
                    window.showmaximizedbutton = true;
                    window.showminimizebutton = true;
                    window.showmenubutton = true;
                    window.showhelpbutton = true;
                    this.minimized = false;
                    this.repaint();
                }

                if ( this.key == "$minimize" || this.key == "AltD" )
                {
                    //Console.Clear();
                    //Console.BackgroundColor = ConsoleColor.DarkBlue;
                    //Console.ForegroundColor = ConsoleColor.White;
                    //Console.Clear();

                    Function.ext_wallpaper(0);

                    string title = "!Desktop Calculator [minimized]";

                    this.x1 = 3;
                    this.y1 = 25;
                    this.x2 = title.Length - 1;
                    this.y2 = 25;

                    window = new Window(this.x1, this.y1, this.x2, this.y2, title, null);
                    window.shadow = false;
                    window.showclosebutton = false;
                    this.minimized = true;
                    window.isMinimized = true;

                    window.show();

                    //this.minimized = true;
                    //window.isMinimized = true;
                    //window.show();
                    //window.minimize();

                }

                if ( this.key == "$repaint" || this.key == "F5" )
                {
                    this.repaint();
                }

                if ( this.key == "$restore" || this.key == "AltR" )
                {
                    this.maxed = false;
                    this.minimized = false;
                    this.repaint();
                }

                if ( this.key == "$help" )
                {
                    window.lost_focus();
                    this.showhelp();
                }

                if ( this.key == "$mhelp" )
                {
                    this.repaint();
                    window.lost_focus();
                    this.showhelp();
                }

                if ( this.key == "$mclose" )
                {
                    window.show();
                    this.repaint();
                    window.lost_focus();
                    this.closecalc();
                }

                if ( this.key == "AltF" || this.key == "$menu0" ) this.callFileMenu();
                if ( this.key == "AltS" || this.key == "$menu1" ) this.callSettingsMenu();
                if ( this.key == "AltH" || this.key == "$menu2" ) this.callHelpMenu();
                if ( this.key == "AltQ" ) this.closecalc();
                if ( this.key == "AltN" ) this.newCalc();
                if ( this.key == "F1" ) this.showhelp();

                if ( this.key == "0" || this.key == "1" || this.key == "2" || this.key == "3" || 
                     this.key == "4" || this.key == "5" || this.key == "6" || this.key == "7" ||
                     this.key == "8" || this.key == "9" || this.key == "+" || this.key == "-" ||
                     this.key == "/" || this.key == "*" || this.key == "Enter" ||
                     this.key == "Backspace" || this.key == "," ) 
                {

                    if ( this.key == "Enter" ) 
                    {
                        this.number_buttons[14].Click();

                        if ( this.calcmode == 0 ) // plus 
                        {
                            try
                            {
                                this.result += Int32.Parse(this.buffer);
                            }

                            catch ( Exception ) { }

                            this.txtfield.clear();
                            this.txtfieldresults.setContent2("+" + this.buffer);
                            this.txtfield.setContent(0, $"{(this.result)}");

                            this.calcmode = -1;
                            this.buffer = "";
                            this.res = true;
                        }

                        if ( this.calcmode == 1 ) // minus
                        {
                            try
                            {
                                this.result -= Int32.Parse(this.buffer);
                            }

                            catch (Exception) { }

                            this.txtfield.clear();
                            this.txtfieldresults.setContent2("-" + this.buffer);
                            this.txtfield.setContent(0, $"{(this.result)}");

                            this.calcmode = -1;
                            this.buffer = "";
                            this.res = true;
                        }

                        if ( this.calcmode == 2 ) // multiply
                        {
                            try
                            {
                                this.result *= Int32.Parse(this.buffer);
                            }

                            catch (Exception) { }

                            this.txtfield.clear();
                            this.txtfieldresults.setContent2("*" + this.buffer);
                            this.txtfield.setContent(0, $"{(this.result)}");

                            this.calcmode = -1;
                            this.buffer = "";
                            this.res = true;
                        }

                        if ( this.calcmode == 3 ) // divide
                        {
                            try
                            {
                                this.result /= Int32.Parse(this.buffer);
                            }

                            catch (Exception) { }

                            this.txtfield.clear();
                            this.txtfieldresults.setContent2(":" + this.buffer);
                            this.txtfield.setContent(0, $"{(this.result)}");

                            this.calcmode = -1;
                            this.buffer = "";
                            this.res = true;
                        }

                    }
                    else if ( this.key == "+" || this.key == "-" || this.key == "/" || this.key == "*" ||
                              this.key == "Backspace" || this.key == "," )
                    {

                        if (this.key == ",")
                        {
                            this.number_buttons[15].Click();
                        }

                        if (this.key == "Backspace" || this.key == $"{(char)27}" )
                        {

                            this.number_buttons[16].Click();

                                string tmp = "";

                                for (int i = 0; i < this.buffer.Length - 1; i++)
                                {
                                    tmp += this.buffer[i];
                                }

                                this.buffer = tmp;

                                this.txtfield.clear();

                                this.txtfield.setContent(0, this.buffer);
                        }


                        if (this.key == "+") // plus 
                        {
                            // this.math_op_buttons[1].Click();
                            this.number_buttons[11].Click();
                            this.txtfield.clear();

                            if (this.res == false && this.buffer.Length > 0 )
                            {
                                this.result = Int32.Parse(this.buffer);
                             
                                this.txtfieldresults.setContent2("+"+this.buffer);
                            }
                            this.buffer = "";

                            this.calcmode = 0;
                        }

                        if ( this.key == "-" ) // minus
                        {
                            this.number_buttons[12].Click();
                            this.txtfield.clear();

                            if (this.res == false && this.buffer.Length > 0)
                            {
                                this.result = Int32.Parse(this.buffer);

                                this.txtfieldresults.setContent2("-" + this.buffer);
                            }

                            this.buffer = "";
                            this.calcmode = 1;
                        }

                        if (this.key == "/") // divide
                        {
                            this.number_buttons[13].Click();

                            this.txtfield.clear();


                            if (this.res == false && this.buffer.Length > 0)
                            {
                                this.result = Int32.Parse(this.buffer);

                                this.txtfieldresults.setContent2(":" + this.buffer);
                            }
                            this.buffer = "";
                            this.calcmode = 3;
                        }

                        if ( this.key == "*" ) // multiply
                        {
                            this.number_buttons[10].Click();

                            this.txtfield.clear();

                            if (this.res == false && this.buffer.Length > 0)
                            {
                                this.result = Int32.Parse(this.buffer);

                                this.txtfieldresults.setContent2("*" + this.buffer);
                            }
                            this.buffer = "";
                            this.calcmode = 2;
                        }
                    }
                    else
                    {
                        this.number = Int32.Parse(this.key);

                        if ( this.number == 0 )
                        {
                            this.number_buttons[9].Click();
                        }
                        else if ( this.number > 0 && number < 10 )
                        {
                            this.number_buttons[this.number - 1].Click();
                        }

                        this.buffer += this.key;
                        this.txtfield.setContent(0, this.key);
                    }
                }
            }
        }
        private void closecalc()
        {
            int index = 0;

            MessageBox messagebox = new MessageBox(this.window, 36, 12, 75, 20, 2,
            "Quitting Calculator...",
            "^Do you want to quit?^Back to !Desktop Terminal ?^>_");

            index = messagebox.handle();

            if (index == 1)
            {
                Window.ext_run();
            }
            else
            {
                this.repaint();
            }
        }

        private void callFileMenu ()
        {
            int index = 0;
            
            int x = 0;
            int y = 0;

            if ( this.maxed == false )
            {
                x = 32;
                y = 4;
            }
            else if ( this.maxed == true )
            {
                x = 3;
                y = 3;
            }

            MenuBuilder tmp = new MenuBuilder(x, y, this.filemenu, "File");

            // check if the command was sent from a key 

            if ( this.key == "AltF" )
            {
                tmp.is_key = true;
            }

            MessageBox messagebox = new MessageBox(this.window, 36, 12, 75, 19, 2,
                "Quitting Calculator...",
                "^Do you want to quit?^Back !Desktop Terminal ?");

            while (true)
            {
                // index = tmp.getValue();
                index = tmp.handle();

                if (index != 99 && index != 100) break;
            }

            if (index == -1)
            {
                // window.show();
                window.show();

                this.key = "$close";
                this.repaint();
            }

            if (index == 0)
            {
                // this.repaint();
                // window.show();
                window.show();

                this.key = "$close";
                this.newCalc();
            }

            if (index == 2)
            {
                // window.show();
                window.show();
                this.key = "$close";
                this.repaint();
                this.closecalc();
            }
        }

        private void callHelpMenu ()
        {
            int index = 0;
            int x = 0;
            int y = 0;

            if ( this.maxed == false )
            {
                x = 52;
                y = 4;
            }
            else if ( this.maxed == true )
            {
                x = 23;
                y = 3;
            }

            MenuBuilder tmp = new MenuBuilder(x, y, this.helpmenu, "Help");

            if ( this.key == "AltH" )
            {
                tmp.is_key = true;
            }

            while (true)
            {
                index = tmp.handle();

                if (index == 0)
                {
                    this.repaint();
                    this.showhelp();
                    break;
                }

                if ( index == 2 ) // get help 
                {
                    this.repaint();

                    ReleaseNotes rn = new ReleaseNotes(this.window, this.x1 + 2, this.y1 + 4,
                        this.x2 - 1, this.y2 - 2);

                    rn.create();
                    this.repaint();
                    break;
                }

                if ( index == -1 ) // esc key 
                {
                    this.repaint();
                    break;
                }
            }
        }

        private void cleardata ()
        {
            this.txtfield.clear();
            this.txtfield.setContent(0, "Clearing data...");
            this.txtfieldresults.clear();
            this.txtfield.clear();
            this.result = 0;
            this.counter = 0;
        }

        private void showhelp ()
        {
            MessageBox messagebox = new MessageBox(this.window, 36, 12, 75, 21, 0,
            "About !Desktop Calculator...",
            "^Version 3.0a ^Developed by Martin Steinkasserer^2020^>_^");
            messagebox.handle();
            this.repaint();
        }

        private void newCalc()
        {
            if ( this.maxed == false )
            {
                Calc calc = new Calc();
                calc.run();
            }
            else if ( this.maxed == true )
            {
                Calc calc = new Calc();
                calc.maxed = true;
                calc.run();
            }
        }
        private void callSettingsMenu()
        {
            int index = 0;
            int x = 0;
            int y = 0;

            if (this.maxed == false)
            {
                x = 40;
                y = 4;
            }
            else if (this.maxed == true)
            {
                x = 11;
                y = 3;
            }

            MenuBuilder tmp = new MenuBuilder(x, y, this.settingsmenu, "Settings");

            if ( this.key == "AltS" )
            {
                tmp.is_key = true;
            }

            while (true)
            {
                index = tmp.handle();

                if (index == -1) // escape sequence was initiated 
                {
                    this.key = "$close";
                    this.repaint();
                    break;
                }

                if ( index == 0 ) // standard mode
                {
                    this.settingsmenu[0] = "Standard        " + (char)27;
                    this.settingsmenu[1] = "Scientist        ";
                    this.isScientist = false;
                }

                if ( index == 1 ) // scientist mode
                {
                    this.settingsmenu[0] = "Standard         ";
                    this.settingsmenu[1] = "Scientist       " + (char)27;
                    this.isScientist = true;
                }

                if ( index == 3 ) // options window 
                {
                    string[] file_menu = new string[]
                    {
                        "_File",
                    };

                    Window options_window = new Window(this.x1 + 2, this.y1 + 3, this.x2 - 2, this.y2 - 5,
                        "Options for !Desktop Calculator",
                        file_menu);

                    this.repaint();
                    
                    this.window.lost_focus();

                    options_window.showcontextmenu = false;
                    options_window.show();

                    while (true)
                    {
                        this.key = options_window.handle();

                        if ( this.key == "$close" )
                        {
                            break;
                        }
                    }

                    index = this.settingsmenu.Length - 1;
                }

                if ( index > -1 && index < this.settingsmenu.Length ) 
                {
                    // this.key = "$close";
                    this.repaint();
                    this.key = "";
                    break;
                }
            }
        }
        private void savedata ()
        {
            // code to save data before any menu has been called.
        }

        private void restoredata ()
        {
            // code to restore data from savedata method
        }
    }
}

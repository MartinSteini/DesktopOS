/*
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

namespace CSCMD
{
    class Calc
    {
        private Button[] number_buttons = new Button[10];
        private Button[] math_op_buttons = new Button[4];
        private Button result_button;
        private int oplength { get; set; }

        private Textfield txtfield;
        private Textfield txtfieldresults;

        private string[] operations = new string[10];
        private int result { get; set; }

        private string buffer;

        private bool clearflag;

        private string[] math_op_symbols = new string[]
        {
            "*", "+", "-", "/"
        };

        private bool updatescreen;

        private Window window;
        private int x1 { get; set; }
        private int x2 { get; set; }
        private int y1 { get; set; }
        private int y2 { get; set; }
        private string key;
        private int op { get; set; }

        private Calc newCalc;

        private string[] filemenubar = new string[]
        {
            "File", "Help"
        };

        private string[] filemenu = new string[]
        {
            "New Calculations",
            "-",
            "Exit            "
        };

        private string[] helpmenu = new string[]
        {         "Info about C# Calculator",
            "-",
            "Get Help...             "
        };

        public Calc()
        {
            this.key = null;
            this.buffer = "";
            this.clearflag = false;
            this.updatescreen = false;
        }

        public void run ()
        {
            this.repaint();
            this.mainloop();
        }

        private void resetMenues()
        {
            Tools.gotoxy(38, 2);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(this.filemenubar[1]);

            Tools.gotoxy(31, 2);
            Console.Write(this.filemenubar[0]);
        }
        private void repaint()
        {
            this.x1 = 30;
            this.x2 = 80;
            this.y1 = 1;
            this.y2 = 23;

            window = new Window(this.x1, this.y1, this.x2, this.y2, "C# Calculator", this.filemenubar);
            window.show();
            txtfield = new Textfield(x1 + 2, y1 + 3, x2 - 1, y1 + 5, "");
            txtfield.show();
            txtfieldresults = new Textfield(this.x1 + 27, this.y1 + 6, this.x2 - 1, this.y1 + 17, "");
            txtfieldresults.show();

            int numbers = 1;
            int counter = 0;
            int tmpx = x1;
            int tmpy = y1;

            for (int j = 0; j < number_buttons.Length - 1; j++)
            {

                this.number_buttons[j] = new Button(tmpx + 3, tmpy + 6, tmpx + 7, tmpy + 9, $"{(numbers)}");

                this.number_buttons[j].show();

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

            // build button for number zero 

            this.number_buttons[9] = new Button(tmpx + 3, tmpy + 6, tmpx + 20, tmpy + 9, "0");

            this.number_buttons[9].show();

            // build math operation buttons 

            tmpx = x1;
            tmpy = y1;

            for (int i = 0; i < this.math_op_buttons.Length; i++)
            {
                this.math_op_buttons[i] = new Button(tmpx + 22, tmpy + 6, tmpx + 25, tmpy + 9, this.math_op_symbols[i]);

                this.math_op_buttons[i].show();

                tmpy += 4;
            }

            // build result button 

            this.result_button = new Button(57, 19, this.x2 - 2, 22, "=");
            this.result_button.show();

            int index = 0;
            int mindex = 0;
            bool mathop;

            Tools.cursor_state(false);

            Thread.Sleep(50);
        }

        private void mainloop ()
        {
            int result = 0;
            string key = null;
            int number = 0;
            bool setinput = false;
            int mindex = 0;
            string buffer = null;
            bool addition = false;

            while ( true )
            {
                key = Tools.getKey();

                if (key.Contains("AltF") == true)
                {
                    setinput = false;
                    MenuBuilder filemenu = new MenuBuilder(31, 4, this.filemenu, "");
                    mindex = filemenu.getValue();

                    if ( mindex == -1 )
                    {
                        this.repaint();
                    }
                }

                if ( key.Contains("Enter") == true || key.Contains("Alt") == true || key.Contains("Shift") == true )
                {
                    number = 11;
                    key = "";
                    setinput = false;
                }
                else
                {
                    if ( key == "+" )
                    {
                        addition = true;
                    }

                    setinput = true;
                }

                try
                {
                    number = Int32.Parse(key);
                }
                catch ( Exception ex ) { }

                this.txtfield.setContent(0, number.ToString());

                if (setinput == true)
                {

                    if (number > -1 && number < 10)
                    {
                        if (number == 0)
                        {
                            this.number_buttons[9].Click();
                        }
                        else
                        {
                            this.number_buttons[number - 1].Click();
                        }
                    }

                    buffer += key;
                    this.txtfield.setContent(0, key);

                    if ( addition == true )
                    {

                    }

                }
            }
        }
    }
}
*/


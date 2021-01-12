using System;
using System.Collections;
//using System.Collections;
//using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WindowsInput;

namespace CSCMD
{
    class Button
    {
        private int x1 { get; set; }
        private int x2 { get; set; }
        private int y1 { get; set; }
        private int y2 { get; set; }
        private bool debug { get; set; }
        public int delay { get; set; }
        public string caption { get; set; }
        public bool shadow { get; set; }
        public bool buttonstyle3d { get; set; }
        public ConsoleColor background { get; set; }
        public ConsoleColor foreground { get; set; }
        private ConsoleColor hovercolor { get; set; }
        private ConsoleColor shortkeycolor { get; set; }
        public bool hover { get; set; }
        public string sendmessage { get; set; }

        struct COORDS
        {
            public int x1;
            public int y1;
            public int x2;
            public int y2;
        }

        private COORDS coords;
        public Button( int x1, int y1, int x2, int y2, string caption )
        {
            this.caption = caption;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.shadow = false;
            this.delay = 75;
            this.shortkeycolor = ConsoleColor.Black;
            this.hover = false;
            this.sendmessage = "";
            this.coords.x1 = this.x1 * 13;
            this.coords.x2 = this.x2 * 13;
            this.coords.y1 = this.y1 * 28;
            this.coords.y2 = this.y2 * 28;
            this.debug = false;
            this.buttonstyle3d = false;

            if ( this.coords.y1 == this.coords.y2 )
            {
                this.coords.y2 = this.coords.y1 + 2 * 28;
            }
        }

        #region oldversion of creating buttons
        public void create2 ()
        {
            char ch = ' ';

            string buffer = "";
            int counter = 0;
            ArrayList inlay = new ArrayList();

            int dx = 0;
            int xc = 0;

            this.shadow = shadow;

            Console.BackgroundColor = this.background;
            Console.ForegroundColor = this.foreground;

            if (this.y1 != this.y2) // check if the y coordinates are at the same line 
            {

                for (int x = this.x1; x < this.x2; x++)
                {
                    for (int y = this.y1; y < this.y2; y++)
                    {
                        Tools.gotoxy(x, y);
                        Console.Write(" ");
                    }
                }
            }
            else // otherwise do this method to determine correct button drawings 
            {
                for (int x = this.x1; x < this.x2; x++)
                {
                    Tools.gotoxy(x, this.y1 + 1);
                    Console.Write(" ");
                }
            }

            if ( this.shadow == true && this.y1 != this.y2 )
            {
                Tools.shadowcolor();

                for (int x = x1 + 1; x < x2; x++)
                {
                    Tools.gotoxy(x, y2);
                    Console.Write(" ");
                }

                for (int y = y1 + 1; y < y2 + 1; y++)
                {
                    Tools.gotoxy(x2, y);
                    Console.Write(" ");
                }
            }
            else if ( shadow == true && this.y1 == this.y2 ) 
            {
                Tools.shadowcolor();

                for (int x = x1 + 1; x < x2+1; x++)
                {
                    Tools.gotoxy(x, this.y2+2);
                    Console.Write(" ");
                }

                Console.SetCursorPosition(this.x2, this.y1 + 1);
                Console.Write(" ");
            }


            // Tools.buttoncolor();

            dx = ((x2 - caption.Length + x1) / 2);

            for (int i = 0; i < this.caption.Length; i++)
            {
                ch = this.caption[i];

                if (ch != '_')
                {
                    Tools.gotoxy(dx + xc, y1 + 1);
                    Console.Write(ch);
                    xc++;
                }
                else if (ch == '_')
                {
                    xc++;
                    ch = this.caption[i + 1];
                    Console.ForegroundColor = this.foreground;
                    Tools.gotoxy(dx + xc, y1 + 1);
                    Console.Write(ch);
                    xc++;
                    i++;
                    Console.ForegroundColor = this.foreground;
                }
            }
        }

        #endregion
        public string handle ()
        {
            POINT point = Mouse.getMousePosition();
            InputSimulator mHandle = new InputSimulator();

            if ( point.X > this.coords.x1 && point.X < this.coords.x2 &&
                    point.Y > this.coords.y1 && point.Y < this.coords.y2 )
            {
                var handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);
                var record = new NativeMethods.INPUT_RECORD();
                // int mode = 0;
                uint recordLen = 0;

                // if (!(NativeMethods.GetConsoleMode(handle, ref mode))) { throw new Win32Exception(); }
                // if (!(NativeMethods.SetConsoleMode(handle, NativeMethods.ENABLE_MOUSE_INPUT))) { throw new Win32Exception(); }
                if (!(NativeMethods.ReadConsoleInput(handle, ref record, 1, ref recordLen))) { throw new Win32Exception(); }

                if (record.MouseEvent.dwButtonState != 1 )
                {
                    handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);
                    record = new NativeMethods.INPUT_RECORD();
                    recordLen = 0;
                    if (!(NativeMethods.ReadConsoleInput(handle, ref record, 1, ref recordLen))) { throw new Win32Exception(); }
                }

                if (this.debug == true)
                {
                    Tools.gotoxy(0, 1);
                    Console.WriteLine(record);
                    Tools.gotoxy(0, 2);
                    Console.WriteLine(recordLen);
                    Tools.gotoxy(0, 3);
                    Console.WriteLine(record.MouseEvent.dwButtonState);
                }

                if (this.hover == false)
                {
                    if (this.caption == "=")
                    {
                        this.background = ConsoleColor.White;
                        this.foreground = ConsoleColor.Green;
                    }
                    else
                    {
                        this.background = ConsoleColor.Black;
                        this.foreground = ConsoleColor.White;
                    }

                    this.create();
                    this.hover = true;

                    Thread.Sleep(20);
                }

                Mouse.sendMouseUp();
                
                if (record.MouseEvent.dwButtonState == 1)
                {
                    // Thread.Sleep(20);

                    if (this.caption != "=")
                    {
                        return this.caption;
                    }
                    else
                    {
                        return "Enter";
                    }

                }

                if ( record.KeyEvent.bKeyDown == true ) 
                {
                    if ( record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Add )
                    {
                        return "+";
                    }

                    if ( record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Multiply )
                    {
                        return "*";
                    }

                    if ( record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.OemMinus )
                    {
                        return "-";
                    }

                    if ( record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Divide )
                    {
                        return "/";
                    }
                }
            }
            else if ( point.X != this.coords.x1 && point.X != this.coords.x2 &&
                        point.Y != this.coords.y1 && point.Y != this.coords.y2 )
            {

                if (this.hover == true)
                {
                    if (this.caption == "=")
                    {
                        this.background = ConsoleColor.Green;
                        this.foreground = ConsoleColor.White;
                    }
                    else
                    {
                        this.background = ConsoleColor.White;
                        this.foreground = ConsoleColor.Black;
                    }

                    this.create();
                    this.hover = false;
                    
                    Thread.Sleep(20);
                }
            }
            return "";
        }
        public void show ()
        {
            char ch = ' ';

            int dx = 0;
            int xc = 0;

            this.shadow = shadow;
            
            if (this.background == 0 && this.foreground == 0)
            {
                Tools.buttoncolor();
            }
            else
            {
                Console.BackgroundColor = this.background;
                Console.ForegroundColor = this.foreground;
            }

            if (this.y1 != this.y2) // check if the y coordinates are at the same line 
            {

                for (int x = this.x1; x < this.x2; x++)
                {
                    for (int y = this.y1; y < this.y2; y++)
                    {
                        Tools.gotoxy(x, y);
                        Console.Write(" ");
                    }
                }
            }
            else // otherwise do this method to determine correct button drawings 
            {
                for ( int x = this.x1; x < this.x2; x++ )
                {
                    Tools.gotoxy(x, this.y1+1);
                    Console.Write(" ");
                }
            }

            if ( this.shadow == true )
            {
                Tools.shadowcolor();

                for (int x = x1 + 1; x < x2; x++)
                {
                    Tools.gotoxy(x, y2);
                    Console.Write(" ");
                }

                for (int y = y1 + 1; y < y2 + 1; y++)
                {
                    Tools.gotoxy(x2, y);
                    Console.Write(" ");
                }
            }

            // Tools.buttoncolor();

            dx = ((x2 - caption.Length + x1) / 2);

            for ( int i = 0; i < this.caption.Length; i++ )
            {
                ch = this.caption[i];

                if ( ch != '_' )
                {
                    Tools.gotoxy(dx + xc, y1 + 1);
                    Console.Write(ch);
                    xc++;
                }
                else if ( ch == '_' )
                {
                    xc++;
                    ch = this.caption[i+1];
                    Console.ForegroundColor = this.shortkeycolor;
                    Tools.gotoxy(dx + xc, y1 + 1);
                    Console.Write(ch);
                    xc++;
                    i++;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
            }
        }

        public void create ()
        {
            string buffer = "";
            ArrayList inlay = new ArrayList();
            int counter = 0;
            int dx = 0;
            int dy = 0;

            if ( this.y1 != this.y2 ) 
            {
                counter = this.y1;

                while ( counter < this.y2 )
                {
                    for ( int x = this.x1; x < this.x2; x++ )
                    {
                        buffer += " ";
                    }
                    inlay.Add(buffer);
                    buffer = "";
                    counter++;
                }

                // calculating x and y delta of the button
                dx = (this.x2 - this.caption.Length + this.x1) / 2;
                dy = (this.y2 - 1 + this.y1) / 2;
                counter = this.y1;
            }
            else if ( this.y1 == this.y2 ) 
            {
                // code for one lined button
                for ( int x = this.x1; x < this.x2; x++ )
                {
                    buffer += " ";
                }
                inlay.Add(buffer);
                buffer = "";
                counter = this.y1 + 1;

                dy = this.y1 + 1;
                dx = (this.x2 - this.caption.Length + this.x1) / 2;
            }

            Console.BackgroundColor = this.background;
            Console.ForegroundColor = this.foreground;

            foreach ( string s in inlay )
            {
                Console.SetCursorPosition(this.x1, counter);
                Console.Write(s);
                
                counter++;
            }

            Console.SetCursorPosition(dx, dy);
            Console.Write(this.caption);
        }

        public void destroy ()
        {
            Console.BackgroundColor = ConsoleColor.Blue;

            int counter = 0;
            string buffer = "";
            ArrayList inlay = new ArrayList();

            counter = this.y1;

            if ( this.y1 != this.y2 )
            {
                while ( counter < this.y2 )
                {
                    for ( int x = this.x1; x < this.x2; x++ )
                    {
                        buffer += " ";
                    }
                    inlay.Add(buffer);
                    buffer = "";
                    counter++;
                }
            }
            else if ( this.y1 == this.y2 ) 
            {
                for ( int x = this.x1; x < this.x2; x++ )
                {
                    buffer += " ";
                }
                inlay.Add(buffer);
            }

            counter = this.y1;

            foreach ( string s in inlay )
            {
                Console.SetCursorPosition(this.x1,counter);
                Console.Write(s);
                counter++;
            }
        }

        public void Click ()
        {
            if ( this.buttonstyle3d == true )
            {
                bool oldvalue = this.shadow;

                this.destroy();

                this.x1++;
                this.y1++;
                this.x2++;
                this.y2++;
                this.shadow = false;
                this.create();

                Thread.Sleep(this.delay);

                this.destroy();
                this.shadow = oldvalue;
                this.x1--;
                this.y1--;
                this.x2--;
                this.y2--;

                this.create();

                Thread.Sleep(this.delay);
            }
            else // flat style
            {
                if (this.caption != "=")
                {
                    this.background = ConsoleColor.White;
                    this.foreground = ConsoleColor.Black;
                    this.create();
                    Thread.Sleep(this.delay);

                    this.background = ConsoleColor.Black;
                    this.foreground = ConsoleColor.White;
                    this.create();
                    Thread.Sleep(this.delay);

                    if ( this.hover == false )
                    {
                        this.background = ConsoleColor.White;
                        this.foreground = ConsoleColor.Black;
                        this.create();
                    }
                }
                else
                {
                    this.background = ConsoleColor.Green;
                    this.foreground = ConsoleColor.White;
                    this.create();
                    Thread.Sleep(this.delay);

                    this.background = ConsoleColor.White;
                    this.foreground = ConsoleColor.Green;
                    this.create();
                    Thread.Sleep(this.delay);

                    if ( this.hover == false )
                    {
                        this.background = ConsoleColor.Green;
                        this.foreground = ConsoleColor.White;
                        this.create();
                    }
                }
            }
        }
    }


}

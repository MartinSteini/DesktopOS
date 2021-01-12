using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSCMD
{
    class MenuBuilder
    {
        private bool detectkey { get; set; }
        private int index { get; set; }
        private int length { get; set; }
        private int arraylength { get; set; }
        private int saveindex { get; set; }
        // private ConsoleColor forecolor;
        private ConsoleColor backcolor;
        private ConsoleColor itemcolor { get; set; }
        private ConsoleColor backgrounditemcolor { get; set; }
        private int maxlength;
        // private Window window;

        private int delay { get; set; }
        private int x { get; set; }
        private int y { get; set; }
        public bool is_key { get; set; }
        private string[] _array { get; set; }
        private string title { get; set; }
        public bool shadow { get; set; }
        public bool osflag;
        public int selectMenu { get; set; }
        private bool isinitialized { get; set; }
        public bool systemmenu { get; set; }
        public bool isexpaned { get; set; }
        private bool debug { get; set; }
        private bool is_menu_open { get; set; }
        private char c { get; set; }
        struct COORDS
        {
            public int x1;
            public int x2;
            public int y1;
            public int y2;
        }

        public enum Animation
        {
            NoAnimation = 0,
            Upwards = 1,
            Downwards = 2,
            Sidewards_Left = 3,
            Sidewards_Right = 4,
            Win98_Mode = 5
        };

        private Animation _animation;

        public void setAnimation_Direction ( Animation in_animation )
        {
            this._animation = in_animation;
        }

        public Animation getAnimation_Direction ()
        {
            return this._animation;
        }

        public MenuBuilder( int x, int y, string[] _array, string title)
        {
            this.index = 0;
            this.maxlength = 0;
            this.backcolor = ConsoleColor.DarkBlue;
            this.itemcolor = ConsoleColor.Black;
            this.x = x;
            this.y = y;
            this._array = _array;
            this.title = title;
            this.shadow = false;
            this.osflag = false;
            this.isinitialized = false;
            this.arraylength = 0;
            this.systemmenu = false;
            this.detectkey = false;
            this.delay = 150;
            this.c = (char)26; // character sign for the detecting of the menu 
            this.debug = false;
            this._animation = MenuBuilder.Animation.Downwards;
            this.is_key = false;
            this.is_menu_open = false;
        }

        #region Version 2.0

        public int handle()
        {
            bool quit = false;
            POINT point;
            COORDS[] coords = new COORDS[this._array.Length]; 

            if ( this.isinitialized == false )
            {
                this.Update_Menu();

                for (int i = 0; i < this._array.Length; i++)
                {
                    if ( this._array[i].Contains("-") == false )
                    {
                        coords[i].x1 = this.x * 13;
                        coords[i].x2 = (this.x + this._array[i].Length) * 13;
                        coords[i].y1 = (this.y + i) * 28;
                        coords[i].y2 = (this.y + i + 1) * 28;
                    }
                }
                this.isinitialized = true;
            }

            var handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);
            int mode = 0;

            if (!(NativeMethods.GetConsoleMode(handle, ref mode))) { throw new Win32Exception(); }

            mode |= NativeMethods.ENABLE_MOUSE_INPUT;
            mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;
            mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;

            var record = new NativeMethods.INPUT_RECORD();
            uint recordLen = 0;

            if (!(NativeMethods.SetConsoleMode(handle, NativeMethods.ENABLE_MOUSE_INPUT))) { throw new Win32Exception(); }

            while ( !quit )
            {
                if (!(NativeMethods.ReadConsoleInput(handle, ref record, 1, ref recordLen))) { throw new Win32Exception(); }

                if ( record.EventType == NativeMethods.KEY_EVENT )
                {
                    if ( record.KeyEvent.bKeyDown == true )
                    {
                        this.debug = false;

                        if (this.debug == true)
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.WriteLine(this.selectMenu);
                        }

                        if ( record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.LeftArrow )
                        {
                            if ( this.selectMenu > 0 )
                            {
                                this.selectMenu--;
                            }
                            else
                            {
                                this.selectMenu = 0;
                            }

                            this.detectkey = true;
                        }

                        if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.RightArrow ) 
                        {
                            if ( this.selectMenu < 4 )
                            {
                                this.selectMenu++;
                            }
                            else
                            {
                                this.selectMenu = 4;
                            }
                        }

                        if ( record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.DownArrow )
                        {
                            if ( this.index < length)
                            {
                                this.index++;
                            }

                            if ( this._array[index].Contains("-") == true )
                            {
                                this.index++;
                            }
                            this.detectkey = true;
                            this.Update_Menu();
                            Thread.Sleep(100);
                        }

                        if ( record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.UpArrow )
                        {
                            if ( this.index > 0 )
                            {
                                this.index--;
                            }

                            if ( this._array[index].Contains("-") == true )
                            {
                                this.index--;
                            }

                            this.detectkey = true;
                            
                            this.Update_Menu();
                            Thread.Sleep(100);
                        }

                        if ( record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Enter || 
                             record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Spacebar )
                        {

                            if ( this._array[index].Contains(this.c) == false ||
                                 this._array[index].Contains(this.c) == true ) 
                            {
                                Console.BackgroundColor = ConsoleColor.White;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.SetCursorPosition(this.x, this.y + index);
                                Console.Write(this._array[index].ToUpper());
                                Thread.Sleep(this.delay);

                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.SetCursorPosition(this.x, this.y + index);
                                Console.Write(this._array[index].ToUpper());
                                Thread.Sleep(this.delay);
                            }

                            return this.index;
                        }

                        if ( record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Escape ) 
                        {
                            return -1;
                        }
                    }
                }
                else if ( record.EventType == NativeMethods.MOUSE_EVENT )
                {
                    point = Mouse.getMousePosition();

                    this.debug = false;

                    // code for destroying menu by left click
                    
                    if ( point.X < coords[0].x1 || point.X > coords[0].x2 ||
                         point.Y < coords[0].y1 || point.Y > coords[coords.Length-1].y2 ) 
                    {
                        if ( record.MouseEvent.dwButtonState == 1 )
                        {
                            return -1;
                        }
                    }

                    // code for checking menu data and point with cursor on it or keyboard

                    for ( int i = 0; i < this._array.Length; i++ )
                    {
                        if ( point.X > coords[i].x1 && point.X < coords[i].x2 &&
                             point.Y > coords[i].y1 && point.Y < coords[i].y2 )
                        {

                            if (this.debug == true)
                            {
                                Console.SetCursorPosition(0, 0);
                                Console.Write("                                           ");
                                string str = $"X: {point.X}\t Y: {point.Y}";
                                Console.SetCursorPosition(0, 0);
                                Console.Write(str);

                                Console.SetCursorPosition(0, 2);
                                Console.Write($"X1: {coords[i].x1}");
                                Console.SetCursorPosition(0, 4);
                                Console.Write($"X2: {coords[i].x2}");

                                Console.SetCursorPosition(0, 6);
                                Console.Write($"Y1: {coords[i].y1}");
                                Console.SetCursorPosition(0, 8);
                                Console.Write($"Y2: {coords[i].y2}");

                                Console.SetCursorPosition(0, 10);
                                Console.Write($"Array Length: {coords.Length}");
                    
                                Console.SetCursorPosition(0, 12);
                                Console.Write($"Selected Array Index: {i}");
                                
                                Console.SetCursorPosition(0, 14);
                                Console.Write("                                      ");
                                Console.SetCursorPosition(0, 14);
                                Console.Write($"Selected Array Name: {this._array[i]}");
                            }

                            if ( this._array[i].Contains("-") == false )
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                                Tools.gotoxy(this.x, this.y + i);

                                Console.Write(this._array[i].ToUpper());
                            }

                            if ( record.MouseEvent.dwButtonState == 1 ) 
                            {
                                if (this._array[i].Contains(this.c) == false ||
                                     this._array[i].Contains(this.c) == true)
                                {
                                    Console.BackgroundColor = ConsoleColor.White;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.SetCursorPosition(this.x, this.y + i);
                                    Console.Write(this._array[i].ToUpper());
                                    Thread.Sleep(this.delay);

                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.SetCursorPosition(this.x, this.y + i);
                                    Console.Write(this._array[i].ToUpper());
                                    Thread.Sleep(this.delay);
                                }

                                quit = true;
                                return i;
                            }
                        }
                        else if (point.X != coords[i].x1 && point.X != coords[i].x2 &&
                                  point.Y != coords[i].y1 && point.Y != coords[i].y2)
                        {
                            if (this._array[i].Contains("-") == false)
                            {
                                Console.BackgroundColor = ConsoleColor.White;
                                Console.ForegroundColor = ConsoleColor.Black;

                                Tools.gotoxy(this.x, this.y + i);

                                Console.Write(this._array[i]);
                            }
                        }
                    }
                }
            }
            return 0;
        }
        private void Update_Menu () 
        {
            if ( this.is_key == true && this.is_menu_open == false ) 
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(this.x, this.y - 1);
                Console.Write(this.title.ToUpper());
                Thread.Sleep(this.delay - 100);

                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                Console.SetCursorPosition(this.x, this.y - 1);
                Console.Write(this.title.ToUpper());
                Thread.Sleep(this.delay);

                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(this.x, this.y - 1);
                Console.Write(this.title.ToUpper());

                this.is_menu_open = true;
            }

            this.create();

            Console.BackgroundColor = this.itemcolor;
            Console.ForegroundColor = ConsoleColor.White;

            Tools.gotoxy(this.x, this.y + this.index);
            Console.WriteLine(_array[index].ToUpper());

            if (this.systemmenu == false && this.title.Contains("=") == false)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                Tools.gotoxy(this.x, this.y - 1);
                Console.Write(this.title.ToUpper());
            }
        }

        private void create()
        {
            string buffer = "";

            this.length = this._array.Length - 1;
            Console.CursorVisible = false;
            // Window keyhandler = new Window();

            // calculating maxlength of strings from the array
            // to use for calculting correct border sizes.
            
            for (int i = 0; i < this._array.Length; i++)
            {
                if (this._array[i].Length > maxlength)
                {
                    this.maxlength = this._array[i].Length;
                }
            }

            // draw borders

            // Tools.setbackground(ConsoleColor.White);
            Console.BackgroundColor = ConsoleColor.White;

            // this draws the x coordinate of the menu
            if (this._animation == Animation.NoAnimation)
            {
                if (this.detectkey != true)
                {
                    for (int x = -1; x < this.maxlength + 1; x++)
                    {
                        for (int y = 0; y < this._array.Length + 1; y++)
                        {
                            try
                            {
                                Console.SetCursorPosition(this.x + x, this.y + y);
                                Console.Write(" ");
                            }

                            catch (Exception)
                            {
                                Function.ext_wallpaper(9);

                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.White;

                                Console.SetCursorPosition(55, 5);
                                Console.WriteLine("!Desktop OS encountered an unknown error");
                                Console.SetCursorPosition(55, 6);
                                Console.WriteLine("due executing internal core commands!");
                                Console.SetCursorPosition(55, 7);
                                Console.WriteLine("FUNCTION: MenuBuilder at Create() : 0xFFADF1BE04");
                                Console.SetCursorPosition(55, 9);
                                Console.WriteLine("Press any key to proceed!");

                                Console.ReadKey();
                                Environment.Exit(0);
                            }

                            if (this.shadow == true)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Tools.gotoxy(this.x + x + 1, this.y + y);
                                Console.Write(" ");
                                Tools.gotoxy(this.x + x + 1, this.y + this._array.Length + 1);
                                Console.Write(" ");
                                Console.BackgroundColor = ConsoleColor.White;
                            }
                        }

                        //for (int j = 0; j < 5500; j++)
                        //{
                        //    Thread.Sleep(1 / 2);
                        //}
                    }
                }
            }
            else if (this._animation == Animation.Sidewards_Left) // used for standard menus
            {
                if (this.detectkey != true)
                {
                    for (int x = -1; x < this.maxlength + 1; x++)
                    {
                        for (int y = 0; y < this._array.Length + 1; y++)
                        {
                            try
                            {
                                Console.SetCursorPosition(this.x + x, this.y + y);
                                Console.Write(" ");
                            }

                            catch (Exception)
                            {
                                Function.ext_wallpaper(9);

                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.White;

                                Console.SetCursorPosition(55, 5);
                                Console.WriteLine("!Desktop OS encountered an unknown error");
                                Console.SetCursorPosition(55, 6);
                                Console.WriteLine("due executing internal core commands!");
                                Console.SetCursorPosition(55, 7);
                                Console.WriteLine("FUNCTION: MenuBuilder at Create() : 0xFFADF1BE04");
                                Console.SetCursorPosition(55, 9);
                                Console.WriteLine("Press any key to proceed!");

                                Console.ReadKey();
                                Environment.Exit(0);
                            }

                            if (this.shadow == true)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Tools.gotoxy(this.x + x + 1, this.y + y);
                                Console.Write(" ");
                                Tools.gotoxy(this.x + x + 1, this.y + this._array.Length + 1);
                                Console.Write(" ");
                                Console.BackgroundColor = ConsoleColor.White;
                            }
                        }

                        for (int j = 0; j < 5500; j++)
                        {
                            Thread.Sleep(1 / 2);
                        }
                    }
                }
            }
            else if (this._animation == Animation.Sidewards_Right)
            {
                if (this.detectkey != true)
                {
                    for (int x = this.maxlength; x > -2; x--)
                    {
                        for (int y = 0; y < this._array.Length + 1; y++)
                        {
                            try
                            {
                                Console.SetCursorPosition(this.x + x, this.y + y);
                                Console.Write(" ");
                            }

                            catch (Exception)
                            {
                                Function.ext_wallpaper(9);

                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.White;

                                Console.SetCursorPosition(55, 5);
                                Console.WriteLine("!Desktop OS encountered an unknown error");
                                Console.SetCursorPosition(55, 6);
                                Console.WriteLine("due executing internal core commands!");
                                Console.SetCursorPosition(55, 7);
                                Console.WriteLine("FUNCTION: MenuBuilder at Create() : 0xFFADF1BE04");
                                Console.SetCursorPosition(55, 9);
                                Console.WriteLine("Press any key to proceed!");

                                Console.ReadKey();
                                Environment.Exit(0);
                            }

                            if (this.shadow == true)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Tools.gotoxy(this.x + x + 1, this.y + y);
                                Console.Write(" ");
                                Tools.gotoxy(this.x + x + 1, this.y + this._array.Length + 1);
                                Console.Write(" ");
                                Console.BackgroundColor = ConsoleColor.White;
                            }
                        }

                        for (int j = 0; j < 5500; j++)
                        {
                            Thread.Sleep(1 / 2);
                        }
                    }
                }
            }
            else if (this._animation == Animation.Downwards)
            {
                if (this.detectkey != true)
                {
                    for (int y = this.y; y < this.y + this._array.Length + 1; y++)
                    {
                        for (int x = -1; x < this.maxlength + 1; x++)
                        {
                            try
                            {
                                Console.SetCursorPosition(this.x + x, y);
                                Console.Write(" ");
                            }

                            catch (Exception)
                            {
                                Function.ext_wallpaper(9);

                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.White;

                                Console.SetCursorPosition(55, 5);
                                Console.WriteLine("!Desktop OS encountered an unknown error");
                                Console.SetCursorPosition(55, 6);
                                Console.WriteLine("due executing internal core commands!");
                                Console.SetCursorPosition(55, 7);
                                Console.WriteLine("FUNCTION: MenuBuilder at Create() : 0xFFADF1BE04");
                                Console.SetCursorPosition(55, 9);
                                Console.WriteLine("Press any key to proceed!");

                                Console.ReadKey();
                                Environment.Exit(0);
                            }

                            if (this.shadow == true)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Tools.gotoxy(this.x + x + 1, this.y + y);
                                Console.Write(" ");
                                Tools.gotoxy(this.x + x + 1, this.y + this._array.Length + 1);
                                Console.Write(" ");
                                Console.BackgroundColor = ConsoleColor.White;
                            }
                        }

                        Thread.Sleep(10);
                    }
                }
            }
            else if (this._animation == Animation.Upwards) // animation for start menu
            {
                if (this.detectkey != true)
                {
                    for (int y = this.y + this._array.Length; y > this.y - 1; y--)
                    {
                        for (int x = -1; x < this.maxlength + 1; x++)
                        {
                            try
                            {
                                Console.SetCursorPosition(this.x + x, y);
                                Console.Write(" ");
                            }

                            catch (Exception)
                            {
                                Function.ext_wallpaper(9);

                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.ForegroundColor = ConsoleColor.White;

                                Console.SetCursorPosition(55, 5);
                                Console.WriteLine("!Desktop OS encountered an unknown error");
                                Console.SetCursorPosition(55, 6);
                                Console.WriteLine("due executing internal core commands!");
                                Console.SetCursorPosition(55, 7);
                                Console.WriteLine("FUNCTION: MenuBuilder at Create() : 0xFFADF1BE04");
                                Console.SetCursorPosition(55, 9);
                                Console.WriteLine("Press any key to proceed!");

                                Console.ReadKey();
                                Environment.Exit(0);
                            }

                            if (this.shadow == true)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Tools.gotoxy(this.x + x + 1, this.y + y);
                                Console.Write(" ");
                                Tools.gotoxy(this.x + x + 1, this.y + this._array.Length + 1);
                                Console.Write(" ");
                                Console.BackgroundColor = ConsoleColor.White;
                            }
                        }

                        Thread.Sleep(10);
                    }
                }
            }
            else if (this._animation == Animation.Win98_Mode) // Windows 98 specified animation
            {

            }

            Tools.setbackground(this.backcolor);

            // calculating the delta x from top of the menubuilder

            int dx = ((this.maxlength - this.title.Length) / 2) + this.x;

            if ( this.title != "" )
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                //Console.SetCursorPosition(this.x - 1, this.y - 1);
                //Console.Write(" ");

                Console.SetCursorPosition(this.x-1, this.y - 1);
                Console.WriteLine(" " + title.ToUpper());
            }

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;


            for (int i = 0; i < this._array.Length; i++)
            {
                Tools.gotoxy(this.x, this.y + i);

                if (this._array[i].Contains("-") == false)
                {
                    Console.WriteLine(this._array[i]);
                }
                else
                {
                    buffer = "";

                    for (int j = 0; j < this.maxlength; j++)
                    {
                        buffer += (char)713;
                    }

                    Console.Write(buffer);
                }
            }
        }

        public void destroyMenu()
        {
            if (this.osflag == false)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Cyan;
            }

            // Console.BackgroundColor = ConsoleColor.DarkBlue;

            for (int x = this.x - 1; x < (this.x + this.maxlength) + 2; x++)
            {
                for (int y = this.y; y < (this.y + this._array.Length) + 2; y++)
                {
                    Tools.gotoxy(x, y);
                    Console.Write(" ");
                }
            }
        }

        #endregion

        #region Version 1.0
        public int getValue()
        {
            ConsoleKeyInfo cki;
            int length = this._array.Length - 1;
            Tools.cursor_state(false);
            Window keyhandler = new Window();

            // calculating maxlength of strings from the array
            // to use for calculting correct border sizes.

            for (int i = 0; i < this._array.Length; i++)
            {
                if (this._array[i].Length > maxlength)
                {
                    this.maxlength = this._array[i].Length;
                }
            }

            // draw borders

            Tools.setbackground(ConsoleColor.White);

            // this draws the x coordinate of the menu

            for (int i = 0; i < this.maxlength; i++)
            {
                Tools.gotoxy(i + this.x, this.y);
                Console.Write(" ");

                Tools.gotoxy(i + this.x, this.y + this._array.Length);
                Console.Write(" ");
            }

            // this draws the y coordinate of the menu

            for (int i = 0; i < _array.Length + 1; i++)
            {
                Tools.gotoxy(this.x - 1, this.y + i);
                Console.Write(" ");
                Tools.gotoxy(this.x + this.maxlength, this.y + i);
                Console.Write(" ");
            }

            // draw shadow effect 
            if (this.shadow == true)
            {
                Tools.setbackground(ConsoleColor.Black);

                for (int i = 0; i < this.maxlength + 2; i++)
                {
                    Tools.gotoxy(this.x + i, this.y + this._array.Length + 1);
                    Console.Write(" ");
                }

                for (int i = 0; i < this._array.Length + 1; i++)
                {
                    Tools.gotoxy(this.x + this.maxlength + 1, this.y + i);
                    Console.Write(" ");
                }
            }

            Tools.setbackground(this.backcolor);

            // calculating the delta x from top of the menubuilder

            int dx = ((this.maxlength - this.title.Length) / 2) + this.x;

            Tools.setbackground(ConsoleColor.White);
            Tools.setforeground(ConsoleColor.Black);
            Tools.gotoxy(this.x - 1, this.y - 1);
            Console.Write(" ");

            if (this.title.Length != 0)
            {
                Tools.gotoxy(this.x, this.y - 1);
                Console.WriteLine(title);
            }

            Tools.setbackground(ConsoleColor.White);
            Tools.setforeground(ConsoleColor.Black);

            // initializing


            while (true)
            {
                for (int i = 0; i < this._array.Length; i++)
                {
                    Tools.gotoxy(this.x, this.y + i);

                    if (this._array[i].Contains("-") == false)
                    {
                        Console.WriteLine(this._array[i]);
                    }
                    else
                    {
                        for (int j = 0; j < this.maxlength; j++)
                        {
                            Console.Write((char)713);
                        }
                    }
                }

                // end of initializing

                // code for selecting item with color

                Tools.setbackground(this.itemcolor);
                Tools.setforeground(ConsoleColor.White);

                Tools.gotoxy(this.x, this.y + this.index);
                Console.WriteLine(_array[index]);
                // turn color back to normal
                // Tools.setbackground(this.backcolor);
                // Tools.setforeground(this.forecolor);

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;

                cki = Console.ReadKey(true);

                if (cki.Key == ConsoleKey.UpArrow)
                {
                    if (index > 0)
                    {
                        index--;
                    }

                    if (_array[index].Contains("-") == true)
                    {
                        index--;
                    }
                }

                if (cki.Key == ConsoleKey.DownArrow && index <= length)
                {
                    if (index < length)
                    {
                        index++;
                    }

                    if (_array[index].Contains("-") == true)
                    {
                        index++;
                    }
                }

                if (title != "")
                {
                    if (cki.Key == ConsoleKey.Escape)
                    {
                        this.destroyMenu();

                        return -1;
                    }
                }

                if (cki.Key == ConsoleKey.Enter || cki.Key == ConsoleKey.Spacebar)
                {
                    return index;
                }
            }
        }

        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSCMD
{
    class MessageBox
    {
        private int button { get; set; }
        private int x1 { get; set; }
        private int x2 { get; set; }
        private int y1 { get; set; }
        private int y2 { get; set; }
        private Window wnd;
        private string title { get; set; }
        private string text { get; set; }
        private ConsoleColor background { get; set; }
        private ConsoleColor foreground { get; set; }
        private ConsoleColor titlecolor { get; set; }
        private ConsoleColor shadowcolor { get; set; }
        private ConsoleColor itemcolor { get; set; }
        public bool shadow { get; set; }
        private bool isinitialized { get; set; }

        private string closebutton = " X ";
        private int dx { get; set; }
        private int delay { get; set; }
        private bool scrolltext { get; set; }
        private bool showclosebutton { get; set; }

        public struct COORDS
        {
            public int x1;
            public int y1;
            public int x2;
            public int y2;
        }

        private string[] _buttondata = new string[]
        {
            "    OK    ",
            "[   YES   ]   [   NO   ]   [   CANCEL   ]",
            "    YES          NO    ",
            "    YES    ",
            "    NO    "
        };

        private COORDS coords;

        // button = 0 , OK 
        // button = 1, Yes No Cancel not implemented
        // button = 2, Yes No 
        public MessageBox( Window hWnd, int _x1, int _y1, int _x2, int _y2, int _button, string _title, string _text ) 
        {
            this.x1 = _x1;
            this.y1 = _y1;
            this.x2 = _x2;
            this.y2 = _y2;
            this.title = _title;
            this.text = _text;
            this.shadow = false;
            this.button = _button;
            this.background = ConsoleColor.Gray;
            this.foreground = ConsoleColor.Black;
            this.titlecolor = ConsoleColor.White;
            this.shadowcolor = ConsoleColor.Black;
            this.itemcolor = ConsoleColor.Black;
            this.isinitialized = false;
            this.coords.x1 = _x1;
            this.coords.x2 = _x2;
            this.coords.y1 = _y1;
            this.coords.y2 = _y2;
            this.dx = 0;
            this.scrolltext = false;
            this.showclosebutton = false;
            this.wnd = hWnd;
            this.delay = 150;
        }

        #region Version 2.0

        public int handle()
        {
            if (this.wnd != null)
            {
                this.wnd.lost_focus();
            }
            else
            {
                Function.ext_wallpaper(9);

                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;

                Console.SetCursorPosition(55, 5);
                Console.WriteLine("!Desktop OS encountered an unknown error");
                Console.SetCursorPosition(55, 6);
                Console.WriteLine("due executing internal core commands!");
                Console.SetCursorPosition(55, 7);
                Console.WriteLine("FUNCTION: MessageBox at Handle() : 0x00AAFFBB11");
                Console.SetCursorPosition(55, 9);
                Console.WriteLine("Press any key to proceed!");

                Console.ReadKey();
                Environment.Exit(0);
            }

            bool quit = false;
            bool hoveryes = false;
            POINT point;
            int status = 0; // 0 = NO, 1 = YES
            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;

            if (this.isinitialized == false)
            {
                // calculating the delta of the x coordinates for drawing button caption in the center 

                dx = ((this.x2 - this._buttondata[button].Length) + this.x1) / 2;
                
                switch ( button )
                {
                    case 0:
                        this.showclosebutton = false;
                        break;
                    case 1:
                    case 2:
                        this.showclosebutton = true;
                        break;
                }

                this.create();
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
                
                // point = Mouse.getMousePosition();

                if (record.EventType == NativeMethods.KEY_EVENT)
                {
                    if (record.KeyEvent.bKeyDown == true)
                    {
                        if (this.button == 0) // "OK"
                        {
                            if ( record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Enter || 
                                 record.KeyEvent.dwControlKeyState == 2 && record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.O ||
                                 record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.O || 
                                 record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Spacebar ||
                                 record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Escape )    
                            {
                                quit = true;
                                this.Restore_ok();
                                Thread.Sleep(this.delay);
                                this.Select_ok();
                                Thread.Sleep(this.delay);
                                return 0;
                            }
                        }

                        if ( this.button == 2 ) // "YES" and "NO"
                        { 
                            if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Tab)
                            {
                                if (status == 0)
                                {
                                    this.Select_yes();
                                    status = 1;
                                }
                                else if (status == 1)
                                {
                                    this.Select_no();
                                    status = 0;
                                }
                            }

                            if ( record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Escape )
                            {
                                return 2;
                            }

                            if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.LeftArrow)
                            {
                                status = 1;
                                this.Select_yes();
                            }

                            if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.RightArrow)
                            {
                                status = 0;
                                this.Select_no();
                            }

                            if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Enter ||
                                 record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Spacebar )
                            {
                                if ( status == 0 ) // no 
                                {
                                    this.Restore_YesNo();
                                    Thread.Sleep(this.delay);
                                    this.Select_no();
                                    Thread.Sleep(this.delay);
                                    return 2;
                                }

                                if ( status == 1 ) // yes
                                {
                                    this.Restore_YesNo();
                                    Thread.Sleep(this.delay);
                                    this.Select_yes();
                                    Thread.Sleep(this.delay);
                                    return 1;
                                }

                                quit = true;
                            }

                            if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Y ||
                                record.KeyEvent.dwControlKeyState == 2 && record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Y)
                            {
                                this.Select_yes();
                                Thread.Sleep(this.delay);
                                this.Restore_YesNo();
                                Thread.Sleep(this.delay);
                                this.Select_yes();

                                return 1;
                            }

                            if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.N ||
                                record.KeyEvent.dwControlKeyState == 2 && record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.N)
                            {
                                this.Select_no();
                                Thread.Sleep(this.delay);
                                this.Restore_YesNo();
                                Thread.Sleep(this.delay);
                                this.Select_no();
                                Thread.Sleep(this.delay);

                                return 2;
                            }
                        }
                    }
                }
                else if (record.EventType == NativeMethods.MOUSE_EVENT)
                {
                    point = Mouse.getMousePosition();

                    if (this.button == 2) // YES and NO 
                    {
                        // code for "X" button
                        if (this.showclosebutton == true)
                        {
                            x1 = (this.x2 - 3) * 13;
                            x2 = this.x2 * 13;
                            y1 = this.y1 * 28;
                            y2 = (this.y1 + 1) * 28;

                            if (point.X > x1 && point.X < x2 &&
                                 point.Y > y1 && point.Y < y2)
                            {
                                Console.BackgroundColor = ConsoleColor.White;
                                Console.ForegroundColor = ConsoleColor.Red;

                                Tools.gotoxy(this.x2 - 3, this.y1);
                                Console.Write(this.closebutton);

                                if (record.MouseEvent.dwButtonState == 1)
                                {
                                    quit = true;
                                    this.Restore_X();
                                    Thread.Sleep(this.delay);
                                    this.Select_X();
                                    Thread.Sleep(this.delay);
                                    return 2;
                                }
                            }
                            else if (point.X != x1 && point.X != x2 &&
                                      point.Y != y1 && point.Y != y2)
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.White;

                                Tools.gotoxy(this.x2 - 3, this.y1);
                                Console.Write(this.closebutton);
                            }
                        }

                        // code for button "YES" 
                        x1 = (this.dx) * 13;
                        x2 = (this.dx + this._buttondata[3].Length) * 13;
                        y1 = (this.y2 - 2) * 28;
                        y2 = (this.y2 - 1) * 28;

                        if (point.X > x1 && point.X < x2 &&
                             point.Y > y1 && point.Y < y2)
                        {

                            this.Select_yes();
                            hoveryes = true;

                            if (record.MouseEvent.dwButtonState == 1)
                            {
                                quit = true;
                                this.Restore_YesNo();
                                Thread.Sleep(this.delay);
                                this.Select_yes();
                                Thread.Sleep(this.delay);
                                return 1;
                            }
                        }
                        else if (point.X != x1 && point.X != x2 &&
                                  point.Y != y1 && point.Y != y2)
                        {
                            hoveryes = false;
                            this.Restore_YesNo();
                        }

                        // code for "NO" 

                        if (hoveryes == false)
                        {
                            x1 = (this.dx + this._buttondata[3].Length - this._buttondata[4].Length) * 13;
                            x2 = (this.dx + this._buttondata[3].Length + this._buttondata[4].Length) * 13;
                            y1 = (this.y2 - 2) * 28;
                            y2 = (this.y2 - 1) * 28;

                            if (point.X > x1 && point.X < x2 &&
                                 point.Y > y1 && point.Y < y2)
                            {
                                this.Select_no();

                                if (record.MouseEvent.dwButtonState == 1)
                                {
                                    quit = true;
                                    this.Restore_YesNo();
                                    Thread.Sleep(this.delay);
                                    this.Select_no();
                                    Thread.Sleep(this.delay);
                                    return 2;
                                }
                            }
                            else if (point.X != x1 && point.X != x2 &&
                                      point.Y != y1 && point.Y != y2)
                            {
                                this.Restore_YesNo();
                            }
                        }
                    }

                    if (this.button == 0)
                    {
                        // code for "OK"
                        x1 = (this.dx) * 13;
                        x2 = (this.dx + this._buttondata[button].Length) * 13;
                        y1 = (this.y2 - 2) * 28;
                        y2 = (this.y2 - 1) * 28;

                        if (point.X > x1 && point.X < x2 &&
                             point.Y > y1 && point.Y < y2)
                        {
                            this.Select_ok();

                            if (record.MouseEvent.dwButtonState == 1)
                            {
                                quit = true;
                                this.Restore_ok();
                                Thread.Sleep(this.delay);
                                this.Select_ok();
                                Thread.Sleep(this.delay);
                                return 0;
                            }
                        }
                    }

                    if ( this.button == 1 ) // yes no cancel
                    {
                        // write code in future releases here
                    }
                }
            }
            return 0;
        }

        private void Select_X ()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(this.x2 - 3, this.y1);
            Console.Write(" X ");
        }

        private void Restore_X ()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(this.x2 - 3, this.y1);
            Console.Write(" X ");
        }

        private void Select_ok()
        {
            this.selectedItem();
            Tools.gotoxy(dx, this.y2 - 2);
            Console.Write(this._buttondata[button]);
        }
        private void Restore_ok()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;

            Tools.gotoxy(dx, this.y2 - 2);
            Console.Write(this._buttondata[button]);
        }

        private void Restore_YesNo()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;

            Tools.gotoxy(this.dx, this.y2 - 2);
            Console.Write(this._buttondata[2]);
        }

        private void Select_yes ()
        {
            Tools.setbackground(this.background);
            Tools.setforeground(this.foreground);
            Tools.gotoxy(dx, this.y2 - 2);
            Console.Write(this._buttondata[button]);

            this.selectedItem();

            Tools.gotoxy(dx, this.y2 - 2);
            Console.Write(this._buttondata[button + 1]);
            Thread.Sleep(150);
        }
        private void Select_no ()
        {

            Tools.setbackground(this.background);
            Tools.setforeground(this.foreground);
            Tools.gotoxy(dx, this.y2 - 2);
            Console.Write(this._buttondata[button]);

            this.selectedItem();

            Tools.gotoxy(dx + this._buttondata[button + 1].Length + 2, this.y2 - 2);
            Console.WriteLine(this._buttondata[button + 2]);
            Thread.Sleep(150);
        }

        public void create()
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Blue;

            for (int x = this.x1; x < this.x2; x++)
            {
                for (int y = this.y1; y < this.y2; y++)
                {
                    // slide in title of messagebox
                    Console.BackgroundColor = this.titlecolor;
                    Console.SetCursorPosition(x, this.y1);
                    Console.Write(" ");


                    // slide in background messagebox 
                    Console.BackgroundColor = this.background;
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");

                    // slide in shadow effect 
                    if (this.shadow == true)
                    {
                        Console.BackgroundColor = this.shadowcolor;
                        Console.SetCursorPosition(x + 1, y + 1);
                        Console.Write(" ");
                    }
                }
            }

            if (this.showclosebutton == true)
            {
                Tools.setbackground(ConsoleColor.Red);
                Tools.setforeground(ConsoleColor.White);
                Tools.gotoxy(this.x2 - 3, this.y1);
                Console.Write(this.closebutton);
            }

            Tools.setbackground(this.titlecolor);
            Tools.setforeground(ConsoleColor.White);

            int dx = 0;

            dx = (this.x2 - this.title.Length + this.x1) / 2;

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            Tools.gotoxy(dx, this.y1);
            Console.Write(this.title);

            Tools.setbackground(this.background);
            Tools.setforeground(this.foreground);

            char ch = ' ';
            int _y = y1 + 1;
            int _x = x1 + 1;

            for (int i = 0; i < this.text.Length; i++)
            {
                ch = text[i];

                if (ch == '^')
                {
                    _y++;
                    _x = this.x1 + 1;
                }
                else
                {
                    _x++;
                }

                if ( ch == '$' )
                {
                    for ( int x = 0; x < 8; x++ )
                    {
                        Console.Write(" ");
                        _x++;
                    }
                }

                if ( ch == '>' )
                {
                    ch = text[i + 1];
                    for ( int x = this.x1; x < this.x2-2; x++ )
                    {
                        Console.Write(ch);
                    }
                }
                
                Tools.gotoxy(_x, _y);
                if ( ch != '^' && ch != '>' && ch != '$' ) 
                    Console.Write(ch);

                if (Console.KeyAvailable != true && this.scrolltext == true)
                {
                    Thread.Sleep(1);
                }
            }

            dx = ((this.x2 - this._buttondata[button].Length) + this.x1) / 2;

            Tools.gotoxy(dx, y2 - 2);
            Console.WriteLine(this._buttondata[button]);

            if (this.button == 2)
            {
                Tools.setbackground(this.background);
                Tools.gotoxy(dx, y2 - 2);
                Console.Write(this._buttondata[button]);

                this.selectedItem();

                Tools.gotoxy(dx + this._buttondata[button + 1].Length + 2, this.y2 - 2);
                Console.WriteLine(this._buttondata[button + 2]);

                // status = 0;
            }

            if (this.button == 0)
            {
                this.selectedItem();
                Tools.gotoxy(dx, this.y2 - 2);
                Console.Write(this._buttondata[button]);
            }
        }
        public void destroy()
        {
            Tools.setbackground(ConsoleColor.DarkBlue);
            Tools.setforeground(ConsoleColor.Black);

            for (int x = this.x1; x < this.x2; x++)
            {
                Tools.gotoxy(x, this.y1);
                Console.Write(" ");
            }

            Tools.setbackground(ConsoleColor.DarkBlue);

            for (int x = this.x1 + 1; x < this.x2 + 1; x++)
            {
                Tools.gotoxy(x, this.y2);
                Console.Write(" ");
            }

            for (int y = this.y1 + 1; y < this.y2 + 1; y++)
            {
                Tools.gotoxy(this.x2, y);
                Console.Write(" ");
            }

            if (this.shadow == true || this.shadow == false)
            {
                Tools.setbackground(ConsoleColor.DarkBlue);

                for (int x = this.x1; x < this.x2; x++)
                {
                    for (int y = this.y1 + 1; y < this.y2; y++)
                    {
                        Tools.gotoxy(x, y);
                        Console.Write(" ");
                    }
                }
            }
        }

        private void selectedItem()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion
    }
}

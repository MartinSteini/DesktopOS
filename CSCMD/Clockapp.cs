using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace CSCMD
{
    class Clockapp
    {
        // v1.0 initial release of !Desktop Clock
        // nothing special maybe a bit buggy

        // v1.1 some fixes

        // v2.0 
        // shadowing the time with some effects of asciiart class

        // v2.1.0a

        // buggy seconds 

        // v2.1.1a
        // finally fixed the buggy seconds and it should run perfetcly !

        // 2.2 will add more child window processes to the parent window


        private int x1 { get; set; }
        private int x2 { get; set; }
        private int y1 { get; set; }
        private int y2 { get; set; }
        private bool minimized { get; set; }
        private bool maximized { get; set; }

        private string ischecked = "" + (char)27;
        private string buffer { get; set; }
        private bool initialized { get; set; }
        private bool use3dfonts { get; set; }
        private int level { get; set; }
        
        private Window window;
        private bool showdateandtime { get; set; }
        private bool showmodernclock { get; set; }
        private bool isloaed { get; set; }
        private bool isdrawn { get; set; }

        private string minimizedmsg = "!Desktop Clock [minimized]";
        private string key { get; set; }

        private Taskbar tb = new Taskbar();

        private Function function = new Function();

        private Window childWindow;

        private string[] filemenu = new string[]
        {
            "_File", "_Settings", "_Help"
        };

        private string[] fm = new string[]
        {
            "Start a new instance",
            "-",
            "Exit                "
        };

        private string[] sm = new string[]
        {
            "Change Timezone         ",
            "-",
            "Layout                  ",
            "Analoge Clock           ",
            "Normal Digital Clock    ",
            "Modern Digital Clock   ",
            "-",
            "Show Date & Time       ",
            "Show Time               ",
            "-",
            "Use 3D Fonts           " + "" + (char)27 + "",
            "-",
            "Thickness Level        " + "" + (char)26 + ""
        };

        private string[] thicknessmenu = new string[]
        {
            "Thick       ",
            "Thicker    ",
            "Thickest    "
        };

        private string[] hm = new string[]
        {
            "About         ",
            "-",
            "Get Updates..."
        };

        private DateTime currenttime;

        private string[] savehour = new string[2];
        private string[] savemin = new string[2];
        private string[] savesec = new string[2];

        public Clockapp()
        {
            this.x1 = 25;
            this.x2 = 80;
            this.y1 = 3;
            this.y2 = 20;
            this.showmodernclock = true;
            this.showdateandtime = true;
            this.sm[5] += this.ischecked;
            this.sm[7] += this.ischecked;
            this.initialized = false;
            this.use3dfonts = true;
            this.thicknessmenu[1] += this.ischecked;
            this.level = 2;
            this.isloaed = false;
            this.isdrawn = false;
        }

        private void repaint()
        {
            if ( this.isloaed == false )
            {
                Function.ext_wallpaper(6);
                this.isloaed = true;
                Thread.Sleep(50);
                Function.ext_wallpaper(0);
            }

            if ( this.key == "$restore" || this.key == "AltR" ) 
            {
                Function.ext_wallpaper(0);
            }

            if (this.minimized == false)
            {
                window = new Window(this.x1, this.y1, this.x2, this.y2, "!Desktop Clock", this.filemenu);

                // window.shadow = false;
                window.showmenubutton = true;
                window.showminimizebutton = true;
                window.showmaximizebutton = true;
                window.showhelpbutton = true;
            }
            else if (this.minimized == true)
            {
                //Console.Clear();
                //Console.BackgroundColor = ConsoleColor.Blue;
                //Console.ForegroundColor = ConsoleColor.White;
                //Console.Clear();

                Function.ext_wallpaper(0);

                window = new Window(this.x1, this.y1, this.x2, this.y2, this.minimizedmsg, null);

                window.shadow = false;
                window.showmenubutton = false;
                window.showminimizebutton = false;
                window.showhelpbutton = false;
                window.showclosebutton = false;
                window.isMinimized = true;
            }

            if (this.maximized == true)
            {
                window = new Window(1,1,103,25, "!Desktop Clock", this.filemenu);

                // window.shadow = false;
                window.showmenubutton = true;
                window.showminimizebutton = true;
                window.showhelpbutton = true;
                window.showclosebutton = true;
                window.showmaximizebutton = false;
                window.showmaximizedbutton = true;
            }
            else if (this.maximized == false)
            {
                // code for something important ???
            }

            window.show();
        }

        public void run()
        {
            this.initialized = false;
            this.repaint();
            tb.show();

            while (true)
            {
                key = window.handle();
                
                if ( this.minimized == true ) // check if the window is currently minimized
                {
                    if ( key == "AltF" || key == "AltS" || key == "AltH" || key == "AltD" ) 
                    {
                        key = "";
                    }
                }

                this.doclock( this.level );

                if ( key == "$close" || key == "$mclose" || key == "$help" || key == "$mhelp" || key == "$minimize" ||
                        key == "$restore" || key == "AltQ" || key == "AltR" || key == "AltD" || key == "F1" ||
                        key == "AltF" || key == "AltS" || key == "AltH" || key == "AltN" || 
                        key == "$menu0" || key == "$menu1" || key == "$menu2" || key == "$maximize" ||
                        key == "AltU" || key == "+" || key == "-" )
                {
                    break;
                }
                else if ( key == "$repaint" )
                {
                    key = "";
                    this.repaint();
                }
            }

            if ( key == "+" )
            {
                if ( this.level > 0 && this.level < 4 )
                {
                    this.level++;
                }
                
                this.run();
            }

            if ( key == "-" )
            {
                if ( this.level < 4 )
                {
                    this.level--;
                }
                
                this.run();
            }

            if (key == "$close" || key == "AltQ")
            {
                this.closeapp();
            }

            if (key == "$maximize" || key == "AltU")
            {
                this.initialized = false;
                this.maximize();
            }

            if (key == "$mclose")
            {
                this.repaint();
                this.closeapp();
            }

            if (key == "$help" || key == "F1")
            {
                this.showhelp();
            }

            if (key == "$mhelp")
            {
                this.repaint();
                this.showhelp();
            }

            if (key == "$minimize" || key == "AltD")
            {
                this.minimize();
            }

            if (key == "AltR" || key == "$restore")
            {
                this.initialized = false;
                this.restore();
            }

            if ( this.minimized == false && key == "AltF" || key == "$menu0" )
            {
                this.showfilemenu();
            }

            if ( key == "AltS" || key == "$menu1" )
            {
                this.showsettingsmenu();
            }

            if ( key == "AltN" )
            {
                if (this.maximized == true)
                {
                    Clockapp cl = new Clockapp();
                    cl.maximized = true;
                    cl.run();
                }
                else
                {
                    Clockapp cl = new Clockapp();
                    cl.minimized = false;
                    cl.run();
                }
            }

            if ( key == "AltH" || key == "$menu2" ) 
            {
                this.showhelpmenu();
            }
        }

        private void showfilemenu()
        {
            int index = 0;

            MenuBuilder filemenu;

            if (this.maximized == true)
            {
                filemenu = new MenuBuilder(3, 3, this.fm, "File");
            }
            else
            {
                filemenu = new MenuBuilder(27, 5, this.fm, "File");
            }

            if ( this.key == "AltF" )
            {
                filemenu.is_key = true;
            }

            // index = filemenu.getValue();
            index = filemenu.handle();

            if (index == -1)
            {
                this.run();
            }

            if (index == 0) // start a new instance of the app
            {
                if ( this.maximized == true )
                {
                    Clockapp cl = new Clockapp();
                    cl.maximized = true;
                    cl.run();
                }
                else
                {
                    Clockapp cl = new Clockapp();
                    cl.minimized = false;
                    cl.run();
                }
            }

            if (index == 2) // this will send an exit 
            {
                this.repaint();
                this.closeapp();
            }

        }
        private void showsettingsmenu()
        {
            int index = 0;

            MenuBuilder filemenu;
            MenuBuilder thickmenu;

            if (this.maximized == true)
            {
                filemenu = new MenuBuilder(11, 3, this.sm, "Settings");
                thickmenu = new MenuBuilder(36, 16, this.thicknessmenu, "Thickness    ");
                // thickmenu = new MenuBuilder(36, 15, this.thicknessmenu, "");
            }
            else
            {
                filemenu = new MenuBuilder(35, 5, this.sm, "Settings");
                thickmenu = new MenuBuilder(61, 18, this.thicknessmenu, "Thickness    ");
                // thickmenu = new MenuBuilder(61, 17, this.thicknessmenu, "");
            }

            if ( this.key == "AltS" )
            {
                filemenu.is_key = true;
            }

            index = filemenu.handle();

            if (index == -1 || index > 2 && index < 4)
            {
                this.run();
            }

            if ( index == 0 ) // change timezone window 
            {
                if ( this.maximized == true )
                {
                    this.x1 = 01;
                    this.y1 = 01;
                    this.x2 = 103;
                    this.y2 = 25;
                }

                string[] menu = new string[]
                {
                    "_File"
                };

                this.childWindow = new Window(this.x1 + 2, this.y1 + 3, this.x2 - 2, this.y2 - 2,
                    "Change Timezones", menu );
                this.childWindow.showclosebutton = true;
                this.childWindow.showcontextmenu = false;
                this.repaint();

                this.window.lost_focus();

                this.childWindow.show();

                while (true)
                {
                    this.key = this.childWindow.handle();

                    if ( this.key == "$close" || this.key == "Escape" ||
                         this.key == "AltQ" ) 
                    {
                        break;
                    }

                    if ( this.key == "$menu0" )
                    {

                    }
                }
                this.run();
            }

            if (index == 2) // layout window 
            {
                if (this.maximized == true)
                {
                    this.x1 = 01;
                    this.y1 = 01;
                    this.x2 = 103;
                    this.y2 = 25;
                }

                string[] menu = new string[]
                {
                    "_File"
                };

                this.childWindow = new Window(this.x1 + 2, this.y1 + 3, this.x2 - 2, this.y2 - 2,
                    "Layout", menu);

                this.childWindow.showclosebutton = true;
                this.childWindow.showcontextmenu = false;

                this.repaint();

                this.window.lost_focus();

                this.childWindow.show();

                while (true)
                {
                    this.key = this.childWindow.handle();

                    if ( this.key == "$close" || this.key == "Escape" || 
                         this.key == "AltQ" ) 
                    {
                        break;
                    }
                }
                this.run();
            }

            if ( index == 3 ) // analogue clock
            {
                this.analog_clock();
            }

            if (index == 4) // normal clock 
            {
                this.sm[4] = "Normal Digital Clock   " + this.ischecked;
                this.sm[5] = "Modern Digital Clock    ";

                this.showmodernclock = false;
                this.initialized = false;
                this.run();
            }

            if (index == 5) // modern clock 
            {
                this.sm[4] = "Normal Digital Clock    ";
                this.sm[5] = "Modern Digital Clock   " + this.ischecked;

                this.showmodernclock = true;
                this.initialized = false;
                this.run();
            }


            if (index == 7) // set date and time
            {
                this.sm[7] = "Show Date & Time       " + this.ischecked;
                this.sm[8] = "Show Time               ";

                this.showdateandtime = true;
                this.run();
            }

            if (index == 8) // set only time
            {
                this.sm[7] = "Show Date & Time        ";
                this.sm[8] = "Show Time              " + this.ischecked;

                this.showdateandtime = false;
                this.run();
            }

            if (index == 10) // use 3d fonts enable or disable 
            {
                if (this.use3dfonts == true)
                {
                    this.use3dfonts = false;
                    this.sm[10] = "Use 3D Fonts            ";
                }
                else if (this.use3dfonts == false)
                {
                    this.use3dfonts = true;
                    this.sm[10] = "Use 3D Fonts           " + "" + (char)27 + "";
                }

                this.initialized = false;
                this.run();
            }

            if ( index == 12 ) // thickness menu 
            {
                index = 0;

                // index = thickmenu.getValue();
                thickmenu.systemmenu = true;
                thickmenu.setAnimation_Direction(MenuBuilder.Animation.Sidewards_Left);
                index = thickmenu.handle();

                if ( index == -1 ) // escape code 
                {
                    this.run();
                }

                //                "Thick       ",
                //            "Thicker    ",
                //            "Thickest    "
                
                if ( index == 0 ) // thick 
                {
                    this.level = 1;
                    this.thicknessmenu[0] = $"Thick      {this.ischecked}";
                    this.thicknessmenu[1] = "Thicker     ";
                    this.thicknessmenu[2] = "Thickest    ";
                }

                if ( index == 1 ) // thicker 
                {
                    this.level = 2;
                    this.thicknessmenu[0] = "Thick       ";
                    this.thicknessmenu[1] = $"Thicker    {this.ischecked}";
                    this.thicknessmenu[2] = "Thickest    ";
                }

                if ( index == 2 ) // thickest 
                {
                    this.level = 3;
                    this.thicknessmenu[0] = "Thick       ";
                    this.thicknessmenu[1] = "Thicker     ";
                    this.thicknessmenu[2] = $"Thickest   {this.ischecked}";
                }

                this.run();
            }
        }
        private void analog_clock()
        {

        }

        private void showhelpmenu()
        {
            int index = 0;

            MenuBuilder filemenu;
            
            if (this.maximized == true)
            {
                filemenu = new MenuBuilder(23, 3, this.hm, "Help");
            }
            else
            {
                filemenu = new MenuBuilder(47, 5, this.hm, "Help");
            }

            if (this.key == "AltH")
            {
                filemenu.is_key = true;
            }


            // index = Convert.ToInt32(filemenu.getValue());
            index = filemenu.handle();

            if (index == -1 || index == 2)
            {
                this.run();
            }

            if (index == 0) // show help 
            {
                this.repaint();
                this.showhelp();
                this.run();
            }
        }

        private void closeapp()
        {
            OS os = new OS();
            int index = 0;

            MessageBox msgbox = new MessageBox( window , 30, 10, 75, 17, 2, "Quitting !Desktop Clock...", "^Do you really want to quit ?^>_");
            index = msgbox.handle();

            if (index == 1) // yes 
            {
                os.initialize();
            }

            if (index == 2) // no 
            {
                this.key = "";

                if (this.maximized == true)
                {
                    this.maximized = true;
                    this.run();
                }

                if ( this.maximized == false )
                {
                    this.maximized = false;
                    this.run();
                }
            }
        }

        private void showhelp()
        {

            MessageBox msgbox = new MessageBox( window , 30, 10, 75, 19, 0, "About !Desktop Clock", "^!Desktop Clock Version 2.0a^^by Martin Steinkasserer, 2020^>_^");
            // MessageBox msgbox = new MessageBox(this.x1+2, this.y1+3,this.x2-2, this.y2-3, 0, "About !Desktop Clock", "^!Desktop Clock v2.0.1a^^by Martin Steinkasserer, 2020^>_^");
            msgbox.handle();
            this.key = "";
            this.run();
        }

        private void maximize()
        {
            this.x1 = 1;
            this.y1 = 2;
            this.x2 = 103;
            this.y2 = 10;

            this.minimized = false;
            this.maximized = true;

            this.run();
        }

        private void minimize()
        {
            this.x1 = 3;
            this.x2 = this.minimizedmsg.Length - 1;
            this.y1 = 25;
            this.y2 = 25;

            window.isMinimized = true;
            this.minimized = true;
            this.maximized = false;
            this.run();
        }

        private void restore()
        {
            this.x1 = 25;
            this.x2 = 80;
            this.y1 = 3;
            this.y2 = 20;
            this.minimized = false;
            this.maximized = false;

            this.run();
        }
        private void doclock( int thicklevel )
        {
            string tmp = "";
            string buffer = "";

            this.currenttime = DateTime.Now;

            tmp = $"{this.currenttime}";


            if (this.showmodernclock == true) // show the text styled version of the system time
            {
                if (this.showdateandtime == false)
                {
                    for (int i = 11; i < 19; i++)
                    {
                        buffer += tmp[i].ToString();
                    }
                    tmp = buffer;
                }
            }

            if (this.showmodernclock == false) // show the digital clock version of clockapp
            {
                char ch = ' ';

                string[] hour = new string[2];
                string[] min = new string[2];
                string[] sec = new string[2];

                int x_shadow = 1;
                int y_shadow = 1;

                Function ascii = new Function();
                ascii.thicknesslevel = thicklevel;

                // getting the hour data of system time
                for (int i = 11; i < 13; i++)
                {
                    ch = tmp[i];

                    hour[i - 11] += ch.ToString();
                }

                // getting minutes
                for (int i = 14; i < 16; i++)
                {
                    ch = tmp[i];
                    min[i - 14] += ch.ToString();
                }

                // getting seconds
                for (int i = 17; i < 19; i++)
                {
                    ch = tmp[i];
                    sec[i - 17] += ch.ToString();
                }

                // saveing hour , min and second data into separate array
                // if the data is incorrect do the update screen 
                // draw the points between numbers 

                if ( this.maximized == true )
                {
                    this.x1 = 10;
                    this.y1 = 03;
                }
                else
                {
                    this.x1 = 25;
                    this.y1 = 03;
                }

                if ( this.initialized == false )
                {
                    ascii.drawasciichar(this.x1 + 23, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.asciipoints);
                }

                if ( this.maximized == true && this.isdrawn == false ) 
                {
                    ascii.drawasciichar(this.x1 + 49, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.asciipoints);
                    this.isdrawn = true;
                }

                if (this.initialized == false)
                {
                    if (this.maximized == false)
                    {
                        ascii.drawasciichar(this.x1 + 23, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.asciipoints);

                        for (int i = 0; i < 2; i++)
                        {
                            this.savehour[i] = "";
                            this.savemin[i] = "";
                            this.savesec[i] = "";
                        }
                    }
                    else if ( this.maximized == true )
                    {
                        ascii.drawasciichar(this.x1 + 23, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.asciipoints);
                        ascii.drawasciichar(this.x1 + 49, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.asciipoints);

                        for ( int i = 0; i < 2; i++ )
                        {
                            this.savehour[i] = "";
                            this.savemin[i] = "";
                            this.savesec[i] = "";
                        }
                    }
                }

                if ( this.initialized == true )
                {
                    if ( this.savehour[0] != hour[0] || this.savehour[1] != hour[1] ) 
                    {
                        if (savehour[0] != hour[0]) // update the first diigit of hour
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            for (int x = 5; x < 19; x++)
                            {
                                for (int y = 3; y < 17; y++)
                                {
                                    Tools.gotoxy(this.x1 + x, this.y1 + y);
                                    Console.Write(" ");
                                }
                            }
                        }

                        if (savehour[1] != hour[1]) // update the second diigit of hour
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            for (int x = 15; x < 27; x++)
                            {
                                for (int y = 3; y < 17; y++)
                                {
                                    Tools.gotoxy(this.x1 + x, this.y1 + y);
                                    Console.Write(" ");
                                }
                            }
                        }
                    }

                    if (this.savemin[0] != min[0] || this.savemin[1] != min[1])
                    {
                        if (savemin[0] != min[0]) // update the first diigit of minutes
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            for (int x = 29; x < 43; x++)
                            {
                                for (int y = 3; y < 17; y++)
                                {
                                    Tools.gotoxy(this.x1 + x, this.y1 + y);
                                    Console.Write(" ");
                                }
                            }
                        }

                        if (savemin[1] != min[1]) // update the second diigit of minutes
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            for (int x = 41; x < 52; x++)
                            {
                                for (int y = 3; y < 17; y++)
                                {
                                    Tools.gotoxy(this.x1 + x, this.y1 + y);
                                    Console.Write(" ");
                                }
                            }
                        }
                    }
                    
                    if ((this.savesec[0] != sec[0] || this.savesec[1] != sec[1]) && this.maximized == true)
                    {
                        StringBuilder sbuilder = new StringBuilder();

                        if (savesec[0] != sec[0]) // update the first diigit of seconds
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;

                            for (int x = 56; x < 69; x++)
                            {
                                for (int y = 3; y < 17; y++)
                                {
                                    Tools.gotoxy(this.x1 + x, this.y1 + y);
                                    Console.Write(" ");
                                }
                            }
                        }

                        if (savesec[1] != sec[1]) // update the second diigit of seconds
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;

                            for (int x = 67; x < 81; x++)
                            {
                                for (int y = 3; y < 17; y++)
                                {
                                    Tools.gotoxy(this.x1 + x, this.y1 + y);
                                    Console.Write(" ");
                                }
                            }
                        }
                    }
                }

                // first digit of hour
                if (savehour[0] != hour[0])
                {
                    if (hour[0] == "1")
                    {
                        ascii.drawasciichar(this.x1 + 5, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii1);
                    }
                    else if (hour[0] == "2")
                    {
                        ascii.drawasciichar(this.x1 + 5, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii2);
                    }
                    else if (hour[0] == "0")
                    {
                        ascii.drawasciichar(this.x1 + 5, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii0);
                    }
                }

                // second digit of hour

                if (savehour[1] != hour[1])
                {

                    if (hour[1] == "9")
                    {
                        ascii.drawasciichar(this.x1 + 15, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii9);
                    }

                    if (hour[1] == "8")
                    {
                        ascii.drawasciichar(this.x1 + 15, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii8);
                    }

                    if (hour[1] == "7")
                    {
                        ascii.drawasciichar(this.x1 + 15, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii7);
                    }

                    if (hour[1] == "6")
                    {
                        ascii.drawasciichar(this.x1 + 15, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii6);
                    }

                    if (hour[1] == "5")
                    {
                        ascii.drawasciichar(this.x1 + 15, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii5);
                    }

                    if (hour[1] == "4")
                    {
                        ascii.drawasciichar(this.x1 + 15, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii4);
                    }

                    if (hour[1] == "3")
                    {
                        ascii.drawasciichar(this.x1 + 15, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii3);
                    }

                    if (hour[1] == "2")
                    {
                        ascii.drawasciichar(this.x1 + 15, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii2);
                    }

                    if (hour[1] == "1")
                    {
                        ascii.drawasciichar(this.x1 + 15, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii1);
                    }

                    if (hour[1] == "0")
                    {
                        ascii.drawasciichar(this.x1 + 15, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii0);
                    }
                }

                // getting the minute data of system time first digit

                if (savemin[0] != min[0])
                {
                    if (min[0] == "0")
                    {
                        ascii.drawasciichar(this.x1 + 31, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii0);
                    }

                    if (min[0] == "1")
                    {
                        ascii.drawasciichar(this.x1 + 31, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii1);
                    }

                    if (min[0] == "2")
                    {
                        ascii.drawasciichar(this.x1 + 31, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii2);
                    }

                    if (min[0] == "3")
                    {
                        ascii.drawasciichar(this.x1 + 31, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii3);
                    }

                    if (min[0] == "4")
                    {
                        ascii.drawasciichar(this.x1 + 31, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii4);
                    }

                    if (min[0] == "5")
                    {
                        ascii.drawasciichar(this.x1 + 31, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii5);
                    }
                }


                // getting the minute data of system time second digit

                if (savemin[1] != min[1])
                {
                    if (min[1] == "0")
                    {
                        ascii.drawasciichar(this.x1 + 41, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii0);
                    }

                    if (min[1] == "1")
                    {
                        ascii.drawasciichar(this.x1 + 41, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii1);
                    }

                    if (min[1] == "2")
                    {
                        ascii.drawasciichar(this.x1 + 41, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii2);
                    }

                    if (min[1] == "3")
                    {
                        ascii.drawasciichar(this.x1 + 41, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii3);
                    }

                    if (min[1] == "4")
                    {
                        ascii.drawasciichar(this.x1 + 41, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii4);
                    }

                    if (min[1] == "5")
                    {
                        ascii.drawasciichar(this.x1 + 41, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii5);
                    }

                    if (min[1] == "6")
                    {
                        ascii.drawasciichar(this.x1 + 41, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii6);
                    }

                    if (min[1] == "7")
                    {
                        ascii.drawasciichar(this.x1 + 41, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii7);
                    }

                    if (min[1] == "8")
                    {
                        ascii.drawasciichar(this.x1 + 41, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii8);
                    }

                    if (min[1] == "9")
                    {
                        ascii.drawasciichar(this.x1 + 41, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii9);
                    }
                }

                if (this.maximized == true) // if the window is maximized show the seconds 
                {
                    // first digits of the seconds ...

                    if (sec[0] != savesec[0])
                    {
                        if (sec[0] == "0")
                        {
                            ascii.drawasciichar(this.x1 + 57, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii0);
                        }

                        if (sec[0] == "1")
                        {
                            ascii.drawasciichar(this.x1 + 57, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii1);
                        }

                        if (sec[0] == "2")
                        {
                            ascii.drawasciichar(this.x1 + 57, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii2);
                        }

                        if (sec[0] == "3")
                        {
                            ascii.drawasciichar(this.x1 + 57, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii3);
                        }

                        if (sec[0] == "4")
                        {
                            ascii.drawasciichar(this.x1 + 57, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii4);
                        }

                        if (sec[0] == "5")
                        {
                            ascii.drawasciichar(this.x1 + 57, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii5);
                        }
                    }

                    // second digit of the seconds
                    if (sec[1] != savesec[1])
                    {
                        if (sec[1] == "0")
                        {
                            ascii.drawasciichar(this.x1 + 69, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii0);
                        }

                        if (sec[1] == "1")
                        {
                            ascii.drawasciichar(this.x1 + 69, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii1);
                        }

                        if (sec[1] == "2")
                        {
                            ascii.drawasciichar(this.x1 + 69, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii2);
                        }

                        if (sec[1] == "3")
                        {
                            ascii.drawasciichar(this.x1 + 69, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii3);
                        }

                        if (sec[1] == "4")
                        {
                            ascii.drawasciichar(this.x1 + 69, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii4);
                        }

                        if (sec[1] == "5")
                        {
                            ascii.drawasciichar(this.x1 + 69, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii5);
                        }

                        if (sec[1] == "6")
                        {
                            ascii.drawasciichar(this.x1 + 69, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii6);
                        }

                        if (sec[1] == "7")
                        {
                            ascii.drawasciichar(this.x1 + 69, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii7);
                        }
                        if (sec[1] == "8")
                        {
                            ascii.drawasciichar(this.x1 + 69, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii8);
                        }

                        if (sec[1] == "9")
                        {
                            ascii.drawasciichar(this.x1 + 69, this.y1 + 3, x_shadow, y_shadow, this.use3dfonts, AsciiArt.ascii9);
                        }
                    }
                }

                this.savehour[0] = hour[0];
                this.savehour[1] = hour[1];
                this.savemin[0] = min[0];
                this.savemin[1] = min[1];
                this.savesec[0] = sec[0];
                this.savesec[1] = sec[1];

                this.initialized = true;
            }

            int dx = (this.x2 - tmp.Length + this.x1) / 2;
            int dy = (this.y2 - 1 + this.y1) / 2;

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            if (this.showmodernclock == true)
            {
                Tools.gotoxy(dx, dy);
                Console.Write(tmp);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;

namespace CSCMD
{
    class hny
    {
        private Window window = null;
        private InputSimulator im = null;
        private Random rnd = new Random();
        private int count { get; set; }
        private int maxfireworks { get; set; }

        private string[] house1 = new string[]
        {
          "  ____||____  ",
        @" ///////////\ ",
       @"///////////__\",
        "|    _    |  |",
        "|[] | | []|[]|",
        "|   | |   |  |",
        };

        private string[] house2 = new string[]
        {
        "             `'::. ",
        " ,%%&%,  ________|| ,%%&%,   ,%%&%,   ,%%&%,  ",
       @"%%&&%%&%/\     _   \%&&%%&% %%&&%%&% %%&&%%&% ",
       @"&%&%%&&&__\___/^\___%%&%%&& &%&%%&&& &%&%%&&& ",
       @" '%%%%'|  | []   [] |%%%%'   '%%%%'   '%%%%'  ",
        "   ||  |  |   .-.   | ||       ||       ||    ",
        "~~~||~~|__|@@_|||_@@|~||~~~~~~~||~~~~~~~||~~~~",
        };

        private string[] fireworks_effects = new string[]
        {
        "        .''. ",
       @"       :_\/_:",
       @" .''.  : /\ :",
       @":_\/_:  '..:::.",
       @": /\ :   :::::::",
        " '..'     ':::'"
        };

        private string[] fireworks_effects_2 = new string[]
        {
        "        .''. ",
        "       :    :",
        " .''.  :    :",
        ":    :  '..'''.",
        ":    :   :     :",
        " '..'     '...'"
        };

        private string[] newyear = new string[]
        {
            "#     #   #######   #######    ###### ",
            " #   #    #         #     #    #     #",
            "   #      #####     #######    ###### ",
            "   #      #         #     #    #    # ",
            "   #      #######   #     #    #     #"
        };

        private string[] newyearmsg = new string[]
        {
            "#     #    #####    ######    ######    #     #    #     #   #######   #     #",
            "#     #   #     #   #     #   #     #    #   #     ##    #   #         #     #",
            "#######   #######   ######    ######       #       #  #  #   #####     #  #  #",
            "#     #   #     #   #         #            #       #    ##   #         # # # #",
            "#     #   #     #   #         #            #       #     #   #######   #     #"
        };

        private string[,] sky = new string[100, 15];

        public hny()
        {
            this.maxfireworks = 10;
            this.count = 10;
        }
        private void drawnewyear()
        {
            int loop = 0;
            int x = 0;
            int y = 0;

            while (loop < count)
            {
                int midx = (100 - this.newyearmsg[0].Length + 1) / 2;
                int r_zahl = this.rnd.Next() % 15;
                char ch = ' ';
                int counter = 0;
                int s_r_zahl = 0;

                while (true)
                {
                    if (r_zahl > 0 && r_zahl != s_r_zahl)
                    {
                        break;
                    }
                    else
                    {
                        r_zahl = this.rnd.Next() % 15;
                    }
                }

                s_r_zahl = r_zahl;

                counter = 0;

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = (ConsoleColor)r_zahl;

                foreach (string s in this.newyearmsg)
                {
                    Console.SetCursorPosition(midx, 3 + counter);
                    Console.WriteLine(s);
                    counter++;
                }

                counter = 0;
                midx = (100 - this.newyear[0].Length + 1) / 2;

                foreach (string s in this.newyear)
                {
                    Console.SetCursorPosition(midx, 10 + counter);
                    Console.WriteLine(s);
                    counter++;
                }

                x = this.rnd.Next() % 15;
                y = 10;

                while (true)
                {
                    if (x > 0)
                    {
                        break;
                    }
                    else
                    {
                        x = rnd.Next() % 15;
                    }
                }

                #region firework
                /*
                this.drawfirework(this.fireworks_effects, x,y);
                this.drawfirework(this.fireworks_effects, 92 - x, y);
                Thread.Sleep(300);
                this.deletefirework(this.fireworks_effects, x,y);
                this.deletefirework(this.fireworks_effects, 92 - x, y);
                Thread.Sleep(50);
                this.drawfirework(this.fireworks_effects_2, x,y);
                this.drawfirework(this.fireworks_effects_2, 92 - x, y);
                Thread.Sleep(300);
                this.deletefirework(this.fireworks_effects_2, x,y);
                this.deletefirework(this.fireworks_effects_2, 92 - x, y);
                Thread.Sleep(50);
                */
                #endregion

                Thread.Sleep(500);

                counter = 0;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Black;

                midx = (100 - this.newyear[0].Length + 1) / 2;

                foreach (string s in this.newyear)
                {
                    Console.SetCursorPosition(midx, 10 + counter);
                    Console.WriteLine(s);
                    counter++;
                }

                midx = (100 - this.newyearmsg[0].Length + 1) / 2;
                counter = 0;

                foreach (string s in this.newyearmsg)
                {
                    Console.SetCursorPosition(midx, 3 + counter);
                    Console.WriteLine(s);
                    counter++;
                }

                // this.drawsky();

                loop++;
            }
        }

        private void drawblanksky()
        {
            Console.ForegroundColor = ConsoleColor.Black;

            for (int x = 1; x < 100; x++)
            {
                for (int y = 1; y < 15; y++)
                {
                    if (this.sky[x, y] == ".")
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(this.sky[x, y]);
                    }
                }
            }
        }

        public void run()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
            this.im = new InputSimulator();
            this.im.Mouse.MoveMouseBy(1000, 10);
            Console.Clear();

            string msg = "";

            this.window = new Window(0, 0, 104, 26,
                "", null);

            this.window.showclosebutton = false;
            this.window.showtaskbar = false;
            this.window.background = ConsoleColor.Gray;

            this.window.show();
            this.background();

            this.im = new InputSimulator();

            this.createsky();

            this.drawsky();

            Thread.Sleep(250);

            while (true)
            {
                msg = this.window.handle();

                if (msg == "Escape")
                {
                    break;
                }

                if (msg == "")
                {
                    if (Console.KeyAvailable == false)
                    {
                        this.im.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SPACE);
                        this.drawsky();
                        this.animations();
                        this.drawnewyear();
                    }
                }

            }

            this.close();
        }

        private void animations()
        {
            int rndx = this.rnd.Next() % 100;
            int rndy = this.rnd.Next() % 10;
            
            int counter = 0;
            int y = 0;

            while (counter < this.maxfireworks)
            {
                y = 16;

                while (true)
                {
                    if (rndx > 9 && rndx < 89 &&
                         rndy > 1 && rndy < 11)
                    {
                        break;
                    }
                    else
                    {
                        rndx = rnd.Next() % 100;
                        rndy = rnd.Next() % 10;
                    }
                }

                while ( y > ( rndy+5 ) )
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(rndx+7, y);
                    Console.Write("'");
                    Thread.Sleep(50);

                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(rndx+7, y);
                    Console.Write("'");
                    Thread.Sleep(50);
                    y--;
                }

                this.drawfirework(this.fireworks_effects, rndx, rndy);
                Thread.Sleep(300);
                this.deletefirework(this.fireworks_effects, rndx, rndy);
                Thread.Sleep(50);
                this.drawfirework(this.fireworks_effects_2, rndx, rndy);
                Thread.Sleep(300);
                this.deletefirework(this.fireworks_effects_2, rndx, rndy);
                Thread.Sleep(50);

                rndx = 0;
                rndy = 0;

                counter++;

                this.drawsky();

                Thread.Sleep(200);
            }
        }

        private void close()
        {
            Function.extern_run();
        }

        private void background()
        {
            ArrayList inlay = new ArrayList();
            string buffer = "";
            int counter = 1;
            char ch = ' ';
            
            while ( counter < 26 )
            {
                for ( int x = 1; x < 104; x++ )
                {
                    buffer += " ";
                }
                inlay.Add(buffer);
                buffer = "";
                counter++;
            }

            counter = 1;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            foreach ( string s in inlay )
            {
                if ( counter > 23 )
                {
                    Console.BackgroundColor = ConsoleColor.White;
                }

                Console.SetCursorPosition(1, counter);
                Console.Write(s);

                counter++;
            }

            inlay.Clear();

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            this.drawhouses();

        }
        private void drawhouses ()
        {
            this.drawhouse(this.house1, 2, 17);
            this.drawhouse(this.house1, 12, 18);
            this.drawhouse(this.house1, 2, 20);

            this.drawhouse(this.house1, 72, 18);
            this.drawhouse(this.house1, 89, 18);
            this.drawhouse(this.house1, 79, 20);

            this.drawhouse(this.house2, 26, 17);

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(1, 23);
            Console.Write("~");
            Console.SetCursorPosition(103, 23);
            Console.Write("~");

        }
        private void drawhouse ( string[] array, int x, int y )
        {
            char ch = ' ';
            int counter = 0;

            foreach (string s in array)
            {
                Console.SetCursorPosition(x, y + counter);

                for (int i = 0; i < s.Length; i++)
                {
                    ch = s[i];

                    Console.ForegroundColor = ConsoleColor.White;

                    if (counter < 3)
                    {
                        if (ch == '/' || ch == '\\' || ch == '|' || ch == '_' )
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }

                    if ( counter > 2 )
                    {
                        if (ch == '|' || ch == '_')
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        }

                        if ( ch == '^' || ch == '/' || ch == '\\' || ch == '_' || ch == '@' )
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        if (ch == '[' || ch == ']')
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    Console.Write(ch);
                }
                counter++;
            }
        }
        private void createsky()
        {
            Random rnd = new Random();
            int r_zahl = 0;

            for ( int x = 1; x < 100; x++ )
            {
                for ( int y = 1; y < 15; y++ )
                {
                    r_zahl = rnd.Next() % 100;

                    if ( r_zahl < 10 ) 
                    {
                        this.sky[x, y] = ".";
                    }
                }
            }
        }
        private void drawsky () 
        {
            Console.ForegroundColor = ConsoleColor.White;

            for ( int x = 1; x < 100; x++ )
            {
                for ( int y = 1; y < 15; y++ )
                {
                    if (this.sky[x, y] == ".")
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(this.sky[x, y]);
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Black;
        }

        private void drawfirework ( string[] array, int x, int y )
        {
            char ch = ' ';
            int counter = 0;
            Random rnd = new Random();
            int r_zahl = 0;

            while (true)
            {
                r_zahl = rnd.Next() % 15;
                if ( r_zahl != 0 )
                {
                    break;
                }
            }

            foreach ( string s in array )
            {
                Console.SetCursorPosition(x, y + counter);

                for ( int i = 0; i < s.Length; i++ )
                {
                    ch = s[i];
                    Console.ForegroundColor = (ConsoleColor)r_zahl;
                    Console.Write(ch);
                }
                counter++;
            }
        }

        private void deletefirework( string[] array, int x, int y )
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Black;

            int counter = 0;

            foreach (string s in array)
            {
                Console.SetCursorPosition(x, y + counter);
                Console.WriteLine(s);
                counter++;
            }
        }

    }
}

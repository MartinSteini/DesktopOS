using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CSCMD
{
    class Debug
    {
        public static void getUnicodeTable ()
        {
            ConsoleKeyInfo cki;

            int y = 20;

            for ( int i = 0; i < 4096; i++ )
            {
                Console.WriteLine($"{i}.\t{(char)i}");
         
                if ( i >= y )
                {
                    y += 20;
                    cki = Console.ReadKey(true);

                    if (cki.Key == ConsoleKey.Escape)
                        break;
                }

            }
        }

        public static void getAsciiTable ()
        {
            char asciicode = ' ';
            ConsoleKeyInfo cki;
            int y = 20;

            for (int i = 0; i < 10000; i++)
            {
                asciicode = Convert.ToChar(i);
                Console.WriteLine($"{i} = {asciicode}");

                if (i >= y)
                {
                    y += 20;
                    cki = Console.ReadKey(true);

                    if (cki.Key == ConsoleKey.Escape)
                        break;
                }
            }
        }
    }
}

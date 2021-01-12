using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSCMD
{
    class ComboBox
    {
        private int x { get; set; }
        private int y { get; set; }

        private int width = 20;
        
        private string[] array;
        private int index { get; set; }

        public ComboBox ( int _x, int _y, string[] _array )
        {
            this.x = _x;
            this.y = _y;
            this.array = _array;
        }

        public void show ()
        {
            Console.BackgroundColor = ConsoleColor.Green;

            for ( int x = this.x; x < this.x+this.width; x++ )
            {
                Tools.gotoxy(x, this.y);
                Console.Write(" ");
            }

            Tools.gotoxy(this.x + this.width, this.y);
            Console.Write((char)25);

        }
        public int getValue ()
        {

            return 0;
        }
    }
}

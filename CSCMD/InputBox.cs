using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCMD
{
    class InputBox
    {
        private int x1 { get; set; }
        private int y1 { get; set; }
        private int x2 { get; set; }
        private int y2 { get; set; }
        private string title { get; set; }
        private int button { get; set; }

        private string[] _buttondata = new string[]
        {
            "[     OK     ]", 
            "[   CANCEL   ]"
        };

        private Textfield[] txtfield = new Textfield[2];
        public InputBox( int _x1, int _y1, int _x2, int _y2, int _button, string _title )
        {
            this.x1 = _x1;
            this.y1 = _y1;
            this.x2 = _x2;
            this.y2 = _y2;
            this.button = _button;
            this.title = _title;
        }

        public void show ()
        {
            Tools.setbackground(ConsoleColor.Blue);

            for ( int x = this.x1; x < this.x2; x++ )
            {
                for ( int y = this.y1; y < this.y2; y++ )
                {
                    Tools.gotoxy(x, y);
                    Console.Write(" ");
                }
            }

            Tools.toolbarcolor();

            for ( int x = this.x1; x < this.x2; x++ )
            {
                Tools.gotoxy(x, this.y1);
                Console.Write(" ");
            }
            int dx = (this.x2 - this.title.Length + this.x1) / 2;

            Tools.gotoxy(dx , this.y1);
            Console.Write(this.title);

            Tools.shadowcolor();

            for ( int x = this.x1+1; x < this.x2+1; x++ )
            {
                Tools.gotoxy(x, this.y2);
                Console.Write(" ");
            }

            for ( int y = this.y1+1; y < this.y2+1; y++ )
            {
                Tools.gotoxy(this.x2, y);
                Console.Write(" ");
            }

            this.txtfield[0] = new Textfield(this.x1 + 1, this.y1 + 2, this.x2 - 16, this.y1 + 3,"Name");
            this.txtfield[0].show();

            this.txtfield[1] = new Textfield(this.x1 + 1, this.y1 + 4, this.x2 - 16, this.y1 + 5,"Lastname");
            this.txtfield[1].show();

            Tools.gotoxy(this.x2 - this._buttondata[0].Length - 1, this.y1 + 2);
            Console.Write(this._buttondata[0]);

            Tools.gotoxy(this.x2 - this._buttondata[1].Length - 1, this.y1 + 4);
            Console.Write(this._buttondata[1]);

        }
        public void destroy ()
        {

            Tools.windowcolor();

            for (int x = this.x1; x < this.x2; x++)
            {
                for (int y = this.y1; y < this.y2; y++)
                {
                    Tools.gotoxy(x, y);
                    Console.Write(" ");
                }
            }

            for (int x = this.x1; x < this.x2; x++)
            {
                Tools.gotoxy(x, this.y1);
                Console.Write(" ");
            }

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
        }
    }
}

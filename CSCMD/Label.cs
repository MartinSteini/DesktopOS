using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace CSCMD
{
    class Label
    {
        private int x1 { get; set; }
        private int y1 { get; set; }
        private int x2 { get; set; }
        private int y2 { get; set; }
        private string text { get; set; }
        private ConsoleColor foregroundcolor { get; set; }
        private ConsoleColor backgroundcolor { get; set; }
        public int delay { get; set; }
        public bool lined { get; set; }
        public Label( int _x1, int _y1, int _x2, int _y2, string _text, ConsoleColor _foregroundcolor, ConsoleColor _backgroundcolor ) 
        {
            this.x1 = _x1;
            this.y1 = _y1;
            this.x2 = _x2;
            this.y2 = _y2;
            this.text = _text;
            this.foregroundcolor = _foregroundcolor;
            this.backgroundcolor = _backgroundcolor;
        }

        public void show()
        {
            ArrayList frame = new ArrayList();
            ArrayList text = new ArrayList();

            string buffer = "";
            int counter = 0;
            
            char ch = ' ';

            for (int y = this.y1; y < this.y2; y++)
            {
                for (int x = this.x1; x < this.x2; x++)
                {
                    buffer += " ";
                }
                frame.Add(buffer);
                buffer = "";
            }

            counter = 0;
            Console.BackgroundColor = this.backgroundcolor;
            Console.ForegroundColor = this.foregroundcolor;

            foreach (string s in frame)
            {
                Console.SetCursorPosition(this.x1, this.y1 + counter);
                Console.WriteLine(s);
                counter++;
            }

            int dx = (this.x2 - this.text.Length + this.x1) / 2;

            for (int i = 0; i < this.text.Length; i++)
            {
                ch = this.text[i];

                // ^ \n
                // $ \t
                // > draw the complete line with given character 

                if ( ch != '^' && ch != '$' && ch != '>' ) 
                {
                    buffer += ch.ToString();
                }
                else if ( ch == '$' )
                {
                    buffer += "\t";
                }
                else if ( ch == '>' )
                {
                    ch = this.text[i + 1];

                    for ( int x = this.x1; x < this.x2-1; x++ )
                    {
                        buffer += ch.ToString();
                    }
                }
                else if ( ch == '^' )
                {
                    text.Add(buffer);
                    buffer = "";
                }
            }
            counter = 0;
            foreach (string s in text)
            {
                Console.SetCursorPosition(this.x1, this.y1 + counter);
                Console.WriteLine(s);
                counter++;
            }
        }
    }
}

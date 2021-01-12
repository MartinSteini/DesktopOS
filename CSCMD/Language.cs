using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCMD
{
    class Language
    {
        // private int default_lang;


        private string[] lang = new string[]
        {
            "English",
            "German",
            "French",
            "Spanish"
        };
        public Language()
        {
          //  this.default_lang = 0;
        }
        public void setLanguage ( int index )
        {
            Console.Write("\nUsed Language : ");

            Console.WriteLine($"{this.lang[index]}");
        }
    }
}

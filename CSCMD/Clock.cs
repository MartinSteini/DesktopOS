using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSCMD
{
    class Clock
    {
        public void ShowTime ()
        {
            for (; ;)
            {
                Console.WriteLine(DateTime.Now.ToString());
                Thread.Sleep(1000);
            }
        }
    }
}

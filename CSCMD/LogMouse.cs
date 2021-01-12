using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using WindowsInput;

namespace CSCMD
{
    internal class LogMouse
    {
        public static void Do ( Action quit )
        {
            Hook.GlobalEvents().MouseDown += (sender, e) =>
            {
                if ( e.Button == MouseButtons.Left )
                {
                    quit();
                }
            };
        }
    }
}

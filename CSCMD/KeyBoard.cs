using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSCMD
{
    class KeyBoard
    {
        [DllImport("user32.dll", EntryPoint = "keybd_event")]

        public static extern void keybd_event(byte bVk,byte bScan, int dwFlags, int dwExtraInfo );        

    }
}

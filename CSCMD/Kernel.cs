using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCMD
{
    class Kernel
    {
        public bool showposition { get; set; }
        private string oskernel { get; set; }
        public POINT point { get; set; }
        public string KeyRespond { get; set; }
        public Kernel()
        {
            this.showposition = false;
            this.oskernel = "1.0.2000";
            this.KeyRespond = "";
        }

        public string getKernelVersion()
        {
            return this.oskernel;
        }

        private void initilizeKeyBoard ()
        {
            var handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);

            int mode = 0;
            if (!(NativeMethods.GetConsoleMode(handle, ref mode))) { throw new Win32Exception(); }

            mode |= NativeMethods.ENABLE_MOUSE_INPUT;
            mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;
            mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;

            if (!(NativeMethods.SetConsoleMode(handle, NativeMethods.ENABLE_MOUSE_INPUT))) { throw new Win32Exception(); }

            var record = new NativeMethods.INPUT_RECORD();

            uint recordLen = 0;

            if (!(NativeMethods.ReadConsoleInput(handle, ref record, 1, ref recordLen))) { throw new Win32Exception(); }
        }

        private void Mouse()
        {
            var handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);

            int mode = 0;
            if (!(NativeMethods.GetConsoleMode(handle, ref mode))) { throw new Win32Exception(); }

            mode |= NativeMethods.ENABLE_MOUSE_INPUT;
            mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;
            mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;

            if (!(NativeMethods.SetConsoleMode(handle, NativeMethods.ENABLE_MOUSE_INPUT))) { throw new Win32Exception(); }

            var record = new NativeMethods.INPUT_RECORD();

            uint recordLen = 0;

            if (!(NativeMethods.ReadConsoleInput(handle, ref record, 1, ref recordLen))) { throw new Win32Exception(); }

            // POINT point;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WebApp
{
    internal class WindowsEvents
    {
        public static IntPtr TitleBar(int code, out bool handled)
        {
            handled = false;

            if (code == Winuser.WM.NCHITTEST)
            {
                try
                {
                    return new IntPtr(Winuser.HT.CAPTION);
                }
                catch (OverflowException)
                {
                    handled = true;
                }
            }

            return new IntPtr(Winuser.HT.CAPTION);
        }
    }
}

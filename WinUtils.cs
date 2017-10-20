using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace HotKeysLib
{
    public static class WinUtils
    {

        private static readonly Dictionary<string, IntPtr> ActiveWindows = new Dictionary<string, IntPtr>();

        private static bool EnumTheWindows(IntPtr hWnd, IntPtr lParam)
        {

            int windowNameSize = WindowsFunctions.GetWindowTextLength(hWnd);

            if (hWnd == WindowsFunctions.GetShellWindow()) return true;
            if (windowNameSize == 0) return true;
            if (!WindowsFunctions.IsWindowVisible(hWnd)) return true;


            StringBuilder sb = new StringBuilder(windowNameSize + 1);
            WindowsFunctions.GetWindowText(hWnd, sb, windowNameSize + 1);

            ActiveWindows.Add(sb.ToString(), hWnd);

            return true;
        }

        private static void UpdateDictionary()
        {
            ActiveWindows.Clear();
            WindowsFunctions.EnumWindows(EnumTheWindows, IntPtr.Zero);
        }

        public static List<string> GetActiveWindowNames()
        {
            UpdateDictionary();
            return ActiveWindows.Keys.ToList();
        }

        public static IntPtr GetWindowHandle(string windowName)
        {
            UpdateDictionary();
            return ActiveWindows.ContainsKey(windowName) ? ActiveWindows[windowName] : IntPtr.Zero;
        }
    }
}

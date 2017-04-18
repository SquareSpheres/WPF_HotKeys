using System;
using System.Collections.Generic;
using System.Windows.Interop;
using System.Linq;

namespace HotKey
{

    /// <summary>
    /// Class for easily setting global hot keys in a WPF application
    /// </summary>
    public class HotKeyManager
    {

        // Message identifier
        const int WM_HOTKEY = 0x0312;

        private static HotKeyManager hotKeyInstance;

        private readonly Dictionary<VirtualKeys, HotKey> hotKeys = new Dictionary<VirtualKeys, HotKey>();
        private readonly HashSet<int> hotKeys_id = new HashSet<int>();
        private readonly Random rand = new Random();

        private HotKeyManager()
        {
            ComponentDispatcher.ThreadPreprocessMessage += ThreadMessageEventHandler;
        }

        public static HotKeyManager getInstance()
        {
            if (hotKeyInstance == null) { hotKeyInstance = new HotKeyManager(); }
            return hotKeyInstance;
        }


        private void ThreadMessageEventHandler(ref MSG msg, ref bool handled)
        {
            if (!handled && msg.message == WM_HOTKEY)
            {
                int id = (int)msg.wParam;
                VirtualKeys virtualKey = (VirtualKeys)(((int)msg.lParam >> 16) & 0xFFFF);


                if (hotKeys.ContainsKey(virtualKey))
                {
                    hotKeys[virtualKey].hotKeyPressed();
                }
            }
        }


        /// <summary>
        /// Not very clever, but based on probability it will work just fine. Assuming there is 100-120 keys on a keyboard,
        /// the chance of picking an id already assigned ID is 120/Int32.MaxValue. Chance of picking an assigned value 10 times
        /// in a row would be (120/Int32.MaxValue)10 = 2.9683411e-73 which is already close to impossible.
        /// 
        /// Just for hypothetical reasons, let’s assume there was 2000000000 keys on the keyboard, and every key had an ID assigned to it.
        /// The running time of the algorithm would still be just fine. The chance of picking an already assigned ID 10 times in a row is 49%.
        /// 100 times in a row is 0.00081%, and 1000 times is 1.2593031e-31%. 
        /// 
        /// This method does not care about number of keys on your keyboard. It will only find an available ID in the range 0-2147483647. If non avaiable, it will return false.
        /// </summary>
        /// <param name="in_val">The in value.</param>
        /// <returns></returns>
        private bool generateRandomID(ref int in_val)
        {
            in_val = rand.Next(Int32.MaxValue);
            while (hotKeys_id.Contains(in_val) && hotKeys_id.Count < (Int32.MaxValue - 1))
            {
                in_val = rand.Next(Int32.MaxValue);
            }

            return hotKeys_id.Add(in_val);
        }


        /// <summary>
        /// Register a new hot key. If registration was unsuccessful it will return false, otherwise it will return true.
        /// </summary>
        /// <param name="key">Keyboard key.</param>
        /// <param name="handler">A EventHandler/>.</param>
        /// <returns></returns>
        public bool registerNewHotkey(VirtualKeys key, EventHandler handler)
        {
            int id = -1;
            if (!generateRandomID(ref id))
            {
                Console.Error.WriteLine("Unable to create unique ID");
                return false;
            }

            HotKey hotkey = HotKey.RegisterHotKey(key, id);
            if (hotkey == null)
            {
                Console.Error.WriteLine(HotKey.ErrorMsg);
                return false;
            }

            hotkey.HotKeyPressed += handler;
            hotKeys.Add(key, hotkey);
            hotKeys_id.Add(id);

            return true;
        }


        // TODO : Make bool, and get errorMsg from HotKey
        public void unregisterHotKey(VirtualKeys key)
        {
            if (hotKeys.ContainsKey(key))
            {
                HotKey tmpHotKey = hotKeys[key];
                hotKeys_id.Remove(tmpHotKey.ID);
                hotKeys.Remove(tmpHotKey.Key);
                tmpHotKey.Dispose();
            }
        }


        public bool addHotKeyAction(VirtualKeys key, EventHandler handler)
        {
            if (!hotKeys.ContainsKey(key))
            {
                Console.WriteLine("Hot key is not registered!");
                return false;
            }

            hotKeys[key].HotKeyPressed += handler;
            return true;
        }

        public bool removeHotKeyAction(VirtualKeys key, EventHandler handler)
        {
            if (!hotKeys.ContainsKey(key))
            {
                Console.WriteLine("Hot key is not registered!");
                return false;
            }

            hotKeys[key].HotKeyPressed -= handler;
            return true;
        }


        /// <summary>
        /// Gets all keys already assigned.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public String[] getActiveHotKeys()
        {
            throw new NotImplementedException();

        }

        /// <summary>
        /// Gets all available keys.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public String[] getAvailableHotKeys()
        {
            throw new NotImplementedException();
        }

    }
}

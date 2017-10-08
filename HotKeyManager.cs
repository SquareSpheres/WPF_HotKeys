/*
* This is free and unencumbered software released into the public domain.
* 
* Anyone is free to copy, modify, publish, use, compile, sell, or
* distribute this software, either in source code form or as a compiled
* binary, for any purpose, commercial or non-commercial, and by any
* means.
* 
* In jurisdictions that recognize copyright laws, the author or authors
* of this software dedicate any and all copyright interest in the
* software to the public domain.We make this dedication for the benefit
* of the public at large and to the detriment of our heirs and
* successors.We intend this dedication to be an overt act of
* relinquishment in perpetuity of all present and future rights to this
* software under copyright law.
*
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
* IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
* OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
* ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
* OTHER DEALINGS IN THE SOFTWARE.
*
* For more information, please refer to<http://unlicense.org>
*/
using System;
using System.Collections.Generic;
using System.Windows.Interop;
using System.Linq;

namespace HotKey
{

    /// <summary>
    /// Class for easily setting global hot keys in a WPF application
    /// </summary>
    public class HotKeyManager : AbstractHotKeyManager
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static HotKeyManager hotKeyInstance;

        /// <summary>
        /// Get a hot key manager. There can only be on hot key manager. This is a singelton class.
        /// </summary>
        /// <returns>a <see cref="HotKeyManager"/></returns>
        public static IHotKeyManager GetInstance()
        {
            if (hotKeyInstance == null)
            {
                hotKeyInstance = new HotKeyManager();
            }
            return hotKeyInstance;
        }


        public override bool RegisterNewHotkey(VirtualKeys key, EventHandler handler)
        {
            int id = GenerateID();

            try
            {
                HotKey hotkey = HotKey.RegisterHotKey(key, id);
                hotkey.HotKeyPressedEvent += handler;
                hotKeys.Add(key, hotkey);
                hotKeys_id.Add(id);
                return true;
            }
            catch (HotKeyException e)
            {
                logger.Warn("Failed to register hotKey : {0}", e.Message);
                return false;
            }
        }

        public override bool UnregisterHotKey(VirtualKeys key)
        {
            if (!hotKeys.ContainsKey(key))
            {
                logger.Warn("Hotkey {0} is not assigned", key);
                return false;
            }

            try
            {
                HotKey tmpHotKey = hotKeys[key];
                tmpHotKey.UnregisterHotKey();
                hotKeys_id.Remove(tmpHotKey.ID);
                hotKeys.Remove(tmpHotKey.Key);
                return true;
            }
            catch (HotKeyException e)
            {
                logger.Warn("Failed to unregister hotKey : {0}", e.Message);
                return false;
            }
        }

        public override bool AddHotKeyAction(VirtualKeys key, EventHandler handler)
        {
            if (!hotKeys.ContainsKey(key))
            {
                logger.Warn("No hotKey associated with {0}", key);
                return false;
            }

            hotKeys[key].HotKeyPressedEvent += handler;
            return true;
        }

        public override bool RemoveHotKeyAction(VirtualKeys key, EventHandler handler)
        {
            if (!hotKeys.ContainsKey(key))
            {
                logger.Warn("No hotKey associated with {0}", key);
                return false;
            }

            hotKeys[key].HotKeyPressedEvent -= handler;
            return true;
        }


        /// <summary>
        /// Gets all keys already assigned.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<VirtualKeys> GetActiveHotKeys()
        {
            return hotKeys.Keys.ToList();
        }
        /// <summary>
        /// Gets all available keys.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<VirtualKeys> GetAvailableHotKeys()
        {
            IEnumerable<VirtualKeys> virtualKeys = Enum.GetValues(typeof(VirtualKeys)).Cast<VirtualKeys>();
            IEnumerable<VirtualKeys> activeVirtualKeys = GetActiveHotKeys();
            return virtualKeys.Except(activeVirtualKeys).ToList<VirtualKeys>();
        }


    }
}

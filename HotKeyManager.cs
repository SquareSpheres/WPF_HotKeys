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
using System.Linq;

namespace HotKeysLib
{

    /// <inheritdoc />
    /// <summary>
    /// Class for easily setting global hot keys in a WPF application
    /// </summary>
    public class HotKeyManager : AbstractHotKeyManager
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static HotKeyManager _hotKeyInstance;

        /// <summary>
        /// Get a hot key manager. There can only be on hot key manager. This is a singleton class.
        /// </summary>
        /// <returns>a <see cref="HotKeyManager"/></returns>
        public static IHotKeyManager GetInstance()
        {
            return _hotKeyInstance ?? (_hotKeyInstance = new HotKeyManager());
        }


        /// <inheritdoc />
        public override void RegisterNewHotkey(VirtualKeys key, EventHandler<HotKeyPressedEventArgs> handler)
        {
            RegisterNewHotkey(key, 0, handler);
        }

        /// <inheritdoc />
        public override void RegisterNewHotkey(VirtualKeys key, Modifiers modifiers, EventHandler<HotKeyPressedEventArgs> handler)
        {
            int id = GetGenerateId();

            try
            {
                HotKey hotkey = HotKey.RegisterHotKey(IntPtr.Zero, key, modifiers, id);
                hotkey.HotKeyPressedEvent += handler;
                // remove no repeat before adding to list to avoid getting different hash
                modifiers = (Modifiers)((uint)modifiers & 0xF);
                HotKeys.Add(new KeyModifierCombination(key, modifiers), hotkey);
                HotKeysId.Add(id);
            }
            catch (HotKeyException e)
            {
                Logger.Warn("Failed to register hotKey : {0}", e.Message);
                throw;
            }
        }


        /// <inheritdoc />
        public override void UnregisterHotKey(VirtualKeys key)
        {
            UnregisterHotKey(key, 0);
        }

        /// <inheritdoc />
        public override void UnregisterHotKey(VirtualKeys key, Modifiers modifiers)
        {

            // remove no repeat before adding to list to avoid getting different hash
            modifiers = (Modifiers)((uint)modifiers & 0xF);

            KeyModifierCombination combination = new KeyModifierCombination(key, modifiers);
            if (!HotKeys.ContainsKey(combination))
            {
                String error = $"Failed to unregister hotKey : HotKey \"{key}+{modifiers}\" is not registered";
                Logger.Warn(error);
                throw new HotKeyException(error);
            }

            try
            {
                HotKey tmpHotKey = HotKeys[combination];
                tmpHotKey.UnregisterHotKey();
                HotKeysId.Remove(tmpHotKey.Id);
                HotKeys.Remove(combination);
            }
            catch (HotKeyException e)
            {
                Logger.Warn("Failed to unregister hotKey : {0}", e.Message);
                throw;
            }
        }

        //TODO EXCEPTION HANDLING
        /// <inheritdoc />
        public override void UnregisterAll()
        {
            foreach (var hotKeysValue in HotKeys.Values)
            {
                hotKeysValue.UnregisterHotKey();
            }

            HotKeys.Clear();
        }

        /// <inheritdoc />
        public override void AddHotKeyAction(VirtualKeys key, Modifiers modifiers, EventHandler<HotKeyPressedEventArgs> handler)
        {
            // remove no repeat before adding to list to avoid getting different hash
            modifiers = (Modifiers)((uint)modifiers & 0xF);
            KeyModifierCombination combination = new KeyModifierCombination(key, modifiers);

            if (!HotKeys.ContainsKey(combination))
            {
                String error = $"Failed to unregister hotKey : {modifiers}+{key}";
                Logger.Warn(error);
                throw new HotKeyException(error);
            }

            HotKeys[combination].HotKeyPressedEvent += handler;
        }

        /// <inheritdoc />
        public override void RemoveHotKeyAction(VirtualKeys key, Modifiers modifiers, EventHandler<HotKeyPressedEventArgs> handler)
        {
            // remove no repeat before adding to list to avoid getting different hash
            modifiers = (Modifiers)((uint)modifiers & 0xF);
            KeyModifierCombination combination = new KeyModifierCombination(key, modifiers);

            if (!HotKeys.ContainsKey(combination))
            {
                String error = $"No hotKey associated with {modifiers}+{key}";
                Logger.Warn(error);
                throw new HotKeyException(error);
            }

            HotKeys[combination].HotKeyPressedEvent -= handler;
        }


        /// <inheritdoc />
        public override List<Tuple<VirtualKeys, Modifiers>> GetActiveHotKeys()
        {
            return HotKeys.Select(hotKey => new Tuple<VirtualKeys, Modifiers>(hotKey.Key.Key, hotKey.Key.Modifiers)).ToList();
        }

        /// <inheritdoc />
        public override bool IsHotKeyAvailable(VirtualKeys virtualKey, Modifiers modifiers)
        {
            return !HotKeys.ContainsKey(new KeyModifierCombination(virtualKey, modifiers));
        }
    }
}

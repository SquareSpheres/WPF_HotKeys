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

namespace HotKeysLib
{

    /// <inheritdoc />
    /// <summary>
    /// A abstract Hotkey manager implementing core functionality like the component dispatcher.
    /// It also initializes containers for the hotkeys, and hotkeys_id.
    /// </summary>
    /// <seealso cref="T:HotKeysLib.IHotKeyManager" />
    public abstract class AbstractHotKeyManager : IHotKeyManager
    {

        protected class KeyModifierCombination
        {
            public KeyModifierCombination(VirtualKeys key, Modifiers modifiers)
            {
                Key = key;
                Modifiers = modifiers;
            }

            public Modifiers Modifiers { get; }
            public VirtualKeys Key { get; }

            public override bool Equals(object obj)
            {
                return obj is KeyModifierCombination combination &&
                       Key == combination.Key &&
                       Modifiers == combination.Modifiers;
            }

            public override int GetHashCode()
            {
                var hashCode = 1342178661;
                hashCode = hashCode * -1521134295 + Key.GetHashCode();
                hashCode = hashCode * -1521134295 + Modifiers.GetHashCode();
                return hashCode;
            }
        }

        // Message identifier
        protected const int WM_HOTKEY = 0x0312;

        protected readonly Dictionary<KeyModifierCombination, HotKey> HotKeys = new Dictionary<KeyModifierCombination, HotKey>();
        protected readonly HashSet<int> HotKeysId = new HashSet<int>();

        private int _idCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractHotKeyManager"/> class.
        /// </summary>
        protected AbstractHotKeyManager()
        {
            ComponentDispatcher.ThreadPreprocessMessage += ThreadMessageEventHandler;
        }


        /// <summary>
        /// Generates a new unique identifier.
        /// </summary>
        /// <returns>A unique identifies</returns>
        protected int GetGenerateId()
        {
            return _idCount++;
        }

        /// <summary>
        /// Thread message event handler.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="handled">if set to <c>true</c> [handled].</param>
        private void ThreadMessageEventHandler(ref MSG msg, ref bool handled)
        {
            if (handled || msg.message != WM_HOTKEY) return;
            int id = (int)msg.wParam;
            VirtualKeys virtualKey = (VirtualKeys)(((int)msg.lParam >> 16) & 0xFFFF);
            Modifiers modifiers = (Modifiers)((int)msg.lParam & 0x0000ffff);

            KeyModifierCombination keyModifierCombination = new KeyModifierCombination(virtualKey, modifiers);

            if (HotKeys.ContainsKey(keyModifierCombination))
            {
                HotKeyPressedEventArgs args = new HotKeyPressedEventArgs(msg.pt_x, msg.pt_y, msg.time, id, virtualKey);
                HotKeys[keyModifierCombination].HotKeyPressed(args);
            }
        }

        /// <inheritdoc cref="IHotKeyManager.RegisterNewHotkey(VirtualKeys,System.EventHandler{HotKeyPressedEventArgs})"/>
        public abstract void RegisterNewHotkey(VirtualKeys key, EventHandler<HotKeyPressedEventArgs> handler);
        /// <inheritdoc cref="IHotKeyManager.RegisterNewHotkey(VirtualKeys,Modifiers,System.EventHandler{HotKeyPressedEventArgs})"/>
        public abstract void RegisterNewHotkey(VirtualKeys key, Modifiers modifiers, EventHandler<HotKeyPressedEventArgs> handler);
        /// <inheritdoc cref="IHotKeyManager.UnregisterHotKey(VirtualKeys)"/>
        public abstract void UnregisterHotKey(VirtualKeys key);
        /// <inheritdoc cref="IHotKeyManager.UnregisterHotKey(VirtualKeys,Modifiers)"/>
        public abstract void UnregisterHotKey(VirtualKeys key, Modifiers modifiers);
        /// <inheritdoc cref="IHotKeyManager.AddHotKeyAction"/>
        public abstract void AddHotKeyAction(VirtualKeys key, Modifiers modifiers, EventHandler<HotKeyPressedEventArgs> handler);
        /// <inheritdoc cref="IHotKeyManager.RemoveHotKeyAction"/>
        public abstract void RemoveHotKeyAction(VirtualKeys key, Modifiers modifiers, EventHandler<HotKeyPressedEventArgs> handler);
        /// <inheritdoc cref="IHotKeyManager.GetActiveHotKeys"/>
        public abstract List<Tuple<VirtualKeys, Modifiers>> GetActiveHotKeys();
        /// <inheritdoc cref="IHotKeyManager.IsHotKeyAvailable"/>
        public abstract bool IsHotKeyAvailable(VirtualKeys virtualKey, Modifiers modifiers);
    }
}

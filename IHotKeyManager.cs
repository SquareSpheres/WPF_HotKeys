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

namespace HotKeysLib
{
    /// <summary>
    /// HotKeyManager interface
    /// </summary>
    public interface IHotKeyManager
    {
        /// <summary>
        /// Register a hotkey.
        /// </summary>
        /// <param name="key">Keyboard key.</param>
        /// <param name="handler">A EventHandler/>.</param>
        /// <exception cref="HotKeyException">Thrown if registration fails in any way</exception>
        void RegisterNewHotkey(VirtualKeys key, EventHandler<HotKeyPressedEventArgs> handler);
        /// <summary>
        /// Registers a hotkey with modifiers, i.e. you have to press the modifiers in combination with the key specified.
        /// E.g. <see cref="Modifiers.MOD_CONTROL"/> with <see cref="VirtualKeys.A"/> means the hotkey will active if you
        /// press Control+A
        /// </summary>
        /// <param name="key">Keyboard key.</param>
        /// <param name="modifiers">The keys that must be pressed in combination with the key specified. You can supply</param>
        /// <param name="handler">A EventHandler.
        /// multiple by separating them with bitwise-or. E.g. MOD_CONTROL|MOD_ALT</param>
        void RegisterNewHotkey(VirtualKeys key, Modifiers modifiers, EventHandler<HotKeyPressedEventArgs> handler);
        /// <summary>
        /// Unregisters a hotkey.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <exception cref="HotKeyException">Thrown if unregistration fails in any way</exception>
        void UnregisterHotKey(VirtualKeys key);
        /// <summary>
        /// Unregisters a hot key - modifier combination.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifiers">The modifiers.</param>
        /// <exception cref="HotKeyException">Thrown if unregistration fails in any way</exception>
        void UnregisterHotKey(VirtualKeys key, Modifiers modifiers);
        /// <summary>
        /// Unregisters all active hot keys.
        /// </summary>
        void UnregisterAll();
        /// <summary>
        /// Add a action to an already initialized hotKey
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifiers">The modifiers.</param>
        /// <param name="handler">The action.</param>
        /// <exception cref="HotKeyException">Thrown if Hotkey does not exist</exception>
        void AddHotKeyAction(VirtualKeys key, Modifiers modifiers, EventHandler<HotKeyPressedEventArgs> handler);
        /// <summary>
        /// Removes a action from an already initialized hotKey
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifiers">The modifiers.</param>
        /// <param name="handler">The action.</param>
        /// <exception cref="HotKeyException">Thrown if action does not exist</exception>
        void RemoveHotKeyAction(VirtualKeys key, Modifiers modifiers, EventHandler<HotKeyPressedEventArgs> handler);
        /// <summary>
        /// Gets the active hot keys.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="Tuple"/> containing <see cref="VirtualKeys"/> and <see cref="Modifiers"/></returns>
        List<Tuple<VirtualKeys, Modifiers>> GetActiveHotKeys();
        /// <summary>
        /// Determines whether a hotKey combination is available.
        /// </summary>
        /// <param name="virtualKey">A virtual key.</param>
        /// <param name="modifiers">The modifiers. use 0 if no modifiers is used</param>
        /// <returns>
        ///   <c>true</c> if the combination is available; otherwise, <c>false</c>.
        /// </returns>
        bool IsHotKeyAvailable(VirtualKeys virtualKey, Modifiers modifiers);
    }
}

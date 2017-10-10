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

namespace HotKey
{
    public interface IHotKeyManager
    {
        /// <summary>
        /// Register a new hot key. If registration was unsuccessful it will return false, otherwise it will return true.
        /// </summary>
        /// <param name="key">Keyboard key.</param>
        /// <param name="handler">A EventHandler/>.</param>
        /// <returns>true if hotkey was successfully registered, false otherwise.</returns>
        bool RegisterNewHotkey(VirtualKeys key, EventHandler handler);
        /// <summary>
        /// Unregisters a hotkey.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>true if unregister was successfull, false otherwise</returns>
        bool UnregisterHotKey(VirtualKeys key);
        /// <summary>
        /// Add a action to an already initialized hotKey
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="handler">The action.</param>
        /// <returns>true if action was successfully added. false otherwise.</returns>
        bool AddHotKeyAction(VirtualKeys key, EventHandler handler);
        /// <summary>
        /// Removes a action from an already initialized hotKey
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="handler">The action.</param>
        /// <returns>true if action was successfully removed. false otherwise.</returns>
        bool RemoveHotKeyAction(VirtualKeys key, EventHandler handler);
        /// <summary>
        /// Gets the active hot keys.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="VirtualKeys"/></returns>
        List<VirtualKeys> GetActiveHotKeys();
        /// <summary>
        /// Gets the available hot keys.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="VirtualKeys"/>k</returns>
        List<VirtualKeys> GetAvailableHotKeys();
    }
}

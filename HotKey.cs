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
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace HotKeysLib
{

    /// <summary>
    /// Class used to represent a hotkey. The class should be used in combination with <see cref="IHotKeyManager"/>.
    /// The class will clean up unmanaged resources on finalization.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class HotKey : IDisposable
    {

        public event EventHandler<HotKeyPressedEventArgs> HotKeyPressedEvent;

        private readonly VirtualKeys _key;
        private bool _disposed;

        public int Id { get; }
        public string ErrorMsg { get; private set; }

        private HotKey(VirtualKeys key, int id)
        {
            _key = key;
            Id = id;
        }


        ~HotKey() { Dispose(false); }


        /// <summary>
        /// Register a hot key. If the registration was successful it will return a <see cref="HotKey" /> object.
        /// If registration was unsuccessful it will throw a <see cref="HotKeyException" />.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifiers">The modifiers.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="HotKeyException">If hotKey registration failed</exception>
        public static HotKey RegisterHotKey(VirtualKeys key, Modifiers modifiers, int id)
        {

            if (!WindowsFunctions.RegisterHotKey(IntPtr.Zero, id, (uint) modifiers, (uint)key))
            {
                throw new HotKeyException(new Win32Exception(Marshal.GetLastWin32Error()).Message);
            }

            return new HotKey(key, id);
        }

        /// <summary>
        /// Unregisters a hot key. If it fails, it will throw a <see cref="HotKeyException"/>
        /// </summary>
        /// <exception cref="HotKeyException">If hotKey removal failed</exception>
        public void UnregisterHotKey()
        {
            if (!WindowsFunctions.UnregisterHotKey(IntPtr.Zero, Id))
            {
                ErrorMsg = new Win32Exception(Marshal.GetLastWin32Error()).Message;
                throw new HotKeyException(ErrorMsg);
            }
        }


        /// <summary>
        /// Called when the associated hot key is pressed. Do not call this method manually. Use <see cref="HotKeyManager"/>
        /// </summary>
        public void HotKeyPressed(HotKeyPressedEventArgs args)
        {
            HotKeyPressedEvent?.Invoke(this, args);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                //dispose managed resources
            }

            //dispose unmanaged resources
            WindowsFunctions.UnregisterHotKey(IntPtr.Zero, Id);
            _disposed = true;
        }


    }
}

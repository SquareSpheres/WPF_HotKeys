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

namespace HotKey
{
    /// <summary>
    /// Easily set global hoy keys. To unregister a hot key simply dispose the object. Associate an action with the hot key implementing the <see cref="IHotKeyAction"/>
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class HotKey : IDisposable

    {

        public event EventHandler HotKeyPressedEvent;

        //Either ALT key must be held down.
        const int MOD_ALT = 0x0001;
        //Either CTRL key must be held down.
        const int MOD_CONTROL = 0x0002;
        //Changes the hotkey behavior so that the keyboard auto-repeat does not yield multiple hotkey notifications.
        const int MOD_NOREPEAT = 0x4000;

        // Last error message, if any
        private String errorMsg;

        // ID used for registering hot key
        private readonly int id;
        // Keyboard key
        private readonly VirtualKeys key;

        private bool disposed;

        public VirtualKeys Key => key;
        public int ID => id;

        public string ErrorMsg { get => errorMsg; }

        /// <summary>
        /// Register a hot key. If the registration was successful it will return a <see cref="HotKey" /> object.
        /// If registration was unsuccessful it will throw a <see cref="HotKeyException"/>, and a error message will be
        /// saved to <see cref="ErrorMsg" />.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ID">The identifier.</param>
        /// <exception cref="HotKeyException">If hotKey registration failed</exception>
        /// <returns></returns>
        public static HotKey RegisterHotKey(VirtualKeys key, int ID)
        {
            if (!WindowsFunctions.RegisterHotKey(IntPtr.Zero, ID, MOD_NOREPEAT, (int)key))
            {
                throw new HotKeyException(new Win32Exception(Marshal.GetLastWin32Error()).Message);
            }
            else
            {
                return new HotKey(key, ID);
            }
        }

        /// <summary>
        /// Unregisters a hot key. If it fails, it will throw a <see cref="HotKeyException"/>
        /// </summary>
        /// <exception cref="HotKeyException">If hotKey removal failed</exception>
        public void UnregisterHotKey()
        {
            if (!WindowsFunctions.UnregisterHotKey(IntPtr.Zero, this.ID))
            {
                errorMsg = new Win32Exception(Marshal.GetLastWin32Error()).Message;
                throw new HotKeyException(ErrorMsg);
            }
        }


        private HotKey(VirtualKeys key, int ID)
        {
            this.key = key;
            this.id = ID;
        }


        ~HotKey()
        {
            Dispose(false);
        }


        /// <summary>
        /// Called when the accociated hot key is pressed. Do not call this method manually. Use <see cref="HotKeyManager"/>
        /// </summary>
        public void HotKeyPressed()
        {
            HotKeyPressedEvent?.Invoke(this, EventArgs.Empty);

        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                //dipose managed resoruces
            }

            //dispose unmanaged resources
            WindowsFunctions.UnregisterHotKey(IntPtr.Zero, this.id);
            disposed = true;
        }


    }
}

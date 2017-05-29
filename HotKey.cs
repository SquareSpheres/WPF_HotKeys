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
        private static String errorMsg;

        // ID used for registering hot key
        private readonly int id;
        // Keyboard key
        private readonly VirtualKeys key;

        private bool disposed;

        public VirtualKeys Key => key;
        public int ID => id;

        public static string ErrorMsg { get => errorMsg; }



        /// <summary>
        /// Register a hot key. If the registration was successful it will return a <see cref="HotKey"/> object.
        /// If registration was unsuccessful it will return null. See <see cref="ErrorMsg"/> for last error.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ID">The identifier.</param>
        /// <returns></returns>
        public static HotKey RegisterHotKey(VirtualKeys key, int ID)
        {
            if (!WindowsFunctions.RegisterHotKey(IntPtr.Zero, ID, MOD_NOREPEAT, (int)key))
            {
                errorMsg = new Win32Exception(Marshal.GetLastWin32Error()).Message;
                return null;
            }
            else
            { 
                return new HotKey(key, ID);
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

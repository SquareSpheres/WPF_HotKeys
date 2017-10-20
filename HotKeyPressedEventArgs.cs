using System;

namespace HotKeysLib

{
    public class HotKeyPressedEventArgs : EventArgs
    {

        public HotKeyPressedEventArgs(int ptX, int ptY, int time, int hotKeyId, VirtualKeys key, Modifiers modifiers)
        {
            PtX = ptX;
            PtY = ptY;
            Time = time;
            Key = key;
            HotKeyId = hotKeyId;
            Modifiers = modifiers;
        }

        /// <summary>
        /// Mouse x-location
        /// </summary>
        public int PtX { get; }
        /// <summary>
        /// Mouse y-location
        /// </summary>
        public int PtY { get; }
        /// <summary>
        /// Current time in millis
        /// </summary>
        public int Time { get; }
        /// <summary>
        /// HotKey ID
        /// </summary>
        public int HotKeyId { get; }
        /// <summary>
        /// Virtual key pressed
        /// </summary>
        public VirtualKeys Key { get; }
        /// <summary>
        /// Active modifiers
        /// </summary>
        public Modifiers Modifiers { get; }

    }
}


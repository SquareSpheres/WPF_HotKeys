using System;

namespace HotKeysLib

{
    public class HotKeyPressedEventArgs : EventArgs
    {
        
        public HotKeyPressedEventArgs(int ptX, int ptY, int time, int hotKeyId, VirtualKeys key)
        {
            PtX = ptX;
            PtY = ptY;
            Time = time;
            Key = key;
            HotKeyId = hotKeyId;
        }

        public int PtX { get; }
        public int PtY { get; }
        public int Time { get; }
        public int HotKeyId { get; }
        public VirtualKeys Key { get; }
      
    }
}


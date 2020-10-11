using System;

namespace beClean.DAL.DataServices.BluetoothClassic
{
    public class BluetoothRecivedEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
        public string Content { get; set; }
        public BluetoothRecivedEventArgs(byte[] data,string content)
        {
            Data = data;
            Content = content;
        }
    }
}

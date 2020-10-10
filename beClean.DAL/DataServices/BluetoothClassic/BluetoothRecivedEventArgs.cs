using System;

namespace beClean.DAL.DataServices.BluetoothClassic
{
    public class BluetoothRecivedEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
        public BluetoothRecivedEventArgs(byte[] data)
        {
            Data = data;
        }
    }
}

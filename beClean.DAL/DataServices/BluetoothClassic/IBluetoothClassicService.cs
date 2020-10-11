using Android.Bluetooth;
using System;
using System.Collections.Generic;

namespace beClean.DAL.DataServices.BluetoothClassic
{
    public interface IBluetoothClassicService
    {
        IEnumerable<BluetoothDevice> deviceList { get; set; }
        IEnumerable<byte> recivedData { get; set; }
        IEnumerable<BluetoothDevice> PairedDevices();
        BluetoothDevice btDevice { get; set; }
        bool IsScanning { get; set; }
        bool IsConnected { get; set; }
        void SendData(string message);
        bool CheckBluetooth();
        void Connect(BluetoothDevice device);
        void Disconnect();

        event EventHandler<BluetoothRecivedEventArgs> OnDataReceived;

    }
}

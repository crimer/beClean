using Android.Bluetooth;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace beClean.DAL.DataServices.BluetoothClassic
{
    public interface IBluetoothClassicService
    {
        BluetoothDevice btDevice { get; set; }
        bool IsScanning { get; }
        ObservableCollection<BluetoothDevice> deviceList { get; set; }
        void SendData(string message);
        bool CheckBluetooth();
        void Connect(BluetoothDevice device);
        void Disconnect();
        IEnumerable<BluetoothDevice> PairedDevices();

        event EventHandler<BluetoothRecivedEventArgs> OnDataReceived;

    }
}

using Plugin.BluetoothClassic.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace beClean.DAL.DataServices.BClassic
{
    public interface IBClassic
    {
        IBluetoothManagedConnection BltConnection { get; set; }
        IBluetoothAdapter BltAdapter { get; set; }
        IEnumerable<BluetoothDeviceModel> deviceList { get; set; }
        List<byte> recivedData { get; set; }
        BluetoothDeviceModel btDevice { get; set; }
        bool IsScanning { get; set; }
        bool IsConnected { get; set; }
        IEnumerable<BluetoothDeviceModel> GetDevices();
        void SendCommand<T>(string message,T data);
        void SendCommand(string message);
        bool CheckBluetooth();
        IBluetoothManagedConnection Connect(BluetoothDeviceModel device);
        void Disconnect();

        event EventHandler<BCRecivedEventArgs> OnDataReceived;
        event EventHandler<TransmittedEventArgs> OnDataTransmitted;
        event EventHandler<ThreadExceptionEventArgs> OnErrorCatch;
        event EventHandler<StateChangedEventArgs> OnConnectionChanged;
    }
}

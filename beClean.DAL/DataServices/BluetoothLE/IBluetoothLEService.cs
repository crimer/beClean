using Plugin.BLE.Abstractions.Contracts;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace beClean.Services.DataServices.BluetoothLE
{
    public interface IBluetoothLEService
    {
        IBluetoothLE bluetoothLE { get; set; }
        IAdapter bluetoothAdapter { get; set; }
        IDevice btDevice { get; set; }
        bool IsScanning { get; }
        ObservableCollection<IDevice> deviceList { get; set; }
        void SendData(string data, string deviceId);
        Task StartScanning();
        Task StopScanning();
        string GetDeviceName(string deviceId);
        Task<bool> Connect(IDevice device);
        Task Disconnect();

    }
}

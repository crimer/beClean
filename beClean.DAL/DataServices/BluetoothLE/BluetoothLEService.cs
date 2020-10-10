using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace beClean.DAL.DataServices.BluetoothLE
{
    public class BluetoothLEService : IBluetoothLEService
    {
        public IBluetoothLE bluetoothLE { get; set; }
        public IAdapter bluetoothAdapter { get; set; }
        public BluetoothState BluetoothState { get; set; }
        public IDevice btDevice { get; set; }
        public bool IsScanning => bluetoothAdapter.IsScanning;
        public ObservableCollection<IDevice> deviceList { get; set; }

        public BluetoothLEService()
        {
            bluetoothLE = CrossBluetoothLE.Current;
            bluetoothAdapter = CrossBluetoothLE.Current.Adapter;
            bluetoothAdapter.ScanMode = ScanMode.Balanced;
            deviceList = new ObservableCollection<IDevice>();

            bluetoothLE.StateChanged += OnStateChanged;
            bluetoothAdapter.DeviceDiscovered += OnDeviceDiscovered;
        }


        public async Task StartScanning()
        {
            try
            {
                deviceList.Clear();
                await bluetoothAdapter.StartScanningForDevicesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"--- StartScanning error: {ex.Message}");
            }
            
        }
        public async Task StopScanning()
        {
            try
            {
                await bluetoothAdapter.StopScanningForDevicesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"--- StopScanning error: {ex.Message}");
            }
            
        }

        private void OnStateChanged(object sender, BluetoothStateChangedArgs e)
        {
            Debug.WriteLine($"Bluetooth state changed to => {e.NewState}");
            BluetoothState = e.NewState;
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            
            Debug.WriteLine($"DeviceFound: {e.Device.Name}");
            deviceList.Add(e.Device);
            if(!string.IsNullOrWhiteSpace(e.Device.Name) && e.Device.Name.Contains("HC-06"))
            {
                btDevice = e.Device;
                Debug.WriteLine($"Device {e.Device.Name} connected!");
            }
        }

        public string GetDeviceName(string deviceId)
        {
            throw new NotImplementedException();
        }

        public void SendData(string data, string deviceId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Connect(IDevice device)
        {
            await bluetoothAdapter.ConnectToDeviceAsync(device);
            
            var service = await device.GetServiceAsync(device.Id);

            //var data = service.GetCharacteristicAsync(device.Id);

            return true;
        }

        public async Task Disconnect()
        {
            try
            {
                if (btDevice != null) await bluetoothAdapter.DisconnectDeviceAsync(btDevice);
            }
            catch (Exception ex)
            {

                Debug.WriteLine($"--- Disconnect error: {ex.Message}");
            }
        }

    }
}

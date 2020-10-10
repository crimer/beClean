using beClean.DAL.DataServices;
using beClean.Views.Base;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace beClean.Views.DevicesPage.DeviceBLE
{
    public class DeviceBLEVM : BaseVM
    {
        #region Props
        public ICommand ScanDevicesCommand => MakeCommand(async () => await ScanDevices());
        public ICommand SelectCommand => MakeCommand(async () => await SelectDevice());
        public ICommand DisconnectCommand => MakeCommand(async () => await Disconnect());

        public ObservableCollection<IDevice> BluetoothLEDevices
        {
            get => Get<ObservableCollection<IDevice>>();
            set => Set(value);
        }
        public IDevice SelectedDevice
        {
            get => Get<IDevice>();
            set => Set(value);
        }
        public ObservableCollection<byte> RecivedData
        {
            get => Get<ObservableCollection<byte>>();
            set => Set(value);
        }
        public string Text
        {
            get => Get<string>();
            set => Set(value);
        }
        public string ConnectionState
        {
            get => Get<string>();
            set => Set(value);
        }
        public bool Scanning
        {
            get => Get<bool>();
            set => Set(value);
        }

        #endregion
        public DeviceBLEVM() : base("Устройства")
        {
            Scanning = false;
            BluetoothLEDevices = new ObservableCollection<IDevice>();
            RecivedData = new ObservableCollection<byte>();
        }
        private async Task ScanDevices()
        {
            Scanning = true;
            BluetoothLEDevices = null;
            try
            {
                BluetoothLEDevices = DataServices.BluetoothLE.deviceList;
                var t = DataServices.BluetoothLE.bluetoothAdapter.GetSystemConnectedOrPairedDevices().ToList();
                //ConnectedDevices = new ObservableCollection<IDevice>(t);
                if (Scanning)
                    await DataServices.BluetoothLE.StartScanning();
                else
                    await DataServices.BluetoothLE.StopScanning();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Attention", ex.Message, "Ok");
            }
            Scanning = false;
        }
        private async Task SelectDevice()
        {
            try
            {
                await DataServices.BluetoothLE.Connect(SelectedDevice);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Attention", ex.Message, "Ok");
            }

        }
        public async Task Disconnect()
        {
            try
            {
                await DataServices.BluetoothLE.Disconnect();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Attention", ex.Message, "Ok");
            }
        }
    }
}

using Android.Bluetooth;
using beClean.DAL.DataServices;
using beClean.Views.Base;
using Plugin.BluetoothClassic.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace beClean.Views.DevicesPage.DeviceBC
{
    public class DeviceBCVM : BaseVM
    {
        #region Props
        
        public ICommand ScanDevicesCommand => MakeCommand(async () => await ScanDevices());
        public ICommand SelectCommand => MakeCommand(async () => await SelectDevice());
        public ICommand DisconnectCommand => MakeCommand(async () => await Disconnect());
        public IBluetoothManagedConnection BltConnection { get; internal set; }
        public ObservableCollection<BluetoothDeviceModel> BluetoothClassicDevices
        {
            get => Get<ObservableCollection<BluetoothDeviceModel>>();
            set => Set(value);
        }
        public BluetoothDeviceModel SelectedDevice
        {
            get => Get<BluetoothDeviceModel>();
            set => Set(value);
        }
        public ObservableCollection<byte> RecivedData
        {
            get => Get<ObservableCollection<byte>>();
            set => Set(value);
        }
        public string Msg
        {
            get => Get<string>();
            set => Set(value);
        }
        public ConnectionState ConnectionState
        {
            get => Get<ConnectionState>();
            set => Set(value);
        }
        public bool Scanning
        {
            get => Get<bool>();
            set => Set(value);
        }
        #endregion

        public DeviceBCVM() : base("Устройства", false)
        {
            Scanning = false;
            BluetoothClassicDevices = new ObservableCollection<BluetoothDeviceModel>();
            RecivedData = new ObservableCollection<byte>();
            //DataServices.BluetoothClassic.OnDataReceived += BluetoothClassic_OnDataReceived;
            if (!App.BltAdapter.Enabled)
                App.BltAdapter.Enable();
        }

        //private void BluetoothClassic_OnDataReceived(object sender, DAL.DataServices.BluetoothClassic.BluetoothRecivedEventArgs e)
        //{
        //    string valor = System.Text.Encoding.ASCII.GetString(e.Data);
        //    Debug.WriteLine($"--- Data Reviced from arduino : {valor}");
        //    Msg = e.Content;

        //}

        private void OnStateChanged(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            ConnectionState = stateChangedEventArgs.ConnectionState;
        }

        private void OnTransmitted(object sender, TransmittedEventArgs transmittedEventArgs)
        {
            throw new NotImplementedException();
        }

        private void OnError(object sender, ThreadExceptionEventArgs threadExceptionEventArgs)
        {
            Debug.WriteLine($"--- OnError: {threadExceptionEventArgs.Exception.Message}");
        }

        private void OnRecived(object sender, RecivedEventArgs recivedEventArgs)
        {
            for (int index = 0; index < recivedEventArgs.Buffer.Length; index++)
            {
                RecivedData.Add(recivedEventArgs.Buffer.ToArray()[index]);

                Msg = System.Text.Encoding.ASCII.GetString(RecivedData.ToArray());
            }
        }
        //public byte[] ReadFully(Stream input)
        //{
        //    byte[] buffer = new byte[16 * 1024];
        //    try
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            int read;
        //            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        //            {
        //                ms.Write(buffer, 0, read);
        //            }
        //            return ms.ToArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"ReadFully error: {ex.Message}");
        //    }
        //    return new byte[1024];
        //}
        private async Task ScanDevices()
        {
            Scanning = true;
            BluetoothClassicDevices = null;
            try
            {
                var devices = App.BltAdapter.BondedDevices;
                BluetoothClassicDevices = new ObservableCollection<BluetoothDeviceModel>(devices);

                //var devices = DataServices.BluetoothClassic.PairedDevices();
                //BluetoothClassicDevices = new ObservableCollection<BluetoothDevice>(devices);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Attention", ex.Message, "Ok");
            }
            Scanning = false;
        }
        private async Task Disconnect()
        {
            try
            {
                //DataServices.BluetoothClassic.Disconnect();
                App.BltAdapter.Disable();
                if (BltConnection != null)
                {
                    BltConnection.Dispose();
                    BltConnection.OnRecived -= OnRecived;
                    BltConnection.OnError -= OnError;
                    BltConnection.OnStateChanged -= OnStateChanged;
                    BltConnection.OnTransmitted -= OnTransmitted;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Attention", ex.Message, "Ok");
            }
        }
        private async Task SelectDevice()
        {
            try
            {
                var connected = await TryConnect(SelectedDevice);
                //DataServices.BluetoothClassic.Connect(SelectedDevice);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Attention", ex.Message, "Ok");
            }
        }
        private async Task<bool> TryConnect(BluetoothDeviceModel bluetoothDeviceModel)
        {
            var connection = App.BltAdapter.CreateManagedConnection(bluetoothDeviceModel);
            try
            {
                connection.Connect();

                BltConnection = connection;

                BltConnection.OnRecived += OnRecived;
                BltConnection.OnError += OnError;
                BltConnection.OnStateChanged += OnStateChanged;
                BltConnection.OnTransmitted += OnTransmitted;

                return true;
            }
            catch (BluetoothConnectionException exception)
            {
                await App.Current.MainPage.DisplayAlert("Connection error",
                    $"Can not connect to the device: {bluetoothDeviceModel.Name}({bluetoothDeviceModel.Address}).\n" +
                        $"Exception: \"{exception.Message}\"\n" +
                        "Please, try another one.",
                    "Close");

                return false;
            }
            catch (Exception exception)
            {
                await App.Current.MainPage.DisplayAlert("Generic error", exception.Message, "Close");

                return false;
            }

        }
    }
}

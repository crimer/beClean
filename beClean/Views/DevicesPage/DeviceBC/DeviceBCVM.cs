using beClean.Droid.Services;
using beClean.Services.DataServices;
using beClean.Services.DataServices.BClassic;
using beClean.Services.DataServices.Notifications;
using beClean.Views.Base;
using Plugin.BluetoothClassic.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace beClean.Views.DevicesPage.DeviceBC
{
    public class DeviceBCVM : BaseVM
    {
        #region Props
        public ICommand ScanDevicesCommand => MakeCommand(async () => await ScanDevices());
        public ICommand SelectCommand => MakeCommand(async () => await SelectDevice());
        public ICommand DisconnectCommand => MakeCommand(async () => await Disconnect());

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
        
        private readonly INotificationService _notificationService;
        public DeviceBCVM() : base("Устройства", false)
        {
            Scanning = false;
            BluetoothClassicDevices = new ObservableCollection<BluetoothDeviceModel>();
            RecivedData = new ObservableCollection<byte>();

            if (!DataServices.BClassic.CheckBluetooth())
                DataServices.BClassic.BltAdapter.Enable();

            if (DataServices.BClassic.BltConnection != null)
            {
                DataServices.BClassic.OnDataReceived += OnRecived;
                DataServices.BClassic.BltConnection.OnError += OnError;
                DataServices.BClassic.BltConnection.OnStateChanged += OnStateChanged;
                DataServices.BClassic.BltConnection.OnTransmitted += OnTransmitted;
            }

            _notificationService = DependencyService.Get<INotificationService>();
            _notificationService.NotificationReceived += Notify();
        }

        private static EventHandler Notify()
        {
            return (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
            };
        }

        ~DeviceBCVM()
        {
            _notificationService.NotificationReceived -= Notify();
            if (DataServices.BClassic.BltConnection != null)
            {
                DataServices.BClassic.OnDataReceived -= OnRecived;
                DataServices.BClassic.BltConnection.OnError -= OnError;
                DataServices.BClassic.BltConnection.OnStateChanged -= OnStateChanged;
                DataServices.BClassic.BltConnection.OnTransmitted -= OnTransmitted;
            }
        }

        private void OnStateChanged(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            ConnectionState = stateChangedEventArgs.ConnectionState;
        }

        private void OnTransmitted(object sender, TransmittedEventArgs transmittedEventArgs)
        {
            Debug.WriteLine("Transmitted");
        }

        private void OnError(object sender, ThreadExceptionEventArgs threadExceptionEventArgs)
        {
            Debug.WriteLine($"--- OnError: {threadExceptionEventArgs.Exception.Message}");
        }

        private void OnRecived(object sender, BCRecivedEventArgs recivedEventArgs)
        {
            Msg = recivedEventArgs.RawJson;
            Debug.WriteLine($"--- Data Recived: {Msg}");
        }

        private async Task ScanDevices()
        {
            _notificationService.CreateNotification("Тест уведомлений", "Привет как дела!");
            Scanning = true;
            BluetoothClassicDevices.Clear();
            try
            {
                var devices = DataServices.BClassic.BltAdapter.BondedDevices;
                BluetoothClassicDevices = new ObservableCollection<BluetoothDeviceModel>(devices);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Внимание ошибка", ex.Message, "Ok");
            }
            Scanning = false;
        }
        private async Task Disconnect()
        {
            try
            {
                DataServices.BClassic.Disconnect();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Внимание ошибка", ex.Message, "Ok");
            }
        }
        private async Task SelectDevice()
        {
            try
            {
                var connection = DataServices.BClassic.Connect(SelectedDevice);
                if (connection != null)
                {
                    DataServices.BClassic.OnDataReceived += OnRecived;
                    connection.OnError += OnError;
                    connection.OnStateChanged += OnStateChanged;
                    connection.OnTransmitted += OnTransmitted;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Внимание ошибка", ex.Message, "Ok");
            }
        }
    }
}

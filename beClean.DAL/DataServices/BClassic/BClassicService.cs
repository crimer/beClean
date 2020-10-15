using Android.App;
using beClean.Droid.Services;
using beClean.Services.Models;
using Newtonsoft.Json;
using Plugin.BluetoothClassic.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace beClean.Services.DataServices.BClassic
{
    public class BClassicService : IBClassic
    {
        public IBluetoothManagedConnection BltConnection { get; set; }
        public IBluetoothAdapter BltAdapter { get; set; }
        public IEnumerable<BluetoothDeviceModel> deviceList { get; set; }
        public List<byte> recivedData { get; set; }
        public string RevicedString { get; set; }
        public BluetoothDeviceModel btDevice { get; set; }
        public bool IsScanning { get; set; }
        public bool IsConnected { get; set; }

        private event EventHandler<BCRecivedEventArgs> BluetoothDataReceived;
        private event EventHandler<TransmittedEventArgs> BluetoothDataTransmitted;
        private event EventHandler<ThreadExceptionEventArgs> BluetoothErrorCatch;
        private event EventHandler<StateChangedEventArgs> BluetoothConnectionChanged;

        public event EventHandler<TransmittedEventArgs> OnDataTransmitted
        {
            add
            {
                BluetoothDataTransmitted += value;
            }
            remove
            {
                BluetoothDataTransmitted -= value;
            }
        }
        public event EventHandler<ThreadExceptionEventArgs> OnErrorCatch
        {
            add
            {
                BluetoothErrorCatch += value;
            }
            remove
            {
                BluetoothErrorCatch -= value;
            }
        }
        public event EventHandler<StateChangedEventArgs> OnConnectionChanged
        {
            add
            {
                BluetoothConnectionChanged += value;
            }
            remove
            {
                BluetoothConnectionChanged -= value;
            }
        }
        public event EventHandler<BCRecivedEventArgs> OnDataReceived
        {
            add
            {
                BluetoothDataReceived += value;
            }
            remove
            {
                BluetoothDataReceived -= value;
            }
        }

        public BClassicService()
        {
            deviceList = new List<BluetoothDeviceModel>();
            recivedData = new List<byte>();
        }
   


        /// <summary>
        /// Включен ли Bluetooth на телефоне
        /// </summary>
        /// <returns></returns>
        public bool CheckBluetooth() => BltAdapter.Enabled;
        
        /// <summary>
        /// Список подключенных устройств
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BluetoothDeviceModel> GetDevices() => BltAdapter.BondedDevices;

        /// <summary>
        /// Подключение к устройству
        /// </summary>
        /// <param name="device">Модель устройства к которому подключаемся</param>
        /// <returns></returns>
        public IBluetoothManagedConnection Connect(BluetoothDeviceModel device)
        {
            // Включаем Bluetooth на телефоне если он отключен
            if (!CheckBluetooth())
                BltAdapter.Enable();
            var connection = BltAdapter.CreateManagedConnection(device);
            try
            {
                connection.Connect();
                BltConnection = connection;
                if(BltConnection != null)
                {
                    BltConnection.OnRecived += OnRecived;
                    BltConnection.OnError += OnError;
                    BltConnection.OnStateChanged += OnStateChanged;
                    BltConnection.OnTransmitted += OnTransmitted;
                }
                return BltConnection;
            }
            catch (BluetoothConnectionException exception)
            {
                Debug.WriteLine($"Can not connect to the device: {device.Name}({device.Address})");

                return BltConnection;
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Connect error: {exception.Message}");

                return BltConnection;
            }
        }

        /// <summary>
        /// Отключение соединения с устройством
        /// </summary>
        public void Disconnect()
        {
            if (BltConnection != null)
            {
                BltConnection.Dispose();
                BltConnection.OnRecived -= OnRecived;
                BltConnection.OnError -= OnError;
                BltConnection.OnStateChanged -= OnStateChanged;
                BltConnection.OnTransmitted -= OnTransmitted;
            }
        }

        /// <summary>
        /// Отправка комманды в Arduino
        /// </summary>
        /// <typeparam name="T">Тип аргументов для комманды</typeparam>
        /// <param name="message">Название комманды</param>
        /// <param name="data">Аргумент</param>
        public void SendCommand<T>(string message, T data)
        {
            string command = BuildCommand<T>(message, data);
            Memory<byte> memory = new Memory<byte>(Encoding.ASCII.GetBytes(command));
            BltConnection.Transmit(memory);
        }
        
        public void SendCommand(string message)
        {
            string command = BuildCommand(message);
            Memory<byte> memory = new Memory<byte>(Encoding.ASCII.GetBytes(command));
            BltConnection.Transmit(memory);
        }

        /// <summary>
        /// Построение комманды для отправки
        /// </summary>
        /// <typeparam name="T">Тип аргументов для комманды</typeparam>
        /// <param name="command">Название комманды</param>
        /// <param name="data">Аргумент</param>
        /// <returns>Собранныя строка для отправки</returns>
        private string BuildCommand<T>(string command, T data = default)
        {
            string result = command.Trim().ToUpper();
            if(data != null)
            {
                result += '@';
                result += data.ToString();
            }
            result += '\n';
            return result;
        }
        private string BuildCommand(string command)
        {
            string result = command.Trim().ToUpper();
            result += '\n';
            return result;
        }

        private void OnStateChanged(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            BluetoothConnectionChanged?.Invoke(this, stateChangedEventArgs);
        }

        private void OnError(object sender, System.Threading.ThreadExceptionEventArgs threadExceptionEventArgs)
        {
            BluetoothErrorCatch?.Invoke(this, threadExceptionEventArgs);
            Disconnect();
        }

        private void OnTransmitted(object sender, TransmittedEventArgs transmittedEventArgs)
        {
            Debug.WriteLine(System.Text.Encoding.UTF8.GetString(transmittedEventArgs.Buffer.ToArray()));
        }

        private void OnRecived(object sender, RecivedEventArgs recivedEventArgs)
        {
            for (int index = 0; index < recivedEventArgs.Buffer.Length; index++)
            {
                byte simbol = recivedEventArgs.Buffer.ToArray()[index];
                // \r - 13
                // \n = 10
                if (simbol == 10)
                {
                    RevicedString = System.Text.Encoding.UTF8.GetString(recivedData.ToArray());
                    recivedData.Clear();
                }
                recivedData.Add(simbol);
            }
            
            
            IEnumerable<Datum> datas = JsonConvert.DeserializeObject<DeviceData>(RevicedString).Data;
            if(datas.Any(x => x.Value == "20"))
            {
                string title = $"Hello message";
                string message = $"Hello Nikita Shevchenko";
                DataServices.Notifications.ScheduleNotification(title, message);
            }

            BluetoothDataReceived?.Invoke(this, new BCRecivedEventArgs(recivedData.ToArray(), RevicedString));
        }
    }
}

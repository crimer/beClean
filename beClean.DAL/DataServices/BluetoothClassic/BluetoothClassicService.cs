using Android.Bluetooth;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace beClean.DAL.DataServices.BluetoothClassic
{
    public class BluetoothClassicService : IBluetoothClassicService
    {
        private CancellationTokenSource _ct { get; set; }
        public BluetoothDevice btDevice { get; set; }
        public IEnumerable<BluetoothDevice> deviceList { get; set; }
        public IEnumerable<byte> recivedData { get; set; }
        public string MessageToSend { get; set; }
        public bool IsScanning { get; set; }
        public bool IsConnected { get; set; }

        private BluetoothAdapter BluetoothAdapter = null;
        private BluetoothSocket BluetoothSocket = null;
        private Stream outputStream = null;
        private Stream inputStream = null;

        private string DeviceAddress = "98:D3:C1:FD:4B:3A";
        private UUID DeviceUUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");

        private event EventHandler<BluetoothRecivedEventArgs> BluetoothDataReceived;
        public event EventHandler<BluetoothRecivedEventArgs> OnDataReceived
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

        public BluetoothClassicService()
        {
            _ct = new CancellationTokenSource();
            deviceList = new List<BluetoothDevice>();
            recivedData = new List<byte>();
            BluetoothAdapter = BluetoothAdapter.DefaultAdapter;
        }
        public void Connect(BluetoothDevice device)
        {
            var ter = device.GetUuids();
            var t = System.Text.Encoding.ASCII.GetBytes("Hello BB from Arduino! Counter:199");
            Debug.WriteLine(t.Length);
            ConnectDevice(device);
        }

        public void Disconnect()
        {
            if (_ct != null)
            {
                Debug.WriteLine("BluetoothSocket Disconnected!");
                BluetoothSocket.Close();
                _ct.Cancel();
            }
        }

        public IEnumerable<BluetoothDevice> PairedDevices()
        {
            List<BluetoothDevice> devices = new List<BluetoothDevice>();

            foreach (var device in BluetoothAdapter.BondedDevices)
                devices.Add(device);

            deviceList = devices;

            return deviceList;
        }


        public bool CheckBluetooth()
        {
            bool enabled = false;

            if (BluetoothAdapter == null)
                Debug.WriteLine("Bluetooth adapter not found");
            else
                Debug.WriteLine("Bluetooth adapter found!");


            if (!BluetoothAdapter.IsEnabled)
                Debug.WriteLine("Bluetooth adapter OFF");
            else
            {
                Debug.WriteLine("Bluetooth adapter ON");
                enabled = true;
            }

            return enabled;
        }

        private void ConnectDevice(BluetoothDevice device)
        {
            BluetoothAdapter.CancelDiscovery();
            while (_ct.IsCancellationRequested == false)
            {
                try
                {
                    Thread.Sleep(250);

                    if (!CheckBluetooth()) return;

                    Debug.WriteLine("Try to connect to " + device.Name);

                    if (device == null)
                        Debug.WriteLine("Named device not found.");
                    else
                    {
                        BluetoothSocket = device.CreateRfcommSocketToServiceRecord(DeviceUUID);
                        if (BluetoothSocket != null)
                        {
                            BluetoothSocket.Connect();

                            if (BluetoothSocket.IsConnected)
                            {
                                Debug.WriteLine("BluetoothSocket Connected!");
                                BeginListenForData();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        BluetoothSocket.Close();
                        Debug.WriteLine($"Connection closed with error: {ex.Message}");
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Impossible to connect");
                    }
                }
                //finally
                //{
                //    if (BluetoothSocket != null)
                //        BluetoothSocket.Close();
                //    device = null;
                //    BluetoothAdapter = null;
                //}
            }
        }

        public void BeginListenForData()
        {
            try
            {
                inputStream = BluetoothSocket.InputStream;
            }
            catch (System.IO.IOException ex)
            {
                Debug.WriteLine($"beginListenForData error: {ex.Message}");
            }

            byte[] buffer = new byte[38];
            int bytes;

            while (true)
            {

                try
                {
                    bytes = inputStream.Read(buffer, 0, buffer.Length);

                    if (bytes <= 0) return;

                    //buffer = ReadFully(inputStream);
                    //using (MemoryStream ms = new MemoryStream())
                    //{
                    //    int read;
                    //    while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    //    {
                    //        ms.Write(buffer, 0, read);
                    //    }
                    //    buffer = ms.ToArray();
                    //    

                    //}

                    string content = System.Text.Encoding.ASCII.GetString(buffer);
                    BluetoothDataReceived?.Invoke(this, new BluetoothRecivedEventArgs(buffer, content));
                }
                catch (Java.IO.IOException)
                {

                }
            }
        }

        public void SendData(string data)
        {
            try
            {
                outputStream = BluetoothSocket.OutputStream;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error with OutputStream when write to Serial port" + e.Message);
            }

            Java.Lang.String message = (Java.Lang.String)data;

            byte[] msgBuffer = message.GetBytes();

            try
            {
                outputStream.Write(msgBuffer, 0, msgBuffer.Length);
                Debug.WriteLine("Message sent");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error with  when write message to Serial port" + e.Message);
                //status.Text = "Error with  when write message to Serial port";
            }
        }
    }
}

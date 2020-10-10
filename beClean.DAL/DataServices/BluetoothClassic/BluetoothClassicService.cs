using Android.Bluetooth;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace beClean.DAL.DataServices.BluetoothClassic
{
    public class BluetoothClassicService : IBluetoothClassicService
    {
        private CancellationTokenSource _ct { get; set; }
        public string MessageToSend { get; set; }
        public bool IsScanning { get; set; }
        public ObservableCollection<BluetoothDevice> deviceList { get; set; }
        public BluetoothDevice btDevice { get; set; }
        private BluetoothAdapter BluetoothAdapter = null;
        private BluetoothSocket BluetoothSocket = null;
        private Stream outputStream = null;
        private Stream inputStream = null;
        private static string DeviceAddress = "98:D3:C1:FD:4B:3A";
        private static UUID DeviceUUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");

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
            deviceList = new ObservableCollection<BluetoothDevice>();
            BluetoothAdapter = BluetoothAdapter.DefaultAdapter;
        }
        public void Connect(BluetoothDevice device)
        {
            var ter = device.GetUuids();
            ConnectDevice(device);
        }

        public void Disconnect()
        {
            if (_ct != null)
            {
                Debug.WriteLine("Send a cancel to task!");
                BluetoothSocket.Close();
                _ct.Cancel();
            }
        }

        public IEnumerable<BluetoothDevice> PairedDevices()
        {
            deviceList.Clear();

            foreach (var bd in BluetoothAdapter.BondedDevices)
                deviceList.Add(bd);

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
                        //UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");

                        //BluetoothSocket = device.CreateInsecureRfcommSocketToServiceRecord(DeviceUUID);
                        BluetoothSocket = device.CreateRfcommSocketToServiceRecord(DeviceUUID);
                        if (BluetoothSocket != null)
                        {
                            BluetoothSocket.Connect();

                            if (BluetoothSocket.IsConnected)
                            {
                                Debug.WriteLine("BluetoothSocket Connected!");
                                beginListenForData();
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
        public byte[] ReadFully(Stream input)
        {
           byte[] buffer = new byte[16 * 1024];
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReadFully error: {ex.Message}");
            }
            return new byte[1024];
        }
        public void beginListenForData()
        {
            try
            {
                inputStream = BluetoothSocket.InputStream;
            }
            catch (System.IO.IOException ex)
            {
                Debug.WriteLine($"beginListenForData error: {ex.Message}");
            }

            //byte[] buffer = new byte[1024];
            byte[] buffer = new byte[1024];
            int bytes;

            while (true)
            {
                
                try
                {
                    bytes = inputStream.Read(buffer, 0, 1024);
                    //Debug.WriteLine("bytes resived:" + bytes.ToString());
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

                    //string valor = System.Text.Encoding.ASCII.GetString(buffer);
                    BluetoothDataReceived?.Invoke(this, new BluetoothRecivedEventArgs(buffer));
                    //Debug.WriteLine(valor);


                    // transform string for deleate all symbols except 1-4(command from canny).
                    //string command = new string(valor.Where(char.IsDigit).ToArray());

                    //if (command.Length > 0)
                    //{
                    //status.Text = "data successfully readed";
                    //System.Console.WriteLine("command  " + command);
                    //switch (Int32.Parse(command))
                    //{
                    //    case 0:
                    //        rSwitch.Text = "reed switch - disconnected ";
                    //        EndSensor.Text = "end sensor - not pressed ";
                    //        break;
                    //    case 1:
                    //        rSwitch.Text = "reed switch - disconnected ";
                    //        EndSensor.Text = "end sensor - pressed ";
                    //        break;
                    //    case 2:
                    //        rSwitch.Text = "reed switch - connected ";
                    //        EndSensor.Text = "end sensor - not pressed ";
                    //        break;
                    //    case 3:
                    //        rSwitch.Text = "reed switch - connected ";
                    //        EndSensor.Text = "end sensor - pressed ";
                    //        break;
                    //}
                    //}
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

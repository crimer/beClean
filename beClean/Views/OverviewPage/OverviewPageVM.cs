using beClean.DAL.DataServices;
using beClean.DAL.DataServices.BClassic;
using beClean.DAL.Models;
using beClean.Views.Base;
using Newtonsoft.Json;
using Plugin.BluetoothClassic.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace beClean.Views.OverviewPage
{
    public class OverviewPageVM : BaseVM
    {
        public ObservableCollection<Datum> Datum
        {
            get => Get<ObservableCollection<Datum>>();
            set => Set(value);
        }
        public string Json
        {
            get => Get<string>();
            set => Set(value);
        }
        public OverviewPageVM() : base("Просмотр")
        {
            //Devices = new ObservableCollection<DeviceObject>(new[]
            //{
            //    new DeviceObject(){Id=1,Name="Давление", Value = 23.0f, IconSource= "resource://beClean.Resources.Svg.devices.pressure.svg"},
            //    new DeviceObject(){Id=2,Name="Температура", Value = 22.3f, IconSource= "resource://beClean.Resources.Svg.devices.temperature.svg"},
            //    new DeviceObject(){Id=3,Name="CO2", Value=14f , IconSource= "resource://beClean.Resources.Svg.devices.carbon-dioxide.svg"},
            //    new DeviceObject(){Id=4,Name="Влажность",Value=44.0f, IconSource= "resource://beClean.Resources.Svg.devices.humidity.svg"},
            //    new DeviceObject(){Id=1,Name="Освященность",Value=12, IconSource= "resource://beClean.Resources.Svg.devices.lightbulb.svg"},
            //    new DeviceObject(){Id=2,Name="Пламя",Value=23, IconSource= "resource://beClean.Resources.Svg.devices.fire.svg"},
            //});
            
            if (DataServices.BClassic.BltConnection != null)
            {
                DataServices.BClassic.OnDataReceived += OnRecived;
                DataServices.BClassic.BltConnection.OnError += OnError;
                DataServices.BClassic.BltConnection.OnTransmitted += OnTransmitted;
            }
        }
        ~OverviewPageVM()
        {
            DataServices.BClassic.OnDataReceived -= OnRecived;
            DataServices.BClassic.BltConnection.OnError -= OnError;
            DataServices.BClassic.BltConnection.OnTransmitted -= OnTransmitted;
        }
        
        private void OnTransmitted(object sender, TransmittedEventArgs transmittedEventArgs)
        {
            throw new NotImplementedException();
        }

        private void OnError(object sender, ThreadExceptionEventArgs threadExceptionEventArgs)
        {
            Debug.WriteLine($"--- OnError: {threadExceptionEventArgs.Exception.Message}");
        }

        private void OnRecived(object sender, BCRecivedEventArgs recivedEventArgs)
        {
            Json = recivedEventArgs.Content;
            Debug.WriteLine($"--- Data Recived: {Json}");
            IEnumerable<Datum> datas = JsonConvert.DeserializeObject<DeviceData>(Json).Data;
            Datum = new ObservableCollection<Datum>(datas);
        }
    }
}

using beClean.Services.DataServices;
using beClean.Services.DataServices.BClassic;
using beClean.Services.Models;
using beClean.Views.Base;
using Newtonsoft.Json;
using Plugin.BluetoothClassic.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            // Mock
            Datum = new ObservableCollection<Datum>(new[]
            {
                new Datum(){ Name="Давление", Value = "23.0", Type="Temp"},
                new Datum(){ Name="Давление", Value = "23.0", Type="Humidity"},
                new Datum(){ Name="Давление", Value = "23.0", Type="Fire"},
                new Datum(){ Name="Давление", Value = "23.0", Type="Light"},
            });

            if (DataServices.BClassic.BltConnection != null)
            {
                DataServices.BClassic.OnDataReceived += OnRecived;
                DataServices.BClassic.BltConnection.OnError += OnError;
                DataServices.BClassic.BltConnection.OnTransmitted += OnTransmitted;
            }
        }


        ~OverviewPageVM()
        {
            if (DataServices.BClassic.BltConnection != null) 
            {
                DataServices.BClassic.OnDataReceived -= OnRecived;
                DataServices.BClassic.BltConnection.OnError -= OnError;
                DataServices.BClassic.BltConnection.OnTransmitted -= OnTransmitted;
            }

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
            Json = recivedEventArgs.RawJson;
            Debug.WriteLine($"--- Data Recived: {Json}");
            IEnumerable<Datum> datas = JsonConvert.DeserializeObject<DeviceData>(Json).Data;
            Datum = new ObservableCollection<Datum>(datas);
        }
    }
}

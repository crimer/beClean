using beClean.Services.DataServices;
using beClean.Services.DataServices.BClassic;
using beClean.Services.Models;
using beClean.Views.Base;
using Newtonsoft.Json;
using Plugin.BluetoothClassic.Abstractions;
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
            //Datum = new ObservableCollection<Datum>(new[]
            //{
            //    new Datum(){ Value = "23.0", Type=Consts.TEMP_PARAM},
            //    new Datum(){ Value = "23.0", Type=Consts.HUMIDITY_PARAM},
            //    new Datum(){ Value = "23.0", Type=Consts.FIRE_PARAM},
            //    new Datum(){ Value = "23.0", Type=Consts.LIGHT_PARAM},
            //    new Datum(){ Value = "23.0", Type=Consts.PRESSUE_PARAM},
            //    new Datum(){ Value = "23.0", Type=Consts.CO_PARAM},
            //    new Datum(){ Value = "23.0", Type=Consts.CO2_PARAM}
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
            if (DataServices.BClassic.BltConnection != null)
            {
                DataServices.BClassic.OnDataReceived -= OnRecived;
                DataServices.BClassic.BltConnection.OnError -= OnError;
                DataServices.BClassic.BltConnection.OnTransmitted -= OnTransmitted;
            }

        }

        private void OnTransmitted(object sender, TransmittedEventArgs transmittedEventArgs)
        {
            Debug.WriteLine($"--- OnTransmitted");
        }

        private void OnError(object sender, ThreadExceptionEventArgs threadExceptionEventArgs)
        {
            Debug.WriteLine($"--- OnError: {threadExceptionEventArgs.Exception.Message}");
        }

        private void OnRecived(object sender, BCRecivedEventArgs recivedEventArgs)
        {
            Json = recivedEventArgs.RawJson;
            var datas = JsonConvert.DeserializeObject<DeviceData>(Json).Data;
            Datum = new ObservableCollection<Datum>(datas);
        }
    }
}

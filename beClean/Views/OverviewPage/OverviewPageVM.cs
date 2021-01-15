using beClean.Services.DataServices;
using beClean.Services.DataServices.BClassic;
using beClean.Services.DataServices.Notifications;
using beClean.Services.Models;
using beClean.Views.Base;
using Newtonsoft.Json;
using Plugin.BluetoothClassic.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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
        private int _fireCount;
        public ICommand NotifyCommand => MakeCommand(NotifyImpl);
        private readonly INotificationService _notificationService;
        public OverviewPageVM() : base("Просмотр")
        {
            _notificationService = DependencyService.Get<INotificationService>();
            _fireCount = 0;
            
            // Mock
            //Datum = new ObservableCollection<Datum>(new[]
            //{
            //    new Datum(){ Value = "23.0", Type=Consts.TEMP_PARAM},
            //    new Datum(){ Value = "57", Type=Consts.HUMIDITY_PARAM},
            //    new Datum(){ Value = "Горим", Type=Consts.FIRE_PARAM},
            //    new Datum(){ Value = "44", Type=Consts.LIGHT_PARAM},
            //    new Datum(){ Value = "15", Type=Consts.PRESSUE_PARAM},
            //    new Datum(){ Value = "20", Type=Consts.CO_PARAM},
            //    new Datum(){ Value = "19", Type=Consts.CO2_PARAM}
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

        private void NotifyImpl()
        {
            _notificationService.CreateNotification("Тест уведомлений", "Тест уведомлений");
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
            IEnumerable<Datum> datas = JsonConvert.DeserializeObject<DeviceData>(Json).Data;
            
            Datum fire = datas.Where(item => item.Type == Consts.FIRE_PARAM).Select(x => x).FirstOrDefault();

            if (fire.Value == "Горим")
                _fireCount++;

            if (fire.Value != "Горим")
                _fireCount = 0;


            if (fire.Value == "Горим" && _fireCount == 1)
                _notificationService.CreateNotification("Внимание", "Возможно возникновение пожара");

            Datum = new ObservableCollection<Datum>(datas);
        }
    }
}

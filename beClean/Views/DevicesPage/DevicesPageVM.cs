using beClean.Models;
using beClean.Views.Base;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

namespace beClean.Views.DevicesPage
{
    public class DevicesPageVM : BaseVM
    {
        public ICommand HiCommand => MakeCommand(HiCommandImpl);
        public IEnumerable<DeviceObject> Devices
        {
            get => Get<IEnumerable<DeviceObject>>();
            set => Set(value);
        }

        private void HiCommandImpl()
        {
            Debug.WriteLine("Command");
        }

        public DevicesPageVM() : base("Устройства")
        {
            Devices = new List<DeviceObject>(new[]
            {
                new DeviceObject(){Id=1,Name="Прибор 1"},
                new DeviceObject(){Id=2,Name="Прибор 2"},
                new DeviceObject(){Id=3,Name="Прибор 3"},
                new DeviceObject(){Id=4,Name="Прибор 4"},
                new DeviceObject(){Id=1,Name="Прибор 1"},
                new DeviceObject(){Id=2,Name="Прибор 2"},
                new DeviceObject(){Id=3,Name="Прибор 3"},
                new DeviceObject(){Id=4,Name="Прибор 4"},
            });
            Debug.WriteLine(Devices);
        }
    }
}

using beClean.Models;
using beClean.Views.Base;
using System.Collections.Generic;

namespace beClean.Views.OverviewPage
{
    public class OverviewPageVM : BaseVM
    {
        public IEnumerable<DeviceObject> Devices
        {
            get => Get<IEnumerable<DeviceObject>>();
            set => Set(value);
        }
        public OverviewPageVM() : base("Просмотр")
        {
            Devices = new List<DeviceObject>(new[]
            {
                new DeviceObject(){Id=1,Name="Давление", Value = 23.0f, IconSource= "resource://beClean.Resources.Svg.devices.pressure.svg"},
                new DeviceObject(){Id=2,Name="Температура", Value = 22.3f, IconSource= "resource://beClean.Resources.Svg.devices.temperature.svg"},
                new DeviceObject(){Id=3,Name="CO2", Value=14f , IconSource= "resource://beClean.Resources.Svg.devices.carbon-dioxide.svg"},
                new DeviceObject(){Id=4,Name="Влажность",Value=44.0f, IconSource= "resource://beClean.Resources.Svg.devices.humidity.svg"},
                new DeviceObject(){Id=1,Name="Освященность",Value=12, IconSource= "resource://beClean.Resources.Svg.devices.lightbulb.svg"},
                new DeviceObject(){Id=2,Name="Пламя",Value=23, IconSource= "resource://beClean.Resources.Svg.devices.fire.svg"},
            });
        }
    }
}

using beClean.Services.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace beClean.Conventers
{
    public class MetricIconConventer : IValueConverter
    {
        //    new DeviceObject(){Id=1,Name="Давление", Value = 23.0f, IconSource= "resource://beClean.Resources.Svg.devices.pressure.svg"},
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string metricType = (string)value;
            string icon = "resource://beClean.Resources.Svg.devices.";
            switch (metricType)
            {
                case "Temp":
                    icon += "temperature.svg";
                    break;
                case "Humidity":
                    icon += "humidity.svg";
                    break;
                case "Fire":
                    icon += "fire.svg";
                    break;
                case "Light":
                    icon += "lightbulb.svg";
                    break;
                default:
                    icon = "";
                    break;
            }
            return icon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

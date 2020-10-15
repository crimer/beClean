using beClean.Services.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace beClean.Conventers
{
    public class MetricTypeConventer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string metricType = (string)value;
            string data = "";
            switch (metricType)
            {
                case "Temp":
                    data = "°C";
                    break;
                case "Humidity":
                    data = "%";
                    break;
                case "Fire":
                case "Light":
                    data = "";
                    break;
                default:
                    data = "";
                    break;
            }
            return data;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

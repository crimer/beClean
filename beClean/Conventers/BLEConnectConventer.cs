using Plugin.BLE.Abstractions;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace beClean.Conventers
{
    public class BLEConnectConventer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (DeviceState)value;
            Color color;
            switch (state)
            {
                case DeviceState.Disconnected:
                    color = Color.Red;
                    break;
                case DeviceState.Connecting:
                    color = Color.Yellow;
                    break;
                case DeviceState.Connected:
                    color = Color.Green;
                    break;
                case DeviceState.Limited:
                    color = Color.Aqua;
                    break;
                default:
                    color = Color.Red;
                    break;
            }
            return color;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

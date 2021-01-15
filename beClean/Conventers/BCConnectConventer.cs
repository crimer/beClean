using Plugin.BluetoothClassic.Abstractions;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace beClean.Conventers
{
    public class BCConnectConventer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (ConnectionState)value;
            Color color;
            switch (state)
            {
                case ConnectionState.Created:
                case ConnectionState.Initializing:
                case ConnectionState.Connecting:
                    color = Color.Yellow;
                    break;
                case ConnectionState.Connected:
                    color = Color.Green;
                    break;
                case ConnectionState.Reconnecting:
                    color = Color.Orange;
                    break;
                case ConnectionState.ErrorOccured:
                case ConnectionState.Disconnecting:
                case ConnectionState.Disconnected:
                case ConnectionState.Disposing:
                case ConnectionState.Disposed:
                    color = Color.Red;
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

using Plugin.BluetoothClassic.Abstractions;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace beClean.Conventers
{
    public class ConnectionStatusConventer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (ConnectionState)value;
            string connectionText = string.Empty;
            switch (state)
            {
                case ConnectionState.Created:
                    connectionText = "Создано";
                    break;
                case ConnectionState.Initializing:
                    connectionText = "Начинается...";
                    break;
                case ConnectionState.Connecting:
                    connectionText = "Подключение...";
                    break;
                case ConnectionState.Connected:
                    connectionText = "Подключено";
                    break;
                case ConnectionState.Reconnecting:
                    connectionText = "Переподключение...";
                    break;
                case ConnectionState.ErrorOccured:
                    connectionText = "Ошибка";
                    break;
                case ConnectionState.Disconnecting:
                    connectionText = "Отключение...";
                    break;
                case ConnectionState.Disconnected:
                    connectionText = "Отключено";
                    break;
                case ConnectionState.Disposing:
                case ConnectionState.Disposed:
                    connectionText = "Соединение потеряно";
                    break;
                default:
                    connectionText = "";
                    break;
            }
            return connectionText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

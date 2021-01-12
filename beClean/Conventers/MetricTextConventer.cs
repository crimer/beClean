using System;
using System.Globalization;
using Xamarin.Forms;

namespace beClean.Conventers
{
    public class MetricTextConventer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string metricType = (string)value;
            string metricNme = string.Empty;
            switch (metricType)
            {
                case Consts.TEMP_PARAM:
                    metricNme = "Температура";
                    break;
                case Consts.HUMIDITY_PARAM:
                    metricNme = "Влажность";
                    break;
                case Consts.FIRE_PARAM:
                    metricNme = "Индикация пожара";
                    break;
                case Consts.LIGHT_PARAM:
                    metricNme = "Освешенность";
                    break;
                case Consts.CO_PARAM:
                    metricNme = "Угарный газ";
                    break;
                case Consts.CO2_PARAM:
                    metricNme = "Углекислый газ";
                    break;
                case Consts.PRESSUE_PARAM:
                    metricNme = "Атмосферное давление";
                    break;
                default:
                    metricNme = "";
                    break;
            }
            return metricNme;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

﻿using System;
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
                case Consts.TEMP_PARAM:
                    data = "°C";
                    break;
                case Consts.HUMIDITY_PARAM:
                    data = "%";
                    break;
                case Consts.FIRE_PARAM:
                case Consts.LIGHT_PARAM:
                case Consts.CO_PARAM:
                case Consts.CO2_PARAM:
                case Consts.PRESSUE_PARAM:
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
            return value;
        }
    }
}

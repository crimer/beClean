﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace beClean.Conventers
{
    public class MetricIconConventer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string metricType = (string)value;
            string icon = "resource://beClean.Resources.Svg.devices.";
            switch (metricType)
            {
                case Consts.TEMP_PARAM:
                    icon += "temperature.svg";
                    break;
                case Consts.HUMIDITY_PARAM:
                    icon += "humidity.svg";
                    break;
                case Consts.FIRE_PARAM:
                    icon += "fire.svg";
                    break;
                case Consts.LIGHT_PARAM:
                    icon += "lightbulb.svg";
                    break;
                case Consts.CO_PARAM:
                    icon += "CO.svg";
                    break;
                case Consts.CO2_PARAM:
                    icon += "CO2.svg";
                    break;
                case Consts.PRESSUE_PARAM:
                    icon += "pressure.svg";
                    break;
                default:
                    icon = "";
                    break;
            }
            return icon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

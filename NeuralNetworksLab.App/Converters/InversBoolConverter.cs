using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NeuralNetworksLab.App.Converters
{
    [ValueConversion(sourceType: typeof(bool), targetType: typeof(bool))]
    public class InversBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && bool.TryParse(value.ToString(), out bool parsed))
            {
                return !parsed;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

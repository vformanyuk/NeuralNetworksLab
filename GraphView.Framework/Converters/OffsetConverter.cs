using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GraphView.Framework.Converters
{
    public class OffsetConverter : IValueConverter
    {
        public double ManagedOffset { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && double.TryParse(value.ToString(), out var val))
            {
                return val + ManagedOffset;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

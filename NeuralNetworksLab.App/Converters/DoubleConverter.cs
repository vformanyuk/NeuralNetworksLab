using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace NeuralNetworksLab.App.Converters
{
    [ValueConversion(sourceType:typeof(double), targetType:typeof(string))]
    public class DoubleConverter : IValueConverter
    {
        private char _dSeparator = '\0';
        private bool _isEmptyString;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "0";

            if (_isEmptyString)
            {
                _isEmptyString = false;
                return string.Empty;
            }

            string postfix = _dSeparator == '\0' ? string.Empty : _dSeparator.ToString();
            _dSeparator = '\0';

            return value + postfix;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strRepr = value?.ToString();

            if (string.IsNullOrEmpty(strRepr))
            {
                _isEmptyString = true;
                return 0;
            }

            var last = strRepr.Last();
            if (!Char.IsDigit(last))
            {
                if (culture.NumberFormat.NumberDecimalSeparator.Contains(last) ||
                    CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator.Contains(last))
                {
                    _dSeparator = last;
                }
            }

            if (double.TryParse(strRepr, out double v)) return v;

            if (double.TryParse(strRepr, NumberStyles.Any, CultureInfo.InvariantCulture, out v)) return v;

            return DependencyProperty.UnsetValue;
        }
    }
}

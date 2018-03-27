using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NeuralNetworksLab.App.Converters
{
    public class IntegerConverter : IValueConverter
    {
        public bool Unsigned { get; set; }

        private bool _isEmptyString;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "0";

            if (_isEmptyString)
            {
                _isEmptyString = false;
                return string.Empty;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strRepr = value?.ToString();

            if (string.IsNullOrEmpty(strRepr))
            {
                _isEmptyString = true;
                return 0;
            }

            if (Unsigned && strRepr.StartsWith(CultureInfo.CurrentCulture.NumberFormat.NegativeSign))
            {
                strRepr = strRepr.Replace(CultureInfo.CurrentCulture.NumberFormat.NegativeSign, string.Empty);
            }

            if (Unsigned && strRepr.StartsWith(CultureInfo.InvariantCulture.NumberFormat.NegativeSign))
            {
                strRepr = strRepr.Replace(CultureInfo.InvariantCulture.NumberFormat.NegativeSign, string.Empty);
            }

            if (int.TryParse(strRepr, out int v)) return v;

            if (int.TryParse(strRepr, NumberStyles.Any, CultureInfo.InvariantCulture, out v)) return v;

            return DependencyProperty.UnsetValue;
        }
    }
}

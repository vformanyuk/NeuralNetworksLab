using System;
using System.Windows;
using NeuralNetworkLab.Infrastructure.Common.Properties;

namespace NeuralNetworksLab.App.Controls
{
    public class PropertyPresenterViewModel<T> : DependencyObject
    {
        public static readonly DependencyProperty PresentingValueProperty = DependencyProperty.Register(
            "PresentingValue", typeof(string), typeof(PropertyPresenterViewModel<T>), new PropertyMetadata(default(string), OnPresentingValueChanged));

        private static void OnPresentingValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == null) return;

            var vm = (PropertyPresenterViewModel<T>) d;
            if (vm._converter != null)
            {
                var converted = vm._converter.Invoke(e.NewValue);
                if (converted.Item1)
                {
                    vm._dataEmitter?.OnNext(converted.Item2);
                }
            }
        }

        public string PresentingValue
        {
            get => (string) GetValue(PresentingValueProperty);
            set => SetValue(PresentingValueProperty, value);
        }

        private readonly IObserver<T> _dataEmitter;
        private readonly Func<object, (bool,T)> _converter;
        public PropertyPresenterViewModel(NeuralNetworkProperty<T> model, IObserver<T> dataEmitter, Func<object,(bool,T)> validator)
        {
            this.PresentingValue = model.Value.ToString();

            _dataEmitter = dataEmitter;
            _converter = validator;
        }
    }
}

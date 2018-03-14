using System;
using System.Collections.Generic;
using System.Windows;
using NeuralNetworkLab.Infrastructure.Common.Properties;

namespace NeuralNetworksLab.App.Controls
{
    public class PropertyPresenterViewModel<T,V> : DependencyObject
    {
        public static readonly DependencyProperty PresentingValueProperty = DependencyProperty.Register(
            "PresentingValue", typeof(V), typeof(PropertyPresenterViewModel<T,V>), new PropertyMetadata(default(V), OnPresentingValueChanged));

        private static void OnPresentingValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == null) return;

            var vm = (PropertyPresenterViewModel<T,V>) d;
            if (vm._converter != null)
            {
                var converted = vm._converter.Invoke(e.NewValue);
                if (converted.Item1)
                {
                    vm._dataEmitter?.OnNext(converted.Item2);
                }
            }
        }

        public V PresentingValue
        {
            get => (V) GetValue(PresentingValueProperty);
            set => SetValue(PresentingValueProperty, value);
        }

        public IEnumerable<T> ValuesSet { get; }

        private readonly IObserver<T> _dataEmitter;
        private readonly Func<object, (bool,T)> _converter;
        public PropertyPresenterViewModel(NeuralNetworkProperty<T> model, V presentingValue, IObserver<T> dataEmitter, Func<object,(bool,T)> converter)
        {
            this.ValuesSet = model.ValuesCollection.Values;
            this.PresentingValue = presentingValue;

            _dataEmitter = dataEmitter;
            _converter = converter;
        }
    }
}

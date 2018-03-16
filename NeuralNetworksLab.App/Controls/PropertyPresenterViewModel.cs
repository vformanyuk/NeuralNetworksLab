using System;
using System.Collections.Generic;
using System.Windows;
using NeuralNetworkLab.Infrastructure.Common.Properties;

namespace NeuralNetworksLab.App.Controls
{
    public class PropertyPresenterViewModel<T> : DependencyObject
    {
        public static readonly DependencyProperty PresentingValueProperty = DependencyProperty.Register(
            "PresentingValue", typeof(T), typeof(PropertyPresenterViewModel<T>), new PropertyMetadata(default(T), OnPresentingValueChanged));

        private static void OnPresentingValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == null || e.NewValue == DependencyProperty.UnsetValue) return;

            var vm = (PropertyPresenterViewModel<T>) d;
            vm._dataEmitter?.OnNext((T) e.NewValue);
        }

        public T PresentingValue
        {
            get => (T) GetValue(PresentingValueProperty);
            set => SetValue(PresentingValueProperty, value);
        }

        public IEnumerable<T> ValuesSet { get; }

        private readonly IObserver<T> _dataEmitter;
        public PropertyPresenterViewModel(NeuralNetworkProperty<T> model, IObserver<T> dataEmitter)
        {
            this.ValuesSet = model.ValuesCollection.Values;

            _dataEmitter = dataEmitter;
        }
    }
}

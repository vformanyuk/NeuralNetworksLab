using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;
using NeuralNetworkLab.Infrastructure.Common.Properties;

namespace NeuralNetworksLab.App.Controls
{
    public class PropertyPresenterBase<T,U> : UserControl
    {
        public static readonly DependencyProperty PropertyDataProperty = DependencyProperty.Register(
            "PropertyData", typeof(NeuralNetworkProperty<T>), typeof(PropertyPresenterBase<T, U>), new PropertyMetadata(null, OnPropertyDataChanged));

        private static void OnPropertyDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is NeuralNetworkProperty<T> model)) return;

            var presenter = (PropertyPresenterBase<T, U>)d;
            presenter.DataContext = new PropertyPresenterViewModel<T, U>(model, presenter._presentingConverter.Invoke(model.Value), presenter._valueSetter, presenter._converter);

            if (presenter._subscribtionToken != null)
            {
                presenter._subscribtionToken.Dispose();
                presenter._subscribtionToken = null;
            }

            presenter._subscribtionToken = presenter._valueSetter
                .Subscribe(o =>
                {
                    if (presenter._intervalSubscription != null)
                    {
                        presenter._intervalSubscription.Dispose();
                        presenter._intervalSubscription = null;
                    }

                    var valueToSet = o;
                    presenter._intervalSubscription = Observable.Timer(TimeSpan.FromMilliseconds(100))
                        .Subscribe(_ => model.PropertySetter.Invoke(valueToSet));
                });
        }

        public NeuralNetworkProperty<T> PropertyData
        {
            get => (NeuralNetworkProperty<T>)GetValue(PropertyDataProperty);
            set => SetValue(PropertyDataProperty, value);
        }

        private IDisposable _subscribtionToken;
        private IDisposable _intervalSubscription;
        private readonly Subject<T> _valueSetter;
        private readonly Func<object, (bool, T)> _converter;
        private readonly Func<T, U> _presentingConverter;

        public PropertyPresenterBase(Func<object, (bool, T)> valueConverter, Func<T,U> presentingValueConverter)
        {
            _valueSetter = new Subject<T>();
            _converter = valueConverter;
            _presentingConverter = presentingValueConverter;
        }
    }
}

using System;
using System.Globalization;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;
using NeuralNetworkLab.Infrastructure.Common.Properties;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworksLab.App.Controls
{
    /// <summary>
    /// Interaction logic for DoublePropertyPresenter.xaml
    /// </summary>
    public partial class DoublePropertyPresenter : UserControl
    {
        public static readonly DependencyProperty PropertyDataProperty = DependencyProperty.Register(
            "PropertyData", typeof(IGenericProperty), typeof(DoublePropertyPresenter), new PropertyMetadata(null, OnPropertyDataChanged));

        public IGenericProperty PropertyData
        {
            get => (IGenericProperty)GetValue(PropertyDataProperty);
            set => SetValue(PropertyDataProperty, value);
        }

        private static void OnPropertyDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is DoubleProperty model))
                return;

            var presenter = (DoublePropertyPresenter) d;
            presenter.DataContext = new PropertyPresenterViewModel<double>(model, presenter._valueSetter, o =>
            {
                var result = double.TryParse(o.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out double v);
                if (!result)
                {
                    result = double.TryParse(o.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out v);
                }
                return (result, v);
            });

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

                    double valueToSet = o;
                    presenter._intervalSubscription = Observable.Timer(TimeSpan.FromMilliseconds(100))
                        .Subscribe(_ => model.PropertySetter.Invoke(valueToSet));
                });
        }

        private IDisposable _intervalSubscription;
        private readonly Subject<double> _valueSetter;
        private IDisposable _subscribtionToken;
        public DoublePropertyPresenter()
        {
            InitializeComponent();

            _valueSetter = new Subject<double>();
        }
    }
}

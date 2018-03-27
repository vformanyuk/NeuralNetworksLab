using System.Windows;
using System.Windows.Controls;
using NeuralNetworksLab.App.Converters;

namespace NeuralNetworksLab.App.Controls
{
    /// <summary>
    /// Interaction logic for IntegerPropertyPresenter.xaml
    /// </summary>
    public partial class IntegerPropertyPresenter : UserControl
    {
        public static readonly DependencyProperty UnsignedProperty = DependencyProperty.Register(
            "Unsigned", typeof(bool), typeof(IntegerPropertyPresenter), new PropertyMetadata(default(bool), UseUnsignedChanged));

        private static void UseUnsignedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var d = (IntegerPropertyPresenter) dependencyObject;
            d._converter.Unsigned = (bool)dependencyPropertyChangedEventArgs.NewValue;
        }

        public bool Unsigned
        {
            get => (bool) GetValue(UnsignedProperty);
            set => SetValue(UnsignedProperty, value);
        }


        private readonly IntegerConverter _converter;
        public IntegerPropertyPresenter()
        {
            InitializeComponent();

            _converter = (IntegerConverter)this.FindResource("IntConverter");
        }
    }
}

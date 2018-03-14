using System.Globalization;

namespace NeuralNetworksLab.App.Controls
{
    /// <summary>
    /// Interaction logic for DoublePropertyPresenter.xaml
    /// </summary>
    public partial class DoublePropertyPresenter : PropertyPresenterBase<double, string>
    {
        private static (bool, double) ConvertFromObject(object o)
        {
            var result = double.TryParse(o.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out double v);
            if (!result)
            {
                result = double.TryParse(o.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out v);
            }
            return (result, v);
        }

        public DoublePropertyPresenter() : base(ConvertFromObject, v => v.ToString(CultureInfo.CurrentCulture))
        {
            InitializeComponent();
        }
    }
}

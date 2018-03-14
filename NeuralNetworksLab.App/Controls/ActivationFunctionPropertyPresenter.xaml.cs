using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworksLab.App.Controls
{
    /// <summary>
    /// Interaction logic for ActivationFunctionPropertyPresenter.xaml
    /// </summary>
    public partial class ActivationFunctionPropertyPresenter : PropertyPresenterBase<IFunctor, IFunctor>
    {
        public ActivationFunctionPropertyPresenter() : base(o => (true, (IFunctor) o),
                                                            v => v)
        {
            InitializeComponent();
        }
    }
}

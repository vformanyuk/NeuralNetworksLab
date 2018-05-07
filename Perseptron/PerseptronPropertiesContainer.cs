using NeuralNetworkLab.Infrastructure.Common.Settings;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace Perseptron
{
    public class PerseptronPropertiesContainer : IPropertiesContainer
    {
        public IFunctor ActivationFunction { get; internal set; }
        public IFunctor ActivationFunctionDerivative { get; internal set; }
        public double Bias { get; internal set; }

        public PerseptronPropertiesContainer(ISettingsProvider settings)
        {
            var settingsActivationFunction = (ActivationFunctionSettingsItem)settings[typeof(Perseptron)][Plugin.PerseptronActivationFunctionSettingsKey];
            if (settingsActivationFunction != null)
            {
                this.ActivationFunction = settingsActivationFunction.Value;
                this.ActivationFunctionDerivative = settingsActivationFunction.Derivative;
            }
        }
    }
}

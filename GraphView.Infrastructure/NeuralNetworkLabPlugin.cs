using System;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure
{
    public abstract class NeuralNetworkLabPlugin
    {
        protected readonly ISettingsProvider _settings;
        protected NeuralNetworkLabPlugin(ISettingsProvider settings)
        {
            _settings = settings;
        }

        public abstract Type NeuronType { get; }

        public abstract NeuronNode CreateNeuronNode();
        public abstract NeuronBase CreateNeuronModel();

        public abstract IPropertiesProvider Properties { get; }
    }
}

using System;
using Autofac;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Common;
using NeuralNetworkLab.Infrastructure.Common.Functors;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace Perseptron
{
    public class Plugin : NeuralNetworkLabPlugin
    {
        public const string PerseptronActivationFunctionSettingsKey = "activ";

        public Plugin(ISettingsProvider settings) : base(settings)
        {
            settings.AddProperty(typeof(Perseptron), new ActivationFunctionProperty(PerseptronActivationFunctionSettingsKey, new Sigmoid()));
        }

        public override IToolbarElement ToolbarElement => throw new NotImplementedException();

        public override IPropertiesProvider Properties => throw new NotImplementedException();

        public override NeuronBase CreateNeuronModel()
        {
            return new Perseptron(this._settings);
        }

        public override NeuronNode CreateNeuronNode(NeuronBase neuron)
        {
            if(neuron is Perseptron perseptron)
            {
                return new PerseptronNode(perseptron);
            }
            throw new ArgumentException(nameof(neuron) + " is not of type " + typeof(Perseptron).Name);
        }
    }
}

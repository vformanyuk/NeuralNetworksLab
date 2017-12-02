using System;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace Perseptron
{
    public class Plugin : NeuralNetworkLabPlugin
    {
        public Plugin(ISettingsProvider settings) : base(settings)
        {

        }

        public override IToolbarElement ToolbarElement => throw new NotImplementedException();

        public override IPropertiesProvider Properties => throw new NotImplementedException();

        public override NeuronBase CreateNeuronModel()
        {
            return new Perseptron();
        }

        public override NeuronNode CreateNeuronNode(NeuronBase neuron)
        {
            return new PerseptronNode(neuron as Perseptron);
        }
    }
}

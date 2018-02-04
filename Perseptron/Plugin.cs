using System;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Common.Functors;
using NeuralNetworkLab.Infrastructure.Common.Settings;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace Perseptron
{
    public class Plugin : NeuralNetworkLabPlugin
    {
        public const string PerseptronActivationFunctionSettingsKey = "s_p_activ";

        public Plugin(ISettingsProvider settings) : base(settings)
        {
            settings.AddProperty(typeof(Perseptron), new ActivationFunctionSettingsItem(PerseptronActivationFunctionSettingsKey, new Sigmoid()));
            Properties = new PerseptronProperties(settings);
        }

        public override IPropertiesProvider Properties { get; }

        public override NeuronBase CreateNeuronModel()
        {
            return new Perseptron(this._settings);
        }

        public override Type NeuronType => typeof(Perseptron);

        public override NeuronNode CreateNeuronNode()
        {
            return new PerseptronNode();
        }
    }
}

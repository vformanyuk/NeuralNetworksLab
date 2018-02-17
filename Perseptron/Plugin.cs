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
        public const string PerseptronBiasSettingsKey = "s_p_bias";

        public Plugin(ISettingsProvider settings) : base(settings)
        {
            settings.AddProperty(typeof(Perseptron), new ActivationFunctionSettingsItem(PerseptronActivationFunctionSettingsKey, new Sigmoid()));
            settings.AddProperty(typeof(Perseptron), new BiasSettingsItem(PerseptronBiasSettingsKey, 0));
            PropertiesProvider = new PerseptronProperties(settings);
        }

        public override IPropertiesContrianer CreatePropertiesContrianer()
        {
            return new PerseptronPropertiesContainer(_settings);
        }

        public override IPropertiesProvider PropertiesProvider { get; }

        public override NeuronBase CreateNeuronModel(IPropertiesContrianer properties)
        {
            return new Perseptron(this._settings, properties as PerseptronPropertiesContainer);
        }

        public override Type NeuronType => typeof(Perseptron);

        public override NeuronNode CreateNeuronNode()
        {
            return new PerseptronNode(this._settings);
        }
    }
}

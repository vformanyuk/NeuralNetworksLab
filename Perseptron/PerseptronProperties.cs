using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Common;
using NeuralNetworkLab.Infrastructure.Common.Properties;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace Perseptron
{
    public class PerseptronProperties : IPropertiesProvider
    {
        const string ActivationFunctionKey = "p_p_ActivationFunction";
        const string BiasKey = "p_p_Bias";

        protected readonly ISettingsProvider _settingsProvider;

        private readonly Dictionary<string, IGenericProperty> _perseptronProperties;

        public PerseptronProperties(ISettingsProvider settings)
        {
            _settingsProvider = settings;
            _perseptronProperties = new Dictionary<string, IGenericProperty>();
        }

        public IReadOnlyDictionary<string, IGenericProperty> Properties => _perseptronProperties;

        public event EventHandler Loaded;

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Load(Perseptron model)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _perseptronProperties.Clear();
            _perseptronProperties.Add(ActivationFunctionKey, new ActivationFunctionProperty("Activation Function", v =>
            {
                model.ActivationFunction = v;
                model.ActivationFunctionDerivative = ActivationFunctionProperty.Derivatives[v];
            }, model.ActivationFunction));
            _perseptronProperties.Add(BiasKey, new DoubleProperty("Bias", v => model.Bias = v, model.Bias));

            Loaded?.Invoke(this, EventArgs.Empty);
        }

        public void Load(NeuronBase model)
        {
            this.Load(model as Perseptron);
        }

        public void Load(Layer layer)
        {
            throw new NotImplementedException();
        }

        public void Load(IEnumerable<NeuronBase> layer)
        {
            throw new NotImplementedException();
        }
    }
}

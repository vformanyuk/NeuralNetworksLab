using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetworkLab.Infrastructure.Common.Properties;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace Perseptron
{
    public class PerseptronProperties : IPropertiesProvider
    {
        const string ActivationFunctionKey = "p_p_ActivationFunction";
        const string BiasKey = "p_p_Bias";

        protected readonly ISettingsProvider SettingsProvider;

        private readonly HashSet<IGenericProperty> _perseptronProperties;

        public PerseptronProperties(ISettingsProvider settings)
        {
            SettingsProvider = settings;
            _perseptronProperties = new HashSet<IGenericProperty>();
        }

        public IReadOnlyCollection<IGenericProperty> Properties => _perseptronProperties;

        public event EventHandler Loaded;

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Load(IPropertiesContrianer model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!(model is PerseptronPropertiesContainer container))
            {
                throw new ArgumentException("Incorrect propertis container type");
            }

            _perseptronProperties.Clear();
            _perseptronProperties.Add(new ActivationFunctionProperty(ActivationFunctionKey, v =>
            {
                container.ActivationFunction = v;
                container.ActivationFunctionDerivative = ActivationFunctionProperty.Derivatives[v];
            }, container.ActivationFunction));
            _perseptronProperties.Add(new DoubleProperty(BiasKey, v => container.Bias = v, container.Bias));

            Loaded?.Invoke(this, EventArgs.Empty);
        }

        public void Load(IEnumerable<IPropertiesContrianer> contrainers)
        {
            if (contrainers == null)
            {
                throw new ArgumentNullException(nameof(contrainers));
            }

            var perseptronProps = contrainers.OfType<PerseptronPropertiesContainer>().ToList();

            if (perseptronProps.Count == 0)
            {
                return;
            }

            _perseptronProperties.Clear();
            _perseptronProperties.Add(new ActivationFunctionProperty(ActivationFunctionKey, v =>
            {
                var derivative = ActivationFunctionProperty.Derivatives[v];
                foreach (var container in perseptronProps)
                {
                    container.ActivationFunction = v;
                    container.ActivationFunctionDerivative = derivative;
                }
            }, perseptronProps[0].ActivationFunction));
            _perseptronProperties.Add(new DoubleProperty(BiasKey, v =>
            {
                foreach (var container in perseptronProps)
                {
                    container.Bias = v;
                }
            }, perseptronProps[0].Bias));

            Loaded?.Invoke(this, EventArgs.Empty);
        }
    }
}

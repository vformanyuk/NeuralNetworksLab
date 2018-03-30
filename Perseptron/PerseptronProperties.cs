using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using NeuralNetworkLab.Infrastructure.Common.Properties;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace Perseptron
{
    public class PerseptronProperties : IPropertiesProvider
    {
        const string ActivationFunctionKey = "p_p_ActivationFunction";
        const string BiasKey = "p_p_Bias";

        protected readonly ISettingsProvider SettingsProvider;

        private readonly ObservableCollection<IGenericProperty> _perseptronProperties;
        public ObservableCollection<IGenericProperty> Properties => _perseptronProperties;

        public PerseptronProperties(ISettingsProvider settings)
        {
            SettingsProvider = settings;
            _perseptronProperties = new ObservableCollection<IGenericProperty>();
        }

        public event EventHandler Loaded;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
            _perseptronProperties.Add(new ActivationFunctionProperty(ActivationFunctionKey,
                () => container.ActivationFunction,
                v =>
                {
                    container.ActivationFunction = v;
                    container.ActivationFunctionDerivative = ActivationFunctionProperty.Derivatives[v];
                }));
            _perseptronProperties.Add(new DoubleProperty(BiasKey, () => container.Bias, v => container.Bias = v));

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

            _perseptronProperties.Add(new ActivationFunctionProperty(ActivationFunctionKey,
                () => perseptronProps[0].ActivationFunction,
                v =>
                {
                    var derivative = ActivationFunctionProperty.Derivatives[v];
                    foreach (var container in perseptronProps)
                    {
                        container.ActivationFunction = v;
                        container.ActivationFunctionDerivative = derivative;
                    }
                }));

            _perseptronProperties.Add(new DoubleProperty(BiasKey, 
                () => perseptronProps[0].Bias,
                v =>
                {
                    foreach (var container in perseptronProps)
                    {
                        container.Bias = v;
                    }
                }));

            Loaded?.Invoke(this, EventArgs.Empty);
        }
    }
}

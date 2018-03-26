using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class LayerProperties : IPropertiesProvider
    {
        private const string NeuronsCountKey = "p_l_NeuronsCount";
        private const string CompactInputFibersKey = "p_l_CompactInputFibers";
        private const string CompactOutputFibersKey = "p_l_CompactOutputFibers";

        public event EventHandler Loaded;
        public ObservableCollection<IGenericProperty> Properties => _layerProperties;

        private readonly ObservableCollection<IGenericProperty> _layerProperties = new ObservableCollection<IGenericProperty>();
        private readonly INeuronFactory _neuronFactory;

        public LayerProperties(INeuronFactory factory)
        {
            _neuronFactory = factory;
        }

        public void Load(IPropertiesContrianer model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!(model is Layer layer))
            {
                throw new ArgumentException("Not a layer properties requested.");
            }

            _layerProperties.Add(new UintProperty(NeuronsCountKey, v => layer.NeuronsCount = v, layer.NeuronsCount));
            _layerProperties.Add(new BooleanProperty(CompactInputFibersKey, v => layer.UseCompactInputs = v, layer.UseCompactInputs));
            _layerProperties.Add(new BooleanProperty(CompactOutputFibersKey, v => layer.UseCompactOutputs = v, layer.UseCompactOutputs));
            _layerProperties.Add(null); // delimiter

            this.AddCustomProperties(layer, _layerProperties);

            _layerProperties.Add(null); // delimiter
            if (_neuronFactory.PropertyProviders.TryGetValue(layer.NeuronType, out IPropertiesProvider provider))
            {
                provider.Load(layer.NeuronProperties);
                foreach (var providerProperty in provider.Properties)
                {
                    _layerProperties.Add(providerProperty);
                }
            }

            this.Loaded?.Invoke(this, EventArgs.Empty);
        }

        public virtual void AddCustomProperties(Layer layer, ICollection<IGenericProperty> properties)
        {
        }

        public void Load(IEnumerable<IPropertiesContrianer> model)
        {
            throw new NotImplementedException();
        }

        public virtual void Commit()
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

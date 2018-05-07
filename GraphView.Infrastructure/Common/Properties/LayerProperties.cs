using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class LayerProperties : IPropertiesProvider
    {
        public const string NeuronTypeNameKey = "p_l_NeuronType";
        public const string NeuronsCountKey = "p_l_NeuronsCount";
        public const string CompactInputFibersKey = "p_l_CompactInputFibers";
        public const string CompactOutputFibersKey = "p_l_CompactOutputFibers";

        public event EventHandler Loaded;
        public ObservableCollection<IGenericProperty> Properties => _layerProperties;

        private readonly ObservableCollection<IGenericProperty> _layerProperties = new ObservableCollection<IGenericProperty>();
        private readonly INeuronFactory _neuronFactory;
        private Layer _selectedLayer;

        public LayerProperties(INeuronFactory factory)
        {
            _neuronFactory = factory;
        }

        public void UpdateNeuronsProperties(Layer layer)
        {
            if (layer.NeuronType != null && _neuronFactory.PropertyProviders.TryGetValue(layer.NeuronType, out IPropertiesProvider provider))
            {
                _layerProperties.Add(null); // delimiter

                provider.Load(layer.NeuronProperties);
                foreach (var providerProperty in provider.Properties)
                {
                    _layerProperties.Add(providerProperty);
                }
            }
        }

        public void Load(IPropertiesContainer model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!(model is Layer layer))
            {
                throw new ArgumentException("Not a layer properties requested.");
            }

            if (_selectedLayer != null)
            {
                _selectedLayer.PropertyChanged -= LayerPropertyChanged;
            }

            _selectedLayer = layer;
            _selectedLayer.PropertyChanged += LayerPropertyChanged;

            _layerProperties.Clear();

            var layerNames = _neuronFactory.Constructors.Keys.Where(k => k != typeof(Sensor)).Select(l => l.Name);
            _layerProperties.Add(new StringProperty(NeuronTypeNameKey, 
                () => layer.NeuronType?.Name ?? String.Empty, 
                n =>
                {
                    var neuronType =_neuronFactory.Constructors.Keys.FirstOrDefault(k => k.Name == n);
                    if (neuronType != null)
                    {
                        layer.NeuronType = neuronType;
                    }
                }, layerNames));
            _layerProperties.Add(new UintProperty(NeuronsCountKey, () => layer.NeuronsCount, v => layer.NeuronsCount = v));
            _layerProperties.Add(new BooleanProperty(CompactInputFibersKey, () => layer.UseCompactInputs, v => layer.UseCompactInputs = v));
            _layerProperties.Add(new BooleanProperty(CompactOutputFibersKey, () => layer.UseCompactOutputs, v => layer.UseCompactOutputs = v));
            _layerProperties.Add(null); // delimiter

            this.AddCustomProperties(layer, _layerProperties);

            this.UpdateNeuronsProperties(layer);

            this.Loaded?.Invoke(this, EventArgs.Empty);
        }

        private void LayerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            foreach (var genericProperty in _layerProperties)
            {
                genericProperty?.UpdateProperty();
            }
        }

        public virtual void AddCustomProperties(Layer layer, ICollection<IGenericProperty> properties)
        {
        }

        public void Load(IEnumerable<IPropertiesContainer> model)
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

using System;
using System.Collections.Generic;
using NeuralNetworkLab.Infrastructure.Common.Properties;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure
{
    public class LayerProperties : IPropertiesProvider
    {
        private const string NeuronsCountKey = "p_l_NeuronsCount";
        private const string CompactFibersKey = "p_l_CompactFibers";

        public event EventHandler Loaded;
        public IReadOnlyDictionary<string, IGenericProperty> Properties => _csvLayerProperties;

        protected readonly Dictionary<string, IGenericProperty> _csvLayerProperties = new Dictionary<string, IGenericProperty>();

        public void Load(NeuronBase model)
        {
            throw new NotImplementedException();
        }

        public virtual void AddCustomProperties(Layer layer)
        {
        }

        public void Load(Layer layer)
        {
            _csvLayerProperties.Add(NeuronsCountKey, new UintProperty("Nodes count", v => layer.NeuronsCount = v, layer.NeuronsCount));
            _csvLayerProperties.Add(CompactFibersKey, new BooleanProperty("Compact Fibers", v => layer.UseCompactFibersView = v, layer.UseCompactFibersView));

            this.AddCustomProperties(layer);

            this.Loaded?.Invoke(this, EventArgs.Empty);
        }

        public void Load(IEnumerable<NeuronBase> model)
        {
            throw new NotImplementedException();
        }

        public virtual void Commit()
        {
            throw new NotImplementedException();
        }
    }
}

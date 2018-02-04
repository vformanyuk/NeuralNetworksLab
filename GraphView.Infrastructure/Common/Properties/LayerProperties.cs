using System;
using System.Collections.Generic;
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
            _csvLayerProperties.Add(CompactInputFibersKey, new BooleanProperty("Compact Inputs", v => layer.UseCompactInputs = v, layer.UseCompactInputs));
            _csvLayerProperties.Add(CompactOutputFibersKey, new BooleanProperty("Compact Outputs", v => layer.UseCompactOutputs = v, layer.UseCompactOutputs));

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

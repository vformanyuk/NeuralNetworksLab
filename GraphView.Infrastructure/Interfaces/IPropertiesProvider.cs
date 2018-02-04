using System;
using System.Collections.Generic;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface IPropertiesProvider
    {
        event EventHandler Loaded;
        IReadOnlyDictionary<string, IGenericProperty> Properties { get; }
        void Load(NeuronBase model);
        void Load(Layer layer);
        void Load(IEnumerable<NeuronBase> model);
        void Commit();
    }
}

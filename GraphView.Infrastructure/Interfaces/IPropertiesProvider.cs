using System;
using System.Collections.Generic;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface IPropertiesProvider
    {
        event EventHandler Loaded;
        IReadOnlyDictionary<string, IGenericProperty> Properties { get; }
        void Load(NeuronBase model);
        void Commit();
    }
}

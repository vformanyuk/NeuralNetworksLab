using System;
using System.Collections.Generic;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface IPropertiesProvider
    {
        event EventHandler Loaded;
        IReadOnlyCollection<IGenericProperty> Properties { get; }
        void Load(IPropertiesContrianer properties);
        void Load(IEnumerable<IPropertiesContrianer> model);
        void Commit();
    }
}

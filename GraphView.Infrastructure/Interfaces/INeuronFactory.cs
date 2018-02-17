using System;
using System.Collections.Generic;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface INeuronFactory
    {
        IReadOnlyDictionary<Type, Func<IPropertiesContrianer, NeuronBase>> Constructors { get; }
        IReadOnlyDictionary<Type, Func<IPropertiesContrianer>> PropertiesContainerConstructors { get; }
        IReadOnlyDictionary<Type, IPropertiesProvider> PropertyProviders { get; }
    }
}

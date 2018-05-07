using System;
using System.Collections.Generic;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface INeuronFactory
    {
        IReadOnlyDictionary<Type, Func<IPropertiesContainer, NeuronBase>> Constructors { get; }
        IReadOnlyDictionary<Type, Func<INode>> NodeConstructors { get; }
        IReadOnlyDictionary<Type, Func<IPropertiesContainer>> PropertiesContainerConstructors { get; }
        IReadOnlyDictionary<Type, IPropertiesProvider> PropertyProviders { get; }
    }
}

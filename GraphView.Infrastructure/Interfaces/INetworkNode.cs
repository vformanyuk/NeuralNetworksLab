using System;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface INetworkNode : INode
    {
        IPropertiesContrianer Properties { get; }
        Type NeuronType { get; }
    }
}

using System;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface INetworkNode : INode
    {
        IPropertiesContainer Properties { get; }
        Type NeuronType { get; }
    }
}

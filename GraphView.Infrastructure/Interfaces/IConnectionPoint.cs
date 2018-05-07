using System;

namespace NeuralNetworkLab.Interfaces
{
    public interface IConnectionPoint
    {
        bool IsConnected { get; set; }
        INode Host { get; }
        bool CanConnect(IConnectionPoint connectionPoint);
        Guid Id { get; }
    }
}

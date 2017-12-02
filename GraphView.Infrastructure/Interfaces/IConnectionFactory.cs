﻿namespace NeuralNetworkLab.Interfaces
{
    public interface IConnectionsFactory
    {
        IConnection CreateConnection(IConnectionPoint sourcePoint, IConnectionPoint destinationPoint);
    }
}

using System.Collections.Generic;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Events
{
    public class ConnectorsRemovedEventArgs : EventAggregatorEventArgs
    {
        public  IEnumerable<IConnectionPoint> Connectors { get; }

        public ConnectorsRemovedEventArgs(IEnumerable<IConnectionPoint> connectors) => this.Connectors = connectors;
    }
}

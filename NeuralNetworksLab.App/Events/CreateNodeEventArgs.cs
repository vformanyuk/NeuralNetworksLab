using System;
using NeuralNetworkLab.Infrastructure.Events;

namespace NeuralNetworksLab.App.Events
{
    public class CreateNodeEventArgs : EventAggregatorEventArgs
    {
        public CreateNodeEventArgs(string name, Type nodeType)
        {
            Name = name;
            NodeType = nodeType;
        }

        public string Name { get; }
        public Type NodeType { get; }
    }
}

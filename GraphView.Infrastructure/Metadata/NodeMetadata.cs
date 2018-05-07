using System;

namespace NeuralNetworkLab.Infrastructure.Metadata
{
    public class NodeMetadata
    {
        public double X { get; }
        public double Y { get; }
        public Guid Id { get; }
        public NodeType Type { get; }

        public NodeMetadata(double x, double y, Guid id, NodeType nodeType)
        {
            this.X = x;
            this.Y = y;
            this.Id = id;
            this.Type = nodeType;
        }
    }
}

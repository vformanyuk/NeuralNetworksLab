using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Interfaces;

namespace Perseptron
{
    public class PerseptronNode : NeuronNode
    {
        public PerseptronNode()
        {
            Input = new Connector(this);
            Output = new Connector(this);
        }

        public IConnectionPoint Input { get; }
        public IConnectionPoint Output { get; }
    }
}

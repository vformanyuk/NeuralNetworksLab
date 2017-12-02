using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Interfaces;

namespace Perseptron
{
    public class PerseptronNode : NeuronNode
    {
        private IConnectionPoint _inputConnector, _outputConnector;

        public PerseptronNode(Perseptron model) : base(model)
        {
            _inputConnector = new Connector(this);
            _outputConnector = new Connector(this);
        }

        public IConnectionPoint Input => _inputConnector;
        public IConnectionPoint Output => _outputConnector;
    }
}

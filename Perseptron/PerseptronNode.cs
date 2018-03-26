using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworkLab.Interfaces;

namespace Perseptron
{
    public class PerseptronNode : NeuronNode
    {
        public PerseptronNode(ISettingsProvider settings) : base(typeof(Perseptron))
        {
            Input = new Connector(this);
            Output = new Connector(this);
            this.Properties = new PerseptronPropertiesContainer(settings);
        }

        public IConnectionPoint Input { get; }
        public IConnectionPoint Output { get; }
    }
}

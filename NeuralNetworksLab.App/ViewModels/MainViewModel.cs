using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GraphView.Framework;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworkLab.Interfaces;
using NeuralNetworksLab.App.Commands;
using NeuralNetworksLab.App.Events;
using NeuralNetworksLab.App.Services;

namespace NeuralNetworksLab.App.ViewModels
{
    public class MainViewModel
    {
        private IDiagram _diagram;
        private ISettingsProvider _settings;
        private ConnectionsFactory _neuroFibersConnectionFactory;
        private readonly List<NeuralNetworkLabPlugin> _plugins;

        private readonly List<NeuronBase> _neurons = new List<NeuronBase>();
        private readonly List<NeuroFiber> _fibers = new List<NeuroFiber>();

        public IDiagram Diagram => _diagram;

        public ICommand AddNodeCommand => new DelegateCommand(_ =>
        {
            var model = _plugins[0].CreateNeuronModel();
            _neurons.Add(model);

            var node = _plugins[0].CreateNeuronNode(model);
            _diagram.ChildNodes.Add(node);
        });

        public MainViewModel(ISettingsProvider settings, IEnumerable<NeuralNetworkLabPlugin> plugins)
        {
            _neuroFibersConnectionFactory = new ConnectionsFactory(settings);
            _neuroFibersConnectionFactory.ConnectionAdded += OnConnectionAdded;
            _neuroFibersConnectionFactory.ConnectionRemoved += OnConnectionRemoved;

            _diagram = new Diagram(_neuroFibersConnectionFactory);
            _settings = settings;
            _plugins = plugins.ToList();
        }

        private void OnConnectionRemoved(object sender, ConnectionEventArgs e)
        {
            var connection = (NeuroFiberConnection)e.Connection;
            connection.Model.Dispose();
            _fibers.Remove(connection.Model);
            connection.Model = null;
        }

        private void OnConnectionAdded(object sender, ConnectionEventArgs e)
        {
            var sourceNode = (NeuronNode)e.Connection.StartPoint.Host;
            var destinationNode = (NeuronNode)e.Connection.EndPoint.Host;
            var connection = (NeuroFiberConnection)e.Connection;

            var fiberModel = new NeuroFiber(sourceNode.Model, destinationNode.Model, _settings);
            connection.Model = fiberModel;
        }
    }
}

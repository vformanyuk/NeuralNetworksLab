using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GraphView.Framework;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworkLab.Interfaces;
using NeuralNetworksLab.App.Commands;
using NeuralNetworksLab.App.Services;

namespace NeuralNetworksLab.App.ViewModels
{
    public class MainViewModel
    {
        private IDiagram _diagram;
        private ISettingsProvider _settings;
        private readonly List<NeuralNetworkLabPlugin> _plugins = new List<NeuralNetworkLabPlugin>();

        public IDiagram Diagram => _diagram;

        public ICommand AddNodeCommand => new DelegateCommand(_ =>
        {
            var model = _plugins[0].CreateNeuronModel();
            var node = _plugins[0].CreateNeuronNode(model);
            _diagram.ChildNodes.Add(node);
        });

        public MainViewModel(ISettingsProvider settings)
        {
            _diagram = new Diagram(new ConnectionsFactory(settings));
            _settings = settings;
        }

        public void AddPlugin(NeuralNetworkLabPlugin plugin)
        {
            _plugins.Add(plugin);
        }
    }
}

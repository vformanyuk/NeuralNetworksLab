using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Events;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworksLab.App.Commands;
using NeuralNetworksLab.App.Events;

namespace NeuralNetworksLab.App.ViewModels
{
    public class ToolboxViewModel
    {
        public ICommand CreateNodeCommand { get; }

        public IEnumerable<CreateNodeEventArgs> Plugins { get; }

        public ToolboxViewModel(IEnumerable<NeuralNetworkLabPlugin> plugins)
        {
            CreateNodeCommand = new DelegateCommand<CreateNodeEventArgs>(CreatNodeHandler);

            var pluginContainers = plugins.Select(p => new CreateNodeEventArgs(p.NodeType.Name, p.NodeType)).ToList();
            pluginContainers.Add(new CreateNodeEventArgs("Layer", typeof(Layer)));
            pluginContainers.Add(new CreateNodeEventArgs("CSV Layer", typeof(CsvSensorLayer)));

            Plugins = pluginContainers;
        }

        private void CreatNodeHandler(CreateNodeEventArgs container)
        {
            EventAggregator.Publish(container);
        }
    }
}

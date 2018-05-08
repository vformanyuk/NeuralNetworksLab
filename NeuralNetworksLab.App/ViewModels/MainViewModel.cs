using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GraphView.Framework;

using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Events;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworkLab.Infrastructure.Network;
using NeuralNetworkLab.Interfaces;
using NeuralNetworksLab.App.Annotations;
using NeuralNetworksLab.App.Commands;
using NeuralNetworksLab.App.Events;
using NeuralNetworksLab.App.Extensions;
using NeuralNetworksLab.App.Services;

namespace NeuralNetworksLab.App.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ISettingsProvider _settings;
        private readonly INeuronFactory _neuronFactory;
        
        private readonly ConnectionsFactory _neuroFibersConnectionFactory;
        private readonly List<NeuralNetworkLabPlugin> _plugins;

        public ICommand RunSimulationCommand { get; }

        public IDiagram Diagram { get; }
        public ToolboxViewModel Toolbox { get; }
        public ILogAggregator LogAggregator { get; }

        private IPropertiesProvider _propertiesProvider;
        public IPropertiesProvider Properties
        {
            get => _propertiesProvider;
            private set
            {
                if (_propertiesProvider == value) return;

                _propertiesProvider = value;
                OnPropertyChanged();
            }
        }

        //TODO: Create data source genereal component. It contains input dataset and optionally output dataset
        //TODO: Point input layers to datasource.input
        //TODO: Point output node(s) or layer to datasource.output, if any
        public MainViewModel(ISettingsProvider settings, 
                             INeuronFactory neuronFactory,
                             ILogAggregator logAggregator,
                             IEnumerable<NeuralNetworkLabPlugin> plugins)
        {
            _plugins = plugins.ToList();

            _neuronFactory = neuronFactory;

            //TODO: currently logging subscribtions are treated as axons and brakes learning
            LogAggregator = logAggregator;

            _neuroFibersConnectionFactory = new ConnectionsFactory(settings);

            Diagram = new Diagram(_neuroFibersConnectionFactory);
            Diagram.NodeSelectionChanged += NodeSelectionChanged;

            Toolbox = new ToolboxViewModel(_plugins);
            EventAggregator.Subscribe<CreateNodeEventArgs>(OnNodeCreateRequested);
            EventAggregator.Subscribe<ConnectorsRemovedEventArgs>(OnConnectorsRemoved);

            _settings = settings;

            RunSimulationCommand = new DelegateCommand(RunSimulationHandler);
        }

        private void OnConnectorsRemoved(ConnectorsRemovedEventArgs obj)
        {
            var toRemove = (from c in Diagram.Connections
                where obj.Connectors.Contains(c.StartPoint) ||
                      obj.Connectors.Contains(c.EndPoint)
                select c).ToList();

            foreach (var connection in toRemove)
            {
                Diagram.Connections.Remove(connection);
            }

            toRemove.Clear();
        }

        private void OnNodeCreateRequested(CreateNodeEventArgs createNodeEventArgs)
        {
            if(!_neuronFactory.NodeConstructors.TryGetValue(createNodeEventArgs.NodeType, out Func<INode> f)) return;

            Diagram.ChildNodes.Add(f.Invoke());
        }

        private void NodeSelectionChanged(object sender, EventArgs e)
        {
            var selectedNodes = Diagram.ChildNodes.Where(n => n.IsSelected).MostCommon().ToList();

            if (selectedNodes.Count == 0)
            {
                this.Properties = null;
                return;
            }

            var selectedLayer = selectedNodes.OfType<Layer>().FirstOrDefault(); // only one layer may be selected
            if (selectedLayer != null)
            {
                this.Properties = _neuronFactory.PropertyProviders[selectedLayer.GetType()];
                this.Properties.Load(selectedLayer);
                return;
            }

            var selectedNeurons = selectedNodes.OfType<NeuronNode>().ToList();
            this.Properties = _neuronFactory.PropertyProviders[selectedNeurons[0].NeuronType];
            this.Properties.Load(selectedNeurons.Select(n => n.Properties));
        }

        private void RunSimulationHandler()
        {
            NetworkBuilder builder = new NetworkBuilder(Diagram, _neuronFactory, _settings, LogAggregator);
            using (var network = builder.CreateNetwork())
            {
                for (int i = 0; i < 1000; i++)
                {
                    network.RunSimulationAge();
                }
            }

        }

        #region IPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

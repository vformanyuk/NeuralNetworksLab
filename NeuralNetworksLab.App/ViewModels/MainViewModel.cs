using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using GraphView.Framework;

using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Events;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworkLab.Interfaces;
using NeuralNetworksLab.App.Annotations;
using NeuralNetworksLab.App.Events;
using NeuralNetworksLab.App.Extensions;
using NeuralNetworksLab.App.Services;

namespace NeuralNetworksLab.App.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IDiagram _diagram;
        private readonly ISettingsProvider _settings;
        private readonly INeuronFactory _neuronFactory;

        private readonly ConnectionsFactory _neuroFibersConnectionFactory;
        private readonly List<NeuralNetworkLabPlugin> _plugins;

        public IDiagram Diagram => _diagram;

        public ToolboxViewModel Toolbox { get; }

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

        public MainViewModel(ISettingsProvider settings, INeuronFactory neuronFactory, IEnumerable<NeuralNetworkLabPlugin> plugins)
        {
            _plugins = plugins.ToList();

            _neuronFactory = neuronFactory;

            _neuroFibersConnectionFactory = new ConnectionsFactory(settings);

            _diagram = new Diagram(_neuroFibersConnectionFactory);
            _diagram.NodeSelectionChanged += NodeSelectionChanged;

            Toolbox = new ToolboxViewModel(_plugins);
            EventAggregator.Subscribe<CreateNodeEventArgs>(OnNodeCreateRequested);

            _settings = settings;
        }

        private void OnNodeCreateRequested(CreateNodeEventArgs createNodeEventArgs)
        {
            if(!_neuronFactory.NodeConstructors.TryGetValue(createNodeEventArgs.NodeType, out Func<INode> f)) return;

            _diagram.ChildNodes.Add(f.Invoke());
        }

        private void NodeSelectionChanged(object sender, EventArgs e)
        {
            var selectedNodes = _diagram.ChildNodes.Where(n => n.IsSelected).MostCommon().ToList();

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

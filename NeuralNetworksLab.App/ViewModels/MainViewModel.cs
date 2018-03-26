using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GraphView.Framework;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Common.Functors;
using NeuralNetworkLab.Infrastructure.Common.Properties;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworkLab.Interfaces;
using NeuralNetworksLab.App.Annotations;
using NeuralNetworksLab.App.Commands;
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

        public IGenericProperty DoubleProperty { get; }

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

        public ICommand AddNodeCommand => new DelegateCommand(_ =>
        {
            var node = _plugins[0].CreateNeuronNode();
            _diagram.ChildNodes.Add(node);
        });

        public MainViewModel(ISettingsProvider settings, INeuronFactory neuronFactory, IEnumerable<NeuralNetworkLabPlugin> plugins)
        {
            _neuronFactory = neuronFactory;

            _neuroFibersConnectionFactory = new ConnectionsFactory(settings);

            _diagram = new Diagram(_neuroFibersConnectionFactory);
            _diagram.NodeSelectionChanged += NodeSelectionChanged;

            _settings = settings;

            _plugins = plugins.ToList();

            DoubleProperty = new ActivationFunctionProperty("activation F", v => Debug.WriteLine(v), new ReLu());
        }

        private void NodeSelectionChanged(object sender, EventArgs e)
        {
            var selectedNodes = _diagram.ChildNodes.Where(n => n.IsSelected).MostCommon().OfType<NeuronNode>().ToList();

            if (selectedNodes.Count == 0)
            {
                this.Properties = null;
                return;
            }

            this.Properties = _neuronFactory.PropertyProviders[selectedNodes[0].NeuronType];
            this.Properties.Load(selectedNodes.Select(n => n.Properties));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NeuralNetworkLab.Infrastructure.Common.Properties;
using NeuralNetworkLab.Infrastructure.Events;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults.Layers;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworkLab.Infrastructure.FrameworkDefaults
{
    public class Layer : NeuronNode, IPropertiesContainer
    {
        private readonly INeuronFactory _neuronFactory;
        private readonly LayerProperties _propertiesProvider;

        private readonly ObservableCollection<IConnectionPoint> _inputs = new ObservableCollection<IConnectionPoint>();
        private readonly ObservableCollection<IConnectionPoint> _outputs = new ObservableCollection<IConnectionPoint>();

        private IPropertiesContainer _properties;

        public Layer(INeuronFactory factory) : base(typeof(NeuronBase))
        {
            _neuronFactory = factory;
            _compactInputsView = true;
            _compactOutputsView = true;
            _neuronsCount = 1; // each layer has one neuron by defeault

            if (_neuronFactory.PropertyProviders.TryGetValue(this.GetType(), out IPropertiesProvider provider) &&
                provider is LayerProperties layerProperties)
            {
                _propertiesProvider = layerProperties;
            }

            this.Properties = this;
        }

        private bool _compactInputsView;
        public bool UseCompactInputs
        {
            get => _compactInputsView;
            set
            {
                if (_compactInputsView == value) return;

                _compactInputsView = value;
                this.UpdateConnectorsView(true);
                OnPropertyChanged(nameof(UseCompactInputs));
            }
        }

        private bool _compactOutputsView;
        public bool UseCompactOutputs
        {
            get => _compactOutputsView;
            set
            {
                if (_compactOutputsView == value) return;

                _compactOutputsView = value;
                this.UpdateConnectorsView(false);
                OnPropertyChanged(nameof(UseCompactOutputs));
            }
        }

        private uint _neuronsCount;
        public uint NeuronsCount
        {
            get => _neuronsCount;
            set
            {
                if (_neuronsCount == value) return;

                var oldValue = _neuronsCount;
                _neuronsCount = value;

                OnPropertyChanged(nameof(NeuronsCount));

                this.UpdateNeuronsCount((int)(_neuronsCount - oldValue));
            }
        }

        private Type _neuronType;
        public new Type NeuronType
        {
            get => _neuronType;
            set
            {
                if (_neuronType == value) return;

                _neuronType = value;
                base.NeuronType = value;

                _neuronsCount = 1;  // each layer has one neuron by defeault
                OnPropertyChanged(nameof(this.NeuronsCount));

                if (_neuronFactory.PropertiesContainerConstructors.ContainsKey(_neuronType))
                {
                    _properties = _neuronFactory.PropertiesContainerConstructors[_neuronType].Invoke();
                    _propertiesProvider?.UpdateNeuronsProperties(this);
                }

                this.UpdateNeuronsCount((int) this.NeuronsCount);
                OnPropertyChanged(nameof(NeuronType));
            }
        }

        public IEnumerable<IConnectionPoint> Inputs => _inputs;
        public IEnumerable<IConnectionPoint> Outputs => _outputs;
        public IPropertiesContainer NeuronProperties => _properties;

        public LayerRole Role
        {
            get;
            protected set;
        }

        private void UpdateNeuronsCount(int delta)
        {
            if (_neuronType == null) return;
            if(!_neuronFactory.Constructors.ContainsKey(_neuronType)) return;

            if (Math.Sign(delta) < 0) // neurons count decreased
            {
                var toRemove = new List<IConnectionPoint>();
                if (!this.UseCompactInputs)
                {
                    toRemove.AddRange(_inputs.Skip((int)this.NeuronsCount).ToList());
                }

                if (!this.UseCompactOutputs)
                {
                    toRemove.AddRange(_outputs.Skip((int)this.NeuronsCount).ToList());
                }

                EventAggregator.Publish(new ConnectorsRemovedEventArgs(toRemove));
            }
            else // neurons count increased
            {
                for (var i = 0; i < delta; i++)
                {
                    if (!this.UseCompactOutputs || _outputs.Count == 0)
                    {
                        _outputs.Add(new Connector(this));
                    }

                    if (!this.UseCompactInputs || _inputs.Count == 0)
                    {
                        _inputs.Add(new Connector(this));
                    }
                }
            }
        }

        private void UpdateConnectorsView(bool forInputs)
        {
            if (forInputs)
            {
                if (UseCompactInputs)
                {
                    var toRemove = _inputs.Skip(1).ToList();
                    EventAggregator.Publish(new ConnectorsRemovedEventArgs(toRemove));
                    foreach (var connectionPoint in toRemove)
                    {
                        _inputs.Remove(connectionPoint);
                    }
                    toRemove.Clear();
                }
                else
                {
                    for (int i = 1; i < NeuronsCount; i++)
                    {
                        _inputs.Add(new Connector(this));
                    }
                }
            }
            else
            {
                if (UseCompactOutputs)
                {
                    var toRemove = _outputs.Skip(1).ToList();
                    EventAggregator.Publish(new ConnectorsRemovedEventArgs(toRemove));
                    foreach (var connectionPoint in toRemove)
                    {
                        _outputs.Remove(connectionPoint);
                    }
                    toRemove.Clear();
                }
                else
                {
                    for (int i = 1; i < NeuronsCount; i++)
                    {
                        _outputs.Add(new Connector(this));
                    }
                }
            }
        }
    }
}

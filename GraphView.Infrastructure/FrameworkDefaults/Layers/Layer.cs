using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using NeuralNetworkLab.Infrastructure.Events;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworkLab.Infrastructure.FrameworkDefaults
{
    public class Layer : INode, INotifyPropertyChanged, IDisposable, IPropertiesContrianer
    {
        private string _name;
        private double _x, _y;
        private bool _isSelected;

        private readonly INeuronFactory _neuronFactory;

        private readonly List<IConnectionPoint> _inputs = new List<IConnectionPoint>();
        private readonly List<IConnectionPoint> _outputs = new List<IConnectionPoint>();
        private readonly List<IPropertiesContrianer> _properties = new List<IPropertiesContrianer>();

        public Layer(INeuronFactory factory)
        {
            _neuronFactory = factory;
            _compactInputsView = true;
            _compactOutputsView = true;
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
        public Type NeuronType
        {
            get => _neuronType;
            set
            {
                if (_neuronType == value) return;

                _neuronType = value;
                _properties.Clear();
                if (_neuronFactory.PropertiesContainerConstructors.ContainsKey(_neuronType))
                {
                    for (int i = 0; i < this.NeuronsCount; i++)
                    {
                        _properties.Add(_neuronFactory.PropertiesContainerConstructors[_neuronType].Invoke());
                    }
                }
                OnPropertyChanged(nameof(NeuronType));
            }
        }

        private void UpdateNeuronsCount(int delta)
        {
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
                    if (!this.UseCompactOutputs)
                    {
                        _outputs.Add(new Connector(this));
                    }

                    if (!this.UseCompactInputs)
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
                    _inputs.RemoveRange(1, toRemove.Count);
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
                    _outputs.RemoveRange(1, toRemove.Count);
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INode Members

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;

                _name = value;
                OnPropertyChanged();
            }
        }

        public virtual double X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged();
            }
        }

        public virtual double Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public virtual void Dispose()
        {
        }

        public virtual bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;

                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<IPropertiesContrianer> NeuronProperties => _properties;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworkLab.Infrastructure.FrameworkDefaults
{
    public class Layer : INode, INotifyPropertyChanged, IDisposable
    {
        #region Private fields

        private string _name;
        private double _x, _y;
        private bool _isSelected;
        protected List<NeuronBase> _neurons;

        private readonly INeuronFactory _neuronFactory;

        protected List<IConnectionPoint> _inputs = new List<IConnectionPoint>();
        protected List<IConnectionPoint> _outputs = new List<IConnectionPoint>();

        #endregion


        public Layer(INeuronFactory factory) :
            this(factory, null)
        {
        }

        public Layer(INeuronFactory factory, IEnumerable<NeuronBase> neurons)
        {
            _neuronFactory = factory;
            _neurons = neurons?.ToList() ?? new List<NeuronBase>();
            _compactInputsView = true;
            _compactOutputsView = true;
            _neuronsCount = (uint)_neurons.Count;
            for (var i = 0; i < _neuronsCount; i++)
            {
                _inputs.Add(new Connector(this));
                _outputs.Add(new Connector(this));
            }
        }

        private bool _compactInputsView;
        public bool UseCompactInputs
        {
            get => _compactInputsView;
            set
            {
                if (_compactInputsView == value) return;

                _compactInputsView = value;
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

                var oldValue = (long)_neuronsCount;
                _neuronsCount = value;

                OnPropertyChanged(nameof(NeuronsCount));

                this.UpdateNeuronsCount(_neuronsCount - oldValue);
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
                OnPropertyChanged(nameof(NeuronType));

                this.UpdateNeuronsType();
            }
        }

        public IEnumerable<IConnectionPoint> Inputs => _inputs;
        public IEnumerable<IConnectionPoint> Outputs => _outputs;

        public IEnumerable<NeuronBase> Neurons => _neurons;

        #region Private Methods

        private void UpdateNeuronsCount(long delta)
        {
            if(!_neuronFactory.Constructors.ContainsKey(_neuronType)) return;

            if (Math.Sign(delta) < 0) // neurons count decreased
            {
                foreach (var neuronBase in _neurons.Skip((int)_neuronsCount))
                {
                    neuronBase.Dispose();
                    // remove inputs, outputs and fibers
                }

                _neurons.RemoveRange((int)_neuronsCount, (int)Math.Abs(delta));
            }
            else // neurons count increased
            {
                for (int i = 0; i < delta; i++)
                {
                    _neurons.Add(_neuronFactory.Constructors[_neuronType].Invoke());
                    // add inputs and outputs
                }
            }
        }

        private void UpdateNeuronsType()
        {
            if (!_neuronFactory.Constructors.ContainsKey(_neuronType)) return;

            for (int i = 0; i < _neuronsCount; i++)
            {
                //var oldNeuron = _neurons[i];
                _neurons.RemoveAt(i);
                _neurons.Insert(i, _neuronFactory.Constructors[_neuronType].Invoke());
                // update connection if any
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

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
            foreach (var neuronBase in _neurons)
            {
                neuronBase.Dispose();
            }
            _neurons.Clear();
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
    }
}

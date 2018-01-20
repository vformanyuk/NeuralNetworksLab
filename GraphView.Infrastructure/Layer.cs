using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworkLab.Infrastructure
{
    public class Layer : INode, INotifyPropertyChanged, IDisposable
    {
        public Layer() :
            this(null)
        {
        }

        public Layer(IEnumerable<NeuronBase> neurons)
        {
            _neurons = neurons?.ToList() ?? new List<NeuronBase>();
            _compactFibersView = true;
            _neuronsCount = (uint)_neurons.Count;
        }

        private bool _compactFibersView;
        public bool UseCompactFibersView
        {
            get => _compactFibersView;
            set
            {
                if (_compactFibersView == value) return;

                _compactFibersView = value;
                OnPropertyChanged(nameof(UseCompactFibersView));
            }
        }

        private uint _neuronsCount;
        public uint NeuronsCount
        {
            get => _neuronsCount;
            set
            {
                if (_neuronsCount == value) return;

                _neuronsCount = value;
                OnPropertyChanged(nameof(NeuronsCount));
            }
        }



        #region Private Methods

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

        #region Private fields

        private string _name;
        private double _x, _y;
        private bool _isSelected;
        protected List<NeuronBase> _neurons;

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

        private bool _displayAllConntectors;
        public bool DisplayAllConnectors
        {
            get => _displayAllConntectors;
            set
            {
                if (_displayAllConntectors == value) return;

                _displayAllConntectors = value;
                OnPropertyChanged(nameof(DisplayAllConnectors));
            }
        }
    }
}

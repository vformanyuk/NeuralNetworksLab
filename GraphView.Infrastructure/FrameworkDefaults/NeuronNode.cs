using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworkLab.Infrastructure.FrameworkDefaults
{
    public class NeuronNode : INetworkNode, INotifyPropertyChanged, IDisposable
    {
        //public NeuronNode(NeuronBase model)
        //{
        //    _model = model;
        //    _subscribtionToken = model.Distinct()
        //                             .ObserveOnDispatcher()
        //                             .Sample(TimeSpan.FromMilliseconds(500))
        //                             .Subscribe(r =>
        //                             {
        //                                 OnPropertyChanged(nameof(this.NeuronPotential));
        //                             });
        //}

        #region Private Methods

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region INode Members

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;

                _name = value;
                OnPropertyChanged();
            }
        }

        public virtual double X
        {
            get { return _x; }
            set
            {
                if (_x == value) return;

                _x = value;
                OnPropertyChanged();
            }
        }

        public virtual double Y
        {
            get { return _y; }
            set
            {
                if (_y == value) return;

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
        #endregion

        public virtual void Dispose()
        {
        }

        public virtual bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;

                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public double NeuronPotential
        {
            get;
            protected set;
        }

        public IPropertiesContrianer Properties
        {
            get;
            protected set;
        }
    }
}

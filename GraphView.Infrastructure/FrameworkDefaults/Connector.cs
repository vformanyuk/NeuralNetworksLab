using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GraphView.Infrastructure.Annotations;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworkLab.Infrastructure.FrameworkDefaults
{
    public class Connector : IConnectionPoint, INotifyPropertyChanged
    {
        #region Constructors

        public Connector(INode host) : this(host, null)
        {
        }

        public Connector(INode host, Func<Connector, bool> canConnect)
        {
            _canConnect = canConnect;
            _host = host;
        }

        #endregion

        public INode Host => _host;

        #region Public Methods

        /// <summary>
        /// Determines whether this instance can connect the specified connector.
        /// </summary>
        /// <param name="connector">The connector.</param>
        /// <returns></returns>
        public virtual bool CanConnect(Connector connector)
        {
            return _canConnect == null || _canConnect.Invoke(connector);
        }

        #endregion

        #region Private Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IConnectionPoint Members

        /// <summary>
        /// Gets or sets a value indicating whether this instance is connected.
        /// </summary>
        public virtual bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected == value) return;
                _isConnected = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Determines whether this instance can connect the specified connection point.
        /// </summary>
        /// <param name="connectionPoint">The connection point.</param>
        /// <returns></returns>
        bool IConnectionPoint.CanConnect(IConnectionPoint connectionPoint)
        {
            return CanConnect(connectionPoint as Connector);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private fields

        private bool _isConnected;
        private readonly Func<Connector, bool> _canConnect;
        private readonly INode _host;

        #endregion

    }
}
﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using GraphView.Framework.Interfaces;
using GraphView.Infrastructure.Annotations;
using GraphView.Infrastructure.Interfaces;

namespace GraphView.Infrastructure.FrameworkDefaults
{
    public class Connector : IConnectionPoint, INotifyPropertyChanged
    {
        public Connector(CircuitElement hostingElement, ConnectorType type)
        {
            Type = type;
            Element = hostingElement;
        }

        public CircuitElement Element { get; private set; }
        public ConnectorType Type { get; private set; }

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

        public virtual bool CanConnect(Connector connector)
        {
            return Type == ConnectorType.Output && connector.Type == ConnectorType.Output;
        }

        bool IConnectionPoint.CanConnect(IConnectionPoint connectionPoint)
        {
            return CanConnect(connectionPoint as Connector);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _isConnected;
    }
}
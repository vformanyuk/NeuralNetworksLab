using System;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworksLab.App.Events
{
    public class ConnectionEventArgs : EventArgs
    {
        public IConnection Connection
        {
            get;
            private set;
        }

        public ConnectionEventArgs(IConnection connection) => this.Connection = connection;
    }
}

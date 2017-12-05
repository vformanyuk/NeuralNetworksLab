using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphView.Framework.Routers;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworkLab.Interfaces;
using NeuralNetworksLab.App.Events;

namespace NeuralNetworksLab.App.Services
{
    public class ConnectionsFactory : IConnectionsFactory
    {
        private readonly ISettingsProvider _settings;

        public event EventHandler<ConnectionEventArgs> ConnectionAdded;
        public event EventHandler<ConnectionEventArgs> ConnectionRemoved;

        public ConnectionsFactory(ISettingsProvider setting)
        {
            _settings = setting;
        }

        public IConnection CreateConnection(IConnectionPoint sourcePoint, IConnectionPoint destinationPoint)
        {
            if (!sourcePoint.CanConnect(destinationPoint))
            {
                return null;
            }

            var connection = new NeuroFiberConnection(sourcePoint, destinationPoint, new DirectLineRouter());

            this.ConnectionAdded?.Invoke(this, new ConnectionEventArgs(connection));

            return connection;
        }

        void IConnectionsFactory.ConnectionRemoved(IConnection connection)
        {
            this.ConnectionRemoved?.Invoke(this, new ConnectionEventArgs(connection));
        }
    }
}

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

namespace NeuralNetworksLab.App.Services
{
    public class ConnectionsFactory : IConnectionsFactory
    {
        private readonly ISettingsProvider _settings;

        public ConnectionsFactory(ISettingsProvider setting)
        {
            _settings = setting;
        }

        public IConnection CreateConnection(IConnectionPoint sourcePoint, IConnectionPoint destinationPoint)
        {
            return new NeuroFiberConnection(new NeuroFiber(null, null, _settings), sourcePoint, destinationPoint, new DirectLineRouter());
        }
    }
}

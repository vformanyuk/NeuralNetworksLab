using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface INetworkNode : INode
    {
        IPropertiesContrianer Properties { get; }
    }
}

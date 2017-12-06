using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface IPropertiesProvider
    {
        //IReadOnlyDictionary<string, INeuronProperty> Properties { get; }
        void Load(NeuronBase model);
    }
}

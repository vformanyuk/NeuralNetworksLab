using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace Perseptron
{
    public class PerseptronProperties : IPropertiesProvider
    {
        private readonly ISettingsProvider _settingsProvider;

        public PerseptronProperties(ISettingsProvider settings)
        {
        }

        public void Load(NeuronBase model)
        {
            throw new NotImplementedException();
        }
    }
}

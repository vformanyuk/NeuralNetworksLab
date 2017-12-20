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
        protected readonly ISettingsProvider _settingsProvider;

        public PerseptronProperties(ISettingsProvider settings)
        {
            _settingsProvider = settings;
        }

        public void Load(NeuronBase model)
        {
            throw new NotImplementedException();
        }
    }
}

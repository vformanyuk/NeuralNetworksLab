using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworkLab.Infrastructure.Common.Properties;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.SensorLayers
{
    public class CsvSensorLayerProperties : IPropertiesProvider
    {
        private const string DelimiterKey = "p_cl_Delimiter";
        private const string FileKey = "p_cl_File";
        private const string SkipHeaderKey = "p_cl_SkipHeader";

        public event EventHandler Loaded;
        public IReadOnlyDictionary<string, IGenericProperty> Properties => _csvLayerProperties;

        private readonly Dictionary<string, IGenericProperty> _csvLayerProperties = new Dictionary<string, IGenericProperty>();

        public void Load(NeuronBase model)
        {
        }

        public void Load(Layer layer)
        {
            if (!(layer is CsvSensorLayer csvLayer))
            {
                return;
            }

            _csvLayerProperties.Clear();
            _csvLayerProperties.Add(DelimiterKey, new CharProperty("Delimiter", v => csvLayer.Delimiter = v, csvLayer.Delimiter));
            _csvLayerProperties.Add(FileKey, new FileSelectProperty("CSV file", v => csvLayer.FileName = v, csvLayer.FileName));
            _csvLayerProperties.Add(SkipHeaderKey, new BooleanProperty("Skip Header", v => csvLayer.SkipHeaderRow = v, csvLayer.SkipHeaderRow));

            this.Loaded?.Invoke(this, EventArgs.Empty);
        }

        public void Load(IEnumerable<NeuronBase> model)
        {
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }
    }
}

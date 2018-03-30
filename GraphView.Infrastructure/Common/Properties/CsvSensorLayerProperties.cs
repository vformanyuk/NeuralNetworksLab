using System.Collections.Generic;
using System.Linq;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class CsvSensorLayerProperties : LayerProperties
    {
        public const string DelimiterKey = "p_cl_Delimiter";
        public const string FileKey = "p_cl_File";
        public const string SkipHeaderKey = "p_cl_SkipHeader";

        public CsvSensorLayerProperties(INeuronFactory factory) : base(factory)
        {
        }

        public override void AddCustomProperties(Layer layer, ICollection<IGenericProperty> properties)
        {
            if (!(layer is CsvSensorLayer csvLayer))
            {
                return;
            }

            // CSV layer is CSV layer.
            var neuronTypeProperty = properties.FirstOrDefault(p => p.PropertyName == NeuronTypeNameKey);
            if (neuronTypeProperty != null)
            {
                properties.Remove(neuronTypeProperty);
            }

            var compactInputsProperty = properties.FirstOrDefault(p => p.PropertyName == CompactInputFibersKey);
            if (compactInputsProperty != null)
            {
                properties.Remove(compactInputsProperty);
            }

            properties.Add(new CharProperty(DelimiterKey, () => csvLayer.Delimiter ?? default(char), v => csvLayer.Delimiter = v));
            properties.Add(new FileSelectProperty(FileKey, () => csvLayer.FileName, v => csvLayer.FileName = v));
            properties.Add(new BooleanProperty(SkipHeaderKey, () => csvLayer.SkipHeaderRow, v => csvLayer.SkipHeaderRow = v));
        }
    }
}

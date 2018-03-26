using System.Collections.Generic;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class CsvSensorLayerProperties : LayerProperties
    {
        private const string DelimiterKey = "p_cl_Delimiter";
        private const string FileKey = "p_cl_File";
        private const string SkipHeaderKey = "p_cl_SkipHeader";

        public CsvSensorLayerProperties(INeuronFactory factory) : base(factory)
        {
        }

        public override void AddCustomProperties(Layer layer, ICollection<IGenericProperty> properties)
        {
            if (!(layer is CsvSensorLayer csvLayer))
            {
                return;
            }

            properties.Add(new CharProperty(DelimiterKey, v => csvLayer.Delimiter = v, csvLayer.Delimiter));
            properties.Add(new FileSelectProperty(FileKey, v => csvLayer.FileName = v, csvLayer.FileName));
            properties.Add(new BooleanProperty(SkipHeaderKey, v => csvLayer.SkipHeaderRow = v, csvLayer.SkipHeaderRow));
        }
    }
}

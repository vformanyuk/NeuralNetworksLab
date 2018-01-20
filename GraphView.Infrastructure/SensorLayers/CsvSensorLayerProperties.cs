using NeuralNetworkLab.Infrastructure.Common.Properties;

namespace NeuralNetworkLab.Infrastructure.SensorLayers
{
    public class CsvSensorLayerProperties : LayerProperties
    {
        private const string DelimiterKey = "p_cl_Delimiter";
        private const string FileKey = "p_cl_File";
        private const string SkipHeaderKey = "p_cl_SkipHeader";

        public override void AddCustomProperties(Layer layer)
        {
            if (!(layer is CsvSensorLayer csvLayer))
            {
                return;
            }

            _csvLayerProperties.Add(DelimiterKey, new CharProperty("Delimiter", v => csvLayer.Delimiter = v, csvLayer.Delimiter));
            _csvLayerProperties.Add(FileKey, new FileSelectProperty("CSV file", v => csvLayer.FileName = v, csvLayer.FileName));
            _csvLayerProperties.Add(SkipHeaderKey, new BooleanProperty("Skip Header", v => csvLayer.SkipHeaderRow = v, csvLayer.SkipHeaderRow));
        }
    }
}

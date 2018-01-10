using System;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class FileSelectProperty : NeuralNetworkProperty<string>
    {
        public FileSelectProperty(string name, Action<string> propertySetter, string currentValue) : base(name, propertySetter)
        {
            this.Value = currentValue;
        }
    }
}

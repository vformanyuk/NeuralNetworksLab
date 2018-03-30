using System;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class FileSelectProperty : NeuralNetworkProperty<string>
    {
        public FileSelectProperty(string name, Func<string> propertyGetter, Action<string> propertySetter) 
            : base(name, propertyGetter, propertySetter)
        {
        }
    }
}

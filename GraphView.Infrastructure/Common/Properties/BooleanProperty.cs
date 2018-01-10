using System;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class BooleanProperty : NeuralNetworkProperty<bool>
    {
        public BooleanProperty(string name, Action<bool> propertySetter, bool defaultValue) : base(name, propertySetter)
        {
            this.Value = defaultValue;
        }
    }
}

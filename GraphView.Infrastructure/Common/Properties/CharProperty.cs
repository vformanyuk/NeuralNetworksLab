using System;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class CharProperty : NeuralNetworkProperty<char>
    {
        public CharProperty(string name, Action<char> propertySetter, char? defaultValue = null) 
            : base(name, propertySetter)
        {
            if (defaultValue.HasValue)
            {
                this.Value = defaultValue.Value;
            }
        }
    }
}

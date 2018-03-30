using System;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class CharProperty : NeuralNetworkProperty<char>
    {
        public CharProperty(string name, Func<char> propertyGetter, Action<char> propertySetter) 
            : base(name, propertyGetter, propertySetter)
        {
        }
    }
}

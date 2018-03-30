using System;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class BooleanProperty : NeuralNetworkProperty<bool>
    {
        public BooleanProperty(string name, Func<bool> propertyGetter, Action<bool> propertySetter) 
            : base(name, propertyGetter, propertySetter)
        {
        }
    }
}

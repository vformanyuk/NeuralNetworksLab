using System;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class DoubleProperty : NeuralNetworkProperty<double>
    {
        public DoubleProperty(string name, Func<double> getter, Action<double> setter)
            : base(name, propertyGetter: getter, propertySetter: setter)
        {
        }
    }
}

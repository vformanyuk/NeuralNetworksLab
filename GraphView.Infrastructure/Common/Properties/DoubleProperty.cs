using System;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class DoubleProperty : NeuralNetworkProperty<double>
    {
        public DoubleProperty(string name, Action<double> setter, double? defaultValue)
            : base(name, initialValue: defaultValue ?? default(double), propertySetter: setter)
        {
        }
    }
}

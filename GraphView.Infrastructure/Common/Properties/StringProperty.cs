using System;
using System.Collections.Generic;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class StringProperty : NeuralNetworkProperty<string>
    {
        public StringProperty(string name, string initialValue, Action<string> setter,
            IEnumerable<string> defaultValueSet = null) : base(name,
            initialValue: initialValue, propertySetter: setter, defaultValues: defaultValueSet)
        {

        }
    }
}

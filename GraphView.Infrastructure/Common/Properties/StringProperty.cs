using System;
using System.Collections.Generic;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class StringProperty : NeuralNetworkProperty<string>
    {
        public StringProperty(string name, Func<string> getter, Action<string> setter,
            IEnumerable<string> defaultValueSet = null) : base(name,
            propertyGetter: getter, propertySetter: setter, defaultValues: defaultValueSet)
        {

        }
    }
}

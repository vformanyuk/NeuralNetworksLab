using System;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class UintProperty : NeuralNetworkProperty<uint>
    {
        public UintProperty(string name, Func<uint> getter, Action<uint> setter) 
            : base(name, getter, setter)
        {
        }
    }
}

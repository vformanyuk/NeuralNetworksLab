using System;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class UintProperty : NeuralNetworkProperty<uint>
    {
        public UintProperty(string name, Action<uint> setter, uint? defaultValue) : base(name, setter)
        {
            if (defaultValue.HasValue)
            {
                this.Value = defaultValue.Value;
            }
        }
    }
}

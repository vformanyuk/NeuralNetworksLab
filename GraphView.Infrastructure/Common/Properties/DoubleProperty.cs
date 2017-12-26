using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLab.Infrastructure.Common.Properites
{
    public class DoubleProperty : NeuralNetworkProperty<double>
    {
        public DoubleProperty(string name, Action<double> setter, double? defaultValue) : base(name, setter)
        {
            if (defaultValue.HasValue)
            {
                this.Value = defaultValue.Value;
            }
        }
    }
}

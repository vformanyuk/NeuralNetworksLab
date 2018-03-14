using System;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Functors
{
    public class ReLu : IFunctor
    {
        private static readonly Func<double, double> _relu = new Func<double, double>(x => x >= 0 ? x : 0);

        public double Invoke(params double[] arguments)
        {
            return _relu(arguments[0]);
        }

        public override int GetHashCode()
        {
            return _relu.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }

            if(obj is IFunctor functor)
            {
                if (functor is ReLu)
                {
                    return true;
                }

                return false;
            }

            return base.Equals(obj);
        }
    }
}

using System;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Functors
{
    public class ReLuDerivative : IFunctor
    {
        private static readonly Func<double, double> _reluDx = new Func<double, double>(x => x >= 0 ? 1 : 0);

        public double Invoke(params double[] arguments)
        {
            return _reluDx(arguments[0]);
        }

        public override int GetHashCode()
        {
            return _reluDx.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is IFunctor functor)
            {
                if (functor is ReLuDerivative)
                {
                    return true;
                }

                return false;
            }

            return base.Equals(obj);
        }
    }
}

using System;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Functors
{
    public class Sigmoid : IFunctor
    {
        private static Func<double, double> _sigmoid = new Func<double, double>(x => 1d / (1 + Math.Exp(-x)));

        public double Invoke(params double[] arguments)
        {
            return _sigmoid(arguments[0]);
        }

        public override int GetHashCode()
        {
            return _sigmoid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is IFunctor functor)
            {
                if (functor is Sigmoid)
                {
                    return true;
                }

                return false;
            }

            return base.Equals(obj);
        }
    }
}

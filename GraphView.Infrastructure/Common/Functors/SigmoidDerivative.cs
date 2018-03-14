using System;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Functors
{
    public class SigmoidDerivative : IFunctor
    {
        private static readonly Func<double, double> _sigmoidDx;

        static SigmoidDerivative()
        {
            var sigmoid = new Sigmoid();
            _sigmoidDx = new Func<double, double>(x =>
            {
                var sigm = new Sigmoid().Invoke(x);
                return sigm * (1 - sigm);
            });
        }

        public double Invoke(params double[] arguments)
        {
            return _sigmoidDx(arguments[0]);
        }

        public override int GetHashCode()
        {
            return _sigmoidDx.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is IFunctor functor)
            {
                if (functor is SigmoidDerivative)
                {
                    return true;
                }

                return false;
            }

            return base.Equals(obj);
        }
    }
}

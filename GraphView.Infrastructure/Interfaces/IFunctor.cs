using System.Collections.Generic;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface IFunctor
    {
        double Invoke(params double[] arguments);
    }

    public interface IFunctor<U,V> : IFunctor
    {
        U Invoke(params V[] arguments);

        U Invoke(IEnumerable<V> arguments);
    }
}

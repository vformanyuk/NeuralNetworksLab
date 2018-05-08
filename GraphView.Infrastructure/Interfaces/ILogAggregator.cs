using System;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface ILogAggregator : IObserver<string>
    {
        event EventHandler<string[]> LogBatchAvailable;
        void Publish(string message);
    }
}

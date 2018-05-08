using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworksLab.App.Services
{
    public class LogAggregator : ILogAggregator
    {
        private readonly Subject<string> _aggregator = new Subject<string>();

        public LogAggregator()
        {
            _aggregator.Buffer(TimeSpan.FromSeconds(2))
                .SubscribeOn(Scheduler.Default)
                .Subscribe(messages =>
                {
                    if (messages.Count == 0) return;
                    this.LogBatchAvailable?.Invoke(null, messages.ToArray());
                });
        }

        void IObserver<string>.OnNext(string value)
        {
            this.Publish(value);
        }

        void IObserver<string>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        void IObserver<string>.OnCompleted()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<string[]> LogBatchAvailable;

        public void Publish(string message)
        {
            _aggregator.OnNext(message);
        }
    }
}

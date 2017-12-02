using System;

namespace NeuralNetworkLab.Infrastructure
{
    public class SubscribtionToken : IDisposable
    {
        public IObserver<double> Observer { get; }

        private readonly Action<IObserver<double>> _clearAction;

        public SubscribtionToken(IObserver<double> observer, Action<IObserver<double>> clearAction)
        {
            this.Observer = observer;
            this._clearAction = clearAction;
        }

        public void Dispose() => _clearAction.Invoke(Observer);
    }
}

using System;

namespace NeuralNetworkLab.Infrastructure
{
    public class SubscribtionToken<T> : IDisposable
    {
        public T Observer { get; }

        private readonly Action<T> _clearAction;

        public SubscribtionToken(T observer, Action<T> clearAction)
        {
            this.Observer = observer;
            this._clearAction = clearAction;
        }

        public void Dispose() => _clearAction.Invoke(Observer);
    }
}

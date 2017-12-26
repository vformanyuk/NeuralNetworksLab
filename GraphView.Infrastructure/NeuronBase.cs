using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using GraphView.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure
{
    public class NeuronBase : ISubject<double>, ISupportLearning
    {
        // goes from source neuron to target one
        protected readonly List<IObserver<double>> _axons = new List<IObserver<double>>();

        // dendrits that will adjust thier weights with learning error
        protected readonly List<ISupportLearning> _learningDendrits = new List<ISupportLearning>();

        public double Output { get; protected set; }

        public int DendritsCount => _learningDendrits.Count;

        public int AxonsCount => _axons.Count;

        public IDisposable Subscribe(IObserver<double> observer)
        {
            _axons.Add(observer);
            return new SubscribtionToken<IObserver<double>>(observer, o => _axons.Remove(o));
        }

        public IDisposable SubscribeLearner(ISupportLearning learner)
        {
            _learningDendrits.Add(learner);
            return new SubscribtionToken<ISupportLearning>(learner, o => _learningDendrits.Remove(o));
        }

        protected void PropogateError(double error)
        {
            foreach (var learner in _learningDendrits)
            {
                learner.Learn(error);
            }
        }

        public virtual void Learn(double error)
        {
        }

        public virtual void OnNext(double value)
        {
        }

        public virtual void OnError(Exception error)
        {
        }

        public virtual void OnCompleted()
        {
        }
    }
}

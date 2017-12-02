using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using GraphView.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure
{
    public class NeuronBase : ISubject<double>, ISupportLearning
    {
        // goes from source neuron to target one
        protected readonly HashSet<IObserver<double>> _axons = new HashSet<IObserver<double>>();

        // dendrits that will adjust thier weights with learning error
        protected readonly HashSet<ISupportLearning> _learningDendrits = new HashSet<ISupportLearning>();

        public double Output { get; protected set; }

        /// <summary>
        /// Bias value to use with signal accumulation
        /// </summary>
        public double Bias { get; set; }

        public int DendritsCount
        {
            get
            {
                return _learningDendrits.Count;
            }
        }

        public int AxonsCount
        {
            get
            {
                return this._axons.Count;
            }
        }

        public IDisposable Subscribe(IObserver<double> observer)
        {
            _axons.Add(observer);
            return new SubscribtionToken(observer, o => _axons.Remove(o));
        }

        public void SubscribeLearner(ISupportLearning learner)
        {
            _learningDendrits.Add(learner);
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

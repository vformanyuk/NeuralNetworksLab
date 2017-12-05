using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using GraphView.Infrastructure.Interfaces;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure
{
    public class NeuroFiber : ISubject<double>, ISupportLearning, IDisposable
    {
        private NeuronBase _source;
        private List<IObserver<double>> _targets = new List<IObserver<double>>();

        private IDisposable _sourceSubscribtionToken, _learnSubscribtionToken;

        public double Weight { get; set; }

        private readonly ISettingsProvider _settingsProvider;

        public NeuroFiber(NeuronBase source, NeuronBase target, ISettingsProvider settingsProvider)
        {
            _source = source;
            _sourceSubscribtionToken = _source.Subscribe(this);

            _learnSubscribtionToken = target.SubscribeLearner(this);
            _targets.Add(target);

            _settingsProvider = settingsProvider;
        }

        /// <summary>
        /// Error back propogation
        /// </summary>
        /// <param name="error"></param>
        public void Learn(double error)
        {
            // bias axons do not learn
            //if (_isBiasAxon) return;

            // learn: weight = weight - source_output*weights_delta*learning_rate;

            Weight = Weight - _source.Output * error * _settingsProvider.LearningRate;
            _source.Learn(Weight * error);
        }

        /// <summary>
        /// Feed forward values
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(double value)
        {
            var product = value * Weight;
            foreach (var target in _targets)
            {
                target.OnNext(product);
            }
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }

        IDisposable IObservable<double>.Subscribe(IObserver<double> observer)
        {
            _targets.Add(observer);
            return new SubscribtionToken<IObserver<double>>(observer, o => _targets.Remove(o));
        }

        public override string ToString()
        {
            return this.Weight.ToString();
        }

        public void Dispose()
        {
            _sourceSubscribtionToken.Dispose();
            _learnSubscribtionToken.Dispose();
        }
    }
}

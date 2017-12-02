using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using GraphView.Infrastructure.Interfaces;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure
{
    public class NeuroFiber : ISubject<double>, ISupportLearning
    {
        private NeuronBase _source;
        private HashSet<IObserver<double>> _targets = new HashSet<IObserver<double>>();

        public double Weight { get; set; }

        private readonly ISettingsProvider _settings;

        public NeuroFiber(NeuronBase source, NeuronBase target, ISettingsProvider settings)
        {
            _settings = settings;

            _source = source;
            _source.Subscribe(this);

            target.SubscribeLearner(this);
            _targets.Add(target);

            //this.Weight = Settings.NextWeight;
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

            Weight = Weight - _source.Output * error;// * Settings.LearningRate;
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
            return new SubscribtionToken(observer, o => _targets.Remove(o));
        }

        public override string ToString()
        {
            return this.Weight.ToString();
        }
    }
}

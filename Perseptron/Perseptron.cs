using System;
using NeuralNetworkLab.Infrastructure;

namespace Perseptron
{
    public class Perseptron : NeuronBase
    {
        private double _internalAccumulator = 0;
        private double _internalErrorAccumulator = 0;
        private uint _neuroImpulses = 0;
        private uint _backNeuroimpulses = 0;

        public Func<double, double> ActivationFunction { get; set; }
        public Func<double, double> ActivationFunctionDx { get; set; }

        public override void Learn(double error)
        {
            _internalErrorAccumulator += error * ActivationFunctionDx.Invoke(this.Output);
            _backNeuroimpulses++;
            if (_backNeuroimpulses >= this.AxonsCount)
            {
                PropogateError(_internalErrorAccumulator);
                _internalErrorAccumulator = 0;
                _backNeuroimpulses = 0;
            }
        }

        public override void OnNext(double value)
        {
            _internalAccumulator += value;
            _neuroImpulses++;
            if (_neuroImpulses >= this.DendritsCount)
            {
                Output = ActivationFunction(_internalAccumulator + this.Bias);

                _internalAccumulator = 0;
                _neuroImpulses = 0;

                if (this._axons.Count == 0) return;

                foreach (var axon in _axons)
                {
                    axon.OnNext(Output);
                }
            }
        }
    }
}

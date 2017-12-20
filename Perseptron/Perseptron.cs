using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Common;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace Perseptron
{
    public class Perseptron : NeuronBase
    {
        private double _internalAccumulator = 0;
        private double _internalErrorAccumulator = 0;
        private uint _neuroImpulses = 0;
        private uint _backNeuroimpulses = 0;

        private readonly ActivationFunctionProperty _activationFunction;

        public Perseptron(ISettingsProvider settings)
        {
            _activationFunction = (ActivationFunctionProperty)settings[this.GetType()][Plugin.PerseptronActivationFunctionSettingsKey];
        }

        public override void Learn(double error)
        {
            _internalErrorAccumulator += error * _activationFunction.Derivative.Invoke(this.Output);
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
                Output = _activationFunction.Value.Invoke(_internalAccumulator + this.Bias);

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

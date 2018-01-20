using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Common.Settings;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace Perseptron
{
    public class Perseptron : NeuronBase
    {
        private double _internalAccumulator = 0;
        private double _internalErrorAccumulator = 0;
        private uint _neuroImpulses = 0;
        private uint _backNeuroimpulses = 0;

        public Perseptron(ISettingsProvider settings)
        {
            var settingsActivationFunction = (ActivationFunctionSettingsItem)settings[this.GetType()][Plugin.PerseptronActivationFunctionSettingsKey];
            settingsActivationFunction.Changed += (o, e) =>
             {
                 var af = (ActivationFunctionSettingsItem)o;
                 this.ActivationFunction = af.Value;
                 this.ActivationFunctionDerivative = af.Derivative;
             };
        }

        public IFunctor ActivationFunction
        {
            get;
            internal set;
        }

        public IFunctor ActivationFunctionDerivative
        {
            get;
            internal set;
        }

        public double Bias
        {
            get;
            internal set;
        }

        public override void Learn(double error)
        {
            _internalErrorAccumulator += error * this.ActivationFunctionDerivative.Invoke(this.Output);
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
                Output = this.ActivationFunction.Invoke(_internalAccumulator + this.Bias);

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

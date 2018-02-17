using System;
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

        public Perseptron(ISettingsProvider settings, PerseptronPropertiesContainer properties)
        {
            var settingsActivationFunction = (ActivationFunctionSettingsItem)settings[this.GetType()][Plugin.PerseptronActivationFunctionSettingsKey];
            settingsActivationFunction.Changed += (o, e) =>
             {
                 var af = (ActivationFunctionSettingsItem)o;
                 this.ActivationFunction = af.Value;
                 this.ActivationFunctionDerivative = af.Derivative;
             };
            var bias = (BiasSettingsItem) settings[this.GetType()][Plugin.PerseptronBiasSettingsKey];
            bias.Changed += (o, e) => this.Bias = (o as BiasSettingsItem).Value;

            if (properties != null && !properties.ActivationFunction.Equals(settingsActivationFunction.Value))
            {
                this.ActivationFunction = properties.ActivationFunction;
                this.ActivationFunctionDerivative = properties.ActivationFunctionDerivative;
            }
            else
            {
                this.ActivationFunction = settingsActivationFunction.Value;
                this.ActivationFunctionDerivative = settingsActivationFunction.Derivative;
            }

            if (properties != null && Math.Abs(bias.Value - properties.Bias) > 0.0001)
            {
                this.Bias = properties.Bias;
            }
            else
            {
                this.Bias = bias.Value;
            }
        }

        public IFunctor ActivationFunction
        {
            get;
            private set;
        }

        public IFunctor ActivationFunctionDerivative
        {
            get;
            private set;
        }

        public double Bias
        {
            get;
            private set;
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

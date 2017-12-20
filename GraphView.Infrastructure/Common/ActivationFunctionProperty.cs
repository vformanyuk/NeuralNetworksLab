using System;
using System.Collections.Generic;
using NeuralNetworkLab.Infrastructure.Common.Functors;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common
{
    public class ActivationFunctionProperty : NeuralNetworkProperty<IFunctor>
    {
        private static IFunctor _sigmoid = new Sigmoid();
        private static IFunctor _sigmoidDx = new SigmoidDerivative();

        private static IFunctor _relu = new ReLu();
        private static IFunctor _reluDx = new ReLuDerivative();

        private readonly Dictionary<IFunctor, IFunctor> _derivatives;

        public ActivationFunctionProperty(string name = "activation function", IFunctor functionByDefault = null)
            : base(name, null)
        {
            this.ValuesCollection = new Dictionary<string, IFunctor>()
            {
                {"Sigmoid", _sigmoid },
                {"ReLu", _relu }
            };

            _derivatives = new Dictionary<IFunctor, IFunctor>()
            {
                {_sigmoid, _sigmoidDx },
                {_relu, _reluDx }
            };

            this.PropertySetter = ActivationFunctionSet;

            if (functionByDefault != null)
            {
                ActivationFunctionSet(functionByDefault);
            }
        }

        private void ActivationFunctionSet(IFunctor function)
        {
            this.Value = function;

            if (_derivatives.TryGetValue(function, out IFunctor derivative))
            {
                this.Derivative = derivative;
            }
            else
            {
                this.Derivative = null;
            }
        }

        public IFunctor Derivative
        {
            get;
            private set;
        }
    }
}

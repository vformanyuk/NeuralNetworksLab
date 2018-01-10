using System;
using System.Collections.Generic;
using NeuralNetworkLab.Infrastructure.Common.Functors;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class ActivationFunctionProperty : NeuralNetworkProperty<IFunctor>
    {
        private static IFunctor _sigmoid = new Sigmoid();
        private static IFunctor _sigmoidDx = new SigmoidDerivative();

        private static IFunctor _relu = new ReLu();
        private static IFunctor _reluDx = new ReLuDerivative();

        public static readonly Dictionary<IFunctor, IFunctor> Derivatives;

        static ActivationFunctionProperty()
        {
            Derivatives = new Dictionary<IFunctor, IFunctor>()
            {
                {_sigmoid, _sigmoidDx },
                {_relu, _reluDx }
            };
        }

        public ActivationFunctionProperty(string name, Action<IFunctor> setter, IFunctor functionByDefault = null)
            : base(name, setter)
        {
            this.ValuesCollection = new Dictionary<string, IFunctor>()
            {
                {"Sigmoid", _sigmoid },
                {"ReLu", _relu }
            };

            if (functionByDefault != null)
            {
                this.Value = functionByDefault;
            }
        }

        public IFunctor Derivative
        {
            get;
            private set;
        }
    }
}

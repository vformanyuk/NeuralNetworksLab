using System;
using System.Collections.Generic;
using NeuralNetworkLab.Infrastructure.Common.Functors;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class ActivationFunctionProperty : NeuralNetworkProperty<IFunctor>
    {
        private static readonly IFunctor _sigmoid = new Sigmoid();
        private static readonly IFunctor _sigmoidDx = new SigmoidDerivative();

        private static readonly IFunctor _relu = new ReLu();
        private static readonly IFunctor _reluDx = new ReLuDerivative();

        public static readonly Dictionary<IFunctor, IFunctor> Derivatives;

        static ActivationFunctionProperty()
        {
            Derivatives = new Dictionary<IFunctor, IFunctor>()
            {
                {_sigmoid, _sigmoidDx },
                {_relu, _reluDx }
            };
        }

        public ActivationFunctionProperty(string name, Func<IFunctor> getter, Action<IFunctor> setter)
            : base(name, propertyGetter: getter, propertySetter: setter,
                defaultValues: new[] {_sigmoid, _relu})
        {
        }

        public IFunctor Derivative
        {
            get;
            private set;
        }
    }
}

using System.Collections.Generic;
using NeuralNetworkLab.Infrastructure.Common.Functors;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Settings
{
    public class ActivationFunctionSettingsItem : SettingsItem<IFunctor>
    {
        private static IFunctor _sigmoid = new Sigmoid();
        private static IFunctor _sigmoidDx = new SigmoidDerivative();

        private static IFunctor _relu = new ReLu();
        private static IFunctor _reluDx = new ReLuDerivative();

        private static readonly Dictionary<IFunctor, IFunctor> _derivatives;

        static ActivationFunctionSettingsItem()
        {
            _derivatives = new Dictionary<IFunctor, IFunctor>
            {
                {_sigmoid, _sigmoidDx },
                {_relu, _reluDx }
            };
        }

        public ActivationFunctionSettingsItem(string name, IFunctor defaultValue = null)
            : base(name)
        {
            this.ValuesCollection = new Dictionary<string, IFunctor>()
            {
                {"Sigmoid", _sigmoid },
                {"ReLu", _relu }
            };

            if (defaultValue != null)
            {
                _activationFunction = defaultValue;
                this.Derivative = _derivatives[_activationFunction];
            }
        }

        public IFunctor Derivative
        {
            get;
            private set;
        }

        private IFunctor _activationFunction;
        public override IFunctor Value
        {
            get => _activationFunction;
            protected set
            {
                if (_activationFunction.Equals(value)) return;
                _activationFunction = value;
                this.Derivative = _derivatives[value];
                this.RaiseChanged();
            }
        }

        public override bool IsReadonly => false;
    }
}

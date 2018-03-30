using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Windows;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public abstract class NeuralNetworkProperty<T> : IGenericProperty, INotifyPropertyChanged
    {
        private readonly Func<T> _getterFunc;

        protected NeuralNetworkProperty(string name, Func<T> propertyGetter, Action<T> propertySetter, UIElement customEditor = null, IEnumerable<T> defaultValues = null)
        {
            PropertyName = name;
            PropertySetter = propertySetter;
            CustomEditor = customEditor;
            if (defaultValues != null)
            {
                ValuesCollection = defaultValues.ToImmutableList();
            }

            _getterFunc = propertyGetter;
            _value = propertyGetter.Invoke();
        }

        public Action<T> PropertySetter
        {
            get;
        }

        public IEnumerable<T> ValuesCollection { get; protected set; }

        public string PropertyName { get; }

        private bool _isReadonly;
        public virtual bool IsReadonly
        {
            get => _isReadonly;
            set
            {
                if (_isReadonly == value) return;
                _isReadonly = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsReadonly)));
            }
        }

        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                this.PropertySetter(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Value)));
            }
        }

        object IGenericProperty.Value => this.Value;

        public UIElement CustomEditor { get; protected set; }

        public void UpdateProperty()
        {
            _value = _getterFunc.Invoke();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Value)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

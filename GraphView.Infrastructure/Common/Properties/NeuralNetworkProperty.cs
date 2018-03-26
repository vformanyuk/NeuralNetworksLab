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
        protected NeuralNetworkProperty(string name, T initialValue, Action<T> propertySetter, UIElement customEditor = null, IEnumerable<T> defaultValues = null)
        {
            PropertyName = name;
            PropertySetter = propertySetter;
            CustomEditor = customEditor;
            if (defaultValues != null)
            {
                ValuesCollection = defaultValues.ToImmutableList();
            }

            _value = initialValue;
        }

        public Action<T> PropertySetter
        {
            get;
        }

        public IImmutableList<T> ValuesCollection { get; protected set; }

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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

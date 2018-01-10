using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public abstract class NeuralNetworkProperty<T> : IGenericProperty, INotifyPropertyChanged
    {
        protected NeuralNetworkProperty(string name, Action<T> propertySetter, UIElement customEditor = null, IReadOnlyDictionary<string, T> defaultValues = null)
        {
            PropertyName = name;
            PropertySetter = propertySetter;
            CustomEditor = customEditor;
            ValuesCollection = defaultValues;
        }

        public Action<T> PropertySetter
        {
            get;
            protected set;
        }

        public IReadOnlyDictionary<string, T> ValuesCollection { get; protected set; }

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

        public T Value { get; protected set; }

        object IGenericProperty.Value => this.Value;

        public UIElement CustomEditor { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

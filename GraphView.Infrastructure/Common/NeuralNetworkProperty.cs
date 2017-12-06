using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common
{
    public abstract class NeuralNetworkProperty<T> : IGenericProperty
    {
        protected NeuralNetworkProperty(string name, Action<T> propertySetter, UIElement customEditor = null, IReadOnlyDictionary<string, T> defaultValues = null)
        {
            DisplayName = name;
            PropertySetter = propertySetter;
            CustomEditor = customEditor;
            ValuesCollection = defaultValues;
        }

        public Action<T> PropertySetter
        {
            get;
            private set;
        }

        public IReadOnlyDictionary<string, T> ValuesCollection { get; }

        public string DisplayName { get; }

        public bool IsReadonly
        {
            get;
            set;
        }

        public T Value { get; }

        object IGenericProperty.Value
        {
            get
            {
                return this.Value;
            }
        }

        public UIElement CustomEditor { get; }
    }
}

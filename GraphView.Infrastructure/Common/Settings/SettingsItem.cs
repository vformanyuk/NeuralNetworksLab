using System;
using System.Collections.Generic;
using System.Windows;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Settings
{
    public abstract class SettingsItem<T> : ISettingsItem
    {
        protected SettingsItem(string name)
        {
            this.Name = name;
        }

        public IReadOnlyDictionary<string, T> ValuesCollection { get; protected set; }

        protected T _value;
        public virtual T Value
        {
            get => _value;
            protected set
            {
                _value = value;
                RaiseChanged();
            }
        }

        public UIElement CustomEditor { get; protected set; }

        public abstract bool IsReadonly { get; }

        public string Name { get; }

        object ISettingsItem.Value => this.Value;

        public event EventHandler Changed;
        protected void RaiseChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}

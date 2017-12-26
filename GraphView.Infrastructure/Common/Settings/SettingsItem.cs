using System;
using System.Collections.Generic;
using System.Windows;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Common.Settings
{
    public abstract class SettingsItem<T> : ISettingsItem
    {
        protected SettingsItem(string name, T defaultValue)
        {
            this.Name = name;
            this.Value = defaultValue;
        }

        public IReadOnlyDictionary<string, T> ValuesCollection { get; protected set; }

        public virtual T Value { get; protected set; }

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

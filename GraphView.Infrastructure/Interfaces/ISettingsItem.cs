using System;
using System.Windows;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface ISettingsItem
    {
        string Name { get; }
        object Value { get; }
        UIElement CustomEditor { get; }
        event EventHandler Changed;
    }
}

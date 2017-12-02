using System;
using System.Windows;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface IToolbarElement
    {
        DependencyObject View { get; }
    }
}

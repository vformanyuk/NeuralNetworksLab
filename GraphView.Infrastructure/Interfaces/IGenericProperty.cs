using System.Windows;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface IGenericProperty
    {
        string PropertyName { get; }
        bool IsReadonly { get; }
        object Value { get; }
        UIElement CustomEditor { get; }

        void UpdateProperty();
    }
}

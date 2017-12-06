using System.Windows;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface IGenericProperty
    {
        string DisplayName { get; }
        bool IsReadonly { get; }
        object Value { get; }
        //Action<object> PropertySetter { get; }
        UIElement CustomEditor { get; }
        //IReadOnlyDictionary<string, T> ValuesCollection { get; }
    }
}

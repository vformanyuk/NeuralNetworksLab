using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface IPropertiesProvider : INotifyPropertyChanged
    {
        event EventHandler Loaded;
        ObservableCollection<IGenericProperty> Properties { get; }
        void Load(IPropertiesContainer properties);
        void Load(IEnumerable<IPropertiesContainer> model);
        void Commit();
    }
}

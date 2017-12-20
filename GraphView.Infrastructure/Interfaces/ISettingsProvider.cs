using System;
using System.Collections.Generic;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface ISettingsProvider
    {
        double LearningRate { get; set; }

        void AddProperty(Type neuronType, IGenericProperty property);

        IReadOnlyDictionary<string, IGenericProperty> this[Type neuronType]
        {
            get;
        }
    }
}

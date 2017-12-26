using System;
using System.Collections.Generic;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface ISettingsProvider
    {
        double LearningRate { get; set; }

        void AddProperty(Type neuronType, ISettingsItem property);

        IReadOnlyDictionary<string, ISettingsItem> this[Type neuronType]
        {
            get;
        }
    }
}

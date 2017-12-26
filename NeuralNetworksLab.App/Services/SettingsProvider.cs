using System;
using System.Collections.Generic;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworksLab.App.Services
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly Dictionary<Type, IReadOnlyDictionary<string, ISettingsItem>> _neuronSettings;

        public SettingsProvider()
        {
            _neuronSettings = new Dictionary<Type, IReadOnlyDictionary<string, ISettingsItem>>();
        }

        public void AddProperty(Type neuronType, ISettingsItem property)
        {
            if (!_neuronSettings.TryGetValue(neuronType, out var properties))
            {
                properties = new Dictionary<string, ISettingsItem>();
                _neuronSettings.Add(neuronType, properties);
            }
            (properties as Dictionary<string, ISettingsItem>)?.Add(property.Name, property);
        }

        public IReadOnlyDictionary<string, ISettingsItem> this[Type neuronType]
        {
            get
            {
                return _neuronSettings[neuronType];
            }
        }

        public double LearningRate { get; set; }
    }
}

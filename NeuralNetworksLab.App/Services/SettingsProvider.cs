using System;
using System.Collections.Generic;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworksLab.App.Services
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly Dictionary<Type, IReadOnlyDictionary<string, IGenericProperty>> _neuronSettings;

        public SettingsProvider()
        {
            _neuronSettings = new Dictionary<Type, IReadOnlyDictionary<string, IGenericProperty>>();
        }

        public void AddProperty(Type neuronType, IGenericProperty property)
        {
            if (!_neuronSettings.TryGetValue(neuronType, out var properties))
            {
                properties = new Dictionary<string, IGenericProperty>();
                _neuronSettings.Add(neuronType, properties);
            }
            (properties as Dictionary<string, IGenericProperty>)?.Add(property.PropertyName, property);
        }

        public IReadOnlyDictionary<string, IGenericProperty> this[Type neuronType]
        {
            get
            {
                return _neuronSettings[neuronType];
            }
        }

        public double LearningRate { get; set; }
    }
}

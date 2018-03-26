using System;
using System.Collections.Generic;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Common.Properties;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworksLab.App.Services
{
    public class NeuronFactory : INeuronFactory
    {
        public IReadOnlyDictionary<Type, Func<IPropertiesContrianer, NeuronBase>> Constructors { get; }
        public IReadOnlyDictionary<Type, Func<IPropertiesContrianer>> PropertiesContainerConstructors { get; }
        public IReadOnlyDictionary<Type, IPropertiesProvider> PropertyProviders { get; }

        public NeuronFactory(IEnumerable<NeuralNetworkLabPlugin> plugins)
        {
            var constructors = new Dictionary<Type, Func<IPropertiesContrianer, NeuronBase>>();
            var propertiesConstructors = new Dictionary<Type, Func<IPropertiesContrianer>>();
            var propertyProviders = new Dictionary<Type, IPropertiesProvider>();

            foreach (var neuralNetworkLabPlugin in plugins)
            {
                constructors.Add(neuralNetworkLabPlugin.NeuronType, neuralNetworkLabPlugin.CreateNeuronModel);
                propertiesConstructors.Add(neuralNetworkLabPlugin.NeuronType, neuralNetworkLabPlugin.CreatePropertiesContrianer);
                propertyProviders.Add(neuralNetworkLabPlugin.NeuronType, neuralNetworkLabPlugin.PropertiesProvider);
            }

            propertyProviders.Add(typeof(Layer), new LayerProperties(this));
            propertyProviders.Add(typeof(CsvSensorLayer), new CsvSensorLayerProperties(this));

            this.Constructors = constructors;
            this.PropertiesContainerConstructors = propertiesConstructors;
            this.PropertyProviders = propertyProviders;
        }
    }
}
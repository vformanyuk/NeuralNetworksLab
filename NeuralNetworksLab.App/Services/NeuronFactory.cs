using System;
using System.Collections.Generic;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Common.Properties;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworksLab.App.Services
{
    public class NeuronFactory : INeuronFactory
    {
        public IReadOnlyDictionary<Type, Func<IPropertiesContainer, NeuronBase>> Constructors { get; }
        public IReadOnlyDictionary<Type, Func<INode>> NodeConstructors { get; }
        public IReadOnlyDictionary<Type, Func<IPropertiesContainer>> PropertiesContainerConstructors { get; }
        public IReadOnlyDictionary<Type, IPropertiesProvider> PropertyProviders { get; }

        public NeuronFactory(IEnumerable<NeuralNetworkLabPlugin> plugins)
        {
            var constructors = new Dictionary<Type, Func<IPropertiesContainer, NeuronBase>>();
            var nodeConstructors = new Dictionary<Type, Func<INode>>();
            var propertiesConstructors = new Dictionary<Type, Func<IPropertiesContainer>>();
            var propertyProviders = new Dictionary<Type, IPropertiesProvider>();

            foreach (var neuralNetworkLabPlugin in plugins)
            {
                constructors.Add(neuralNetworkLabPlugin.NeuronType, neuralNetworkLabPlugin.CreateNeuronModel);
                nodeConstructors.Add(neuralNetworkLabPlugin.NodeType, neuralNetworkLabPlugin.CreateNeuronNode);
                propertiesConstructors.Add(neuralNetworkLabPlugin.NeuronType, neuralNetworkLabPlugin.CreatePropertiesContrianer);
                propertyProviders.Add(neuralNetworkLabPlugin.NeuronType, neuralNetworkLabPlugin.PropertiesProvider);
            }

            constructors.Add(typeof(Sensor), c => new Sensor()); // sensor has no properties

            propertyProviders.Add(typeof(Layer), new LayerProperties(this));
            propertyProviders.Add(typeof(CsvSensorLayer), new CsvSensorLayerProperties(this));

            nodeConstructors.Add(typeof(Layer), () => new Layer(this));
            nodeConstructors.Add(typeof(CsvSensorLayer), () => new CsvSensorLayer(this));

            this.Constructors = constructors;
            this.NodeConstructors = nodeConstructors;
            this.PropertiesContainerConstructors = propertiesConstructors;
            this.PropertyProviders = propertyProviders;
        }
    }
}
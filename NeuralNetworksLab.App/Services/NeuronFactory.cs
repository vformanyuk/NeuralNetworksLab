using System;
using System.Collections.Generic;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworksLab.App.Services
{
    public class NeuronFactory : INeuronFactory
    {
        public IReadOnlyDictionary<Type, Func<NeuronBase>> Constructors { get; }

        public NeuronFactory(IEnumerable<NeuralNetworkLabPlugin> plugins)
        {
            var constructors = new Dictionary<Type, Func<NeuronBase>>();

            foreach (var neuralNetworkLabPlugin in plugins)
            {
                constructors.Add(neuralNetworkLabPlugin.NeuronType, neuralNetworkLabPlugin.CreateNeuronModel);
            }

            this.Constructors = constructors;
        }
    }
}

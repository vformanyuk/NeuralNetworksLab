using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetworkLab.Infrastructure.FrameworkDefaults;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworkLab.Infrastructure.Metadata;
using NeuralNetworkLab.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Network
{
    public class NetworkBuilder
    {
        private readonly IDiagram _diagram;
        private readonly INeuronFactory _factory;
        private readonly ISettingsProvider _settings;

        public NetworkBuilder(IDiagram diagram, INeuronFactory factory, ISettingsProvider settings)
        {
            _diagram = diagram;
            _factory = factory;
            _settings = settings;
        }

        public NeuralNetwork CreateNetwork()
        {
            var network = new NeuralNetwork(_settings);
            // create neuron nodes + metadata
            this.CreateNeurons(network);
            // create fibers + update metadata
            this.CreateFibers(network);

            return network;
        }

        private readonly Dictionary<Guid, List<Guid>> _layers = new Dictionary<Guid, List<Guid>>();
        private readonly Dictionary<Guid, IEnumerable<(Guid neuronId, Guid inputId, Guid outputId)>> _layerConnectorsMap = new Dictionary<Guid, IEnumerable<(Guid neuronId, Guid inputId, Guid outputId)>>();
        private MetadataContainer _metadata = new MetadataContainer();

        private void CreateNeurons(NeuralNetwork network)
        {
            foreach (var node in _diagram.ChildNodes.OfType<NeuronNode>())
            {
                if (node is Layer layer)
                {
                    _metadata.AddNode(node.X, node.Y, node.Id, NodeType.Layer);

                    _layers.Add(node.Id, new List<Guid>());
                    for (int i = 0; i < layer.NeuronsCount; i++)
                    {
                        var neuron = _factory.Constructors[layer.NeuronType].Invoke(layer.NeuronProperties);
                        var neuronId = Guid.NewGuid();
                        network.AppendNeuron(neuronId, neuron);

                        _layers[layer.Id].Add(neuronId);
                    }

                    if (layer.UseCompactInputs || layer.UseCompactOutputs)
                    {
                        _layerConnectorsMap.Add(layer.Id, this.PrepareNeuronConnectorRelation(layer));
                    }

                    if (layer is IDatasourceProducer datasourceProducer)
                    {
                        var sensors = network.Neurons
                            .Where(n => _layers[layer.Id].Contains(n.Key))
                            .Select(n => n.Value)
                            .OfType<Sensor>();

                        network.AddInputs(datasourceProducer.GetDatasourceConstructor(), sensors);
                    }
                }
                else
                {
                    var neuron = _factory.Constructors[node.NeuronType].Invoke(node.Properties);

                    network.AppendNeuron(node.Id, neuron);

                    _metadata.AddNode(node.X, node.Y, node.Id, NodeType.Neuron);
                }
            }
        }

        private void CreateFibers(NeuralNetwork network)
        {
            foreach (var diagramConnection in _diagram.Connections)
            {
                if (diagramConnection.StartPoint.Host is Layer sourceLayer)
                {
                    if (diagramConnection.EndPoint.Host is Layer destinationLayer)
                    {
                        // each-to-each
                        if (sourceLayer.UseCompactOutputs && destinationLayer.UseCompactInputs)
                        {
                            foreach (var sid in _layers[sourceLayer.Id])
                            {
                                foreach (var did in _layers[destinationLayer.Id])
                                {
                                    //fibers.Add(new NeuroFiber(neurons[sid], neurons[did], _settings));
                                    network.CreateFiberBetween(sid, did);
                                }
                            }
                            continue;
                        }
                        // each-to-one
                        if (sourceLayer.UseCompactOutputs && !destinationLayer.UseCompactInputs)
                        {
                            var destinationDesc = _layerConnectorsMap[destinationLayer.Id].First(m => m.inputId == diagramConnection.EndPoint.Id);
                            foreach (var sid in _layers[sourceLayer.Id])
                            {
                                //fibers.Add(new NeuroFiber(neurons[sid], neurons[destinationDesc.neuronId], _settings));
                                network.CreateFiberBetween(sid, destinationDesc.neuronId);
                            }
                            continue;
                        }
                        //one-to-each
                        if (!sourceLayer.UseCompactOutputs && destinationLayer.UseCompactInputs)
                        {
                            var sourceDesc = _layerConnectorsMap[sourceLayer.Id].First(m => m.outputId == diagramConnection.StartPoint.Id);
                            foreach (var did in _layers[destinationLayer.Id])
                            {
                                //fibers.Add(new NeuroFiber(neurons[sourceDesc.neuronId], neurons[did], _settings));
                                network.CreateFiberBetween(sourceDesc.neuronId, did);
                            }
                            continue;
                        }
                        //one-to-one
                        if (!sourceLayer.UseCompactOutputs && !destinationLayer.UseCompactInputs)
                        {
                            var sourceDesc = _layerConnectorsMap[sourceLayer.Id].First(m => m.outputId == diagramConnection.StartPoint.Id);
                            var destinationDesc = _layerConnectorsMap[destinationLayer.Id].First(m => m.inputId == diagramConnection.EndPoint.Id);
                            //fibers.Add(new NeuroFiber(neurons[sourceDesc.neuronId], neurons[destinationDesc.neuronId], _settings));
                            network.CreateFiberBetween(sourceDesc.neuronId, destinationDesc.neuronId);
                            continue;
                        }
                    }

                    var destinationNode = (NeuronNode) diagramConnection.EndPoint.Host;
                    //each-to-one
                    if (sourceLayer.UseCompactOutputs)
                    {
                        foreach (var sid in _layers[sourceLayer.Id])
                        {
                            //fibers.Add(new NeuroFiber(neurons[sid], neurons[destinationNode.Id], _settings));
                            network.CreateFiberBetween(sid, destinationNode.Id);
                        }
                    }
                    //one-to-one
                    else
                    {
                        var sourceDesc = _layerConnectorsMap[sourceLayer.Id].First(m => m.outputId == diagramConnection.StartPoint.Id);
                        //fibers.Add(new NeuroFiber(neurons[sourceDesc.neuronId], neurons[destinationNode.Id], _settings));
                        network.CreateFiberBetween(sourceDesc.neuronId, destinationNode.Id);
                    }
                    continue;
                }
                
                var sNode = (NeuronNode)diagramConnection.StartPoint.Host;
                if (diagramConnection.EndPoint.Host is Layer dLayer)
                {
                    //one-to-each
                    if (dLayer.UseCompactInputs)
                    {
                        foreach (var did in _layers[dLayer.Id])
                        {
                            //fibers.Add(new NeuroFiber(neurons[sNode.Id], neurons[did], _settings));
                            network.CreateFiberBetween(sNode.Id, did);
                        }
                    }
                    //one-to-one
                    else
                    {
                        var destinationDesc = _layerConnectorsMap[dLayer.Id].First(m => m.inputId == diagramConnection.EndPoint.Id);
                        //fibers.Add(new NeuroFiber(neurons[sNode.Id], neurons[destinationDesc.neuronId], _settings));
                        network.CreateFiberBetween(sNode.Id,destinationDesc.neuronId);
                    }
                    continue;
                }

                //one-to-one
                var dNode = (NeuronNode)diagramConnection.EndPoint.Host;
                //fibers.Add(new NeuroFiber(neurons[sNode.Id], neurons[dNode.Id], _settings));
                network.CreateFiberBetween(sNode.Id, dNode.Id);
            }
        }

        private IEnumerable<(Guid neuronId, Guid inputId, Guid outputId)> PrepareNeuronConnectorRelation(Layer layer)
        {
            var map = new List<(Guid neuronId, Guid inputId, Guid outputId)>();

            var inputs = layer.Inputs.Select(c => c.Id).ToList();
            if (layer.UseCompactInputs)
            {
                for (int i = 1; i < layer.NeuronsCount; i++)
                {
                    inputs.Add(Guid.NewGuid());
                }
            }

            var outputs = layer.Outputs.Select(c => c.Id).ToList();
            if (layer.UseCompactOutputs)
            {
                for (int i = 1; i < layer.NeuronsCount; i++)
                {
                    outputs.Add(Guid.NewGuid());
                }
            }

            for (int i = 0; i < layer.NeuronsCount; i++)
            {
                map.Add((_layers[layer.Id][i], inputs[i], outputs[i]));
            }

            return map;
        }
    }
}

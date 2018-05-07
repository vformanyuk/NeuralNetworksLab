using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.Network
{
    public class NeuralNetwork : IDisposable
    {
        private readonly List<NeuroFiber> _fibers;
        private readonly List<Sensor> _sensors = new List<Sensor>();
        private readonly Dictionary<Guid, NeuronBase> _neurons;

        private IDataSource _dataSource;

        private readonly ISettingsProvider _settings;

        private readonly Random _rnd;

        public NeuralNetwork(ISettingsProvider settings)
        {
            _fibers = new List<NeuroFiber>();
            _neurons = new Dictionary<Guid, NeuronBase>();
            _settings = settings;

            _rnd = new Random(Int32.MaxValue); // max value for debugging
        }

        public void CreateFiberBetween(Guid sourceNodeId, Guid destinationNodeId)
        {
            var fiber = new NeuroFiber(_neurons[sourceNodeId], _neurons[destinationNodeId], _settings)
            {
                Weight = _rnd.NextDouble()
            };
            _fibers.Add(fiber);
        }

        public void AppendNeuron(Guid id, NeuronBase neuron)
        {
            _neurons.Add(id, neuron);
            neuron.Output = _rnd.NextDouble();
        }

        public void AddInputs(IDataSource dataSource, IEnumerable<Sensor> sensors)
        {
            _sensors.AddRange(sensors);
            _dataSource = dataSource;
        }

        public void RunSimulationAge()
        {
            if (_dataSource == null) return;

            var output = _neurons.Values.First(n => n.AxonsCount == 0);

            using (_dataSource.BeginDataFetch())
            {
                double[] data =_dataSource.GetNextDataPortion();
                while (data!=null)
                {
                    for (int i = 0; i < _sensors.Count; i++)
                    {
                        _sensors[i].Emit(data[i]);
                    }

                    output.Learn(data.Last() - output.Output);

                    data = _dataSource.GetNextDataPortion();
                } 
            }
        }

        public IEnumerable<NeuroFiber> Fibers => _fibers;
        public IReadOnlyDictionary<Guid, NeuronBase> Neurons => _neurons;

        public void Dispose()
        {
            foreach (var neuroFiber in _fibers)
            {
                neuroFiber.Dispose();
            }

            foreach (var neuronBase in _neurons)
            {
                neuronBase.Value.Dispose();
            }
            _fibers.Clear();
            _neurons.Clear();
        }
    }
}

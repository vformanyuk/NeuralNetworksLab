using System;
using System.IO;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.FrameworkDefaults
{
    public class CsvSensorLayer : Layer
    {
        // subscribe to EVentAggregator 'emulate' and read csv file emitting got values

        public CsvSensorLayer(INeuronFactory factory) : base(factory)
        {
            this.NeuronType = typeof(Sensor);
        }

        private char? _delimiter;
        public char? Delimiter
        {
            get => _delimiter;
            set
            {
                if (_delimiter == value) return;

                _delimiter = value;
                OnPropertyChanged(nameof(Delimiter));
                TrySetSensors();
            }
        }

        private string _file;
        public string FileName
        {
            get => _file;
            set
            {
                if (_file == value) return;

                _file = value;
                OnPropertyChanged(nameof(FileName));
                TrySetSensors();
            }
        }

        private bool _skipHeaderRow;
        public bool SkipHeaderRow
        {
            get => _skipHeaderRow;
            set
            {
                if (_skipHeaderRow == value) return;

                _skipHeaderRow = value;
                OnPropertyChanged(nameof(SkipHeaderRow));
            }
        }

        private void TrySetSensors()
        {
            if (!_delimiter.HasValue || string.IsNullOrEmpty(_file))
            {
                return;
            }

            if (!File.Exists(_file))
            {
                return;
            }

            using (var rdr = File.OpenText(_file))
            {
                string line = rdr.ReadLine();
                if (!string.IsNullOrEmpty(line) && line.IndexOf(_delimiter.Value) > 0)
                {
                    var sensorsCount = line.Split(new [] {_delimiter.Value}, StringSplitOptions.RemoveEmptyEntries).Length;
                    this.NeuronsCount = (uint) sensorsCount - 1;
                }
            }
        }
    }
}

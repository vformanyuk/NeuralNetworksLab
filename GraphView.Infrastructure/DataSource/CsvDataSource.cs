using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworkLab.Infrastructure.DataSource
{
    public class CsvDataSource : IDataSource
    {
        private StreamReader _reader;
        private readonly char _delimiter;
        private readonly string _file;

        public CsvDataSource(string filePath, char delimiter)
        {
            if (!File.Exists(filePath)) return;

            _delimiter = delimiter;
            _file = filePath;
        }

        public IDisposable BeginDataFetch()
        {
            _reader = File.OpenText(_file);
            this.DataAvailable = true;

            return new DatasourceUseContract(_reader);
        }

        public bool DataAvailable
        {
            get;
            private set;
        }

        public double[] GetNextDataPortion()
        {
            var line = _reader.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                this.DataAvailable = false;
                return null;
            }

            return line.Split(new char[1] {_delimiter}, StringSplitOptions.RemoveEmptyEntries).Select(i=>
            {
                if (double.TryParse(i, out double v)) return v;

                if (double.TryParse(i, NumberStyles.Any, CultureInfo.InvariantCulture, out v)) return v;

                return 0.0;
            }).ToArray();
        }

        private class DatasourceUseContract : IDisposable
        {
            private readonly StreamReader _rdr;
            public DatasourceUseContract(StreamReader rdr)
            {
                _rdr = rdr;
            }

            public void Dispose()
            {
                _rdr?.Dispose();
            }
        }
    }
}

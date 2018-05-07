using System;
using System.Collections.Generic;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface IDataSource
    {
        IDisposable BeginDataFetch();
        bool DataAvailable { get; }
        double[] GetNextDataPortion();
    }
}

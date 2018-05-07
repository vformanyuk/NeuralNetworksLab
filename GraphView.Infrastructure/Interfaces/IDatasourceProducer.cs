using System;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface IDatasourceProducer
    {
        IDataSource GetDatasourceConstructor();
    }
}

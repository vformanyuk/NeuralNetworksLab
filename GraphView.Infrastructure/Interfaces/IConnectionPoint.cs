namespace NeuralNetworkLab.Interfaces
{
    public interface IConnectionPoint
    {
        bool IsConnected { get; set; }
        bool CanConnect(IConnectionPoint connectionPoint);
    }
}

using System.Windows;

namespace NeuralNetworkLab.Interfaces
{
    public interface IRouter
    {
        Point[] CalculateGeometry(Point start, Point end);
    }
}

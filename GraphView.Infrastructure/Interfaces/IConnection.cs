﻿using System.Windows;
using System.Windows.Media;

namespace NeuralNetworkLab.Interfaces
{
    public interface IConnection : ISelectable
    {
        IConnectionPoint StartPoint { get; }
        IConnectionPoint EndPoint { get; }
        IRouter Router { get; }

        PointCollection Data { get; }

        void UpdateConnectionPoints(Point startPoint, Point endPoint);
    }
}

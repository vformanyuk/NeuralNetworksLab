﻿using System.Collections.ObjectModel;

namespace NeuralNetworkLab.Interfaces
{
    public interface IDiagram
    {
        ObservableCollection<INode> ChildNodes { get; }

        IConnectionsFactory ConnectionsFactory { get; }

        ObservableCollection<IConnection> Connections { get; } 
    }
}

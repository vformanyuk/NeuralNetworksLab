using System;
using System.Collections.ObjectModel;
using NeuralNetworkLab.Interfaces;

namespace GraphView.Framework
{
    public class Diagram : IDiagram
    {
        public Diagram(IConnectionsFactory connectionsFactory)
        {
            ChildNodes = new ObservableCollection<INode>();
            Connections = new ObservableCollection<IConnection>();
            ConnectionsFactory = connectionsFactory;
        }

        public ObservableCollection<INode> ChildNodes { get; private set; }
        public IConnectionsFactory ConnectionsFactory { get; private set; }
        public ObservableCollection<IConnection> Connections { get; private set; }
        public event EventHandler NodeSelectionChanged;
        public event EventHandler ConnectionSelectionChanged;

        internal void RaisNodeSelectionChanged()
        {
            NodeSelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void RaisConnectionSelectionChanged()
        {
            ConnectionSelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

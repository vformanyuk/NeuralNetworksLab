using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using NeuralNetworkLab.Interfaces;

namespace GraphView.Framework.Controls
{
    public sealed class DiagramCanvasControl : Canvas
    {
        #region Static fields

        /// <summary>
        /// The diagram property
        /// </summary>
        public static readonly DependencyProperty DiagramProperty = DependencyProperty.Register(
            "Diagram", typeof (IDiagram), typeof (DiagramCanvasControl), new PropertyMetadata(DiagramChanged));

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramCanvasControl"/> class.
        /// </summary>
        public DiagramCanvasControl()
        {
            _nodesSet = new Dictionary<INode, NodeContainerControl>();
            _connections = new Dictionary<IConnection, ConnectionContainerControl>();
            _selectedNodes = new HashSet<INode>();

            Panel.SetZIndex(this, -1);

            Background = new SolidColorBrush(Colors.White);
        }

        #region Public properties

        /// <summary>
        /// Gets or sets the diagram.
        /// </summary>
        public IDiagram Diagram
        {
            get { return (IDiagram)GetValue(DiagramProperty); }
            set { SetValue(DiagramProperty, value); }
        }

        #endregion

        #region Private Methods

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            _currentPosition = e.GetPosition(this);
            _originPoint = _currentPosition;
            _hittestElement = this.HitTest<BaseNodeControl>(_currentPosition);
            if (_hittestElement == null) // if node based element is clicked
            {
                return;
            }

            var connector = _hittestElement as ConnectorControl;
            if (connector != null)
            {
                var sourceConnector = connector;

                // if connector is already connected and Ctrl key pressed - start nodes reconnection
                if (connector.ConnectionPoint.IsConnected && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                {
                    var connections = _connections.Where(c => c.Value.Source.Equals(connector) || c.Value.Destination.Equals(connector)).ToList();
                    if (connections.Count > 0)
                    {
                        var record = connections.First();
                        // this is not a misstake. 
                        // If this is Source connector for connection then virtual connection should start from
                        // oposit connector which is Destination and vice versa.
                        if (record.Value.Source.Equals(connector))
                        {
                            sourceConnector = (ConnectorControl)record.Value.Destination;
                        }
                        else
                        {
                            sourceConnector = (ConnectorControl)record.Value.Source;
                        }

                        Diagram.Connections.Remove(record.Key);
                    }
                }

                // if it is connector - create virtual connection
                // virtual elements excluded from hit test
                var virtualConnector = new VirtualConnectionPoint(sourceConnector)
                {
                    X = _currentPosition.X,
                    Y = _currentPosition.Y
                };
                Children.Add(virtualConnector);

                var virtualConnection = new VirtualConnection(sourceConnector.ConnectionPoint);
                var virtualConnectionContainer = new ConnectionContainerControl(sourceConnector, virtualConnector,
                    virtualConnection, false);
                _connections.Add(virtualConnection, virtualConnectionContainer);
                Children.Add(virtualConnectionContainer);

                _hittestElement = virtualConnector;
            }

            _hittestElement.CaptureMouse();
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            var position = e.GetPosition(this);

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var node = _hittestElement as NodeContainerControl;
                if (node != null)
                {
                    node.X += position.X - _currentPosition.X;
                    node.Y += position.Y - _currentPosition.Y;

                    if (node.Node.IsSelected)
                    {
                        foreach (var selectedNode in _selectedNodes.Where(n => n != node.Node))
                        {
                            selectedNode.X += position.X - _currentPosition.X;
                            selectedNode.Y += position.Y - _currentPosition.Y;
                        }
                    }
                }

                var virtualPoint = _hittestElement as VirtualConnectionPoint;
                if (virtualPoint != null)
                {
                    virtualPoint.X += position.X - _currentPosition.X;
                    virtualPoint.Y += position.Y - _currentPosition.Y;

                    var hittest = this.AreaHitTest<ConnectorControl>(_currentPosition,
                        Constants.VirtualPointXOffset - 5);

                    if (hittest != null)
                    {
                        Mouse.SetCursor(hittest.ConnectionPoint.CanConnect(virtualPoint.SourceConnectionPoint)
                            ? Cursors.Hand
                            : Cursors.No);
                    }

                    var noneVirtualConnection = this.HitTest<ConnectionContainerControl>(new Point(virtualPoint.X, virtualPoint.Y));
                    if (noneVirtualConnection != null)
                    {
                        Mouse.SetCursor(Cursors.Hand);
                    }
                }

                var rectangle = _hittestElement as SelectionRect;
                if (rectangle != null) // update rectangle width/height
                {
                    if (rectangle.SelectionStartPoint.X < position.X)
                    {
                        Canvas.SetLeft(rectangle, rectangle.SelectionStartPoint.X);
                        rectangle.Width = position.X - rectangle.SelectionStartPoint.X;
                    }
                    else
                    {
                        Canvas.SetLeft(rectangle, position.X);
                        rectangle.Width = rectangle.SelectionStartPoint.X - position.X;
                    }

                    if (rectangle.SelectionStartPoint.Y < position.Y)
                    {
                        Canvas.SetTop(rectangle, rectangle.SelectionStartPoint.Y);
                        rectangle.Height = position.Y - rectangle.SelectionStartPoint.Y;
                    }
                    else
                    {
                        Canvas.SetTop(rectangle, position.Y);
                        rectangle.Height = rectangle.SelectionStartPoint.Y - position.Y;
                    }
                }

                // if no node selected and drag sitance riched add Selection Rectangle
                if (_hittestElement == null && 
                    (Math.Abs(_currentPosition.X - _originPoint.X) >= SystemParameters.MinimumHorizontalDragDistance ||
                     Math.Abs(_currentPosition.Y - _originPoint.Y) >= SystemParameters.MinimumVerticalDragDistance))
                {
                    _hittestElement = new SelectionRect(_currentPosition.X, _currentPosition.Y);
                    Children.Add((UIElement)_hittestElement);
                    _hittestElement.CaptureMouse();
                }
            }

            _currentPosition = position;

            base.OnPreviewMouseMove(e);
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            if (_hittestElement != null)
            {
                _hittestElement.ReleaseMouseCapture();

                var point = _hittestElement as VirtualConnectionPoint;
                if (point != null)
                {
                    var hittest = this.AreaHitTest<ConnectorControl>(_currentPosition, Constants.VirtualPointXOffset - 5);
                    if (hittest != null && hittest.ConnectionPoint.CanConnect(point.SourceConnectionPoint))
                    {
                        // if captured element is VirtualConnector and hittest element is ConnectionPoint - create connection
                        var newConnection = _diagram.ConnectionsFactory.CreateConnection(point.SourceConnectionPoint,
                            hittest.ConnectionPoint);
                        if (newConnection != null)
                        {
                            _diagram.Connections.Add(newConnection);

                            // add connection contrainer control to canvas
                            var connectionContainer = new ConnectionContainerControl(point.SourceConnectorControl,
                                hittest,
                                newConnection);
                            _connections.Add(newConnection, connectionContainer);
                            Children.Add(connectionContainer);
                        }
                    }

                    var noneVirtualConnection = this.HitTest<ConnectionContainerControl>(_currentPosition);
                    if (noneVirtualConnection != null)
                    {
                        var oldSource = noneVirtualConnection.Source;
                        var oldDestination = noneVirtualConnection.Destination;
                        var connectionContext = noneVirtualConnection.Connection;

                        ConnectorControl middleConnector = new ConnectorControl
                        {
                            X = _currentPosition.X,
                            Y = _currentPosition.Y,
                            Width = 10,
                            Height = 10,
                            ConnectionPoint = Activator.CreateInstance(noneVirtualConnection.Connection.StartPoint.GetType()) as IConnectionPoint
                        };

                        Canvas.SetLeft(middleConnector, middleConnector.X - middleConnector.Width / 2);
                        Canvas.SetTop(middleConnector, middleConnector.Y - middleConnector.Height / 2);

                        Children.Remove(_connections[connectionContext]);
                        _connections.Remove(connectionContext);

                        Children.Add(middleConnector);

                        var sourceToMiddle = _diagram.ConnectionsFactory.CreateConnection(
                            connectionContext.StartPoint, middleConnector.ConnectionPoint);
                        var middleToTarget = _diagram.ConnectionsFactory.CreateConnection(
                            middleConnector.ConnectionPoint, connectionContext.EndPoint);

                        _diagram.Connections.Add(sourceToMiddle);
                        _diagram.Connections.Add(middleToTarget);

                        var sourceToMiddleContainer = new ConnectionContainerControl(oldSource, middleConnector, sourceToMiddle);
                        _connections.Add(sourceToMiddle, sourceToMiddleContainer);
                        Children.Add(sourceToMiddleContainer);

                        var middleToTargetContainer = new ConnectionContainerControl(middleConnector, oldDestination, middleToTarget);
                        _connections.Add(middleToTarget, middleToTargetContainer);
                        Children.Add(middleToTargetContainer);

                        // add connection from original node to middle connection point
                        var newConnection = _diagram.ConnectionsFactory.CreateConnection(point.SourceConnectionPoint, middleConnector.ConnectionPoint);
                        if (newConnection != null)
                        {
                            _diagram.Connections.Add(newConnection);
                            var connectionContainer = new ConnectionContainerControl(point.SourceConnectorControl,
                                middleConnector,
                                newConnection);
                            _connections.Add(newConnection, connectionContainer);
                            Children.Add(connectionContainer);
                        }
                    }

                    // remove virtual connection and point
                    Children.Remove(point);
                    var virtualConnection = _connections.Keys.FirstOrDefault(k => k is VirtualConnection);
                    if (virtualConnection != null)
                    {
                        Children.Remove(_connections[virtualConnection]);
                        _connections.Remove(virtualConnection);
                    }
                }

                var selectionRect = _hittestElement as SelectionRect;
                if (selectionRect != null) // mass selection
                {
                    double selectionStartX = _currentPosition.X;
                    double selectionStartY = _currentPosition.Y;

                    if (selectionRect.SelectionStartPoint.X < selectionStartX)
                    {
                        selectionStartX = selectionRect.SelectionStartPoint.X;
                    }
                    if (selectionRect.SelectionStartPoint.Y < selectionStartY)
                    {
                        selectionStartY = selectionRect.SelectionStartPoint.Y;
                    }

                    var startPoint = new Point(selectionStartX, selectionStartY);

                    // find elements under selection rect
                    var elements = this.AreaHitTest<NodeContainerControl>(startPoint,
                        selectionRect.ActualWidth,
                        selectionRect.ActualHeight);

                    foreach (var coveredNode in elements.Select(n => n.Node))
                    {
                        ToggleSelection(coveredNode, true);
                    }

                    // remove selection rectangle
                    Children.Remove(selectionRect);
                }

                var node = _hittestElement as NodeContainerControl;
                if (node != null && // single node selection
                    Math.Abs(_currentPosition.X - _originPoint.X) <= SystemParameters.MinimumHorizontalDragDistance &&
                    Math.Abs(_currentPosition.Y - _originPoint.Y) <= SystemParameters.MinimumVerticalDragDistance)
                {
                    if (_selectedNodes.Count >= 1) // after mass selection remove selection from nodes other that clicked one
                    {
                        var removeSelection = _selectedNodes.Where(n => !n.Equals(node.Node)).ToList();
                        foreach (var selectedNode in removeSelection)
                        {
                            ToggleSelection(selectedNode, false);
                        }

                        ToggleSelection(node.Node, true);
                    }
                    else
                    {
                        ToggleSelection(node.Node, !node.Node.IsSelected);
                    }
                }

                var connectionCtrl = _hittestElement as ConnectionContainerControl;
                if (connectionCtrl != null) // connections selection
                {
                    var connections = _connections.Where(c => c.Value.Equals(connectionCtrl)).Select(c => c.Key);
                    foreach (var connection in connections)
                    {
                        connection.IsSelected = !connection.IsSelected;
                    }
                }
            }
            else
            {
                // click on canvas clears selection
                foreach (var selectedNode in _selectedNodes)
                {
                    selectedNode.IsSelected = false;
                }
                _selectedNodes.Clear();
            }

            _hittestElement = null;
            base.OnPreviewMouseUp(e);
        }

        /// <summary>
        /// Adds the existing nodes.
        /// </summary>
        private void AddExistingNodes()
        {
            foreach (var node in _diagram.ChildNodes)
            {
                var control = new NodeContainerControl(node);
                _nodesSet.Add(node, control);
                Children.Add(control);
            }
        }

        /// <summary>
        /// Toggles node selection.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="isSelected">if set to <c>true</c> [is selected].</param>
        private void ToggleSelection(INode node, bool isSelected)
        {
            node.IsSelected = isSelected;
            if (node.IsSelected)
            {
                _selectedNodes.Add(node);
            }
            else
            {
                _selectedNodes.Remove(node);
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the ChildNodes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void ChildNodes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var node in e.NewItems.OfType<INode>())
                    {
                        var control = new NodeContainerControl(node);
                        _nodesSet.Add(node, control);
                        Children.Add(control);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var toRemove = _nodesSet.Where(n => e.OldItems.Contains(n.Key)).ToList();
                    foreach (var pair in toRemove)
                    {
                        _nodesSet.Remove(pair.Key);
                        Children.Remove(pair.Value);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Children.Clear();
                    _nodesSet.Clear();
                    _selectedNodes.Clear();
                    break;
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the Connections control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void Connections_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var connection in e.NewItems.OfType<IConnection>())
                    {
                        connection.StartPoint.IsConnected = true;
                        connection.EndPoint.IsConnected = true;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var connection in e.OldItems.OfType<IConnection>())
                    {
                        connection.StartPoint.IsConnected = false;
                        connection.EndPoint.IsConnected = false;

                        var control = _connections[connection];
                        Children.Remove(control);
                        _connections.Remove(connection);
                        _diagram.ConnectionsFactory.ConnectionRemoved(connection);
                    }
                    break;
            }
        }

        /// <summary>
        /// Diagrams the changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DiagramChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;

            var canvas = (DiagramCanvasControl) d;
            var diagram = (IDiagram) e.NewValue;

            if (canvas._diagram != null)
            {
                canvas._diagram.ChildNodes.CollectionChanged -= canvas.ChildNodes_CollectionChanged;
                canvas._diagram.Connections.CollectionChanged -= canvas.Connections_CollectionChanged;
            }
            canvas._diagram = diagram;
            diagram.ChildNodes.CollectionChanged += canvas.ChildNodes_CollectionChanged;
            diagram.Connections.CollectionChanged += canvas.Connections_CollectionChanged;
            canvas.AddExistingNodes();
        }

        #endregion

        #region Private fields

        private IDiagram _diagram;
        private readonly Dictionary<INode, NodeContainerControl> _nodesSet;
        private readonly Dictionary<IConnection, ConnectionContainerControl> _connections;
        private readonly HashSet<INode> _selectedNodes; 
        private IInputElement _hittestElement;
        private Point _currentPosition, _originPoint;

        #endregion
    }
}
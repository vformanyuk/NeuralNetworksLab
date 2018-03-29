using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using GraphView.Framework.Converters;
using NeuralNetworkLab.Interfaces;

namespace GraphView.Framework.Controls
{
    public sealed class ConnectorControl : BaseNodeControl
    {
        #region Static fields

        public static readonly DependencyProperty ConnectionPointProperty = DependencyProperty.Register(
            "ConnectionPoint", typeof (IConnectionPoint), typeof (ConnectorControl));

        #endregion

        private Point _currentControlOffest;
        private BaseNodeControl _parentNode;
        private readonly OffsetConverter _xConverter, _yConverter;

        static ConnectorControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (ConnectorControl),
                new FrameworkPropertyMetadata(typeof (ConnectorControl)));
        }

        public ConnectorControl()
        {
            _xConverter = new OffsetConverter();
            _yConverter = new OffsetConverter();

            Loaded += ConnectorControl_Loaded;
        }

        private void ConnectorControl_Loaded(object sender, RoutedEventArgs e)
        {
            var parentNode = this.GetParent<BaseNodeControl>();
            if (parentNode == null)
            {
                return;
            }

            _parentNode = parentNode;
            _parentNode.LayoutUpdated += ParentNode_LayoutUpdated;

            var offset = TranslatePoint(new Point(0, 0), _parentNode);
            _currentControlOffest = new Point(offset.X + ActualWidth / 2, offset.Y + ActualHeight / 2);

            _xConverter.ManagedOffset = _currentControlOffest.X;
            SetBinding(XProperty,
                new Binding("X")
                {
                    Source = _parentNode, 
                    Converter = _xConverter
                });

            _yConverter.ManagedOffset = _currentControlOffest.Y;
            SetBinding(YProperty,
                new Binding("Y")
                {
                    Source = _parentNode,
                    Converter = _yConverter
                });
        }

        private void ParentNode_LayoutUpdated(object sender, System.EventArgs e)
        {
            var offset = TranslatePoint(new Point(0, 0), _parentNode);
            var p = new Point(offset.X + ActualWidth / 2, offset.Y + ActualHeight / 2);

            if ((int) (_currentControlOffest.X - p.X) == 0 && (int) (_currentControlOffest.Y - p.Y) == 0)
            {
                return;
            }

            _currentControlOffest = p;

            var xBinding = GetBindingExpression(XProperty);
            if (xBinding != null)
            {
                _xConverter.ManagedOffset = _currentControlOffest.X;
                xBinding.UpdateTarget();
            }
            var yBinding = GetBindingExpression(YProperty);
            if (yBinding != null)
            {
                _yConverter.ManagedOffset = _currentControlOffest.Y;
                yBinding.UpdateTarget();
            }
        }

        public IConnectionPoint ConnectionPoint
        {
            get => (IConnectionPoint) GetValue(ConnectionPointProperty);
            set => SetValue(ConnectionPointProperty, value);
        }
    }
}
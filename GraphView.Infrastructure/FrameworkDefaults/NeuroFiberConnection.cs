using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using GraphView.Infrastructure.Annotations;
using NeuralNetworkLab.Interfaces;
using System.Reactive.Linq;
using System;

namespace NeuralNetworkLab.Infrastructure.FrameworkDefaults
{
    public class NeuroFiberConnection : IConnection, INotifyPropertyChanged
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NeuroFiberConnection"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="router">The router.</param>
        public NeuroFiberConnection(IConnectionPoint source, IConnectionPoint destination, IRouter router)
        {
            StartPoint = source;
            EndPoint = destination;
            Router = router;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the connection points.
        /// Used by container control to mainatin data consistency between view(control) and view model(this type)
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        public void UpdateConnectionPoints(Point startPoint, Point endPoint)
        {
            Data = new PointCollection(Router.CalculateGeometry(startPoint, endPoint));
            OnPropertyChanged(nameof(Data));
        }
        #endregion

        #region Private Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private fields
        private NeuroFiber _model;
        private IDisposable _subscribtionToken;
        #endregion

        #region Public properties
        public double Weight => _model?.Weight ?? 0;

        public NeuroFiber Model
        {
            get
            {
                return _model;
            }
            set
            {
                if (_model != null)
                {
                    _subscribtionToken.Dispose();
                }

                _model = value;

                if (_model != null)
                {
                    _subscribtionToken = _model.Sample(TimeSpan.FromMilliseconds(500))
                                               .SubscribeOnDispatcher()
                                               .Subscribe(r => OnPropertyChanged(nameof(this.Weight)));
                }
            }
        }
        #endregion

        #region IConnection Members

        /// <summary>
        /// Gets the start point.
        /// </summary>
        public IConnectionPoint StartPoint { get; protected set; }
        /// <summary>
        /// Gets the end point.
        /// </summary>
        public IConnectionPoint EndPoint { get; protected set; }
        /// <summary>
        /// Gets the router which calculates points for connection.
        /// </summary>
        public IRouter Router { get; protected set; }
        /// <summary>
        /// Gets the points collection which is used to render the connection.
        /// </summary>
        public PointCollection Data { get; protected set; }

        public bool IsSelected { get; set; }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
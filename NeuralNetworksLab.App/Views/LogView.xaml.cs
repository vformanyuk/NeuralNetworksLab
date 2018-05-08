using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworksLab.App.Views
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        public static readonly DependencyProperty LogAggregatorProperty = DependencyProperty.Register(
            "LogAggregator", typeof(ILogAggregator), typeof(LogView), new PropertyMetadata(default(ILogAggregator),LogAggergatorSet));

        private static void LogAggergatorSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (LogView) d;

            if (e.OldValue != null)
            {
                ((ILogAggregator) e.OldValue).LogBatchAvailable -= view.OnLogsAvaliable;
            }

            var aggregator = (ILogAggregator) e.NewValue;
            aggregator.LogBatchAvailable += view.OnLogsAvaliable;
        }

        private void OnLogsAvaliable(object sender, string[] e)
        {
            this.Dispatcher.Invoke(() =>
            {
                foreach (var message in e)
                {
                    logsContainer.Items.Insert(0,message);
                }
            },DispatcherPriority.Normal);
        }

        public ILogAggregator LogAggregator
        {
            get => (ILogAggregator) GetValue(LogAggregatorProperty);
            set => SetValue(LogAggregatorProperty, value);
        }

        public LogView()
        {
            InitializeComponent();
        }
    }
}

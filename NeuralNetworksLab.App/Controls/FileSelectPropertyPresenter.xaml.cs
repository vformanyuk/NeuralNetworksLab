using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using NeuralNetworkLab.Infrastructure.Common.Properties;

namespace NeuralNetworksLab.App.Controls
{
    /// <summary>
    /// Interaction logic for FileSelectPropertyPresenter.xaml
    /// </summary>
    public partial class FileSelectPropertyPresenter : UserControl
    {
        public FileSelectPropertyPresenter()
        {
            InitializeComponent();
        }

        private void OpenFileHandler(object sender, RoutedEventArgs e)
        {
            if(!(this.DataContext is FileSelectProperty property)) return;

            OpenFileDialog dlg = new OpenFileDialog
            {
                CheckFileExists = true,
                DefaultExt = "csv",
                Filter = "All|*.*",
                Multiselect = false
            };
            dlg.ShowDialog(Application.Current.MainWindow);

            if (!string.IsNullOrEmpty(dlg.FileName))
            {
                property.PropertySetter.Invoke(dlg.FileName);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Interfaces;

namespace NeuralNetworksLab.App.ViewModels
{
    public class SettingsViewModel
    {
        public SettingsViewModel(ISettingsProvider settingsProvider, IEnumerable<NeuralNetworkLabPlugin> plugins)
        {
            
        }
    }
}

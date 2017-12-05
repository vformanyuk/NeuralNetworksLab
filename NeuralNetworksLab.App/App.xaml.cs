using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xml;
using Autofac;
using NeuralNetworkLab.Infrastructure;
using NeuralNetworkLab.Infrastructure.Interfaces;
using NeuralNetworksLab.App.ViewModels;

namespace NeuralNetworksLab.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var builder = new ContainerBuilder();
            var workingDir = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(assembly.Location), "Plugins"));
            foreach (var dll in workingDir.EnumerateFiles("*.dll"))
            {
                assembly = Assembly.LoadFile(dll.FullName);

                var assemblyResources = new ResourceDictionary();
                assemblyResources.Source = new Uri($"pack://application:,,,/{assembly.GetName().Name};component/generic.xaml");
                this.Resources.MergedDictionaries.Add(assemblyResources);

                builder.RegisterAssemblyModules(assembly);
            }

            builder.RegisterInstance(new Services.SettingsProvider()).As<ISettingsProvider>().ExternallyOwned();
            builder.RegisterType<MainViewModel>().SingleInstance();

            var contrianer = builder.Build();

            var mainViewModel = contrianer.Resolve<MainViewModel>();
            foreach(var plugin in contrianer.Resolve<IEnumerable<NeuralNetworkLabPlugin>>())
            {
                mainViewModel.AddPlugin(plugin);
            }

            this.MainWindow = new MainWindow();
            this.MainWindow.DataContext = mainViewModel;
            this.MainWindow.Show();
        }
    }
}

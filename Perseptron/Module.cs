using Autofac;
using NeuralNetworkLab.Infrastructure;

namespace Perseptron
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Plugin>().As<NeuralNetworkLabPlugin>().ExternallyOwned();
        }
    }
}

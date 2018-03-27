using System.Windows;
using System.Windows.Controls;
using NeuralNetworkLab.Infrastructure.Common.Properties;

namespace NeuralNetworksLab.App.TemplateSelectors
{
    public class PropertyTemplateSelector<T> : DataTemplateSelector
    {
        public DataTemplate ListTemplate { get; set; }
        public DataTemplate InputTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is NeuralNetworkProperty<T> model))
            {
                return base.SelectTemplate(item, container); ;
            }

            if (model.ValuesCollection == null)
            {
                return InputTemplate;
            }

            return ListTemplate;
        }
    }
}

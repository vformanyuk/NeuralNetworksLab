using NeuralNetworkLab.Infrastructure.Common.Settings;

namespace Perseptron
{
    public class BiasSettingsItem : SettingsItem<double>
    {
        public BiasSettingsItem(string name, double defaultValue = 1) : base(name)
        {
            this._value = defaultValue;
        }

        public override bool IsReadonly => false;
    }
}

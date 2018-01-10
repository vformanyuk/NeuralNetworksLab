using System;

namespace NeuralNetworkLab.Infrastructure
{
    public class Sensor : NeuronBase
    {
        public void Emit(double value)
        {
            foreach (var observer in _axons)
            {
                observer.OnNext(value);
            }
        }

        public sealed override void Learn(double error)
        {
        }

        public sealed override void OnCompleted()
        {
        }

        public sealed override void OnError(Exception ex)
        {
        }

        public sealed override void OnNext(double x)
        {
        }
    }
}

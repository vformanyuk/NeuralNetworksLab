﻿using System;

namespace NeuralNetworkLab.Infrastructure.Common.Properties
{
    public class DoubleProperty : NeuralNetworkProperty<double>
    {
        public DoubleProperty(string name, Action<double> setter, double? defaultValue) : base(name, setter)
        {
            if (defaultValue.HasValue)
            {
                this.Value = defaultValue.Value;
            }
        }
    }
}

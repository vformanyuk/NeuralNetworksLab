﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLab.Infrastructure.Interfaces
{
    public interface ISettingsProvider
    {
        double LearningRate { get; set; }
    }
}

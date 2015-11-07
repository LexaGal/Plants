﻿using System;
using System.Text.RegularExpressions;

namespace PlantingLib.MeasurableParameters
{
    public abstract class MeasurableParameter
    {
        protected MeasurableParameter(Guid id, int optimal, int min, int max)
        {
            Id = id;
            Optimal = optimal;
            Min = min;
            Max = max;
        }

        public bool IsValid()
        {
            return !Regex.Match(MeasurableType, @"[^a-zA-Z0-9_ ]+").Success &&
                   Min <= Optimal &&
                   Max >= Optimal &&
                   Min >= 0 &&
                   Max >= 0 &&
                   Optimal >= 0 &&
                   Max <= 100;
        }

        public Guid Id { get; private set; }

        public int Optimal { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        
        public string MeasurableType { get; protected set; }
    }
}

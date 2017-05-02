using System;
using PlantingLib.Plants;

namespace PlantsWpf.ArgsForEvents
{
    public class PlantsAreaEventArgs : EventArgs
    {
        public PlantsAreaEventArgs(PlantsArea plantsArea)
        {
            PlantsArea = plantsArea;
        }

        public PlantsArea PlantsArea { get; private set; }
    }
}
using PlantingLib.Plants;

namespace PlantsWpf.ArgsForEvents
{
    public class PlantsAreaEventArgs : System.EventArgs
    {
        public PlantsAreaEventArgs(PlantsArea plantsArea)
        {
            PlantsArea = plantsArea;
        }

        public PlantsArea PlantsArea { get; private set; }
    }
}
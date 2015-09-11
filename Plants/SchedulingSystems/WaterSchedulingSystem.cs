using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planting.MeasuringsProviding;
using Planting.Plants;

namespace Planting.SchedulingSystems
{
    public class WaterSchedulingSystem : SchedulingSystem
    {
        public WaterSchedulingSystem(ISender<SchedulingMessage> sender, PlantsAreas plantsAreas)
            : base(sender)
        {
        }
    }
}

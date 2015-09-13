using Planting.MeasuringsProviding;
using Planting.Messenging;
using Planting.Plants;
using Planting.PlantsRequirements;

namespace Planting.ServiceProvidingSystems
{
    public class WaterSystem : ServiceProvidingSystem
    {
        public WaterSystem(ISender<MeasuringMessage> sender)
            : base(sender)
        {
        }
    }
}

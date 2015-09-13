using Planting.MeasuringsProviding;
using Planting.Messenging;
using Planting.Plants;

namespace Planting.ServiceProvidingSystems
{
    public class NutrientSystem : ServiceProvidingSystem
    {
        public NutrientSystem(ISender<MeasuringMessage> sender)
            : base(sender)
        {
        }
    };
}
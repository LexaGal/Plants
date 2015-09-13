using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planting.MeasuringsProviding;
using Planting.Messenging;
using Planting.Plants;

namespace Planting.ServiceProvidingSystems
{
    public class TemperatureSystem : ServiceProvidingSystem
    {
        public TemperatureSystem(ISender<MeasuringMessage> sender)
            : base(sender)
        {
        }
    };

}

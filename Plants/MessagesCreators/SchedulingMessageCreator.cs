using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planting.MeasuringsProviding;
using Planting.PlantRequirements;
using Planting.SchedulingSystems;

namespace Planting.MessagesCreators
{
    public class SchedulingMessageCreator
    {
        public IList<MeasuringMessage> MeasuringMessages { get; private set; }
        public string PlantsAreaId { get; private set; }

        public SchedulingMessageCreator(IList<MeasuringMessage> measuringMessages, string plantsAreaId)
        {
            MeasuringMessages = measuringMessages;
            PlantsAreaId = plantsAreaId;
        }

        public SchedulingMessage CreateMessage()
        {
            IDictionary<MeasurableTypesEnum, IEnumerable<int>> measuringsDictionary =
                new Dictionary<MeasurableTypesEnum, IEnumerable<int>>();

            IEnumerable<int> temperatureInts = MeasuringMessages
                .Where(m => m.MeasurableType == MeasurableTypesEnum.Temperature)
                .Select(m => m.ParameterValue);

            IEnumerable<int> soilPhInts = MeasuringMessages
                .Where(m => m.MeasurableType == MeasurableTypesEnum.SoilPh)
                .Select(m => m.ParameterValue);

            IEnumerable<int> nutrientInts = MeasuringMessages
                .Where(m => m.MeasurableType == MeasurableTypesEnum.Nutrient)
                .Select(m => m.ParameterValue);

            IEnumerable<int> humidityInts = MeasuringMessages
                .Where(m => m.MeasurableType == MeasurableTypesEnum.Humidity)
                .Select(m => m.ParameterValue);

            measuringsDictionary.Add(MeasurableTypesEnum.Temperature, temperatureInts);
            measuringsDictionary.Add(MeasurableTypesEnum.SoilPh, soilPhInts);
            measuringsDictionary.Add(MeasurableTypesEnum.Nutrient, nutrientInts);
            measuringsDictionary.Add(MeasurableTypesEnum.Humidity, humidityInts);

            return new SchedulingMessage(DateTime.Now, PlantsAreaId, measuringsDictionary);
        }
    }
}

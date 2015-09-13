using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planting.MeasuringsProviding;
using Planting.Messenging;
using Planting.PlantsRequirements;

namespace Planting.MessagesCreators
{
    public class ServicingMessageCreator
    {
        public MeasuringMessage MeasuringMessage { get; private set; }
        public string PlantsAreaId { get; private set; }

        public ServicingMessageCreator(MeasuringMessage measuringMessage, string plantsAreaId)
        {
            MeasuringMessage = measuringMessage;
            PlantsAreaId = plantsAreaId;
        }

        public ServicingMessage CreateMessage()
        {
            KeyValuePair<MeasurableTypesEnum, double> measuring =
                new KeyValuePair<MeasurableTypesEnum, double>(MeasuringMessage.MeasurableType, );
                

            IEnumerable<double> temperatureInts = MeasuringMessages
                .Where(m => m.MeasurableType == MeasurableTypesEnum.Temperature)
                .Select(m => m.ParameterValue);

            IEnumerable<double> soilPhInts = MeasuringMessages
                .Where(m => m.MeasurableType == MeasurableTypesEnum.SoilPh)
                .Select(m => m.ParameterValue);

            IEnumerable<double> nutrientInts = MeasuringMessages
                .Where(m => m.MeasurableType == MeasurableTypesEnum.Nutrient)
                .Select(m => m.ParameterValue);

            IEnumerable<double> humidityInts = MeasuringMessages
                .Where(m => m.MeasurableType == MeasurableTypesEnum.Humidity)
                .Select(m => m.ParameterValue);

            measuringsDictionary.Add(MeasurableTypesEnum.Temperature, temperatureInts);
            measuringsDictionary.Add(MeasurableTypesEnum.SoilPh, soilPhInts);
            measuringsDictionary.Add(MeasurableTypesEnum.Nutrient, nutrientInts);
            measuringsDictionary.Add(MeasurableTypesEnum.Humidity, humidityInts);

            return new ServicingMessage(DateTime.Now, PlantsAreaId, measuringsDictionary);
        }
    }
}

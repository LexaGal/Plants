using System;
using Planting.MeasuringsProviding;
using Planting.Messenging;
using Planting.PlantsRequirements;

namespace Planting.MessagesCreators
{
    public class MeasuringMessageCreator
    {
        public MeasurableParameter MeasurableParameter { get; set; }
        public string PlantsAreaId { get; private set; }
        public double ParameterValue { get; private set; }

        public MeasuringMessageCreator(MeasurableParameter measurableValue, string plantsAreaId, double measuringValue)
        {
            MeasurableParameter = measurableValue;
            PlantsAreaId = plantsAreaId;
            ParameterValue = measuringValue;
        }

        public MeasuringMessage CreateMessage()
        {
            if (ParameterValue < MeasurableParameter.Min || ParameterValue > MeasurableParameter.Max)
            {
                return new MeasuringMessage(DateTime.Now, MessageTypesEnum.CriticalInfo,
                    MeasurableParameter.Type, PlantsAreaId, ParameterValue);
            }
            return new MeasuringMessage(DateTime.Now, MessageTypesEnum.UsualInfo,
                    MeasurableParameter.Type, PlantsAreaId, ParameterValue);
        }
    }
}

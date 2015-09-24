using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.Messenging;

namespace PlantingLib.MessagesCreators
{
    public class MeasuringMessageCreator
    {
        public MeasurableParameter MeasurableParameter { get; set; }
        public Guid PlantsAreaId { get; private set; }
        public double ParameterValue { get; private set; }

        public MeasuringMessageCreator(MeasurableParameter measurableValue, Guid plantsAreaId, double measuringValue)
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
                    MeasurableParameter.MeasurableType, PlantsAreaId, ParameterValue);
            }
            return new MeasuringMessage(DateTime.Now, MessageTypesEnum.UsualInfo,
                    MeasurableParameter.MeasurableType, PlantsAreaId, ParameterValue);
        }
    }
}

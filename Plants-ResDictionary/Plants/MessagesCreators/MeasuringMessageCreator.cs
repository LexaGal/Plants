using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.Messenging;

namespace PlantingLib.MessagesCreators
{
    public class MeasuringMessageCreator
    {
        public MeasurableParameter MeasurableParameter { get; set; }
        public Guid PlantsAreaId { get; }
        public double ParameterValue { get; }

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
                return new MeasuringMessage(DateTime.Now, MessageTypeEnum.CriticalInfo,
                    MeasurableParameter.MeasurableType, PlantsAreaId, ParameterValue);
            }
            return new MeasuringMessage(DateTime.Now, MessageTypeEnum.UsualInfo,
                    MeasurableParameter.MeasurableType, PlantsAreaId, ParameterValue);
        }
    }
}

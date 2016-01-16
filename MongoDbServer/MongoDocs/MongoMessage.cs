using System;
using PlantingLib.Messenging;

namespace MongoDbServer.MongoDocs
{
    public class MongoMessage
    {
        public string objId { get; set; }
        public string plantsareaId { get; set; }
        public DateTime dateTime { get; set; }
        public string messageType { get; set; }
        public string measurableType { get; set; }
        public double parameterValue { get; set; }

        public MongoMessage(MeasuringMessage measuringMessage)
        {
            objId = measuringMessage.Id.ToString();
            plantsareaId = measuringMessage.PlantsAreaId.ToString();
            dateTime = measuringMessage.DateTime;
            measurableType = measuringMessage.MeasurableType;
            messageType = measuringMessage.MeasurableType;
            parameterValue = measuringMessage.ParameterValue;
        }
    }
}
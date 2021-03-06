using System;
using PlantingLib.Plants;

namespace MongoDbServer.MongoDocs
{
    public class MongoPlantsArea
    {
        public MongoPlantsArea(PlantsArea area)
        {
            objId = area.Id.ToString();
            userId = area.UserId.ToString();
            name = $"{area.Plant.Name} area";
            numberOfSensors = area.Sensors.Count;
            dateTime = DateTime.Now;
        }

        public string objId { get; set; }

        public string userId { get; set; }

        public string name { get; set; }

        public int numberOfSensors { get; set; }

        public DateTime dateTime { get; set; }
    }
}
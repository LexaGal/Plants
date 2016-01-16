using System;
using PlantingLib.Plants;

namespace MongoDbServer.MongoDocs
{
    public class MongoPlantsArea
    {
        public string objId { get; set; }

        public string userId { get; set; }

        public string name { get; set; }

        public int numberOfSensors { get; set; }

        public DateTime dateTime { get; set; }

        public MongoPlantsArea(PlantsArea area)
        {
            objId = area.Id.ToString();
            userId = area.UserId.ToString();
            name = $"{area.Plant.Name} area: {area.Id}";
            numberOfSensors = area.Sensors.Count;
            dateTime = DateTime.Now;
        }
    }
}
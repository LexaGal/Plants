using Planting.Plants;

namespace Planting.ServiceProvidingSystems
{
    public class Schedule
    {
        public int Timeout { get; private set; }
        public int ActionDuration { get; private set; }
        public string PlantsAreaId { get; private set; }
        public Plant PlantRequirements { get; private set; }       

        public Schedule(string plantsAreaId, int timeout, int actionDuration, Plant plantRequirements)
        {
            Timeout = timeout;
            ActionDuration = actionDuration;
            PlantRequirements = plantRequirements;
            PlantsAreaId = plantsAreaId;
        }
    }
}

using Database.DatabaseStructure.Repository.Abstract;
using PlantingLib.MappingTypes;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class PlantsAreaRepository : Repository<PlantsAreaMapping>, IPlantsAreaRepository
    {
        public override bool Edit(int id, PlantsAreaMapping value)
        {
            throw new System.NotImplementedException();
        }
    }
}
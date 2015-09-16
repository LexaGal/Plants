using Database.DatabaseStructure.Repository.Abstract;
using PlantingLib.MappingTypes;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class PlantRepository : Repository<PlantMapping>, IPlantRepository
    {
        public override bool Edit(int id, PlantMapping value)
        {
            throw new System.NotImplementedException();
        }
    }
}
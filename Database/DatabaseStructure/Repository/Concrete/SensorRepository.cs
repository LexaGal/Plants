using Database.DatabaseStructure.Repository.Abstract;
using PlantingLib.MappingTypes;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class SensorRepository : Repository<SensorMapping>, ISensorRepository
    {
        public override bool Edit(int id, SensorMapping value)
        {
            throw new System.NotImplementedException();
        }
    }
}
using Database.DatabaseStructure.Repository.Abstract;
using PlantingLib.MappingTypes;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class MeasurableParameterRepository : Repository<MeasurableParameterMapping>, IMeasurableParameterRepository
    {
        public override bool Edit(int id, MeasurableParameterMapping value)
        {
            throw new System.NotImplementedException();
        }
    }
}
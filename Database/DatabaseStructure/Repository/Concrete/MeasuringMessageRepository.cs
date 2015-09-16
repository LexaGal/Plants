using Database.DatabaseStructure.Repository.Abstract;
using PlantingLib.MappingTypes;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class MeasuringMessageRepository : Repository<MeasuringMessageMapping>, IMeasuringMessageRepository
    {
        public override bool Edit(int id, MeasuringMessageMapping value)
        {
            throw new System.NotImplementedException();
        }
    }
}
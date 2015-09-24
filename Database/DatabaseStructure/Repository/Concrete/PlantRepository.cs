using System;
using Database.DatabaseStructure.Repository.Abstract;
using PlantMapping = Database.MappingTypes.PlantMapping;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class PlantRepository : Repository<PlantMapping>, IPlantRepository
    {
        public override bool Edit(Guid id, PlantMapping value)
        {
            throw new System.NotImplementedException();
        }
    }
}
using System;
using Database.DatabaseStructure.Repository.Abstract;
using PlantMapping = Database.MappingTypes.PlantMapping;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class PlantMappingRepository : Repository<PlantMapping>, IPlantMappingRepository
    {
        public override bool Edit(Guid id, PlantMapping value)
        {
            throw new System.NotImplementedException();
        }
    }
}
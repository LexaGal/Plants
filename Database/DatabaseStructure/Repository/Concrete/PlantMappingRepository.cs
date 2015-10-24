using System;
using Database.DatabaseStructure.Repository.Abstract;
using PlantMapping = Database.MappingTypes.PlantMapping;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class PlantMappingRepository : Repository<PlantMapping>, IPlantMappingRepository
    {
        public override bool Edit(Guid id, PlantMapping value)
        {
            PlantMapping plantMapping = Context.PlantsSet.Find(id);
            if (plantMapping == null)
            {
                throw new ArgumentNullException("plantMapping");
            }
            value.CopyTo(plantMapping);
            Context.SaveChanges();
            return true;
        }
    }
}
using System;
using Database.DatabaseStructure.Repository.Abstract;
using PlantMapping = Database.MappingTypes.PlantMapping;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class PlantMappingRepository : Repository<PlantMapping>, IPlantMappingRepository
    {
        public override bool Edit(Guid id, PlantMapping value)
        {
            using (Context = new PlantingDb())
            {
                PlantMapping plantMapping = Context.PlantsSet.Find(id);
                if (plantMapping == null)
                {
                    throw new ArgumentNullException("id", "Can't find plantMapping with such id");
                }
                value.CopyTo(plantMapping);
                Context.SaveChanges();
                return true;
            }
        }
    }
}
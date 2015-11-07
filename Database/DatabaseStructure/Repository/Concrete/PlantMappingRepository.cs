using System;
using Database.DatabaseStructure.Repository.Abstract;
using PlantMapping = Database.MappingTypes.PlantMapping;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class PlantMappingRepository : Repository<PlantMapping>, IPlantMappingRepository
    {
        public override bool Edit(PlantMapping value)
        {
            using (Context = new PlantingDb())
            {
                PlantMapping plantMapping = Context.PlantsSet.Find(value.Id);
                if (plantMapping == null)
                {
                    throw new ArgumentNullException("value", "Can't find plantMapping with such id");
                }
                value.CopyTo(plantMapping);
                Context.SaveChanges();
                return true;
            }
        }
    }
}
using System;
using Database.DatabaseStructure.Repository.Abstract;
using SensorMapping = Database.MappingTypes.SensorMapping;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class SensorMappingRepository : Repository<SensorMapping>, ISensorMappingRepository
    {
        public override bool Edit(SensorMapping value)
        {
            using (Context = new PlantingDb())
            {
                SensorMapping sensorMapping = Context.SensorsSet.Find(value.Id);
                if (sensorMapping == null)
                {
                    throw new ArgumentNullException("value", "Can't find sensorMapping  with such id");
                }
                value.CopyTo(sensorMapping);
                Context.SaveChanges();
                return true;
            }
     
        }
    }
}
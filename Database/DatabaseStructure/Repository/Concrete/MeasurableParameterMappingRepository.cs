using System;
using Database.DatabaseStructure.Repository.Abstract;
using MeasurableParameterMapping = Database.MappingTypes.MeasurableParameterMapping;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class MeasurableParameterMappingRepository : Repository<MeasurableParameterMapping>, IMeasurableParameterMappingRepository
    {
        public override bool Edit(MeasurableParameterMapping value)
        {
            using (Context = new PlantingDb())
            {
                MeasurableParameterMapping measurableParameterMapping = Context.MeasurableParametersSet.Find(value.Id);
                if (measurableParameterMapping == null)
                {
                    throw new ArgumentNullException("value", "Can't find measurableParameterMapping  with such id");
                }
                value.CopyTo(measurableParameterMapping);
                Context.SaveChanges();
                return true;
            }
        }
    }
}
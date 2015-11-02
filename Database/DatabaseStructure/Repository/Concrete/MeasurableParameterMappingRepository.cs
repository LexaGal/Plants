using System;
using Database.DatabaseStructure.Repository.Abstract;
using MeasurableParameterMapping = Database.MappingTypes.MeasurableParameterMapping;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class MeasurableParameterMappingRepository : Repository<MeasurableParameterMapping>, IMeasurableParameterMappingRepository
    {
        public override bool Edit(MeasurableParameterMapping value)
        {
            throw new System.NotImplementedException();
        }
    }
}
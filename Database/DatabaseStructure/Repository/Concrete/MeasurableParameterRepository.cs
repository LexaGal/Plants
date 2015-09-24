using System;
using Database.DatabaseStructure.Repository.Abstract;
using MeasurableParameterMapping = Database.MappingTypes.MeasurableParameterMapping;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class MeasurableParameterRepository : Repository<MeasurableParameterMapping>, IMeasurableParameterRepository
    {
        public override bool Edit(Guid id, MeasurableParameterMapping value)
        {
            throw new System.NotImplementedException();
        }
    }
}
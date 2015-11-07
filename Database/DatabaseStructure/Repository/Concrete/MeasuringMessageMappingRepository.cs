using System;
using Database.DatabaseStructure.Repository.Abstract;
using MeasuringMessageMapping = Database.MappingTypes.MeasuringMessageMapping;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class MeasuringMessageMappingRepository : Repository<MeasuringMessageMapping>, IMeasuringMessageMappingRepository
    {
        public override bool Edit(MeasuringMessageMapping value)
        {
            throw new NotImplementedException();
        }
    }
}
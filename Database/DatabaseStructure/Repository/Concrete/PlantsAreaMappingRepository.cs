using System;
using Database.DatabaseStructure.Repository.Abstract;
using Database.MappingTypes;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class PlantsAreaMappingRepository : Repository<PlantsAreaMapping>, IPlantsAreaMappingRepository
    {
        public override bool Edit(PlantsAreaMapping value)
        {
            throw new NotImplementedException();
        }
    }
}
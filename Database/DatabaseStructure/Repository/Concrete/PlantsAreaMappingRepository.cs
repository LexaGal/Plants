using System;
using Database.DatabaseStructure.Repository.Abstract;
using PlantsAreaMapping = Database.MappingTypes.PlantsAreaMapping;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class PlantsAreaMappingRepository : Repository<PlantsAreaMapping>, IPlantsAreaMappingRepository
    {
        public override bool Edit(PlantsAreaMapping value)
        {
            throw new System.NotImplementedException();
        }
    }
}
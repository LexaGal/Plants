using System;
using Database.DatabaseStructure.Repository.Abstract;
using PlantsAreaMapping = Database.MappingTypes.PlantsAreaMapping;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class PlantsAreaRepository : Repository<PlantsAreaMapping>, IPlantsAreaRepository
    {
        public override bool Edit(Guid id, PlantsAreaMapping value)
        {
            throw new System.NotImplementedException();
        }
    }
}
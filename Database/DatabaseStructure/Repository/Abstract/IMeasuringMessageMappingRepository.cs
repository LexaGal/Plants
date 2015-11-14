using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MeasuringMessageMapping = Database.MappingTypes.MeasuringMessageMapping;

namespace Database.DatabaseStructure.Repository.Abstract
{
    public interface IMeasuringMessageMappingRepository : IRepository<MeasuringMessageMapping>
    {
        Task SaveAsync(List<MeasuringMessageMapping> measuringMessageMappings);
        Task<List<MeasuringMessageMapping>> GetAllAsync(Expression<Func<MeasuringMessageMapping, bool>> func = null);
    }
}
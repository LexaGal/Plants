using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public async Task SaveAsync(List<MeasuringMessageMapping> measuringMessageMappings)
        {
            try
            {
                using (Context = new PlantingDb())
                {
                    Context.MeasuringMessagesSet.AddRange(measuringMessageMappings);
                    await Context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }

        public async Task<List<MeasuringMessageMapping>> GetAllAsync(
            Expression<Func<MeasuringMessageMapping, bool>> func = null)
        {
            try
            {
                using (Context = new PlantingDb())
                {
                    if (func != null)
                    {
                        return
                            await
                                Context.MeasuringMessagesSet
                                    .OrderByDescending(mapping => mapping.DateTime)
                                    .Where(func)
                                    .AsQueryable()
                                    .ToListAsync()
                                    .ConfigureAwait(false);
                    }
                    return await Context.MeasuringMessagesSet.ToListAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                return null;
            }
        }

    }
}
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

        public async Task<int> SaveAsync(List<MeasuringMessageMapping> measuringMessageMappings)
        {
            try
            {
                using (Context = new PlantingDb())
                {
                    Context.MeasuringMessagesSet.AddRange(measuringMessageMappings);
                    return await Context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                return 0;
            }
        }
        
        public int DeleteMany(int n = 1000, Func<MeasuringMessageMapping, bool> func = null)
        { 
            try
            {
                using (Context = new PlantingDb())
                {
                    if (func != null)
                    {
                        foreach (MeasuringMessageMapping measuringMessageMapping in Context.MeasuringMessagesSet
                            .OrderBy(mapping => mapping.DateTime)
                            .Where(func)
                            .Take(Math.Min(n, Context.MeasuringMessagesSet.Count()/3)))
                        {
                            Context.MeasuringMessagesSet.Remove(measuringMessageMapping);
                        }
                        return Context.SaveChanges(); 
                    }

                    foreach (MeasuringMessageMapping measuringMessageMapping in Context.MeasuringMessagesSet
                        .OrderBy(mapping => mapping.DateTime)
                        .Take(Math.Min(n, Context.MeasuringMessagesSet.Count()/3)))
                    {

                         Context.MeasuringMessagesSet.Remove(measuringMessageMapping);
                    }
                    return Context.SaveChanges(); 
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                return 0;
            }
        }
    }
}
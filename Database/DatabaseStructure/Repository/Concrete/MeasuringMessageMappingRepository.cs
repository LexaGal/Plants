using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using Database.DatabaseStructure.Repository.Abstract;
using Database.MappingTypes;
using NLog;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class MeasuringMessageMappingRepository : Repository<MeasuringMessageMapping>,
        IMeasuringMessageMappingRepository
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override bool Edit(MeasuringMessageMapping value)
        {
            throw new NotImplementedException();
        }

        public int SaveMany(List<MeasuringMessageMapping> measuringMessageMappings)
        {
            try
            {
                using (Context = new PlantingDb())
                {
                    Context.MeasuringMessagesSet.AddRange(measuringMessageMappings);
                    return Context.SaveChanges(); //Async().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return 0;
            }
        }

        public int DeleteMany(int n = 1000, Expression<Func<MeasuringMessageMapping, bool>> func = null)
        {
            using (Context = new PlantingDb())
            {
                IQueryable<MeasuringMessageMapping> measuringMessageMappings;
                if (func != null)
                {
                    measuringMessageMappings = Context.MeasuringMessagesSet
                        .OrderBy(mapping => mapping.DateTime)
                        .Where(func)
                        .Take(Math.Min(n, Context.MeasuringMessagesSet.Count()/3));

                    Context.MeasuringMessagesSet.RemoveRange(measuringMessageMappings);

                    return Context.SaveChanges();
                }

                measuringMessageMappings = Context.MeasuringMessagesSet
                    .OrderBy(mapping => mapping.DateTime)
                    .Take(Math.Min(n, Context.MeasuringMessagesSet.Count()/3));

                Context.MeasuringMessagesSet.RemoveRange(measuringMessageMappings);

                try
                {
                    //return
                    Context.SaveChanges();
                    MessageBox.Show("Good!");
                    return 0;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return 0;
                }
            }
        }
    }
}
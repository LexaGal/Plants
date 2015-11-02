using System;
using System.Linq;
using Database.DatabaseStructure.Repository.Abstract;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public abstract class Repository<T> : IRepository<T> where T: class
    {
        protected PlantingDb Context = new PlantingDb();
        //protected string ConnectionString = ConfigurationManager.ConnectionStrings["PlantingDb"].ConnectionString;

        public virtual IQueryable<T> GetAll()
        {
            //! do not using (Context = new PlantingDb())
            return Context.Set<T>().AsQueryable();
        }

        public virtual T Get(Guid id)
        {
            //! do not using (Context = new PlantingDb())
            return Context.Set<T>().Find(id);
        }

        public virtual bool Add(T value)
        {
            using (Context = new PlantingDb())
            {
                Context.Set<T>().Add(value);
                Context.SaveChanges();
                return true;
            }
        }

        public abstract bool Edit(T value);
       
        public virtual bool Delete(Guid id)
        {
            using (Context = new PlantingDb())
            {
                T t = Context.Set<T>().Find(id);
                if (t != null)
                {
                    Context.Set<T>().Remove(t);
                    Context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public virtual void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }
    }
}
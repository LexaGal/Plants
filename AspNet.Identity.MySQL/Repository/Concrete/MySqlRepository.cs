using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AspNet.Identity.MySQL.Database;

namespace AspNet.Identity.MySQL.Repository.Concrete
{
    public abstract class MySqlRepository<T> : IMySqlRepository<T> where T : class
    {
        protected MySQLDatabase Database = new MySQLDatabase();

        public abstract List<T> GetAll(Expression<Func<T, bool>> func = null);

        public abstract T Get(Guid id);

        public abstract bool Save(T value, Guid id);

        public abstract bool Edit(T value);

        public abstract bool Delete(Guid id);

        public virtual void Dispose()
        {
            if (Database != null)
                Database.Dispose();
        }

        protected virtual T CreateMapping(Dictionary<string, string> row)
        {
            throw new NotImplementedException();
        }
    }
}
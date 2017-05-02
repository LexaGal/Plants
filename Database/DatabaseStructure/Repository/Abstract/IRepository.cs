using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Database.DatabaseStructure.Repository.Abstract
{
    public interface IRepository<T> : IDisposable where T : class
    {
        List<T> GetAll(Expression<Func<T, bool>> func = null);
        T Get(Guid id);
        bool Save(T value, Guid id);
        bool Edit(T value);
        bool Delete(Guid id);
    }
}
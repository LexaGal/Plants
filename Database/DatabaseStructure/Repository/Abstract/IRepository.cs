using System;
using System.Linq;

namespace Database.DatabaseStructure.Repository.Abstract
{
    public interface IRepository<T> : IDisposable where T: class
    {
        IQueryable<T> GetAll();
        T Get(Guid id);
        bool Add(T value);
        bool Edit(Guid id, T value);
        bool Delete(Guid id);
    }
}
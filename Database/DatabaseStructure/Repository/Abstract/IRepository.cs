using System;
using System.Linq;

namespace Database.DatabaseStructure.Repository.Abstract
{
    public interface IRepository<T> : IDisposable where T: class
    {
        IQueryable<T> GetAll();
        T Get(int id);
        bool Add(T value);
        bool Edit(int id, T value);
        bool Delete(int id);
    }
}
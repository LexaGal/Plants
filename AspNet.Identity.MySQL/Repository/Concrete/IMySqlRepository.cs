using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Database.DatabaseStructure.Repository.Abstract;

namespace AspNet.Identity.MySQL.Repository.Concrete
{
    public interface IMySqlRepository<T> : IRepository<T> where T : class { }
}
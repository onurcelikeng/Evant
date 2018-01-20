using System;
using System.Collections.Generic;

namespace Evant.DAL.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T Get(object id);

        bool Insert(T entity);

        bool Update(T entity);

        List<T> Where(Func<T, bool> where);

        T First(Func<T, bool> where);

        bool Delete(T entity);
    }
}
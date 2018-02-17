using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Evant.DAL.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> Table { get; }
        DbContext Context { get; }
        Task<List<T>> Where(Expression<Func<T, bool>> where);
        Task<T> First(Expression<Func<T, bool>> first = null);

        Task<List<T>> All();
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Save();
    }
}

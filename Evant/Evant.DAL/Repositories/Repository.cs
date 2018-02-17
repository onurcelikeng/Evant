using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Evant.DAL.EF;
using Evant.DAL.Interfaces.Repositories;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Evant.DAL.Repositories
{
    public class Repository<T> : IDisposable, IRepository<T> where T : class
    {
        private DataContext dbContext;
        private DbSet<T> table;


        public Repository(DataContext dbContext)
        {
            this.dbContext = dbContext;
            this.table = this.dbContext.Set<T>();
        }

        ~Repository()
        {
            Dispose();
        }


        public DbContext Context { get => dbContext; }
        public DbSet<T> Table { get => table; }


        public async Task<List<T>> All()
        {
            return await table.ToListAsync();
        }

        public async Task<List<T>> Where(Expression<Func<T, bool>> where)
        {
            return await Table.Where(where).ToListAsync();
        }

        public async Task<T> First(Expression<Func<T, bool>> first = null)
        {
            return await Table.FirstOrDefaultAsync(first);
        }

        public async Task<bool> Add(T entity)
        {
            table.Add(entity);
            return await Save();
        }

        public async Task<bool> Update(T entity)
        {
            return await Save();
        }

        public async Task<bool> Delete(T entity)
        {
            table.Remove(entity);
            return await Save();
        }

        public async Task<bool> Save()
        {
            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }

            catch
            {
                // that must be added log helper.
                return false;
            }
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }
    }
}

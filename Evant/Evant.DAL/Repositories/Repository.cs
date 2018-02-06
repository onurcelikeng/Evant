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
        private DataContext context;
        private DbSet<T> table;
        public DbContext Context { get => context; }
        public DbSet<T> Table { get => table; }


        public Repository(DataContext context)
        {
            this.context = context;
            this.table = context.Set<T>();
        }

        ~Repository()
        {
            Dispose();
        }


        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T Get(object id)
        {
            return table.Find(id);
        }

        public List<T> Where(Func<T, bool> where)
        {
            return table.Where(where).ToList();
        }

        public bool Insert(T entity)
        {
            table.Add(entity);
            return Save();
        }

        public bool Update(T entity)
        {
            return Save();
        }

        public bool Delete(T entity)
        {
            table.Remove(entity);
            return Save();
        }

        public T First(Func<T, bool> where)
        {
            return table.FirstOrDefault(where);
        }

        private bool Save()
        {
            try
            {
                context.SaveChanges();
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
            this.context.Dispose();
        }
    }
}
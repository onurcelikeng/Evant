using Evant.DAL.EF;
using Evant.DAL.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evant.DAL.Repositories
{
    public class Repository<T> : IDisposable, IRepository<T> where T : class
    {
        private DataContext context;
        private DbSet<T> Table;


        public Repository(DataContext context)
        {
            this.context = context;
            Table = context.Set<T>();
        }

        ~Repository()
        {
            Dispose();
        }


        public IEnumerable<T> GetAll()
        {
            return Table.ToList();
        }

        public T Get(object id)
        {
            return Table.Find(id);
        }

        public List<T> Where(Func<T, bool> where)
        {
            return Table.Where(where).ToList();
        }

        public bool Insert(T entity)
        {
            Table.Add(entity);
            return Save();
        }

        public bool Update(T entity)
        {
            return Save();
        }

        public bool Delete(T entity)
        {
            Table.Remove(entity);
            return Save();
        }

        public T First(Func<T, bool> where)
        {
            return Table.FirstOrDefault(where);
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
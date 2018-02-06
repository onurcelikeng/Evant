using Evant.DAL.EF;
using Evant.DAL.EF.Tables;
using Evant.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evant.DAL.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {

        public CategoryRepository(DataContext dbContext) : base(dbContext)
        {

        }


        public async Task<List<Category>> List()
        {
            return await Table.ToListAsync();
        }

        public async Task<Guid> Add(Category entity)
        {
            try
            {
                await Table.AddAsync(entity);
                await Context.SaveChangesAsync();
                return entity.Id;
            }
            catch
            {
                return Guid.Empty;
            }
        }

    }
}

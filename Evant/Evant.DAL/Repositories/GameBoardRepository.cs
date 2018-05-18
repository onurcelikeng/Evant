using Evant.DAL.EF;
using Evant.DAL.EF.Tables;
using Evant.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evant.DAL.Repositories
{
    public class GameBoardRepository : Repository<GameBoard>, IGameBoardRepository
    {
        public GameBoardRepository(DataContext dbContext) : base(dbContext)
        {

        }


        public async Task<List<GameBoard>> GameBoards()
        {
            return await Table
                .Include(t => t.User)
                .Where(t => t.User.IsActive)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();
        }


    }
}

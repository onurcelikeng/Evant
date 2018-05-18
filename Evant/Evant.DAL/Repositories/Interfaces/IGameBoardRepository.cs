using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evant.DAL.Repositories.Interfaces
{
    public interface IGameBoardRepository : IRepository<GameBoard>
    {
        Task<List<GameBoard>> GameBoards();
    }
}

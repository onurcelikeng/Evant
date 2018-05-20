using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Evant.Constants.GameConstant;

namespace Evant.Helpers
{
    public sealed class GameHelper : IGameHelper
    {
        private readonly IRepository<GameBoard> _gameBoardRepo;
        private readonly ILogHelper _logHelper;


        public GameHelper(IRepository<GameBoard> gameBoardRepo,
            ILogHelper logHelper)
        {
            _gameBoardRepo = gameBoardRepo;
            _logHelper = logHelper;
        }


        public async Task Point(Guid userId, GameType type)
        {
            try
            {
                var response = await _gameBoardRepo.Add(new GameBoard()
                {
                    Id = new Guid(),
                    OperationType = type.ToString(),
                    Point = (int)type,
                    UserId = userId
                });
            }
            catch (Exception ex)
            {
                _logHelper.Log("GameHelper", 500, "Point", ex.Message);
            }
        }

    }
}

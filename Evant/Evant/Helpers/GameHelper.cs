using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evant.Helpers
{
    public sealed class GameHelper
    {
        private readonly IRepository<GameBoard> _gameBoardRepo;
        private readonly ILogHelper _logHelper;


        public GameHelper(IRepository<GameBoard> gameBoardRepo,
            ILogHelper logHelper)
        {
            _gameBoardRepo = gameBoardRepo;
            _logHelper = logHelper;
        }



    }
}

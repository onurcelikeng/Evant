using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.GameBoard;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.DAL.Repositories.Interfaces;
using Evant.Helpers;
using Evant.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Route("api/gameboard")]
    public class GameBoardController : BaseController
    {
        private readonly IGameBoardRepository _gameBoard;
        private readonly ILogHelper _logHelper;


        public GameBoardController(IGameBoardRepository gameBoard,
            ILogHelper logHelper)
        {
            _gameBoard = gameBoard;
            _logHelper = logHelper;
        }


        [HttpGet]
        [Route("{type}")]
        public async Task<IActionResult> GameBorads([FromRoute] string type)
        {
            try
            {
                var boards = (await _gameBoard.GameBoards());
                if (boards.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                var users = new List<List<GameBoard>>();
                if (type == "0")
                {
                    users = boards.Where(t => t.CreatedAt.ToShortDateString() == DateTime.UtcNow.ToShortDateString())
                       .GroupBy(u => u.UserId)
                       .Select(g => g.ToList()).ToList();
                }
                else if (type == "1")
                {
                    var week = DateTimeExtensions.Week();
                    users = boards.Where(t => week.Item1 <= t.CreatedAt && t.CreatedAt <= week.Item2)
                       .GroupBy(u => u.UserId)
                       .Select(g => g.ToList()).ToList();
                }
                else if (type == "2")
                {
                    var month = DateTimeExtensions.Month();
                    users = boards.Where(t => month.Item1 <= t.CreatedAt && t.CreatedAt <= month.Item2)
                       .GroupBy(u => u.UserId)
                       .Select(g => g.ToList()).ToList();
                }

                var list = new List<GameBoardDTO>();
                foreach (var user in users)
                {
                    var model = new GameBoardDTO()
                    {
                        UserId = user[0].UserId,
                        FirstName = user[0].User.FirstName,
                        LastName = user[0].User.LastName,
                        PhotoUrl = user[0].User.Photo
                    };

                    for (int i = 0; i < user.Count; i++)
                    {
                        model.Score += user[i].Point;
                    }

                    list.Add(model);
                }

                return Ok(list.OrderByDescending(o => o.Score));
            }
            catch (Exception ex)
            {
                _logHelper.Log("GameBoardController", 500, "GameBorads", ex.Message);
                return null;
            }
        }

    }
}

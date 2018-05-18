﻿using System;
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
    [Produces("application/json")]
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
        public async Task<IActionResult> GameBorads()
        {
            try
            {
                var boards = (await _gameBoard.GameBoards());
                if (boards.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                var users = boards.GroupBy(u => u.UserId).Select(g => g.ToList()).ToList();

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

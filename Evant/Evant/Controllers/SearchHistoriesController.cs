﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.SearchHistory;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Evant.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/histories")]
    public class SearchHistoriesController : BaseController
    {
        private readonly IRepository<SearchHistory> _searchHistoryRepo;
        private readonly ILogHelper _logHelper;


        public SearchHistoriesController(IRepository<SearchHistory> searchHistoryRepo,
            ILogHelper logHelper)
        {
            _searchHistoryRepo = searchHistoryRepo;
            _logHelper = logHelper;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Histories()
        {
            try
            {
                var histories = (await _searchHistoryRepo.Where(s => s.UserId == User.GetUserId()))
                    .OrderByDescending(s => s.CreatedAt)
                    .Select(h => new SearchHistoryDTO()
                    {
                        HistoryId = h.Id,
                        Keyword = h.Keyword
                    }).ToList();
                if (histories.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(histories);
            }
            catch (Exception ex)
            {
                _logHelper.Log("SearchHistoriesController", 500, "Histories", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPost("{keyword}")]
        public async Task<IActionResult> AddHistory([FromRoute] string keyword)
        {
            try
            {
                Guid userId = User.GetUserId();

                var searchHistory = await _searchHistoryRepo.First(s => s.Keyword.ToLower() == keyword.ToLower() && s.UserId == userId);
                if (searchHistory == null)
                {
                    var entity = new SearchHistory()
                    {
                        UserId = userId,
                        Keyword = keyword
                    };
                    var response = await _searchHistoryRepo.Add(entity);
                    if (response)
                        return Ok("Arama kaydı eklendi.");
                    else
                        return BadRequest("Arama kaydı eklenemedi.");
                }
                else
                {
                    searchHistory.SearchCount += 1;
                    var response = await _searchHistoryRepo.Update(searchHistory);
                    if (response)
                        return Ok("Arama kaydı güncellendi.");
                    else
                        return BadRequest("Arama kaydı güncellenemedi.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("SearchHistoriesController", 500, "GetHistories", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpDelete("{historyId}")]
        public async Task<IActionResult> DeleteHistory(Guid historyId)
        {
            try
            {
                Guid userId = User.GetUserId();

                var history = await _searchHistoryRepo.First(s => s.Id == historyId && s.UserId == userId);
                if (history == null)
                    return NotFound("Kayıt bulunamadı.");

                var response = await _searchHistoryRepo.Delete(history);
                if (response)
                    return Ok("Arama kaydı silindi.");
                else
                    return BadRequest("Arama kaydı silinemedi.");
            }
            catch (Exception ex)
            {
                _logHelper.Log("SearchHistoriesController", 500, "DeleteSaerch", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteHistories()
        {
            try
            {
                Guid userId = User.GetUserId();

                var histories = await _searchHistoryRepo.Where(s => s.UserId == userId);
                if (histories.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                else
                {
                    foreach (var history in histories)
                    {
                        await _searchHistoryRepo.Delete(history);
                    }

                    return Ok("Arama geçmişi silindi.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("SearchHistoriesController", 500, "DeleteHistories", ex.Message);
                return null;
            }
        }

    }
}

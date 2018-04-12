using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evant.Helpers
{
    public sealed class SearchHelper : ISearchHelper
    {
        private readonly IRepository<SearchHistory> _searchHistoryRepo;


        public SearchHelper(IRepository<SearchHistory> searchHistoryRepo)
        {
            _searchHistoryRepo = searchHistoryRepo;
        }


        public async Task<bool> Add(Guid userId, string keyword)
        {
            var searchHistory = await _searchHistoryRepo.First(s => s.Keyword == keyword);
            if(searchHistory == null)
            {
                var entity = new SearchHistory()
                {
                    Id = new Guid(),
                    UserId = userId,
                    Keyword = keyword,
                    SearchCount = 1
                };
                return await _searchHistoryRepo.Add(entity);
            }
            else
            {
                searchHistory.SearchCount += 1;
                var response = await _searchHistoryRepo.Update(searchHistory);
            }
            
            return false;
        }
    }
}

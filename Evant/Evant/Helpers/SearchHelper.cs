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
            var response = await _searchHistoryRepo.First(s => s.Keyword == keyword);
            if(response == null)
            {
                var entity = new SearchHistory()
                {
                    Id = new Guid(),
                    UserId = userId,
                    Keyword = keyword
                };
                return await _searchHistoryRepo.Add(entity);
            }

            return false;
        }
    }
}

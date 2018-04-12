using System;

namespace Evant.Contracts.DataTransferObjects.SearchHistory
{
    public sealed class SearchHistoryDTO
    {
        public Guid HistoryId { get; set; }

        public string Keyword { get; set; }
    }
}

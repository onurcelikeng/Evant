using System;

namespace Evant.DAL.EF.Tables
{
    public class SearchHistory : BaseEntity
    {
        public Guid UserId { get; set; }

        public string Keyword { get; set; }


        // Foreign keys
        public virtual User User { get; set; }
    }
}
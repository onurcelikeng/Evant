using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("SearchHistories")]
    public class SearchHistory : BaseEntity
    {
        public Guid UserId { get; set; }

        [Required]
        public string Keyword { get; set; }


        public virtual User User { get; set; }
    }
}

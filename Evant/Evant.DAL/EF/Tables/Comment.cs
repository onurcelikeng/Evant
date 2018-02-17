using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("Comments")]
    public class Comment : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid EventId { get; set; }

        [Required, MaxLength(140)]
        public string Content { get; set; }


        // Foreign keys
        public virtual User User { get; set; }

        public virtual Event Event { get; set; }
    }
}

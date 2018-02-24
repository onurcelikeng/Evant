using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("GameBoards")]
    public class GameBoard : BaseEntity
    {
        public Guid UserId { get; set; }

        [Required]
        public int Point { get; set; } = 0;

        [Required]
        public string OperationType { get; set; }


        public virtual User User { get; set; }
    }
}

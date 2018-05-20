using System;
using System.ComponentModel.DataAnnotations;

namespace Evant.DAL.EF.Tables
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdateAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false; //soft delete
    }
}

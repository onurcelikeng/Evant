using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evant.DAL.EF.Tables
{
    [Table("Categories")]
    public class Category : BaseEntity
    {
        public Category()
        {
            Events = new List<Event>();
        }


        [Required, MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public string Icon { get; set; }


        public virtual ICollection<Event> Events { get; set; }
    }
}

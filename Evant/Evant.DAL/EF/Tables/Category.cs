using System.Collections.Generic;

namespace Evant.DAL.EF.Tables
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Events = new List<Event>();
        }


        public string Name { get; set; }

        public string Icon { get; set; }


        // Reverse navigation
        public virtual ICollection<Event> Events { get; set; }
    }
}
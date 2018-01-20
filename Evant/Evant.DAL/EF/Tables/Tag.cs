using System.Collections.Generic;

namespace Evant.DAL.EF.Tables
{
    public class Tag : BaseEntity
    {
        public Tag()
        {
            EventTags = new List<EventTag>();
        }


        public string Name { get; set; }


        // Reverse navigation
        public virtual ICollection<EventTag> EventTags { get; set; }
    }
}
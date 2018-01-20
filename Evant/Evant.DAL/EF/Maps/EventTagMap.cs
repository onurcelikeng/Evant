using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class EventTagMap
    {
        public EventTagMap(EntityTypeBuilder<EventTag> entityBuilder)
        {
            entityBuilder.ToTable("EventTags");

            entityBuilder.Property(x => x.TagId).HasColumnName(@"TagId").IsRequired();
            entityBuilder.Property(x => x.EventId).HasColumnName(@"EventId").IsRequired();

            entityBuilder.HasOne(a => a.Tag).WithMany(b => b.EventTags).HasForeignKey(c => c.TagId).OnDelete(DeleteBehavior.Restrict);
            entityBuilder.HasOne(a => a.Event).WithMany(b => b.EventTags).HasForeignKey(c => c.EventId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
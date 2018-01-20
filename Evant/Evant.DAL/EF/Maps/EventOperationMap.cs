using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class EventOperationMap
    {
        public EventOperationMap(EntityTypeBuilder<EventOperation> entityBuilder)
        {
            entityBuilder.ToTable("EventOperations");

            entityBuilder.Property(x => x.UserId).HasColumnName(@"UserId").IsRequired();
            entityBuilder.Property(x => x.EventId).HasColumnName(@"EventId").IsRequired();

            entityBuilder.HasOne(a => a.User).WithMany(b => b.EventOperations).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            entityBuilder.HasOne(a => a.Event).WithMany(b => b.EventOperations).HasForeignKey(c => c.EventId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
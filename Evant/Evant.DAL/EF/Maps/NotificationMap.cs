using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class NotificationMap
    {
        public NotificationMap(EntityTypeBuilder<Notification> entityBuilder)
        {
            entityBuilder.ToTable("Notifications");

            entityBuilder.Property(x => x.UserId).HasColumnName(@"UserId").IsRequired();
            entityBuilder.Property(x => x.SpecialId).HasColumnName(@"SpecialId").IsRequired();
            entityBuilder.Property(x => x.NotificationType).HasColumnName(@"NotificationType").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.Content).HasColumnName(@"Content").IsRequired().HasColumnType("nvarchar(80)");
            entityBuilder.Property(x => x.IsRead).HasColumnName(@"IsRead").IsRequired().HasColumnType("bit");

            entityBuilder.HasOne(a => a.User).WithMany(b => b.Notifications).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
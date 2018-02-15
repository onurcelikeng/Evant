using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class EventMap
    {
        public EventMap(EntityTypeBuilder<Event> entityBuilder)
        {
            entityBuilder.ToTable("Events");

            entityBuilder.Property(x => x.UserId).HasColumnName(@"UserId").IsRequired();
            entityBuilder.Property(x => x.CategoryId).HasColumnName(@"CategoryId").IsRequired();
            entityBuilder.Property(x => x.Title).HasColumnName(@"Title").IsRequired().HasColumnType("nvarchar(40)");
            entityBuilder.Property(x => x.Description).HasColumnName(@"Description").IsRequired().HasColumnType("nvarchar(80)");
            entityBuilder.Property(x => x.StartDate).HasColumnName(@"StartDate").IsRequired().HasColumnType("datetime");
            entityBuilder.Property(x => x.FinishDate).HasColumnName(@"FinishDate").IsRequired().HasColumnType("datetime");
            entityBuilder.Property(x => x.IsPrivate).HasColumnName(@"IsPrivate").IsRequired().HasColumnType("bit");
            entityBuilder.Property(x => x.Photo).HasColumnName(@"Photo").IsRequired().HasColumnType("nvarchar(80)");
            entityBuilder.Property(x => x.City).HasColumnName(@"City").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.Town).HasColumnName(@"Town").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.Latitude).HasColumnName(@"Latitude").IsRequired().HasColumnType("float");
            entityBuilder.Property(x => x.Longitude).HasColumnName(@"Longitude").IsRequired().HasColumnType("float");

            entityBuilder.Property(x => x.TotalParticipants).HasColumnName(@"TotalParticipants").IsRequired().HasColumnType("int");
            entityBuilder.Property(x => x.TotalComments).HasColumnName(@"TotalComments").IsRequired().HasColumnType("int");

            entityBuilder.HasOne(a => a.User).WithMany(b => b.Events).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            entityBuilder.HasOne(a => a.Category).WithMany(b => b.Events).HasForeignKey(c => c.CategoryId);
        }
    }
}

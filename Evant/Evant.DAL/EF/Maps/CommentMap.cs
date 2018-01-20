using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class CommentMap
    {
        public CommentMap(EntityTypeBuilder<Comment> entityBuilder)
        {
            entityBuilder.ToTable("Comments");

            entityBuilder.Property(x => x.Content).HasColumnName(@"Content").IsRequired().HasColumnType("nvarchar(140)");
            entityBuilder.Property(x => x.UserId).HasColumnName(@"UserId").IsRequired();
            entityBuilder.Property(x => x.EventId).HasColumnName(@"EventId").IsRequired();

            entityBuilder.HasOne(a => a.User).WithMany(b => b.EventComments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            entityBuilder.HasOne(a => a.Event).WithMany(b => b.EventComments).HasForeignKey(c => c.EventId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class FriendOperationMap
    {
        public FriendOperationMap(EntityTypeBuilder<FriendOperation> entityBuilder)
        {
            entityBuilder.ToTable("FriendOperations");

            entityBuilder.Property(x => x.FollowerId).HasColumnName(@"FollowerId").IsRequired();
            entityBuilder.Property(x => x.FollowingId).HasColumnName(@"FollowingId").IsRequired();

            entityBuilder.HasOne(a => a.FollowerUser).WithMany(b => b.Followers).HasForeignKey(c => c.FollowerId).OnDelete(DeleteBehavior.Restrict);
            entityBuilder.HasOne(a => a.FollowingUser).WithMany(b => b.Followings).HasForeignKey(c => c.FollowingId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class UserMap
    {
        public UserMap(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.ToTable("Users");

            entityBuilder.Property(x => x.UserSettingId).HasColumnName(@"UserSettingId").IsRequired();
            entityBuilder.Property(x => x.FacebookId).HasColumnName(@"FacebookId").HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.FirstName).HasColumnName(@"FirstName").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.LastName).HasColumnName(@"LastName").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.Email).HasColumnName(@"Email").IsRequired().HasColumnType("nvarchar(40)");
            entityBuilder.Property(x => x.Password).HasColumnName(@"Password").IsRequired().HasColumnType("nvarchar(160)");
            entityBuilder.Property(x => x.Photo).HasColumnName(@"Photo").HasColumnType("nvarchar(100)");
            entityBuilder.Property(x => x.Role).HasColumnName(@"Role").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.IsActive).HasColumnName(@"IsActive").IsRequired().HasColumnType("bit");
            entityBuilder.Property(x => x.IsFacebook).HasColumnName(@"IsFacebook").IsRequired().HasColumnType("bit");

            entityBuilder.HasOne(a => a.UserSetting).WithOne(b => b.User).HasForeignKey<User>(c => c.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}

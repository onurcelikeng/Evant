using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class UserDeviceMap
    {
        public UserDeviceMap(EntityTypeBuilder<UserDevice> entityBuilder)
        {
            entityBuilder.ToTable("UserDevices");

            entityBuilder.Property(x => x.UserId).HasColumnName(@"UserId").IsRequired();
            entityBuilder.Property(x => x.DeviceId).HasColumnName(@"DeviceId").IsRequired().HasColumnType("nvarchar(80)");
            entityBuilder.Property(x => x.PlayerId).HasColumnName(@"PlayerId").IsRequired().HasColumnType("nvarchar(80)");
            entityBuilder.Property(x => x.Brand).HasColumnName(@"Brand").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.Model).HasColumnName(@"Model").IsRequired().HasColumnType("nvarchar(40)");
            entityBuilder.Property(x => x.OS).HasColumnName(@"OS").IsRequired().HasColumnType("nvarchar(40)");
            entityBuilder.Property(x => x.IsLoggedin).HasColumnName(@"IsLoggedin").IsRequired().HasColumnType("bit");

            entityBuilder.HasOne(a => a.User).WithMany(b => b.UserDevices).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
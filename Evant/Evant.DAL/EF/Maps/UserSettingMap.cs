using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class UserSettingMap
    {
        public UserSettingMap(EntityTypeBuilder<UserSetting> entityBuilder)
        {
            entityBuilder.ToTable("UserSettings");
            entityBuilder.HasKey(x => x.UserSettingId);

            entityBuilder.Property(x => x.UserSettingId).HasColumnName(@"UserSettingId").IsRequired();
            entityBuilder.Property(x => x.Theme).HasColumnName(@"Theme").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.Language).HasColumnName(@"Language").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.IsCommentNotif).HasColumnName(@"IsCommentNotif").IsRequired().HasColumnType("bit");
            entityBuilder.Property(x => x.IsEventNewComerNotif).HasColumnName(@"IsEventNewComerNotif").IsRequired().HasColumnType("bit");
            entityBuilder.Property(x => x.IsEventUpdateNotif).HasColumnName(@"IsEventUpdateNotif").IsRequired().HasColumnType("bit");
            entityBuilder.Property(x => x.IsFriendshipNotif).HasColumnName(@"IsFriendshipNotif").IsRequired().HasColumnType("bit");
        }
    }
}

using Evant.DAL.EF.Maps;
using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;

namespace Evant.DAL.EF
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<SearchHistory> UserSearchHistories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Comment> EventComments { get; set; }
        public DbSet<EventOperation> EventOperations { get; set; }
        public DbSet<Address> EventAddresses { get; set; }
        public DbSet<FriendOperation> FriendOperations { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<EventTag> EventTags { get; set; }
        public DbSet<ReportType> ReportTypes { get; set; }
        public DbSet<UserReport> UserReports { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new UserMap(modelBuilder.Entity<User>());
            new UserDeviceMap(modelBuilder.Entity<UserDevice>());
            new SearchHistoryMap(modelBuilder.Entity<SearchHistory>());
            new CategoryMap(modelBuilder.Entity<Category>());
            new EventMap(modelBuilder.Entity<Event>());
            new CommentMap(modelBuilder.Entity<Comment>());
            new EventOperationMap(modelBuilder.Entity<EventOperation>());
            new AddressMap(modelBuilder.Entity<Address>());
            new FriendOperationMap(modelBuilder.Entity<FriendOperation>());
            new LogMap(modelBuilder.Entity<Log>());
            new TagMap(modelBuilder.Entity<Tag>());
            new EventTagMap(modelBuilder.Entity<EventTag>());
            new NotificationMap(modelBuilder.Entity<Notification>());
            new ReportTypeMap(modelBuilder.Entity<ReportType>());
            new UserReportMap(modelBuilder.Entity<UserReport>());
            new UserSettingMap(modelBuilder.Entity<UserSetting>());
        }
    }
}

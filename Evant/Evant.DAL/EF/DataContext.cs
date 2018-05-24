using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;

namespace Evant.DAL.EF
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Business> Business { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<SearchHistory> UserSearchHistories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Comment> EventComments { get; set; }
        public DbSet<EventOperation> EventOperations { get; set; }
        public DbSet<FriendOperation> FriendOperations { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Weather> Weathers { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Comment Entity
            modelBuilder.Entity<Comment>().HasOne(a => a.User).WithMany(b => b.EventComments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>().HasOne(a => a.Event).WithMany(b => b.EventComments).HasForeignKey(c => c.EventId).OnDelete(DeleteBehavior.Restrict);

            //Notification Entity
            modelBuilder.Entity<Notification>().HasOne(a => a.Event).WithMany(b => b.Event_Notifications).HasForeignKey(c => c.EventId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Notification>().HasOne(a => a.Comment).WithMany(b => b.Comment_Notifications).HasForeignKey(c => c.CommentId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Notification>().HasOne(a => a.SenderUser).WithMany(b => b.User_Notifications).HasForeignKey(c => c.SenderUserId).OnDelete(DeleteBehavior.Restrict);

            ////Event Entity
            //modelBuilder.Entity<Event>().HasOne(a => a.User).WithMany(b => b.Events).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<Event>().HasOne(a => a.Category).WithMany(b => b.Events).HasForeignKey(c => c.CategoryId);

            //EventOperaation Entity
            modelBuilder.Entity<EventOperation>().HasOne(a => a.User).WithMany(b => b.EventOperations).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<EventOperation>().HasOne(a => a.Event).WithMany(b => b.EventOperations).HasForeignKey(c => c.EventId).OnDelete(DeleteBehavior.Restrict);

            //FriendOperation Entity
            modelBuilder.Entity<FriendOperation>().HasOne(a => a.FollowerUser).WithMany(b => b.Followings).HasForeignKey(c => c.FollowerUserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FriendOperation>().HasOne(a => a.FollowingUser).WithMany(b => b.Followers).HasForeignKey(c => c.FollowingUserId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}

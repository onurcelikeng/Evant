using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class UserReportMap
    {
        public UserReportMap(EntityTypeBuilder<UserReport> entityBuilder)
        {
            entityBuilder.ToTable("UserReports");

            entityBuilder.Property(x => x.ReportTypeId).HasColumnName(@"ReportTypeId").IsRequired();
            entityBuilder.Property(x => x.ReporterUserId).HasColumnName(@"ReporterUserId").IsRequired();
            entityBuilder.Property(x => x.ReportedUserId).HasColumnName(@"ReportedUserId").IsRequired();

            entityBuilder.HasOne(a => a.ReportType).WithMany(b => b.UserReports).HasForeignKey(c => c.ReportTypeId).OnDelete(DeleteBehavior.Restrict);
            entityBuilder.HasOne(a => a.ReportedUser).WithMany(b => b.ReportedUsers).HasForeignKey(c => c.ReportedUserId).OnDelete(DeleteBehavior.Restrict);
            entityBuilder.HasOne(a => a.ReporterUser).WithMany(b => b.ReporterUsers).HasForeignKey(c => c.ReporterUserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
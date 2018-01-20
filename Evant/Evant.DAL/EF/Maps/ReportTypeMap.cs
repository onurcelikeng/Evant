using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class ReportTypeMap
    {
        public ReportTypeMap(EntityTypeBuilder<ReportType> entityBuilder)
        {
            entityBuilder.ToTable("ReportTypes");

            entityBuilder.Property(x => x.Name).HasColumnName(@"Name").IsRequired().HasColumnType("nvarchar(40)");
            entityBuilder.Property(x => x.Level).HasColumnName(@"Level").HasColumnType("int");
        }
    }
}
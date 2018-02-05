using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class LogMap
    {
        public LogMap(EntityTypeBuilder<Log> entityBuilder)
        {
            entityBuilder.ToTable("Logs");

            entityBuilder.Property(x => x.Ip).HasColumnName(@"Ip").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.Table).HasColumnName(@"Table").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.Message).HasColumnName(@"Message").HasColumnType("nvarchar(40)");
            entityBuilder.Property(x => x.Status).HasColumnName(@"Status").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.Exception).HasColumnName(@"Exception").HasColumnType("nvarchar(80)");
        }
    }
}

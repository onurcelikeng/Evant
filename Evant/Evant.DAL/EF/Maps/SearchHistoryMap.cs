using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class SearchHistoryMap
    {
        public SearchHistoryMap(EntityTypeBuilder<SearchHistory> entityBuilder)
        {
            entityBuilder.ToTable("SearchHistories");

            entityBuilder.Property(x => x.UserId).HasColumnName(@"UserId").IsRequired();
            entityBuilder.Property(x => x.Keyword).HasColumnName(@"Keyword").IsRequired().HasColumnType("nvarchar(80)");

            entityBuilder.HasOne(a => a.User).WithMany(b => b.UserSearchHistories).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
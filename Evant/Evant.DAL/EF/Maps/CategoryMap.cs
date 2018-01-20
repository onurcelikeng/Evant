using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class CategoryMap
    {
        public CategoryMap(EntityTypeBuilder<Category> entityBuilder)
        {
            entityBuilder.ToTable("Categories");

            entityBuilder.Property(x => x.Name).HasColumnName(@"Name").IsRequired().HasColumnType("nvarchar(40)");
            entityBuilder.Property(x => x.Icon).HasColumnName(@"Icon").IsRequired().HasColumnType("nvarchar(80)");
        }
    }
}
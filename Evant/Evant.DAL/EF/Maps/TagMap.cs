using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class TagMap
    {
        public TagMap(EntityTypeBuilder<Tag> entityBuilder)
        {
            entityBuilder.ToTable("Tags");

            entityBuilder.Property(x => x.Name).HasColumnName(@"Name").IsRequired().HasColumnType("nvarchar(40)");
        }
    }
}
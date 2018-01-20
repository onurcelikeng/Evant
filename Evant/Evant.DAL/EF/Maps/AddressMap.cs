using Evant.DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evant.DAL.EF.Maps
{
    public class AddressMap
    {
        public AddressMap(EntityTypeBuilder<Address> entityBuilder)
        {
            entityBuilder.ToTable("Addresses");
            entityBuilder.HasKey(x => x.AddressId);

            entityBuilder.Property(x => x.AddressId).HasColumnName(@"AddressId").IsRequired();
            entityBuilder.Property(x => x.City).HasColumnName(@"City").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.Town).HasColumnName(@"Town").IsRequired().HasColumnType("nvarchar(20)");
            entityBuilder.Property(x => x.Latitude).HasColumnName(@"Latitude").IsRequired().HasColumnType("float");
            entityBuilder.Property(x => x.Longitude).HasColumnName(@"Longitude").IsRequired().HasColumnType("float");
        }
    }
}
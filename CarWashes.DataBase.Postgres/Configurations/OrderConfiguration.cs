using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarWashes.DataBase.Postgres.Configurations
{
	public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
	{
		public void Configure(EntityTypeBuilder<OrderEntity> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();

			builder
				.HasMany(x => x.Services)
				.WithMany(x => x.Orders);
			builder
				.HasOne(x => x.User)
				.WithMany(x => x.Orders);

			builder.Property(x => x.CarBrand).HasMaxLength(20);
			builder.Property(x => x.CarModel).HasMaxLength(20);
			builder.Property(x => x.PlateNumber).HasMaxLength(12);
		}
	}
}

using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarWashes.DataBase.Postgres.Configurations
{
	public class CarwashConfiguration : IEntityTypeConfiguration<CarwashEntity>
	{
		public void Configure(EntityTypeBuilder<CarwashEntity> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();

			builder
				.HasMany(x => x.Staff)
				.WithMany(x => x.Carwashes);

			builder
				.HasMany(x => x.Posts)
				.WithOne(x => x.CarWash);

			builder
				.HasMany(x => x.Services)
				.WithOne(x => x.Carwash);

			builder.Property(x => x.City).HasMaxLength(25);
			builder.Property(x => x.Address).HasMaxLength(30);
			builder.Property(x => x.Phone).HasMaxLength(11);

			builder.Property(x => x.WorkTimeStart).HasColumnType("timetz");
			builder.Property(x => x.WorkTimeEnd).HasColumnType("timetz");
		}
	}
}

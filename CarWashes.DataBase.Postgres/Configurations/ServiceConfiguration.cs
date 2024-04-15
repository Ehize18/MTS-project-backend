using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarWashes.DataBase.Postgres.Configurations
{
	public class ServiceConfiguration : IEntityTypeConfiguration<ServiceEntity>
	{
		public void Configure(EntityTypeBuilder<ServiceEntity> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();

			builder
				.HasOne(x => x.Carwash)
				.WithMany(x => x.Services);
			builder
				.HasMany(x => x.Orders)
				.WithMany(x => x.Services);
			
			builder.Property(x => x.Name).HasMaxLength(50);
		}
	}
}

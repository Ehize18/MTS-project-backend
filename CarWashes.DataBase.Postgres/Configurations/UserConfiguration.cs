using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarWashes.DataBase.Postgres.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
	{
		public void Configure(EntityTypeBuilder<UserEntity> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();

			builder
				.HasOne(x => x.Human)
				.WithMany(x => x.Users);

			builder
				.HasMany(x => x.Orders)
				.WithOne(x => x.User);

			builder.Property(x => x.Role).HasMaxLength(6);
			builder.Property(x => x.Login).HasMaxLength(256);
			builder.Property(x => x.Password).HasMaxLength(256);

			builder.HasIndex(x => x.Login).IsUnique();
		}
	}
}

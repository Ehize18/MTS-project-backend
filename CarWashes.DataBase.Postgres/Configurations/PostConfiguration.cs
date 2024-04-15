using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarWashes.DataBase.Postgres.Configurations
{
	public class PostConfiguration : IEntityTypeConfiguration<PostEntity>
	{
		public void Configure(EntityTypeBuilder<PostEntity> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();

			builder
				.HasOne(x => x.CarWash)
				.WithMany(x => x.Posts);
			builder
				.HasMany(x => x.Orders)
				.WithOne(x => x.Post);
		}
	}
}

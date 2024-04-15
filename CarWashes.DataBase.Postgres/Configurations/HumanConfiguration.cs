using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarWashes.DataBase.Postgres.Configurations
{
	internal class HumanConfiguration : IEntityTypeConfiguration<HumanEntity>
	{
		public void Configure(EntityTypeBuilder<HumanEntity> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();

			builder.Property(x => x.L_Name).HasMaxLength(30);
			builder.Property(x => x.F_Name).HasMaxLength(30);
			builder.Property(x => x.M_Name).HasMaxLength(30);
			builder.Property(x => x.Phone).HasMaxLength(11);
		}
	}
}

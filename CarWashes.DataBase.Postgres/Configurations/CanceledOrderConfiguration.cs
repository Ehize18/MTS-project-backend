using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarWashes.DataBase.Postgres.Configurations
{
	public class CanceledOrderConfiguration : IEntityTypeConfiguration<CanceledOrderEntity>
	{
		public void Configure(EntityTypeBuilder<CanceledOrderEntity> builder)
		{
			builder.HasKey(x => x.OrderId);

			builder.HasOne(x => x.Order)
				.WithOne(x => x.CanceledOrder)
				.HasForeignKey<CanceledOrderEntity>(x => x.OrderId);
		}
	}
}

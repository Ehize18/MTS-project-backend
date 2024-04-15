using CarWashes.DataBase.Postgres.Configurations;
using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWashes.DataBase.Postgres
{
	public class CarWashesDbContext(DbContextOptions<CarWashesDbContext> options)
		: DbContext(options)
	{
		public DbSet<HumanEntity> Humans { get; set; }
		public DbSet<UserEntity> Users { get; set; }
		public DbSet<CarwashEntity> Carwashes { get; set; }
		public DbSet<PostEntity> Posts { get; set; }
		public DbSet<ServiceEntity> Services { get; set; }
		public DbSet<OrderEntity> Orders { get; set; }
		public DbSet<CanceledOrderEntity> CanceledOrders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new HumanConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new OrderConfiguration());
			modelBuilder.ApplyConfiguration(new ServiceConfiguration());
			modelBuilder.ApplyConfiguration(new PostConfiguration());
			modelBuilder.ApplyConfiguration(new CanceledOrderConfiguration());
			modelBuilder.ApplyConfiguration(new CarwashConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}
}

using CarWashes.Core.Models;

namespace CarWashes.DataBase.Postgres.Repositories
{
	public interface IHumansRepository
	{
		Task Add(Human human);
		Task<List<Human>> Get();
	}
}
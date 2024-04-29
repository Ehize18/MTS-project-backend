using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;

namespace CarWashes.Application.Services
{
	public class HumansService : IHumansService
	{
		private readonly IHumansRepository _humansRepository;

		public HumansService(IHumansRepository humansRepository)
		{
			_humansRepository = humansRepository;
		}

		public async Task AddUser(Human human)
		{
			await _humansRepository.Add(human);
		}

		public async Task AddHumanWithUser(Human human, User user)
		{
			await _humansRepository.AddWithUser(human, user);
		}

		public async Task<Human> GetHumanByJwtToken(int id)
		{
			return await _humansRepository.GetByUserId(id);
		}

		public async Task<Human> GetHumanByPhone(string phone)
		{
			return await _humansRepository.GetByPhone(phone);
		}
	}
}

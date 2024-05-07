using CarWashes.Core.Models;
using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWashes.DataBase.Postgres.Repositories
{
	public class PostsRepository
	{
		private readonly CarWashesDbContext _dbContext;
		public PostsRepository(CarWashesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Post>> GetAll()
		{
			var postsEntities = await _dbContext.Posts
				.AsNoTracking()
				.OrderBy(x => x.Id)
				.ToListAsync();
			var posts = postsEntities
				.Select(x => new Post((int)x.Id, x.CarWashId, x.InternalNumber))
				.ToList();
			return posts;
		}

		public async Task<Post?> GetById(int id)
		{
			var postEntity = await _dbContext.Posts
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
			var post = new Post((int)postEntity.Id, postEntity.CarWashId, postEntity.InternalNumber);
			return post;
		}

		public async Task<List<Post>> GetByCarwashId(int carwashId)
		{
			var postEntities = await _dbContext.Posts
				.AsNoTracking()
				.Where(x => x.CarWashId == carwashId)
				.ToListAsync();
			var posts = postEntities
				.Select(x => new Post((int)x.Id, x.CarWashId, x.InternalNumber))
				.ToList();
			return posts;
		}

		public async Task Add(Post post)
		{
			var postEntity = new PostEntity
			{
				Id = post.Id,
				CarWashId = post.CarWashId,
				InternalNumber = post.InternalNumber
			};

			await _dbContext.Posts.AddAsync(postEntity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task Update(int id, int internalNumber)
		{
			await _dbContext.Posts
				.Where(x => x.Id == id)
				.ExecuteUpdateAsync(s => s
					.SetProperty(x => x.InternalNumber, x => internalNumber));
		}
	}
}

namespace CarWashes.Core.Models
{
	public class Post
	{
		public int Id { get; }
		public int CarWashId { get; }
		public int InternalNumber { get; }

		public Post(int id, int carWashId, int internalNumber)
		{
			Id = id;
			CarWashId = carWashId;
			InternalNumber = internalNumber;
		}
	}
}

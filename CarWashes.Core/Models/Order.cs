namespace CarWashes.Core.Models
{
	public class Order
	{
		public int? Id { get; }
		public int UserId { get; }
		public int PostId { get; }

		public string PlateNumber { get; } = string.Empty;
		public string CarBrand { get; } = string.Empty;
		public string CarModel { get; } = string.Empty;
		public int CarReleaseYear { get; }

		public Status Status { get; } = Status.Created;
		public DateTime OrderTime { get; }
		public DateTime CreatedAt { get; }
		public DateTime? UpdatedAt { get; }

		public Order(
			int? id, int userId, int postId,
			string plateNumber, string carBrand, string carModel, int carReleaseYear,
			DateTime orderTime, DateTime createdAt, DateTime? updatedAt)
		{
			Id = id;
			UserId = userId;
			PostId = postId;
			PlateNumber = plateNumber;
			CarBrand = carBrand;
			CarModel = carModel;
			CarReleaseYear = carReleaseYear;
			OrderTime = orderTime;
			CreatedAt = createdAt;
		}
	}
}

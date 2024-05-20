namespace CarWashes.Core.Models
{
	public class Order
	{
		public int Id { get; }
		public int UserId { get; }
		public string FirstName { get; } = string.Empty;
		public string Phone { get; } = string.Empty;
		public int CarwashId { get; }
		public int PostId { get; }
		public int PostInternalNumber { get; }

		public string PlateNumber { get; } = string.Empty;
		public string CarBrand { get; } = string.Empty;
		public string CarModel { get; } = string.Empty;
		public int CarReleaseYear { get; }

		public Status Status { get; }
		public DateTime OrderTime { get; }
		public DateTime CreatedAt { get; }
		public DateTime UpdatedAt { get; }

		public Order(
			int id,
			int userId, string firstName, string phone,
			int carwashId, int postId, int postInternalNumber,
			string plateNumber, string carBrand, string carModel, int carReleaseYear,
			Status status, DateTime orderTime, DateTime createdAt, DateTime updatedAt)
		{
			Id = id;
			UserId = userId;
			FirstName = firstName;
			Phone = phone;
			CarwashId = carwashId;
			PostId = postId;
			PostInternalNumber = postInternalNumber;
			PlateNumber = plateNumber;
			CarBrand = carBrand;
			CarModel = carModel;
			CarReleaseYear = carReleaseYear;
			Status = status;
			OrderTime = orderTime;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
		}
	}
}

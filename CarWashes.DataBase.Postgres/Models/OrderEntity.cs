using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashes.DataBase.Postgres.Models
{
	public enum Status
	{
		Created,
		Confirmed,
		InProgress,
		Finished,
		Rejected,
		Canceled
	}

	public class OrderEntity
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public UserEntity? User { get; set; }
		public int PostId { get; set; }
		public PostEntity? Post { get; set; }

		
		public string PlateNumber { get; set; } = string.Empty;
		public string CarBrand { get; set; } = string.Empty;
		public string CarModel { get; set; } = string.Empty;
		public int CarReleaseYear { get; set; }

		public Status Status { get; set; } = Status.Created;
		public DateTime OrderTime { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		public List<ServiceEntity> Services { get; set; } = [];
		public CanceledOrderEntity? CanceledOrder { get; set; }
	}
}

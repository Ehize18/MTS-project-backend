namespace CarWashes.Core.Models
{
	public class Staff
	{
		public int UserId { get; }
		public int CarWashId { get; }

		public Staff(int userid, int carwashid)
		{
			UserId = userid;
			CarWashId = carwashid;
		}
	}
}

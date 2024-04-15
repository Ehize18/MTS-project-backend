namespace CarWashes.DataBase.Postgres.Models
{
	public class CanceledOrderEntity
	{
		public int OrderId { get; set; }
		public OrderEntity? Order { get; set; }
		public string Reason { get; set; } = string.Empty;
	}
}

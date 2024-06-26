﻿namespace CarWashes.Core.Models
{
	public class Carwash
	{
		public int? Id { get; }
		public string OrgName { get; } = string.Empty;
		public string Name { get; } = string.Empty;
		public string City { get; } = string.Empty;
		public string Address { get; } = string.Empty;
		public string Phone { get; } = string.Empty;
		public string Email { get; } = string.Empty;
		public TimeOnly WorkTimeStart { get; }
		public TimeOnly WorkTimeEnd { get; }

		public Carwash(
			int? id,
			string orgName, string name,
			string city, string address,
			string phone, string email,
			TimeOnly workTimeStart, TimeOnly workTimeEnd)
		{
			Id = id;
			OrgName = orgName;
			Name = name;
			City = city;
			Address = address;
			Phone = phone;
			Email = email;
			WorkTimeStart = workTimeStart;
			WorkTimeEnd = workTimeEnd;
		}
	}
}

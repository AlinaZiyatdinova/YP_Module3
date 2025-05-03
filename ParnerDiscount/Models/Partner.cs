using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParnerDiscount.Models
{
	public class Partner
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Raitning { get; set; }
		public string PatrnerType { get; set; }
		public string INN { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string Director { get; set; }	
		public decimal TotalSales { get; set; }
		public decimal Discount { get; set; }
	}
}

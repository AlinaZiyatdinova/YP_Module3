using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParnerDiscount.Services
{
	public static class PartnerService
	{
		public static decimal GetDiscount(Decimal totalSales)
		{
			if (totalSales > 300000) return 15m;
			if (totalSales > 50000) return 10m;
			if (totalSales > 10000) return 05m;
			return 0m;
		}
	}
}

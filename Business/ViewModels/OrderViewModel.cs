using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels
{
	public class OrderViewModel
	{
		public OrderHeader orderHeader { get; init; }
		public IEnumerable<OrderDatials> orderDatials { get; init; }
	}
}

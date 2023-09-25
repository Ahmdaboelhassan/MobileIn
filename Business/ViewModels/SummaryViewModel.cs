using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels
{
	public class SummaryViewModel
	{
		public OrderHeader orderHeader { get; set; }
		public IEnumerable<ShoppingList> shoppingList { get; set; }
	}
}

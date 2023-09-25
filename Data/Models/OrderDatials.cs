using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
	public class OrderDatials
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int OrderId { get; set; }

		[Required]
		public int ProductId { get; set; }

		public int Count { get; set; }

		public decimal Price { get; set; }

		[ForeignKey("ProductId")]
		[ValidateNever]
		public Mobile Mobile { get; set; }

		[ForeignKey("OrderId")]
		[ValidateNever]
		public OrderHeader OrderHeader { get; set; }

	}
}
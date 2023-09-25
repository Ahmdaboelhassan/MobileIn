using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
	public class OrderHeader
	{
		[Key]
		public int Id { get; set; }

		public string ApplicationUserId { get; set; }

		public DateTime OrderDate { get; set; }

		public DateTime ShippingDate { get; set; }

		public string? OrderStatus { get; set; }

		public string? PaymentStatus { get; set; }
		public DateTime PaymentDate { get; set; }

		public DateTime PaymentDueDate { get; set; }

		public string? SessionId { get; set; }

		public string? PaymentIntendId { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		[Required]
		[StringLength(50)]
		public string firstName { get; set; }
		[Required]
		[StringLength(50)]
		public string lastName { get; set; }
		[Required]
		[StringLength(100)]
		public string address { get; set; }
		[Required]

		public decimal finalPrice { get; set; }

		[ValidateNever]
		[ForeignKey(nameof(ApplicationUserId))]
		public ApplicationUser ApplicationUser { get; set; }
	}
}

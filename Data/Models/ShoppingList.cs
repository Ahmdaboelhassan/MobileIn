using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public sealed class ShoppingList
    {
        public int Id { get; set; }

        public int count { get; set; }

        public string ApplicationUserId { get; set; }
        public int MobileId { get; set; }

        [NotMapped]
		public decimal finalPrice { get; set; }

		[ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser applicationUser { get; set; }

        [ValidateNever]
        [ForeignKey("MobileId")]
        public Mobile Mobile { get; set; }

    }
}

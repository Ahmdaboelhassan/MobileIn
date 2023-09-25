using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string? firstName {  get; set; }
        [StringLength(50)]
        public string? lastName { get; set; }
        [StringLength(100)]
        public string? adress { get; set; }
        public DateTime? BD { get; set; }
        [ValidateNever]
        public string? profilePicture { get; set; }
    }
}

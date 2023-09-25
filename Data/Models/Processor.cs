using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data.Models
{
    public class Processor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte id { get; set; }
        [Required]
        [StringLength(100)]
        public string name { get; set; }
        public byte[]? photo { get; set; } 
        [Required]
        [Display(Name ="Company")]
        public byte companyId { get; set; }
        [ValidateNever]
        public Company company { get; set; }
     }
}


using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class Mobile
{
    public int id { get; set; }
    [Required]
    [StringLength(100)]
    public string name { get; set; }
    [Required]
    [Range(500,50_000)]
    public decimal price { get; set; }
    [Required]
    [StringLength(200)]
    public string description { get; set; }
    [Required]
    public int yearRelease { get; set; }
    public string? photoURL { get; set; }
    [Required]
    public byte RAM { get; set; }
    [Required]
    public string size { get; set; }

    [Required]
    [Display(Name ="Company")]
    public byte companyId { get; set; }
    [Required]
    [Display(Name ="Proccessor")]
    public byte processorId { get; set; }
    [ValidateNever]
    public Company company { get; set; }
    [ValidateNever]
    public Processor processor { get; set; }


}
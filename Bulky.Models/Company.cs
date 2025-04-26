﻿using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models;
public class Company
{
    public int Id { get; set; }
    [Required]
    [Display(Name = "Company Name")]
    public string Name { get; set; }
    [Display(Name = "Street Address")]
    public string? StreetAddress { get; set; }
    [Display(Name = "City")]
    public string? City { get; set; }
    public string? State { get; set; }
    [Display(Name = "Postal Code")]
    public string? PostalCode { get; set; }
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

}

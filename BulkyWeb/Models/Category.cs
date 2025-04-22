﻿namespace BulkyWeb.Models;

public class Category
{
    public int Id { get; set; }
    [DisplayName("Category Name")]
    [MaxLength(50)]
    public string Name { get; set; }
    [DisplayName("Display Order")]
    [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
    public int DisplayOrder { get; set; }
}

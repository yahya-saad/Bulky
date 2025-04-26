namespace BulkyBook.Models;
public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ISBN { get; set; }
    public string Author { get; set; }

    [Required]
    [Range(1, 1000, ErrorMessage = "Price must be between 1 and 1000")]
    [Display(Name = "List Price")]
    public double ListPrice { get; set; }

    [Required]
    [Range(1, 1000, ErrorMessage = "Price must be between 1 and 1000")]
    [Display(Name = "Price for 1-50")]
    public double Price { get; set; }

    [Required]
    [Range(1, 1000, ErrorMessage = "Price must be between 1 and 1000")]
    [Display(Name = "Price for 50+")]
    public double Price50 { get; set; }

    [Required]
    [Range(1, 1000, ErrorMessage = "Price must be between 1 and 1000")]
    [Display(Name = "Price for 100+")]
    public double Price100 { get; set; }

    [ForeignKey("CategoryId")]
    [ValidateNever]
    public Category Category { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    [ValidateNever]
    public string ImageUrl { get; set; }
}

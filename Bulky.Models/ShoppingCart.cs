namespace BulkyBook.Models;
public class ShoppingCart
{
    public int Id { get; set; }
    public int Count { get; set; }

    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; } = null!;
    public string ApplicationUserId { get; set; }

    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }

    [NotMapped]
    public double Price { get; set; }

}

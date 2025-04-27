namespace BulkyBook.Models;
public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    [ForeignKey("OrderId")]
    [ValidateNever]
    public Order Order { get; set; }
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product { get; set; }
    public int Count { get; set; }
    public double Price { get; set; }
}

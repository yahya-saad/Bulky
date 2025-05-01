namespace BulkyBook.Models.ViewModels;
public class OrderVM
{
    public Order Order { get; set; }
    [ValidateNever]
    public IEnumerable<OrderItem> OrderItems { get; set; }
}

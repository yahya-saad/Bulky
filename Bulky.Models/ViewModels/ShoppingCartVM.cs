namespace BulkyBook.Models.ViewModels;
public class ShoppingCartVM
{
    [ValidateNever]
    public IEnumerable<ShoppingCart> CartList { get; set; } = null!;
    public Order Order { get; set; }
}

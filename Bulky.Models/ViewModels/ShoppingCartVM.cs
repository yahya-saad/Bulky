namespace BulkyBook.Models.ViewModels;
public class ShoppingCartVM
{
    public IEnumerable<ShoppingCart> CartList { get; set; } = null!;
    public double OrderTotal { get; set; }
}

using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
    private readonly IUnitOfWork uow;
    public CartController(IUnitOfWork uow)
    {
        this.uow = uow;
    }
    public IActionResult Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var cartList = uow.ShoppingCart.GetAll(c => c.ApplicationUserId == userId, includeProperties: "Product");

        var cartVM = new ShoppingCartVM()
        {
            CartList = cartList,
        };

        foreach (var cart in cartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            cartVM.OrderTotal += (cart.Price * cart.Count);
        }

        return View(cartVM);
    }
    public IActionResult Summary()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var cartList = uow.ShoppingCart.GetAll(c => c.ApplicationUserId == userId, includeProperties: "Product");
        return View(cartList);
    }

    public IActionResult Plus(int cartId)
    {
        var cartFromDb = uow.ShoppingCart.Get(c => c.Id == cartId, tracked: true);
        if (cartFromDb != null)
        {
            cartFromDb.Count += 1;
            uow.Save();
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(int cartId)
    {
        var cartFromDb = uow.ShoppingCart.Get(c => c.Id == cartId, tracked: true);
        if (cartFromDb != null)
        {
            if (cartFromDb.Count == 1)
            {
                uow.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
            }
            uow.Save();
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int cartId)
    {
        var cartFromDb = uow.ShoppingCart.Get(c => c.Id == cartId);
        if (cartFromDb != null)
        {
            uow.ShoppingCart.Remove(cartFromDb);
            uow.Save();
        }
        return RedirectToAction(nameof(Index));
    }


    private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
    {
        if (shoppingCart.Count <= 50)
            return shoppingCart.Product.Price;
        else if (shoppingCart.Count <= 100)
            return shoppingCart.Product.Price50;
        else
            return shoppingCart.Product.Price100;
    }
}

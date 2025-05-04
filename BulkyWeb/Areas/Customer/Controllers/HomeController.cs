using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly IUnitOfWork uow;

    public HomeController(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public IActionResult Index()
    {
        var products = uow.Product.GetAll(includeProperties: "Category");
        return View(products);
    }

    public IActionResult Details(int productId)
    {
        var product = uow.Product.Get(p => p.Id == productId, includeProperties: "Category");
        var cartObj = new ShoppingCart()
        {
            Product = product,
            ProductId = productId,
            Count = 1,
        };
        return View(cartObj);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        shoppingCart.ApplicationUserId = userId;
        shoppingCart.Product = null!; // Prevent EF from trying to insert the product again

        var cartFromDb = uow.ShoppingCart.Get(
            c => c.ApplicationUserId == userId && c.ProductId == shoppingCart.ProductId,
            tracked: true
        );

        if (cartFromDb != null)
        {
            cartFromDb.Count += shoppingCart.Count;
            uow.Save();

        }
        else
        {
            uow.ShoppingCart.Add(shoppingCart);
            uow.Save();

            HttpContext.Session.SetInt32(AppConstants.SessionCart,
                uow.ShoppingCart.GetAll(c => c.ApplicationUserId == userId).Count());
        }

        TempData["success"] = "Cart updated successfully";
        return RedirectToAction(nameof(Index));
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

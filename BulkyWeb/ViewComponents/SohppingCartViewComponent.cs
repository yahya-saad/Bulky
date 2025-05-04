using System.Security.Claims;

namespace BulkyBookWeb.ViewComponents;

public class ShoppingCartViewComponent(IUnitOfWork uow) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var claimIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return View(0);

        var cartCount = HttpContext.Session.GetInt32(AppConstants.SessionCart);

        if (!cartCount.HasValue)
        {
            cartCount = uow.ShoppingCart.GetAll(c => c.ApplicationUserId == userId).Count();
            HttpContext.Session.SetInt32(AppConstants.SessionCart, cartCount.Value);
        }

        return View(cartCount.Value);
    }

}

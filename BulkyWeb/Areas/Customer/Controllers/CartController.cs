using Stripe.Checkout;
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
            Order = new Order()
        };

        foreach (var cart in cartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            cartVM.Order.OrderTotal += (cart.Price * cart.Count);
        }

        return View(cartVM);
    }
    public IActionResult Summary()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var cartList = uow.ShoppingCart.GetAll(c => c.ApplicationUserId == userId, includeProperties: "Product");

        if (!cartList.Any())
        {
            return RedirectToAction(nameof(Index));
        }


        var cartVM = new ShoppingCartVM()
        {
            CartList = cartList,
            Order = new Order()
            {
                ShippingAddress = new Address()
            }
        };

        // Populate Order fields
        cartVM.Order.ApplicationUser = uow.User.Get(u => u.Id == userId)!;
        cartVM.Order.ApplicationUserId = userId;
        cartVM.Order.Name = cartVM.Order.ApplicationUser.UserName;
        cartVM.Order.PhoneNumber = cartVM.Order.ApplicationUser.PhoneNumber;
        cartVM.Order.ShippingAddress.StreetAddress = cartVM.Order.ApplicationUser.StreetAddress;
        cartVM.Order.ShippingAddress.City = cartVM.Order.ApplicationUser.City;
        cartVM.Order.ShippingAddress.State = cartVM.Order.ApplicationUser.State;
        cartVM.Order.ShippingAddress.PostalCode = cartVM.Order.ApplicationUser.PostalCode;

        foreach (var cart in cartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            cartVM.Order.OrderTotal += (cart.Price * cart.Count);
        }


        return View(cartVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Summary(ShoppingCartVM shoppingCartVM)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var cartList = uow.ShoppingCart.GetAll(c => c.ApplicationUserId == userId, includeProperties: "Product");

        if (!ModelState.IsValid)
        {
            shoppingCartVM.CartList = cartList;
            return View(shoppingCartVM);
        }

        var cartVM = new ShoppingCartVM()
        {
            CartList = cartList,
            Order = shoppingCartVM.Order,
        };

        var user = uow.User.Get(u => u.Id == userId);
        cartVM.Order.ApplicationUserId = user.Id;
        cartVM.Order.OrderDate = DateTime.Now;

        // Calculate order total based on cart items
        foreach (var cart in cartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            cartVM.Order.OrderTotal += (cart.Price * cart.Count);
        }

        if (user.CompanyId.GetValueOrDefault() == 0)
        {
            // This is a non-company user
            cartVM.Order.OrderStatus = AppConstants.OrderStatus.StatusPending;
            cartVM.Order.PaymentStatus = AppConstants.PaymentStatus.StatusPending;
        }
        else
        {
            // This is a company user
            cartVM.Order.OrderStatus = AppConstants.OrderStatus.StatusApproved;
            cartVM.Order.PaymentStatus = AppConstants.PaymentStatus.StatusDelayedPayment;
        }

        uow.Order.Add(cartVM.Order);
        uow.Save();


        var sessionLineItems = new List<SessionLineItemOptions>();
        // Insert order items into the database
        foreach (var item in cartList)
        {
            var orderitem = new OrderItem()
            {
                ProductId = item.ProductId,
                OrderId = cartVM.Order.Id,
                Price = item.Price,
                Count = item.Count
            };

            uow.OrderItem.Add(orderitem);

            // Session line items for Stripe Checkout
            sessionLineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(item.Price * 100), // Convert to cents
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Product.Title,
                        // Stripe Does not let you show local images 
                        //Images = new List<string> { imageUrl }

                    },
                },
                Quantity = item.Count,
            });
        }

        uow.Save();

        if (user.CompanyId.GetValueOrDefault() == 0)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{baseUrl}/customer/cart/OrderConfirmation?orderId={cartVM.Order.Id}",
                CancelUrl = $"{baseUrl}/customer/cart/index",
                LineItems = sessionLineItems,
                Mode = "payment",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            uow.Order.UpdateStripePaymentId(cartVM.Order.Id, session.Id, session.PaymentIntentId);
            uow.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        return RedirectToAction(nameof(OrderConfirmation), new { orderId = cartVM.Order.Id });
    }


    public IActionResult OrderConfirmation(int orderId)
    {
        var order = uow.Order.Get(o => o.Id == orderId, includeProperties: "ApplicationUser");

        if (order == null)
            return NotFound();

        if (!string.Equals(order.PaymentStatus, AppConstants.PaymentStatus.StatusDelayedPayment) && !string.IsNullOrEmpty(order.SessionId))
        {
            var service = new SessionService();
            Session session = service.Get(order.SessionId);

            // Check if the session is paid
            if (session.PaymentStatus.ToLower() == "paid")
            {
                uow.Order.UpdateStatus(orderId, AppConstants.OrderStatus.StatusApproved, AppConstants.PaymentStatus.StatusApproved);
                uow.Save();
            }

            // Clear the shopping cart
            var shoppingCartList = uow.ShoppingCart.GetAll(c => c.ApplicationUserId == order.ApplicationUserId);
            uow.ShoppingCart.RemoveRange(shoppingCartList);
            uow.Save();
        }

        return View(orderId);
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

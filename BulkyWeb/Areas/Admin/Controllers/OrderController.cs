using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class OrderController(IUnitOfWork uow) : Controller
{
    [BindProperty]
    public OrderVM OrderVM { get; set; }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Details(int orderId)
    {
        var order = uow.Order.Get(o => o.Id == orderId, includeProperties: "ApplicationUser");
        if (order == null)
        {
            return NotFound();
        }

        OrderVM = new OrderVM()
        {
            Order = order,
            OrderItems = uow.OrderItem.GetAll(o => o.OrderId == orderId, includeProperties: "Product")
        };

        return View(OrderVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PayNow()
    {
        var orderId = OrderVM.Order.Id;
        OrderVM.Order = uow.Order.Get(o => o.Id == orderId, includeProperties: "ApplicationUser");
        OrderVM.OrderItems = uow.OrderItem.GetAll(o => o.OrderId == orderId, includeProperties: "Product");

        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        var options = new SessionCreateOptions
        {
            SuccessUrl = $"{baseUrl}/admin/order/PaymentConfirmation?orderId={orderId}",
            CancelUrl = $"{baseUrl}/admin/order/details?orderId={orderId}",
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
        };

        foreach (var item in OrderVM.OrderItems)
        {
            var lineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(item.Price * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Product.Title,
                    },
                },
                Quantity = item.Count,
            };
            options.LineItems.Add(lineItem);
        }

        var service = new SessionService();
        Session session = service.Create(options);

        uow.Order.UpdateStripeSessionId(orderId, session.Id);
        uow.Save();

        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);
    }

    public IActionResult PaymentConfirmation(int orderId)
    {
        var order = uow.Order.Get(o => o.Id == orderId);

        if (order == null)
            return NotFound();

        if (string.Equals(order.PaymentStatus, AppConstants.PaymentStatus.StatusDelayedPayment))
        {
            var service = new SessionService();
            Session session = service.Get(order.SessionId);

            // Check if the session is paid
            if (session.PaymentStatus.ToLower() == "paid")
            {
                uow.Order.UpdateStripePaymentIntentId(orderId, session.PaymentIntentId);
                uow.Order.UpdateStatus(orderId, order.OrderStatus, AppConstants.PaymentStatus.StatusApproved);
                uow.Save();
            }
        }

        return View(orderId);
    }



    [Authorize(Roles = $"{AppConstants.Roles.Role_Admin},{AppConstants.Roles.Role_Employee}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateOrderDetail()
    {
        if (!ModelState.IsValid)
        {
            OrderVM.OrderItems = uow.OrderItem.GetAll(o => o.OrderId == OrderVM.Order.Id, includeProperties: "Product");
            return View("Details", OrderVM);
        }

        var order = uow.Order.Get(o => o.Id == OrderVM.Order.Id);
        if (order == null)
        {
            return NotFound();
        }

        order.Name = OrderVM.Order.Name;
        order.PhoneNumber = OrderVM.Order.PhoneNumber;
        order.ShippingAddress.StreetAddress = OrderVM.Order.ShippingAddress.StreetAddress;
        order.ShippingAddress.City = OrderVM.Order.ShippingAddress.City;
        order.ShippingAddress.State = OrderVM.Order.ShippingAddress.State;
        order.ShippingAddress.PostalCode = OrderVM.Order.ShippingAddress.PostalCode;

        if (!string.IsNullOrEmpty(OrderVM.Order.Carrier))
            order.Carrier = OrderVM.Order.Carrier;

        if (!string.IsNullOrEmpty(OrderVM.Order.TrackingNumber))
            order.TrackingNumber = OrderVM.Order.TrackingNumber;

        uow.Order.Update(order);
        uow.Save();

        TempData["success"] = "Order status updated successfully.";
        return RedirectToAction(nameof(Details), new { orderId = order.Id });
    }


    [Authorize(Roles = $"{AppConstants.Roles.Role_Admin},{AppConstants.Roles.Role_Employee}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult StartProcessing()
    {
        var orderId = OrderVM.Order.Id;
        uow.Order.UpdateStatus(orderId, AppConstants.OrderStatus.StatusInProcess);
        uow.Save();
        TempData["success"] = "Order status updated successfully.";
        return RedirectToAction(nameof(Details), new { orderId });
    }

    [Authorize(Roles = $"{AppConstants.Roles.Role_Admin},{AppConstants.Roles.Role_Employee}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ShipOrder()
    {
        var orderId = OrderVM.Order.Id;
        var order = uow.Order.Get(o => o.Id == orderId);

        if (order == null)
            return NotFound();

        order.TrackingNumber = OrderVM.Order.TrackingNumber;
        order.Carrier = OrderVM.Order.Carrier;
        order.OrderStatus = AppConstants.OrderStatus.StatusShipped;
        order.ShippingDate = DateTime.Now;

        if (order.PaymentStatus == AppConstants.PaymentStatus.StatusDelayedPayment)
        {
            order.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
        }

        uow.Order.Update(order);
        uow.Save();

        TempData["success"] = "Order Shipped successfully.";
        return RedirectToAction(nameof(Details), new { orderId });
    }

    [Authorize(Roles = $"{AppConstants.Roles.Role_Admin},{AppConstants.Roles.Role_Employee}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CancelOrder()
    {
        var orderId = OrderVM.Order.Id;
        var order = uow.Order.Get(o => o.Id == orderId);

        if (order == null)
            return NotFound();

        if (order.PaymentStatus == AppConstants.PaymentStatus.StatusApproved)
        {
            var options = new RefundCreateOptions
            {
                Reason = RefundReasons.RequestedByCustomer,
                PaymentIntent = order.PaymentIntentId,
            };

            var service = new RefundService();
            Refund refund = service.Create(options);
            if (refund.Status.ToLower() == "succeeded")
            {
                uow.Order.UpdateStatus(orderId, AppConstants.OrderStatus.StatusCancelled, AppConstants.PaymentStatus.StatusRefunded);
            }
            else
            {
                TempData["error"] = "Refund failed. Please try again.";
                return RedirectToAction(nameof(Details), new { orderId });
            }
        }
        else
        {
            uow.Order.UpdateStatus(orderId, AppConstants.OrderStatus.StatusCancelled, AppConstants.PaymentStatus.StatusCancelled);

        }

        uow.Save();

        TempData["success"] = "Order Cancelled successfully.";
        return RedirectToAction(nameof(Details), new { orderId });
    }

    #region APIs
    [HttpGet]
    public IActionResult GetAll(string? status)
    {
        IEnumerable<Order> orders;

        if (User.IsInRole(AppConstants.Roles.Role_Admin) || User.IsInRole(AppConstants.Roles.Role_Employee))
        {
            orders = uow.Order.GetAll(includeProperties: "ApplicationUser");
        }
        else
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            orders = uow.Order.GetAll(o => o.ApplicationUserId == userId, includeProperties: "ApplicationUser");
        }


        var filteredOrders = status?.ToLower() switch
        {
            "all" => orders,
            "pending" => orders.Where(o =>
                string.Equals(o.PaymentStatus, AppConstants.PaymentStatus.StatusDelayedPayment, StringComparison.OrdinalIgnoreCase)),
            "inprocess" => orders.Where(o =>
                string.Equals(o.OrderStatus, AppConstants.OrderStatus.StatusInProcess, StringComparison.OrdinalIgnoreCase)),
            "completed" => orders.Where(o =>
                string.Equals(o.OrderStatus, AppConstants.OrderStatus.StatusShipped, StringComparison.OrdinalIgnoreCase)),
            "approved" => orders.Where(o =>
                string.Equals(o.OrderStatus, AppConstants.OrderStatus.StatusApproved, StringComparison.OrdinalIgnoreCase)),
            _ => orders
        };

        return Json(new { data = filteredOrders });
    }
    #endregion

}

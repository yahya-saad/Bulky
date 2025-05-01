namespace BulkyBook.Utility;
public static class AppConstants
{
    public static class Paths
    {
        public const string ProductImageFolder = @"\images\products";
    }

    public static class Roles
    {
        public const string Role_Admin = "Admin";
        public const string Role_Customer = "Customer";
        public const string Role_Company = "Company";
        public const string Role_Employee = "Employee";
    }

    public static class OrderStatus
    {
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
    }

    public static class PaymentStatus
    {
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusDelayedPayment = "ApprovedForDelayedPayment";
        public const string StatusRejected = "Rejected";
        public const string StatusRefunded = "Refunded";
        public const string StatusCancelled = "Cancelled";
    }

}

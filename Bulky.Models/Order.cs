namespace BulkyBook.Models;
public class Order
{
    public int Id { get; set; }

    public string ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public Address ShippingAddress { get; set; }

    public double OrderTotal { get; set; }

    public DateTime OrderDate { get; set; }
    public DateTime ShippingDate { get; set; }

    public string? OrderStatus { get; set; }
    public string? PaymentStatus { get; set; }
    public string? TrackingNumber { get; set; }
    public string? Carrier { get; set; }

    public DateTime PaymentDate { get; set; }
    public DateOnly PaymentDueDate { get; set; }

    public string? PaymentIntentId { get; set; }
    public string? SessionId { get; set; }


}

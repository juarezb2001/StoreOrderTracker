namespace StoreOrderTracker.Server.Models;

public class CustomerOrder
{
    public string CustomerName { get; set; }
    public string CustomerPhoneNumber { get; set; }
    public string Order { get; set; }
    public float AmountDue { get; set; }
}
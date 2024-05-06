using System.Text.Json.Serialization;

public class Order
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string? Status { get; set; }
    public decimal TotalAmount { get; set; }

    [JsonIgnore]
    public User? User { get; set; }

    [JsonIgnore]
    public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
namespace TaskManagement.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }  // Auto-incrementing ID
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; private set; }  // Calculated property

        // Navigation Properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

        public void CalculateSubtotal()
        {
            Subtotal = Quantity * UnitPrice;
        }

        public override string ToString()
        {
            return $"{Product?.Name} x{Quantity} @ ${UnitPrice:F2} = ${Subtotal:F2}";
        }
    }
}
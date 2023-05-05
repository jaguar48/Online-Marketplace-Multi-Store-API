namespace Online_Marketplace.Shared.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
       
        public string ? PaymentGateway { get; set; }
      
        public string ? TransactionReference { get; set; }
      public string ? SellerBusinessName { get; set; }
        public string? Email { get; set; }

        public string ? shippingmethod { get; set; }  

        public decimal ShippingCost { get; set; }

        public DateTime EstimateDeliveryDate { get; set; }
        public decimal Total { get; set; }
       
        public List<OrderItemDto> OrderItems { get; set; }
    }
}

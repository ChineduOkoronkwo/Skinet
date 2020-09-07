namespace Core.Entities.OrderAggregate
{
    public class DeliveryMethod : BaseEntity
    {
        public DeliveryMethod()
        {
        }

        public DeliveryMethod(string shortName, string deliveryTime, string description, decimal price)
        {
            ShortName = shortName;
            DeliveryTime = deliveryTime;
            Description = description;
            Price = price;
        }

        public string ShortName { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
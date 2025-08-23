namespace SystoQ.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public DateTime Date { get; private set; }
        public decimal TotalAmount { get; private set; }
        public List<SaleItem> Items { get; private set; } = new();

        public Sale(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            Date = DateTime.UtcNow;
            TotalAmount = 0;
        }

        public void AddItem(SaleItem item)
        {
            Items.Add(item);
            TotalAmount += item.Subtotal;
        }
    }
}

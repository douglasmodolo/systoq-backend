namespace SystoQ.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        public Product(string name, string? description, decimal price, int stock)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome é obrigatório.", nameof(name));

            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Preço não pode ser negativo.");

            if (stock < 0)
                throw new ArgumentOutOfRangeException(nameof(stock), "Estoque não pode ser negativo.");

            Name = name;
            Description = description ?? string.Empty;
            Price = price;
            Stock = stock;
        }

        public void UpdateStock(int quantity)
        {
            if (quantity < 0 && Math.Abs(quantity) > Stock)
                throw new InvalidOperationException("Estoque insuficiente para a operação.");
            Stock += quantity;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(newPrice), "Preço não pode ser negativo.");
            Price = newPrice;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Nome é obrigatório.", nameof(newName));
            Name = newName;
        }

        public void UpdateDescription(string? newDescription)
        {
            Description = newDescription ?? string.Empty;
        }
    }
}

using System;

namespace RefactorThis.Domain.Entities
{
    public class ProductOption
    {
        // This is only necessary for EF Core to instantiate objects. This was included in the
        // Domain to avoid needing to create Data Transfer Objects.
        private ProductOption()
        {
        }

        public ProductOption(Guid productId, string name, string description, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            ProductId = productId != Guid.Empty ? productId : throw new ArgumentException(nameof(ProductId));
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException(nameof(name));
            Description = !string.IsNullOrWhiteSpace(description) ? description : throw new ArgumentException(nameof(description));
        }

        public Guid Id { get; private set; }

        public Guid ProductId { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}
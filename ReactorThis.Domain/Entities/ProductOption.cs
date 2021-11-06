using System;

namespace RefactorThis.Domain.Entities
{
    public class ProductOption
    {
        private ProductOption()
        {
        }

        public ProductOption(Guid productId, string name, string description)
        {
            Id = new Guid();
            ProductId = productId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public Guid Id { get; private set; }

        public Guid ProductId { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}
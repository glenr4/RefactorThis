using RefactorThis.Domain.Exceptions;
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

        public ProductOption(Guid id, Guid productId, string name, string description)
        {
            Id = id != Guid.Empty ? id : Guid.NewGuid();
            ProductId = productId != Guid.Empty
                ? productId
                : throw new EmptyArgumentException(string.Format(EmptyArgumentException.MessageTemplate, nameof(ProductId)));
            Name = !string.IsNullOrWhiteSpace(name)
                ? name
                : throw new EmptyArgumentException(string.Format(EmptyArgumentException.MessageTemplate, nameof(name)));
            Description = !string.IsNullOrWhiteSpace(description)
                ? description
                : throw new EmptyArgumentException(string.Format(EmptyArgumentException.MessageTemplate, nameof(description)));
        }

        public Guid Id { get; private set; }

        public Guid ProductId { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}
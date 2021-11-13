using RefactorThis.Domain.Exceptions;
using System;

namespace RefactorThis.Domain.Entities
{
    public class Product
    {
        // This is only necessary for EF Core to instantiate objects. This was included in the
        // Domain to avoid needing to create Data Transfer Objects.
        private Product()
        {
        }

        public Product(Guid id, string name, string description, decimal price, decimal deliveryPrice)
        {
            Id = id != Guid.Empty ? id : Guid.NewGuid();
            Name = !string.IsNullOrWhiteSpace(name)
                ? name
                : throw new EmptyArgumentException(string.Format(EmptyArgumentException.MessageTemplate, nameof(name)));
            Description = !string.IsNullOrWhiteSpace(description)
                ? description
                : throw new EmptyArgumentException(string.Format(EmptyArgumentException.MessageTemplate, nameof(description)));
            Price = price >= 0
                ? price
                : throw new NegativeNumberInvalidException(string.Format(NegativeNumberInvalidException.MessageTemplate, nameof(Price)));
            DeliveryPrice = deliveryPrice >= 0
                ? deliveryPrice
                : throw new NegativeNumberInvalidException(string.Format(NegativeNumberInvalidException.MessageTemplate, nameof(DeliveryPrice)));
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public decimal Price { get; private set; }

        public decimal DeliveryPrice { get; private set; }
    }
}
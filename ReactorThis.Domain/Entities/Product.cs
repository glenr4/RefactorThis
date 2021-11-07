using RefactorThis.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

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
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException(nameof(name));
            Description = !string.IsNullOrWhiteSpace(description) ? description : throw new ArgumentException(nameof(description));
            Price = price >= 0 ? price : throw new NegativeNumberInvalidException(nameof(Price));
            DeliveryPrice = deliveryPrice >= 0 ? deliveryPrice : throw new NegativeNumberInvalidException(nameof(DeliveryPrice));
            _productOptions = new List<ProductOption>();
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public decimal Price { get; private set; }

        public decimal DeliveryPrice { get; private set; }

        private readonly List<ProductOption> _productOptions;
        public IReadOnlyCollection<ProductOption> ProductOptions => _productOptions;

        public void AddProductOption(string name, string description)
        {
            _productOptions.Add(new ProductOption(this.Id, name, description));
        }

        public void RemoveProductOption(Guid id)
        {
            var productOption = _productOptions.Where(po => po.Id == id).FirstOrDefault();

            if (productOption == null) throw new ProductNotFoundException(id.ToString());

            _productOptions.Remove(productOption);
        }
    }
}
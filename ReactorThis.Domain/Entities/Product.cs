using RefactorThis.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RefactorThis.Domain.Entities
{
    public class Product

    {
        private Product()
        {
        }

        public Product(string name, string description, decimal price, decimal deliveryPrice)
        {
            Id = new Guid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
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

        public void AddProductOption(string name, string description, Guid? productOptionId = null)
        {
            _productOptions.Add(new ProductOption(productOptionId, this.Id, name, description));
        }

        public void RemoveProductOption(Guid Id)
        {
            var productOption = _productOptions.Where(po => po.Id == Id).FirstOrDefault();

            if (productOption == null) throw new KeyNotFoundException();

            _productOptions.Remove(productOption);
        }
    }
}
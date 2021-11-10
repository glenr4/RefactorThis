using RefactorThis.Domain.Entities;
using System;

namespace RefactorThis.API.Tests
{
    public class TestProduct : Product
    {
        public TestProduct(Guid id, string name, string description, decimal price, decimal deliveryPrice) : base(id, name, description, price, deliveryPrice)
        {
        }
    }
}
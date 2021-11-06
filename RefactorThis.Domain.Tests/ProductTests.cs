using FluentAssertions;
using RefactorThis.Domain.Entities;
using System;
using Xunit;

namespace RefactorThis.Domain.Tests
{
    public class ProductTests
    {
        [Theory]
        [InlineData("", "description", 0, 0)]
        public void GivenStringArgumentInvalid_WhenConstructProduct_ThenThrowsException(string name, string description, decimal price, decimal deliveryPrice)
        {
            Action act = () => new Product(name, description, price, deliveryPrice);

            act.Should().Throw<ArgumentException>();
        }
    }
}
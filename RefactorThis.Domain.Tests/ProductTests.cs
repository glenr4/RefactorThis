using AutoFixture;
using FluentAssertions;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Exceptions;
using System;
using Xunit;

namespace RefactorThis.Domain.Tests
{
    public class ProductTests
    {
        private Fixture _fixture;

        public ProductTests()
        {
            _fixture = new Fixture();
        }

        [Theory]
        [InlineData("", "description")]
        [InlineData(" ", "description")]
        [InlineData(null, "description")]
        [InlineData("name", "")]
        [InlineData("name", " ")]
        [InlineData("name", null)]
        public void GivenStringArgumentEmpty_WhenConstructProduct_ThenThrowsException(string name, string description)
        {
            Action act = () => new Product(Guid.NewGuid(), name, description, 0, 0);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(-1, -2.1)]
        [InlineData(0, -2.1)]
        public void GivenPriceArgumentIsNegative_WhenConstructProduct_ThenThrowsException(decimal price, decimal deliveryPrice)
        {
            Action act = () => new Product(Guid.NewGuid(), "name", "description", price, deliveryPrice);

            act.Should().Throw<NegativeNumberInvalidException>();
        }

        [Fact]
        public void GivenIdIsEmpty_WhenConstructProduct_ThenProductCreatedWithNewId()
        {
            // Arrange
            var id = Guid.Empty;
            var name = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            decimal price = 1;
            decimal deliveryPrice = 2;

            // Act
            var product = new Product(id, name, description, price, deliveryPrice);

            // Assert
            product.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void GivenArgumentsAreValid_WhenConstructProduct_ThenProductCreatedWithProperties()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            decimal price = 1;
            decimal deliveryPrice = 2;

            // Act
            var product = new Product(id, name, description, price, deliveryPrice);

            // Assert
            product.Id.Should().Be(id);
            product.Name.Should().Be(name);
            product.Description.Should().Be(description);
            product.Price.Should().Be(price);
            product.DeliveryPrice.Should().Be(deliveryPrice);
        }
    }
}
using AutoFixture;
using FluentAssertions;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [InlineData("", "description", 0, 0)]
        [InlineData(" ", "description", 0, 0)]
        [InlineData(null, "description", 0, 0)]
        [InlineData("name", "", 0, 0)]
        [InlineData("name", " ", 0, 0)]
        [InlineData("name", null, 0, 0)]
        public void GivenStringArgumentEmpty_WhenConstructProduct_ThenThrowsException(string name, string description, decimal price, decimal deliveryPrice)
        {
            Action act = () => new Product(name, description, price, deliveryPrice);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("name", "description", -1, 0)]
        [InlineData("name", "description", -1, -2.1)]
        [InlineData("name", "description", 0, -2.1)]
        public void GivenPriceArgumentIsNegative_WhenConstructProduct_ThenThrowsException(string name, string description, decimal price, decimal deliveryPrice)
        {
            Action act = () => new Product(name, description, price, deliveryPrice);

            act.Should().Throw<NegativeNumberInvalidException>();
        }

        [Fact]
        public void GivenArgumentsAreValid_WhenConstructProduct_ThenProductCreatedWithProperties()
        {
            // Arrange
            var name = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            decimal price = 1;
            decimal deliveryPrice = 2;

            // Act
            var product = new Product(name, description, price, deliveryPrice);

            // Assert
            product.Id.Should().NotBeEmpty();
            product.Name.Should().Be(name);
            product.Description.Should().Be(description);
            product.Price.Should().Be(price);
            product.DeliveryPrice.Should().Be(deliveryPrice);
            product.ProductOptions.Should().NotBeNull();
            product.ProductOptions.Should().BeEmpty();
        }

        [Fact]
        public void GivenValidProductOptionArguments_WhenAddProductOption_ThenProductOptionAdded()
        {
            // Arrange
            var product = _fixture.Create<Product>();
            var name = _fixture.Create<string>();
            var description = _fixture.Create<string>();

            // Act
            product.AddProductOption(name, description);

            // Assert
            product.ProductOptions.Should().ContainSingle();
            var actualProductOption = product.ProductOptions.FirstOrDefault();
            actualProductOption.Name.Should().Be(name);
            actualProductOption.Description.Should().Be(description);
            actualProductOption.ProductId.Should().Be(product.Id);
        }

        [Fact]
        public void GivenProductOptionExists_WhenRemoveProductOption_ThenProductOptionRemoved()
        {
            // Arrange
            var product = _fixture.Create<Product>();
            product.AddProductOption(_fixture.Create<string>(), _fixture.Create<string>());
            product.AddProductOption(_fixture.Create<string>(), _fixture.Create<string>());
            product.AddProductOption(_fixture.Create<string>(), _fixture.Create<string>());

            product.ProductOptions.Should().HaveCount(3);

            // Act
            var productOption = product.ProductOptions.FirstOrDefault();
            product.RemoveProductOption(productOption.Id);

            // Assert
            product.ProductOptions.Should().HaveCount(2);
            product.ProductOptions.Should().NotContain(x => x.Id == productOption.Id);
        }

        [Fact]
        public void GivenProductOptionDoesNotExist_WhenRemoveProductOption_ThenThrowException()
        {
            // Arrange
            var product = _fixture.Create<Product>();
            product.AddProductOption(_fixture.Create<string>(), _fixture.Create<string>());
            product.AddProductOption(_fixture.Create<string>(), _fixture.Create<string>());
            product.AddProductOption(_fixture.Create<string>(), _fixture.Create<string>());

            product.ProductOptions.Should().HaveCount(3);

            // Act
            Action act = () => product.RemoveProductOption(Guid.NewGuid());

            // Assert
            act.Should().Throw<KeyNotFoundException>();
            product.ProductOptions.Should().HaveCount(3);
        }
    }
}
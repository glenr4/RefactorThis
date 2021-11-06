using AutoFixture;
using FluentAssertions;
using RefactorThis.Domain.Entities;
using System;
using Xunit;

namespace RefactorThis.Domain.Tests
{
    public class ProductOptionTests
    {
        private Fixture _fixture;

        public ProductOptionTests()
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
        public void GivenStringArgumentEmpty_WhenConstructProductOption_ThenThrowsException(string name, string description)
        {
            Action act = () => new ProductOption(Guid.NewGuid(), name, description);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenProductIdEmpty_WhenConstructProductOption_ThenThrowsException()
        {
            Action act = () => new ProductOption(Guid.Empty, _fixture.Create<string>(), _fixture.Create<string>());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenArgumentsAreValid_WhenConstructProductOption_ThenProductOptionCreatedWithProperties()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var name = _fixture.Create<string>();
            var description = _fixture.Create<string>();

            // Act
            var productOption = new ProductOption(productId, name, description);

            // Assert
            productOption.Id.Should().NotBeEmpty();
            productOption.ProductId.Should().Be(productId);
            productOption.Name.Should().Be(name);
            productOption.Description.Should().Be(description);
        }
    }
}
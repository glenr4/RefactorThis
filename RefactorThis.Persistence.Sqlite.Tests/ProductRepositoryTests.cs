using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.Persistence.Sqlite.Tests
{
    public class ProductRepositoryTests
    {
        private Fixture _fixture;

        public ProductRepositoryTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GivenProductExists_WhenCreateProductOption_ThenProductOptionAddedToProduct()
        {
            // Arrange
            var product = _fixture.Create<Product>();

            using (var context = CreateDbContext())
            {
                context.Products.Add(product);

                context.SaveChanges();
            }

            // Act
            var productOption = new ProductOption(product.Id, _fixture.Create<string>(), _fixture.Create<string>());
            using (var context = CreateDbContext())
            {
                var productRepository = CreateProductRepository(context);

                var result = await productRepository.CreateProductOption(product.Id, productOption);

                // Assert
                result.Should().BeEquivalentTo(productOption);

                var actualResult = context.ProductOptions.Where(po => po.Id == productOption.Id).FirstOrDefault();
                actualResult.Should().BeEquivalentTo(productOption);
            }
        }

        [Fact]
        public void GivenProductIdDoesNotMatch_WhenCreateProductOption_ThenThrowsException()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GivenProductDoesNotExist_WhenCreateProductOption_ThenThrowsException()
        {
            throw new NotImplementedException();
        }

        private RefactorThisDbContext CreateDbContext()
        {
            return new RefactorThisDbContext(
                        new DbContextOptionsBuilder<RefactorThisDbContext>()
                            .UseInMemoryDatabase(databaseName: "inMemDb")
                            .Options);
        }

        private ProductRepository CreateProductRepository(RefactorThisDbContext context)
        {
            return new ProductRepository(context);
        }
    }
}
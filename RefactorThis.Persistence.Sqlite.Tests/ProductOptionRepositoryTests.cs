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
    public class ProductOptionRepositoryTests
    {
        private Fixture _fixture;

        public ProductOptionRepositoryTests()
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
                var productRepository = new ProductOptionRepository(context);

                var result = await productRepository.CreateProductOptionAsync(productOption);

                // Assert
                result.Should().BeEquivalentTo(productOption);

                var actualResult = context.ProductOptions.Where(po => po.Id == productOption.Id).FirstOrDefault();
                actualResult.Should().BeEquivalentTo(productOption);
            }
        }

        [Fact]
        public async Task GivenProductDoesNotExist_WhenCreateProductOption_ThenThrowsException()
        {
            // Arrange
            var product = _fixture.Create<Product>();

            // Act
            var productOption = new ProductOption(product.Id, _fixture.Create<string>(), _fixture.Create<string>());
            using (var context = CreateDbContext())
            {
                var productRepository = new ProductOptionRepository(context);

                Func<Task> act = async () => await productRepository.CreateProductOptionAsync(productOption);

                // Assert
                await act.Should().ThrowAsync<DbUpdateException>();
            }
        }

        private RefactorThisDbContext CreateDbContext()
        {
            return new RefactorThisDbContext(
                        new DbContextOptionsBuilder<RefactorThisDbContext>()
                            .UseInMemoryDatabase(databaseName: "inMemDb")
                            .Options);
        }
    }
}
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Entities;
using RefactorThis.Domain.Exceptions;
using System;
using System.Collections.Generic;
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
        public async Task GivenProductOptionExists_WhenGetProductOption_ThenProductOptionReturned()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var productOption = _fixture.Create<ProductOption>();

            using (var context = CreateDbContext(dbName))
            {
                context.ProductOptions.Add(productOption);

                context.SaveChanges();
            }

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductOptionRepository(context);

                var result = await productRepository.GetProductOptionAsync(productOption.ProductId, productOption.Id);

                // Assert
                result.Should().BeEquivalentTo(productOption);
            }
        }

        [Fact]
        public async Task GivenProductOptionDoesNotExist_WhenGetProductOption_ThenThrowException()
        {
            // Arrange
            var dbName = _fixture.Create<string>();

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductOptionRepository(context);

                Func<Task> act = async () => await productRepository.GetProductOptionAsync(Guid.NewGuid(), Guid.NewGuid());

                // Assert
                await act.Should().ThrowExactlyAsync<ProductOptionNotFoundException>();
            }
        }

        [Fact]
        public async Task GivenProductOptionsExist_WhenGetAllProductOptions_ThenPagedProductOptionsReturned()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var product = _fixture.Create<Product>();
            var productOptions = new List<ProductOption> {
                CreateProductOption(product.Id),
                CreateProductOption(product.Id),
                CreateProductOption(product.Id),
                CreateProductOption(product.Id),
                CreateProductOption(product.Id),
            };

            using (var context = CreateDbContext(dbName))
            {
                context.ProductOptions.AddRange(productOptions);

                context.SaveChanges();
            }

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductOptionRepository(context);

                var result = await productRepository.GetAllProductOptionsAsync(product.Id, page: 1, pageSize: productOptions.Count);

                // Assert
                result.Items.Should().BeEquivalentTo(productOptions);
                result.Should().BeOfType<PagedList<ProductOption>>();
            }
        }

        [Fact]
        public async Task GivenNoProductOptionsExist_WhenGetAllProductOptions_ThenEmptyPagedProductOptionsReturned()
        {
            // Arrange
            var dbName = _fixture.Create<string>();

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductOptionRepository(context);

                var result = await productRepository.GetAllProductOptionsAsync(Guid.NewGuid());

                // Assert
                result.Items.Should().BeEmpty();
                result.Should().BeOfType<PagedList<ProductOption>>();
            }
        }

        [Fact]
        public async Task GivenProductExists_WhenCreateProductOption_ThenProductOptionAddedToProduct()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var product = _fixture.Create<Product>();

            using (var context = CreateDbContext(dbName))
            {
                context.Products.Add(product);

                context.SaveChanges();
            }

            // Act
            var productOption = new ProductOption(Guid.NewGuid(), product.Id, _fixture.Create<string>(), _fixture.Create<string>());
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductOptionRepository(context);

                var result = await productRepository.CreateProductOptionAsync(productOption);

                // Assert
                result.Should().BeEquivalentTo(productOption);
            }
            using (var context = CreateDbContext(dbName))
            {
                var actualResult = context.ProductOptions.Where(po => po.Id == productOption.Id).FirstOrDefault();
                actualResult.Should().BeEquivalentTo(productOption);
            }
        }

        [Fact]
        public async Task GivenProductDoesNotExist_WhenCreateProductOption_ThenThrowsException()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var product = _fixture.Create<Product>();

            // Act
            var productOption = new ProductOption(Guid.NewGuid(), product.Id, _fixture.Create<string>(), _fixture.Create<string>());
            using (var context = CreateDbContext(dbName))
            {
                var productRepository = new ProductOptionRepository(context);

                Func<Task> act = async () => await productRepository.CreateProductOptionAsync(productOption);

                // Assert
                await act.Should().ThrowAsync<DbUpdateException>();
            }
        }

        [Fact]
        public async Task GivenProductOptionAlreadyExists_WhenCreateProductOption_ThenThrowException()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var product = _fixture.Create<Product>();
            var productOption = CreateProductOption(product.Id);

            using (var context = CreateDbContext(dbName))
            {
                context.Products.Add(product);
                context.ProductOptions.Add(productOption);

                context.SaveChanges();
            }

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var ProductOptionRepository = new ProductOptionRepository(context);

                Func<Task> act = async () => await ProductOptionRepository.CreateProductOptionAsync(productOption);

                // Assert
                await act.Should().ThrowExactlyAsync<DbUpdateException>();
            }
        }

        [Fact]
        public async Task GivenProductOptionExists_WhenUpdateProductOption_ThenProductOptionUpdated()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var existingProductOption = _fixture.Create<ProductOption>();

            using (var context = CreateDbContext(dbName))
            {
                context.Add(existingProductOption);

                context.SaveChanges();
            }

            // Act
            var productOption = new ProductOption(existingProductOption.Id, existingProductOption.Id, _fixture.Create<string>(), _fixture.Create<string>());

            using (var context = CreateDbContext(dbName))
            {
                var ProductOptionRepository = new ProductOptionRepository(context);

                var result = await ProductOptionRepository.UpdateProductOptionAsync(productOption);

                // Assert
                result.Should().BeEquivalentTo(productOption);
            }

            using (var context = CreateDbContext(dbName))
            {
                var updatedProductOption = context.ProductOptions.Where(p => p.Id == existingProductOption.Id).FirstOrDefault();

                updatedProductOption.Should().BeEquivalentTo(productOption);
            }
        }

        [Fact]
        public async Task GivenProductOptionDoesNotExist_WhenUpdateProductOption_ThenThrowException()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var roductOption = _fixture.Create<ProductOption>();

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var ProductOptionRepository = new ProductOptionRepository(context);

                Func<Task> act = async () => await ProductOptionRepository.UpdateProductOptionAsync(roductOption);

                // Assert
                await act.Should().ThrowExactlyAsync<DbUpdateConcurrencyException>();
            }
        }

        [Fact]
        public async Task GivenProductOptionExists_WhenDeleteProductOption_ThenProductOptionDeleted()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var productOptions = new List<ProductOption>{
                _fixture.Create<ProductOption>(),
                _fixture.Create<ProductOption>(),
                _fixture.Create<ProductOption>(),
            };

            using (var context = CreateDbContext(dbName))
            {
                context.AddRange(productOptions);

                context.SaveChanges();
            }

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var ProductOptionRepository = new ProductOptionRepository(context);

                var result = await ProductOptionRepository.DeleteProductOptionAsync(productOptions[0].ProductId, productOptions[0].Id);

                // Assert
                result.Should().Be(productOptions[0].Id);
            }

            using (var context = CreateDbContext(dbName))
            {
                var remainingProductOptions = context.ProductOptions.ToList();

                remainingProductOptions.Should().HaveCount(2);
                remainingProductOptions.Should().NotContain(productOptions[0]);
            }
        }

        [Fact]
        public async Task GivenProductOptionDoesNotExist_WhenDeleteProductOption_ThenThrowException()
        {
            // Arrange
            var dbName = _fixture.Create<string>();
            var productOptions = new List<ProductOption>{
                _fixture.Create<ProductOption>(),
                _fixture.Create<ProductOption>(),
                _fixture.Create<ProductOption>(),
            };

            using (var context = CreateDbContext(dbName))
            {
                context.AddRange(productOptions);

                context.SaveChanges();
            }

            // Act
            using (var context = CreateDbContext(dbName))
            {
                var ProductOptionRepository = new ProductOptionRepository(context);

                //Func<Task> act = async () => await ProductOptionRepository.DeleteProductOptionAsync(Guid.NewGuid());

                //// Assert
                //await act.Should().ThrowExactlyAsync<DbUpdateConcurrencyException>();
            }
        }

        private RefactorThisDbContext CreateDbContext(string dbName)
        {
            return new RefactorThisDbContext(
                        new DbContextOptionsBuilder<RefactorThisDbContext>()
                            .UseInMemoryDatabase(databaseName: dbName)
                            .Options);
        }

        private ProductOption CreateProductOption(Guid productId)
        {
            return new ProductOption(Guid.NewGuid(), productId, _fixture.Create<string>(), _fixture.Create<string>());
        }
    }
}
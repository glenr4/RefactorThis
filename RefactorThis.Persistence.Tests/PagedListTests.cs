using AutoFixture;
using FluentAssertions;
using MockQueryable.Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RefactorThis.Persistence.Tests
{
    public class PagedListTests
    {
        private Fixture _fixture;

        public PagedListTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task WhenToPagedList_ThenPropertiesSet()
        {
            // Arrange
            var items = _fixture.CreateMany<string>(10).ToList();
            var mockQueryable = items.AsQueryable().BuildMock();
            var pageSize = 10;
            var pageNumber = 1;

            // Act
            var pagedList = await PagedList<string>.ToPagedListAsync(mockQueryable.Object, pageNumber, pageSize);

            // Assert
            pagedList.Items.Should().BeEquivalentTo(items);
            pagedList.TotalCount.Should().Be(items.Count);
            pagedList.PageSize.Should().Be(pageSize);
            pagedList.CurrentPage.Should().Be(pageNumber);
            pagedList.TotalPages.Should().Be(items.Count / pageSize);
        }

        [Theory]
        [InlineData(100, 10, 10)]
        [InlineData(101, 10, 11)]
        [InlineData(99, 10, 10)]
        public async Task GivenListItems_WhenToPagedList_ThenTotalPagesIsCalculatedAsRoundedUpCountDividedByPageSize(int count, int pageSize, int expectedTotalPages)
        {
            // Arrange
            var items = _fixture.CreateMany<string>(count).ToList();
            var mockQueryable = items.AsQueryable().BuildMock();

            // Act
            var pagedList = await PagedList<string>.ToPagedListAsync(mockQueryable.Object, 1, pageSize);

            // Assert
            pagedList.TotalPages.Should().Be(expectedTotalPages);
        }

        [Theory]
        [InlineData(1, 10, 1, 10)]
        [InlineData(2, 10, 11, 20)]
        [InlineData(1, 5, 1, 5)]
        [InlineData(2, 5, 6, 10)]
        [InlineData(1, 100, 1, 20)]
        public async Task GivenListItemsMoreThanPageSize_WhenToPagedList_ThenCorrectPageReturned(int pageNumber, int pageSize, int firstItem, int lastItem)
        {
            // Arrange
            var items = new List<string>
            {
                "1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20"
            };
            var mockQueryable = items.AsQueryable().BuildMock();

            // Act
            var pagedList = await PagedList<string>.ToPagedListAsync(mockQueryable.Object, pageNumber, pageSize);

            // Assert
            var test = items.Where(i => Int32.Parse(i) >= firstItem && Int32.Parse(i) <= lastItem).ToList();
            pagedList.Items.Should().Contain(test);
        }
    }
}
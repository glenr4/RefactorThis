using RefactorThis.Persistence;
using System.Collections.Generic;

namespace RefactorThis.API.Tests
{
    /// <summary>
    /// Need a public constructor for deserialization
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TestPagedList<T> : PagedList<T>
    {
        public TestPagedList(List<T> items, int count, int pageNumber, int pageSize) : base(items, count, pageNumber, pageSize)
        {
        }
    }
}
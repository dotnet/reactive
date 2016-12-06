using System.Linq;
using Xunit;

namespace Tests
{
    public class AsyncEnumerableQueryTest
    {
        [Fact]
        public void CastToUntypedIAsyncQueryable()
        {
            var query = Enumerable.Empty<string>().ToAsyncEnumerable().AsAsyncQueryable();

            Assert.IsAssignableFrom<IAsyncQueryable>(query);
        }

        [Fact]
        public void CastToUntypedIOrderedAsyncQueryable()
        {
            var query = Enumerable.Empty<string>().ToAsyncEnumerable().AsAsyncQueryable().OrderBy(s => s.Length);

            Assert.IsAssignableFrom<IOrderedAsyncQueryable>(query);
        }
    }
}

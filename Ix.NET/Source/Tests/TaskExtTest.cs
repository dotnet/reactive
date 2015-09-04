using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public partial class AsyncTests
    {
        [TestMethod]
        public async Task ExceptionHandling_ShouldThrowUnwrappedException()
        {
            try
            {
                var asyncEnumerable = AsyncEnumerable.ToAsyncEnumerable(GetEnumerableWithError());
                await asyncEnumerable.ToArray();
            }
            catch (AggregateException)
            {
                Assert.Fail("AggregateException has been thrown instead of SpecificException");
            }
            catch (SpecificException)
            {
            }
        }
        [TestMethod]
        public async Task ExceptionHandling_ShouldThrowUnwrappedException2()
        {
            try
            {
                var asyncEnumerable = AsyncEnumerable.Generate(10, (x) => { throw new SpecificException(); }, (x) => { return 1; }, (x) => { return 2; });
                await asyncEnumerable.ToArray();
            }
            catch (AggregateException)
            {
                Assert.Fail("AggregateException has been thrown instead of SpecificException");
            }
            catch (SpecificException)
            {
            }
        }

        private IEnumerable<int> GetEnumerableWithError()
        {
            yield return 0;
            throw new SpecificException();
        }

        public class SpecificException : Exception { }
    }
}

#if HAS_AWAIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Assert.Fail("AggregateException has been thrown instead of InvalidOperationException");
            }
            catch (InvalidOperationException)
            {
            }
        }

        private IEnumerable<int> GetEnumerableWithError()
        {
            yield return 5;
            throw new InvalidOperationException();
        }

        [TestMethod]
        public async Task ExceptionHandling_ShouldThrowUnwrappedException2()
        {
            try
            {
                var asyncEnumerable = AsyncEnumerable.Generate(15, (x) => { throw new InvalidOperationException(); }, (x) => { return 20; }, (x) => { return 2; });
                await asyncEnumerable.ToArray();
            }
            catch (AggregateException)
            {
                Assert.Fail("AggregateException has been thrown instead of InvalidOperationException");
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
#endif

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
                Assert.Fail();
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

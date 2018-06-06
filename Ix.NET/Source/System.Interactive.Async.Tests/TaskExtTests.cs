// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    public partial class AsyncTests
    {
        [Fact]
        public async Task ExceptionHandling_ShouldThrowUnwrappedException()
        {
            try
            {
                var asyncEnumerable = AsyncEnumerable.ToAsyncEnumerable(GetEnumerableWithError());
                await asyncEnumerable.ToArray();
            }
            catch (AggregateException)
            {
                Assert.True(false, "AggregateException has been thrown instead of InvalidOperationException");
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

        [Fact]
        public async Task ExceptionHandling_ShouldThrowUnwrappedException2()
        {
            try
            {
                var asyncEnumerable = AsyncEnumerable.Generate(15, (x) => { throw new InvalidOperationException(); }, (x) => { return 20; }, (x) => { return 2; });
                await asyncEnumerable.ToArray();
            }
            catch (AggregateException)
            {
                Assert.True(false, "AggregateException has been thrown instead of InvalidOperationException");
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}


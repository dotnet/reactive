// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

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

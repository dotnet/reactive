// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Return : AsyncEnumerableExTests
    {
        [Fact]
        public async Task Return1Async()
        {
            var xs = AsyncEnumerableEx.Return(42);
            await HasNextAsync(xs.GetAsyncEnumerator(), 42);
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Never : AsyncEnumerableExTests
    {
        [Fact]
        public async Task Never1()
        {
            var xs = AsyncEnumerableEx.Never<int>();

            var e = xs.GetAsyncEnumerator();
            Assert.False(e.MoveNextAsync().IsCompleted); // Very rudimentary check
            await e.DisposeAsync();
        }

        [Fact]
        public async Task CancelToken_UnblocksAsync()
        {
            using var cts = new CancellationTokenSource();

            var en = AsyncEnumerableEx.Never<int>().GetAsyncEnumerator(cts.Token);

            try
            {
                cts.CancelAfter(100);

                await Assert.ThrowsAsync<TaskCanceledException>(() => en.MoveNextAsync().AsTask());
            }
            finally
            {
                await en.DisposeAsync();
            }
        }
    }
}

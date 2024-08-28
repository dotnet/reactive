// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if NET6_0_OR_GREATER
#pragma warning disable CA2012 // Use ValueTasks correctly. These tests need to use Result to verify correct operation, so we can't avoid breaking this rule.
#endif

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

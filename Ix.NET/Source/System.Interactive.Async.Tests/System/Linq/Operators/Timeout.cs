// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Timeout : AsyncEnumerableExTests
    {
        [Fact]
        public async Task Timeout_Never()
        {
            var source = AsyncEnumerableEx.Never<int>().Timeout(TimeSpan.FromMilliseconds(100));

            var en = source.GetAsyncEnumerator();

            try
            {
                await en.MoveNextAsync();

                Assert.False(true, "MoveNextAsync should have thrown");
            }
            catch (TimeoutException)
            {
                // expected
            }
            finally
            {
                await en.DisposeAsync();
            }
        }

        [Fact]
        public async Task Timeout_Double_Never()
        {
            var source = AsyncEnumerableEx.Never<int>()
                .Timeout(TimeSpan.FromMilliseconds(300))
                .Timeout(TimeSpan.FromMilliseconds(100));

            var en = source.GetAsyncEnumerator();

            try
            {
                await en.MoveNextAsync();

                Assert.False(true, "MoveNextAsync should have thrown");
            }
            catch (TimeoutException)
            {
                // expected
            }
            finally
            {
                await en.DisposeAsync();
            }
        }

        [Fact]
        public async Task Timeout_Delayed_Main()
        {
            var source = AsyncEnumerable.Range(1, 5)
                .SelectAwait(async v =>
                {
                    await Task.Delay(300);
                    return v;
                })
                .Timeout(TimeSpan.FromMilliseconds(100));

            var en = source.GetAsyncEnumerator();

            try
            {
                await en.MoveNextAsync();

                Assert.False(true, "MoveNextAsync should have thrown");
            }
            catch (TimeoutException)
            {
                // expected
            }
            finally
            {
                await en.DisposeAsync();
            }
        }

        [Fact]
        public async Task Timeout_Delayed_Main_Canceled()
        {
            var tcs = new TaskCompletionSource<int>();

            var source = AsyncEnumerable.Range(1, 5)
                .SelectAwaitWithCancellation(async (v, ct) =>
                {
                    try
                    {
                        await Task.Delay(500, ct);
                    }
                    catch (TaskCanceledException)
                    {
                        tcs.SetResult(0);
                    }
                    return v;
                })
                .Timeout(TimeSpan.FromMilliseconds(250));

            var en = source.GetAsyncEnumerator();

            try
            {
                await en.MoveNextAsync();

                Assert.False(true, "MoveNextAsync should have thrown");
            }
            catch (TimeoutException)
            {
                // expected
            }
            finally
            {
                await en.DisposeAsync();
            }

            Assert.Equal(0, await tcs.Task);
        }
    }
}

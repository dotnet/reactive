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
    public class Never : AsyncEnumerableExTests
    {
        [Fact]
        public async Task Never1()
        {
            var xs = AsyncEnumerableEx.Never<int>();

            var e = xs.GetAsyncEnumerator();
            Assert.False(e.MoveNextAsync().IsCompleted); // Very rudimentary check
            AssertThrows<InvalidOperationException>(() => Nop(e.Current));
            await e.DisposeAsync();
        }

        private void Nop(object o)
        {
        }

        [Fact]
        public void CancelToken_Unblocks()
        {
            var cts = new CancellationTokenSource();

            var en = AsyncEnumerableEx.Never<int>().GetAsyncEnumerator(cts.Token);

            try
            {
                var t = Task.Run(async () =>
                {
                    await Task.Delay(100);
                    cts.Cancel();
                });

                try
                {
                    Assert.True(en.MoveNextAsync().AsTask().Wait(2000));
                }
                catch (AggregateException ex)
                {
                    if (!(ex.InnerException is TaskCanceledException))
                    {
                        throw;
                    }
                }
            }
            finally
            {
                en.DisposeAsync().AsTask().Wait(2000);
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class AsyncEnumerableExTests
    {
        protected static readonly IAsyncEnumerable<int> Return42 = AsyncEnumerableEx.Return(42);
        protected static IAsyncEnumerable<T> Throw<T>(Exception exception) => AsyncEnumerableEx.Throw<T>(exception);

        protected async Task AssertThrowsAsync<TException>(Task t)
            where TException : Exception
        {
            await Assert.ThrowsAsync<TException>(() => t);
        }

        protected async Task AssertThrowsAsync(Task t, Exception e)
        {
            try
            {
                await t;
            }
            catch (Exception ex)
            {
                Assert.Same(e, ex);
            }
        }

        protected Task AssertThrowsAsync<T>(ValueTask<T> t, Exception e)
        {
            return AssertThrowsAsync(t.AsTask(), e);
        }

        protected async Task NoNextAsync<T>(IAsyncEnumerator<T> e)
        {
            Assert.False(await e.MoveNextAsync());
        }

        protected async Task HasNextAsync<T>(IAsyncEnumerator<T> e, T value)
        {
            Assert.True(await e.MoveNextAsync());
            Assert.Equal(value, e.Current);
        }

        protected async Task SequenceIdentity<T>(IAsyncEnumerable<T> enumerable)
        {
            var en1 = enumerable.GetAsyncEnumerator();
            var en2 = enumerable.GetAsyncEnumerator();

            Assert.Equal(en1.GetType(), en2.GetType());

            await en1.DisposeAsync();
            await en2.DisposeAsync();

            var res1 = await enumerable.ToListAsync();
            var res2 = await enumerable.ToListAsync();

            res1.Should().BeEquivalentTo(res2);
        }
    }
}

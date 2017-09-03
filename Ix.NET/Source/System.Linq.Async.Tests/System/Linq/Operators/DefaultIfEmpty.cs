// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class DefaultIfEmpty : AsyncEnumerableTests
    {
        [Fact]
        public void DefaultIfEmpty_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.DefaultIfEmpty<int>(default(IAsyncEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.DefaultIfEmpty<int>(default(IAsyncEnumerable<int>), 42));
        }

        [Fact]
        public void DefaultIfEmpty1()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty();

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 0);
            NoNext(e);
        }

        [Fact]
        public void DefaultIfEmpty2()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty(42);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 42);
            NoNext(e);
        }

        [Fact]
        public void DefaultIfEmpty3()
        {
            var xs = AsyncEnumerable.Return<int>(42).DefaultIfEmpty();

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 42);
            NoNext(e);
        }

        [Fact]
        public void DefaultIfEmpty4()
        {
            var xs = AsyncEnumerable.Return<int>(42).DefaultIfEmpty(24);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 42);
            NoNext(e);
        }

        [Fact]
        public void DefaultIfEmpty5()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty();

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void DefaultIfEmpty6()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void DefaultIfEmpty7()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex).DefaultIfEmpty();

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void DefaultIfEmpty8()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex).DefaultIfEmpty(24);

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task DefaultIfEmpty9()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty(42);

            var res = new[] { 42 };

            Assert.True(res.SequenceEqual(await xs.ToArray()));
        }

        [Fact]
        public async Task DefaultIfEmpty10()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty(42);

            var res = new List<int> { 42 };

            Assert.True(res.SequenceEqual(await xs.ToList()));
        }

        [Fact]
        public async Task DefaultIfEmpty11()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty(42);

            Assert.Equal(1, await xs.Count());
        }


        [Fact]
        public async Task DefaultIfEmpty12()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            var res = new[] { 1, 2, 3, 4 };

            Assert.True(res.SequenceEqual(await xs.ToArray()));
        }

        [Fact]
        public async Task DefaultIfEmpty13()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            var res = new List<int> { 1, 2, 3, 4 };

            Assert.True(res.SequenceEqual(await xs.ToList()));
        }

        [Fact]
        public async Task DefaultIfEmpty14()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            Assert.Equal(4, await xs.Count());
        }

        [Fact]
        public async Task DefaultIfEmpty15()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            await SequenceIdentity(xs);
        }
    }
}

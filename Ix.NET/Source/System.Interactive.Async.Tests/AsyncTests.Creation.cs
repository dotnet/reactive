// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public partial class AsyncTests
    {

        [Fact]
        public void Create_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.CreateEnumerable<int>(default(Func<IAsyncEnumerator<int>>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.CreateEnumerator<int>(null, () => 3, () => Task.FromResult(true)));
        }

        [Fact]
        public void Create_Iterator_Throws()
        {
            var iter = AsyncEnumerable.CreateEnumerator<int>(() => Task.FromResult(true), () => 3, () => Task.FromResult(true));

            var enu = (IAsyncEnumerable<int>)iter;

            AssertThrows<NotSupportedException>(() => enu.GetAsyncEnumerator());
        }

        [Fact]
        public void Return()
        {
            var xs = AsyncEnumerable.Return(42);
            HasNext(xs.GetAsyncEnumerator(), 42);
        }

        [Fact]
        public async Task Never()
        {
            var xs = AsyncEnumerableEx.Never<int>();

            var e = xs.GetAsyncEnumerator();
            Assert.False(e.MoveNextAsync().IsCompleted); // Very rudimentary check
            AssertThrows<InvalidOperationException>(() => Nop(e.Current));
            await e.DisposeAsync();
        }

        [Fact]
        public void Throw_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Throw<int>(null));
        }

        [Fact]
        public void Throw()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).InnerExceptions.Single() == ex);
            AssertThrows<InvalidOperationException>(() => Nop(e.Current));
        }

        private void Nop(object o)
        {
        }

        [Fact]
        public void Range_Null()
        {
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.Range(0, -1));
        }

        [Fact]
        public void Range1()
        {
            var xs = AsyncEnumerable.Range(2, 5);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public void Range2()
        {
            var xs = AsyncEnumerable.Range(2, 0);

            var e = xs.GetAsyncEnumerator();
            NoNext(e);
        }

        [Fact]
        public void Repeat_Null()
        {
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.Repeat(0, -1));
        }

        [Fact]
        public void Repeat1()
        {
            var xs = AsyncEnumerable.Repeat(2, 5);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void Repeat2()
        {
            var xs = AsyncEnumerable.Repeat(2, 0);

            var e = xs.GetAsyncEnumerator();
            NoNext(e);
        }

        [Fact]
        public async Task Repeat3()
        {
            var xs = AsyncEnumerableEx.Repeat(2);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            await e.DisposeAsync();
        }

        [Fact]
        public async Task Repeat4()
        {
            var xs = AsyncEnumerableEx.Repeat(2).Take(5);

            await SequenceIdentity(xs);
        }
    }
}

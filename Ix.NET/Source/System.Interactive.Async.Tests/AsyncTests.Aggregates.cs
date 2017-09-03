// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantTypeArgumentsOfMethod

namespace Tests
{
    public partial class AsyncTests
    {
        private const int WaitTimeoutMs = 5000;

        [Fact]
        public async Task IsEmpty_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.IsEmpty<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.IsEmpty<int>(null, CancellationToken.None));
        }

        [Fact]
        public async Task Min_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Min(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Min(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Min(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Min(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>), CancellationToken.None));
        }

        [Fact]
        public async Task Max_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Max(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Max(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Max(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.Max(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>), CancellationToken.None));
        }

        [Fact]
        public async Task MinBy_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(default(IAsyncEnumerable<int>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(AsyncEnumerable.Return(42), default(Func<int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(AsyncEnumerable.Return(42), default(Func<int, int>), Comparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(AsyncEnumerable.Return(42), x => x, default(IComparer<int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(AsyncEnumerable.Return(42), default(Func<int, int>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(AsyncEnumerable.Return(42), default(Func<int, int>), Comparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinBy(AsyncEnumerable.Return(42), x => x, default(IComparer<int>), CancellationToken.None));
        }

        [Fact]
        public void MinBy1()
        {
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinBy(x => x / 2);
            var res = xs.Result;

            Assert.True(res.SequenceEqual(new[] { 3, 2 }));
        }

        [Fact]
        public void MinBy2()
        {
            var xs = new int[0].ToAsyncEnumerable().MinBy(x => x / 2);

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void MinBy3()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinBy(x => { if (x == 3) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void MinBy4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinBy(x => { if (x == 4) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void MinBy5()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().Concat(AsyncEnumerable.Throw<int>(ex)).MinBy(x => x, Comparer<int>.Default);

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task MaxBy_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(default(IAsyncEnumerable<int>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(AsyncEnumerable.Return(42), default(Func<int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(AsyncEnumerable.Return(42), default(Func<int, int>), Comparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(AsyncEnumerable.Return(42), x => x, default(IComparer<int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(AsyncEnumerable.Return(42), default(Func<int, int>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(AsyncEnumerable.Return(42), default(Func<int, int>), Comparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MaxBy(AsyncEnumerable.Return(42), x => x, default(IComparer<int>), CancellationToken.None));
        }

        [Fact]
        public void MaxBy1()
        {
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MaxBy(x => x / 2);
            var res = xs.Result;

            Assert.True(res.SequenceEqual(new[] { 7, 6 }));
        }

        [Fact]
        public void MaxBy2()
        {
            var xs = new int[0].ToAsyncEnumerable().MaxBy(x => x / 2);

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void MaxBy3()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MaxBy(x => { if (x == 3) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void MaxBy4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MaxBy(x => { if (x == 4) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void MaxBy5()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().Concat(AsyncEnumerable.Throw<int>(ex)).MaxBy(x => x, Comparer<int>.Default);

            AssertThrows<Exception>(() => xs.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        private sealed class Eq : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(Math.Abs(x), Math.Abs(y));
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(Math.Abs(obj));
            }
        }
    }
}

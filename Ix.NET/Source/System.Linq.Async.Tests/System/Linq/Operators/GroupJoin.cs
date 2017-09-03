// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class GroupJoin : AsyncEnumerableTests
    {
        [Fact]
        public void GroupJoin_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(null, AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), null, x => x, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, null, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(null, AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), null, x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, null, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x, default(IEqualityComparer<int>)));
        }

        [Fact]
        public void GroupJoin1()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 4, 7, 6, 2, 3, 4, 8, 9 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            HasNext(e, "0 - 639");
            HasNext(e, "1 - 474");
            HasNext(e, "2 - 28");
            NoNext(e);
        }

        [Fact]
        public void GroupJoin2()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            HasNext(e, "0 - 36");
            HasNext(e, "1 - 4");
            HasNext(e, "2 - ");
            NoNext(e);
        }

        [Fact]
        public void GroupJoin3()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void GroupJoin4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = AsyncEnumerable.Throw<int>(ex);

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void GroupJoin5()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => { throw ex; }, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void GroupJoin6()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => { throw ex; }, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void GroupJoin7()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) =>
            {
                if (x == 1)
                    throw ex;
                return x + " - " + i.Aggregate("", (s, j) => s + j).Result;
            });

            var e = res.GetAsyncEnumerator();
            HasNext(e, "0 - 36");
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}

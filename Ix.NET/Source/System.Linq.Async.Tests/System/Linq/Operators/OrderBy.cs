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
    public class OrderBy : AsyncEnumerableTests
    {
        [Fact]
        public void OrderBy_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderBy<int, int>(default(IAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderBy<int, int>(Return42, default(Func<int, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderBy<int, int>(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderBy<int, int>(Return42, default(Func<int, int>), Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderBy<int, int>(Return42, x => x, default(IComparer<int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending<int, int>(default(IAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending<int, int>(Return42, default(Func<int, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending<int, int>(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending<int, int>(Return42, default(Func<int, int>), Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending<int, int>(Return42, x => x, default(IComparer<int>)));

            var xs = Return42.OrderBy(x => x);

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenBy<int, int>(default(IOrderedAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenBy<int, int>(xs, default(Func<int, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenBy<int, int>(default(IOrderedAsyncEnumerable<int>), x => x, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenBy<int, int>(xs, default(Func<int, int>), Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenBy<int, int>(xs, x => x, default(IComparer<int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending<int, int>(default(IOrderedAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending<int, int>(xs, default(Func<int, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending<int, int>(default(IOrderedAsyncEnumerable<int>), x => x, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending<int, int>(xs, default(Func<int, int>), Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending<int, int>(xs, x => x, default(IComparer<int>)));
        }

        [Fact]
        public void OrderBy1()
        {
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy(x => x);

            var e = ys.GetAsyncEnumerator();
            for (var i = 0; i < 10; i++)
                HasNext(e, i);
            NoNext(e);
        }

        [Fact]
        public void OrderBy2()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy<int, int>(new Func<int, int>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public async Task OrderBy3()
        {
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy(x => x);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void ThenBy2()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy(x => x).ThenBy<int, int>(new Func<int, int>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void OrderByDescending1()
        {
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderByDescending(x => x);

            var e = ys.GetAsyncEnumerator();
            for (var i = 9; i >= 0; i--)
                HasNext(e, i);
            NoNext(e);
        }

        [Fact]
        public void OrderByDescending2()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderByDescending<int, int>(new Func<int, int>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public async Task OrderByDescending3()
        {
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderByDescending(x => x);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void ThenByDescending2()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy<int, int>(x => x).ThenByDescending<int, int>(new Func<int, int>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void OrderByThenBy1()
        {
            var xs = new[] {
                new { Name = "Bart", Age = 27 },
                new { Name = "John", Age = 62 },
                new { Name = "Eric", Age = 27 },
                new { Name = "Lisa", Age = 14 },
                new { Name = "Brad", Age = 27 },
                new { Name = "Lisa", Age = 23 },
                new { Name = "Eric", Age = 42 },
            };

            var ys = xs.ToAsyncEnumerable();

            var ress = xs.OrderBy(x => x.Name).ThenBy(x => x.Age);
            var resa = ys.OrderBy(x => x.Name).ThenBy(x => x.Age);

            Assert.True(ress.SequenceEqual(resa.ToEnumerable()));
        }

        [Fact]
        public void OrderByThenBy2()
        {
            var xs = new[] {
                new { Name = "Bart", Age = 27 },
                new { Name = "John", Age = 62 },
                new { Name = "Eric", Age = 27 },
                new { Name = "Lisa", Age = 14 },
                new { Name = "Brad", Age = 27 },
                new { Name = "Lisa", Age = 23 },
                new { Name = "Eric", Age = 42 },
            };

            var ys = xs.ToAsyncEnumerable();

            var ress = xs.OrderBy(x => x.Name).ThenByDescending(x => x.Age);
            var resa = ys.OrderBy(x => x.Name).ThenByDescending(x => x.Age);

            Assert.True(ress.SequenceEqual(resa.ToEnumerable()));
        }

        [Fact]
        public void OrderByThenBy3()
        {
            var xs = new[] {
                new { Name = "Bart", Age = 27 },
                new { Name = "John", Age = 62 },
                new { Name = "Eric", Age = 27 },
                new { Name = "Lisa", Age = 14 },
                new { Name = "Brad", Age = 27 },
                new { Name = "Lisa", Age = 23 },
                new { Name = "Eric", Age = 42 },
            };

            var ys = xs.ToAsyncEnumerable();

            var ress = xs.OrderByDescending(x => x.Name).ThenBy(x => x.Age);
            var resa = ys.OrderByDescending(x => x.Name).ThenBy(x => x.Age);

            Assert.True(ress.SequenceEqual(resa.ToEnumerable()));
        }

        [Fact]
        public void OrderByThenBy4()
        {
            var xs = new[] {
                new { Name = "Bart", Age = 27 },
                new { Name = "John", Age = 62 },
                new { Name = "Eric", Age = 27 },
                new { Name = "Lisa", Age = 14 },
                new { Name = "Brad", Age = 27 },
                new { Name = "Lisa", Age = 23 },
                new { Name = "Eric", Age = 42 },
            };

            var ys = xs.ToAsyncEnumerable();

            var ress = xs.OrderByDescending(x => x.Name).ThenByDescending(x => x.Age);
            var resa = ys.OrderByDescending(x => x.Name).ThenByDescending(x => x.Age);

            Assert.True(ress.SequenceEqual(resa.ToEnumerable()));
        }
    }
}

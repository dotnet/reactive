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
    public partial class OrderBy : AsyncEnumerableTests
    {
        [Fact]
        public void OrderBy_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.OrderBy<int, int>(default, x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.OrderBy(Return42, default(Func<int, int>)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.OrderBy<int, int>(default, x => x, Comparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.OrderBy(Return42, default, Comparer<int>.Default));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending<int, int>(default, x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending(Return42, default(Func<int, int>)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending<int, int>(default, x => x, Comparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending(Return42, default, Comparer<int>.Default));

            var xs = Return42.OrderBy(x => x);

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ThenBy<int, int>(default, x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ThenBy(xs, default(Func<int, int>)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ThenBy<int, int>(default, x => x, Comparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ThenBy(xs, default, Comparer<int>.Default));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending<int, int>(default, x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending(xs, default(Func<int, int>)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending<int, int>(default, x => x, Comparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending(xs, default, Comparer<int>.Default));
        }

        [Fact]
        public async Task OrderBy_Empty()
        {
            var ys = AsyncEnumerable.Empty<int>().OrderBy(x => x);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task OrderBy1()
        {
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy(x => x);

            var e = ys.GetAsyncEnumerator();
            for (var i = 0; i < 10; i++)
                await HasNextAsync(e, i);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task OrderBy2Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy(new Func<int, int>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task OrderBy3()
        {
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy(x => x);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task ThenBy2Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy(x => x).ThenBy(new Func<int, int>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task OrderByDescending1()
        {
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderByDescending(x => x);

            var e = ys.GetAsyncEnumerator();
            for (var i = 9; i >= 0; i--)
                await HasNextAsync(e, i);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task OrderByDescending2Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderByDescending(new Func<int, int>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task OrderByDescending3()
        {
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderByDescending(x => x);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task ThenByDescending2Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy(x => x).ThenByDescending(new Func<int, int>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
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

        [Fact]
        public async Task OrderBy_Optimize_ToArray()
        {
            foreach (var seed in new[] { 1905, 1948, 1983 })
            {
                var rand = GetRandom(seed, 10_000);
                var randAsync = rand.ToAsyncEnumerable();

                var res = rand.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);
                var resAsync = randAsync.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

                var lst = res.ToArray();
                var lstAsync = await resAsync.ToArrayAsync();

                Assert.True(lst.SequenceEqual(lstAsync));
            }
        }

        [Fact]
        public async Task OrderBy_Optimize_ToList()
        {
            foreach (var seed in new[] { 1905, 1948, 1983 })
            {
                var rand = GetRandom(seed, 10_000);
                var randAsync = rand.ToAsyncEnumerable();

                var res = rand.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);
                var resAsync = randAsync.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

                var lst = res.ToList();
                var lstAsync = await resAsync.ToListAsync();

                Assert.True(lst.SequenceEqual(lstAsync));
            }
        }

        private static IEnumerable<int> GetRandom(int seed, int count)
        {
            var rand = new Random(seed);

            while (count > 0)
            {
                yield return rand.Next();
                count--;
            }
        }
    }
}

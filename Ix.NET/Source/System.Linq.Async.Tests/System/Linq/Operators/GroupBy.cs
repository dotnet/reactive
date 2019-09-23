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
    public partial class GroupBy : AsyncEnumerableTests
    {
        [Fact]
        public void GroupBy_KeySelector_Sync_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int>(default, x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy(Return42, default(Func<int, int>)));
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_Simple1()
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

            var res = ys.GroupBy(x => x.Age / 10);

            var e = res.GetAsyncEnumerator();

            Assert.True(await e.MoveNextAsync());
            Assert.Equal(2, e.Current.Key);
            var g1 = e.Current.GetAsyncEnumerator();
            await HasNextAsync(g1, xs[0]);
            await HasNextAsync(g1, xs[2]);
            await HasNextAsync(g1, xs[4]);
            await HasNextAsync(g1, xs[5]);
            await NoNextAsync(g1);

            Assert.True(await e.MoveNextAsync());
            Assert.Equal(6, e.Current.Key);
            var g2 = e.Current.GetAsyncEnumerator();
            await HasNextAsync(g2, xs[1]);
            await NoNextAsync(g2);

            Assert.True(await e.MoveNextAsync());
            Assert.Equal(1, e.Current.Key);
            var g3 = e.Current.GetAsyncEnumerator();
            await HasNextAsync(g3, xs[3]);
            await NoNextAsync(g3);

            Assert.True(await e.MoveNextAsync());
            Assert.Equal(4, e.Current.Key);
            var g4 = e.Current.GetAsyncEnumerator();
            await HasNextAsync(g4, xs[6]);
            await NoNextAsync(g4);

            await NoNextAsync(e);
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_Simple2()
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

            var res = ys.GroupBy(x => x.Age / 10);

            var e = res.GetAsyncEnumerator();

            Assert.True(await e.MoveNextAsync());
            var g1 = e.Current;
            Assert.Equal(2, g1.Key);

            Assert.True(await e.MoveNextAsync());
            var g2 = e.Current;
            Assert.Equal(6, g2.Key);

            Assert.True(await e.MoveNextAsync());
            var g3 = e.Current;
            Assert.Equal(1, g3.Key);

            Assert.True(await e.MoveNextAsync());
            var g4 = e.Current;
            Assert.Equal(4, g4.Key);

            await NoNextAsync(e);

            var g1e = g1.GetAsyncEnumerator();
            await HasNextAsync(g1e, xs[0]);
            await HasNextAsync(g1e, xs[2]);
            await HasNextAsync(g1e, xs[4]);
            await HasNextAsync(g1e, xs[5]);
            await NoNextAsync(g1e);

            var g2e = g2.GetAsyncEnumerator();
            await HasNextAsync(g2e, xs[1]);
            await NoNextAsync(g2e);

            var g3e = g3.GetAsyncEnumerator();
            await HasNextAsync(g3e, xs[3]);
            await NoNextAsync(g3e);

            var g4e = g4.GetAsyncEnumerator();
            await HasNextAsync(g4e, xs[6]);
            await NoNextAsync(g4e);
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_Empty()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var ys = xs.GroupBy(x => x);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_Throws_Source1()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.GroupBy(x => x);

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_Throws_Source2()
        {
            var ex = new Exception("Bang!");
            var xs = GetXs(ex).ToAsyncEnumerable();
            var ys = xs.GroupBy(x => x);

            var e = ys.GetAsyncEnumerator();

            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        private static IEnumerable<int> GetXs(Exception ex)
        {
            yield return 42;
            yield return 43;
            throw ex;
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_Throws_KeySelector1()
        {
            var ex = new Exception("Bang!");
            var xs = Return42;
            var ys = xs.GroupBy(new Func<int, int>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_Throws_KeySelector2()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.GroupBy(x => { if (x == 3) throw ex; return x; });

            var e = ys.GetAsyncEnumerator();

            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_SequenceIdentity()
        {
            // We're using Kvp here because the types will eval as equal for this test
            var xs = new[]
            {
                new Kvp("Bart", 27),
                new Kvp("John", 62),
                new Kvp("Eric", 27),
                new Kvp("Lisa", 14),
                new Kvp("Brad", 27),
                new Kvp("Lisa", 23),
                new Kvp("Eric", 42)
            };

            var ys = xs.ToAsyncEnumerable();

            var res = ys.GroupBy(x => x.Item / 10);

            await SequenceIdentity(res);
        }

        [Fact]
        public void GroupBy_KeySelector_Sync_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int>(default, x => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy(Return42, default, EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_Comparer_Simple()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, new EqMod(3));

            var e = ys.GetAsyncEnumerator();

            Assert.True(await e.MoveNextAsync());
            var g1 = e.Current;
            Assert.Equal(0, g1.Key);
            var g1e = g1.GetAsyncEnumerator();
            await HasNextAsync(g1e, 0);
            await HasNextAsync(g1e, 3);
            await HasNextAsync(g1e, 6);
            await HasNextAsync(g1e, 9);
            await NoNextAsync(g1e);

            Assert.True(await e.MoveNextAsync());
            var g2 = e.Current;
            Assert.Equal(1, g2.Key);
            var g2e = g2.GetAsyncEnumerator();
            await HasNextAsync(g2e, 1);
            await HasNextAsync(g2e, 4);
            await HasNextAsync(g2e, 7);
            await NoNextAsync(g2e);

            Assert.True(await e.MoveNextAsync());
            var g3 = e.Current;
            Assert.Equal(2, g3.Key);
            var g3e = g3.GetAsyncEnumerator();
            await HasNextAsync(g3e, 2);
            await HasNextAsync(g3e, 5);
            await HasNextAsync(g3e, 8);
            await NoNextAsync(g3e);

            await NoNextAsync(e);
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_Comparer_Group_ToList()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, new EqMod(3));

            var g1a = new List<int> { 0, 3, 6, 9 };
            var g2a = new List<int> { 1, 4, 7 };
            var g3a = new List<int> { 2, 5, 8 };

            var gar = await ys.ToListAsync();

            Assert.Equal(3, gar.Count);

            var gg1 = gar[0];
            var gg1a = await gg1.ToListAsync();
            Assert.Equal(g1a, gg1a);

            var gg2 = gar[1];
            var gg2a = await gg2.ToListAsync();
            Assert.Equal(g2a, gg2a);

            var gg3 = gar[2];
            var gg3a = await gg3.ToListAsync();
            Assert.Equal(g3a, gg3a);
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_Comparer_Group_Count()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, new EqMod(3));

            var gar = await ys.ToListAsync();

            Assert.Equal(3, gar.Count);

            var gg1 = gar[0];
            var gg1a = await gg1.CountAsync();
            Assert.Equal(4, gg1a);

            var gg2 = gar[1];
            var gg2a = await gg2.CountAsync();
            Assert.Equal(3, gg2a);

            var gg3 = gar[2];
            var gg3a = await gg3.CountAsync();
            Assert.Equal(3, gg3a);
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_Comparer_Group_ToArray()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, new EqMod(3));

            var g1a = new[] { 0, 3, 6, 9 };
            var g2a = new[] { 1, 4, 7 };
            var g3a = new[] { 2, 5, 8 };

            var gar = await ys.ToArrayAsync();

            Assert.Equal(3, gar.Length);

            var gg1 = gar[0];
            var gg1a = await gg1.ToArrayAsync();

            Assert.Equal(g1a, gg1a);

            var gg2 = gar[1];
            var gg2a = await gg2.ToArrayAsync();

            Assert.Equal(g2a, gg2a);

            var gg3 = gar[2];
            var gg3a = await gg3.ToArrayAsync();
            Assert.Equal(g3a, gg3a);
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_Comparer_Count()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, new EqMod(3));

            var gar = await ys.CountAsync();

            Assert.Equal(3, gar);
        }

        [Fact]
        public async Task GroupBy_KeySelector_Sync_Comparer_SequenceIdentity()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, new EqMod(3));

            await SequenceIdentity(ys);
        }

        [Fact]
        public void GroupBy_KeySelector_ElementSelector_Sync_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(default, x => x, x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(Return42, default, x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy(Return42, x => x, default(Func<int, int>)));
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_Sync_Simple()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x));

            var e = ys.GetAsyncEnumerator();

            Assert.True(await e.MoveNextAsync());
            var g1 = e.Current;
            Assert.Equal(0, g1.Key);
            var g1e = g1.GetAsyncEnumerator();
            await HasNextAsync(g1e, 'a');
            await HasNextAsync(g1e, 'd');
            await HasNextAsync(g1e, 'g');
            await HasNextAsync(g1e, 'j');
            await NoNextAsync(g1e);

            Assert.True(await e.MoveNextAsync());
            var g2 = e.Current;
            Assert.Equal(1, g2.Key);
            var g2e = g2.GetAsyncEnumerator();
            await HasNextAsync(g2e, 'b');
            await HasNextAsync(g2e, 'e');
            await HasNextAsync(g2e, 'h');
            await NoNextAsync(g2e);

            Assert.True(await e.MoveNextAsync());
            var g3 = e.Current;
            Assert.Equal(2, g3.Key);
            var g3e = g3.GetAsyncEnumerator();
            await HasNextAsync(g3e, 'c');
            await HasNextAsync(g3e, 'f');
            await HasNextAsync(g3e, 'i');
            await NoNextAsync(g3e);

            await NoNextAsync(e);
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_Sync_Group_ToArray()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x));

            var g1a = new[] { 'a', 'd', 'g', 'j' };
            var g2a = new[] { 'b', 'e', 'h' };
            var g3a = new[] { 'c', 'f', 'i' };

            var gar = await ys.ToArrayAsync();

            Assert.Equal(3, gar.Length);

            var gg1 = gar[0];
            var gg1a = await gg1.ToArrayAsync();

            Assert.Equal(g1a, gg1a);

            var gg2 = gar[1];
            var gg2a = await gg2.ToArrayAsync();

            Assert.Equal(g2a, gg2a);

            var gg3 = gar[2];
            var gg3a = await gg3.ToArrayAsync();
            Assert.Equal(g3a, gg3a);
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_Sync_Group_ToList()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x));

            var g1a = new List<char> { 'a', 'd', 'g', 'j' };
            var g2a = new List<char> { 'b', 'e', 'h' };
            var g3a = new List<char> { 'c', 'f', 'i' };

            var gar = await ys.ToListAsync();

            Assert.Equal(3, gar.Count);

            var gg1 = gar[0];
            var gg1a = await gg1.ToListAsync();
            Assert.Equal(g1a, gg1a);

            var gg2 = gar[1];
            var gg2a = await gg2.ToListAsync();
            Assert.Equal(g2a, gg2a);

            var gg3 = gar[2];
            var gg3a = await gg3.ToListAsync();
            Assert.Equal(g3a, gg3a);
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_Sync_Group_Count()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x));

            var gar = await ys.ToListAsync();

            Assert.Equal(3, gar.Count);

            var gg1 = gar[0];
            var gg1a = await gg1.CountAsync();
            Assert.Equal(4, gg1a);

            var gg2 = gar[1];
            var gg2a = await gg2.CountAsync();
            Assert.Equal(3, gg2a);

            var gg3 = gar[2];
            var gg3a = await gg3.CountAsync();
            Assert.Equal(3, gg3a);
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_Sync_Count()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x));

            var gar = await ys.CountAsync();

            Assert.Equal(3, gar);
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_Sync_SequenceIdentity()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x));

            await SequenceIdentity(ys);
        }

        [Fact]
        public void GroupBy_KeySelector_ElementSelector_Sync_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(default, x => x, x => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy(Return42, default, x => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy(Return42, x => x, default(Func<int, int>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_Sync_Comparer_Simple1()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, x => (char)('a' + x), new EqMod(3));

            var e = ys.GetAsyncEnumerator();

            Assert.True(await e.MoveNextAsync());
            var g1 = e.Current;
            Assert.Equal(0, g1.Key);
            var g1e = g1.GetAsyncEnumerator();
            await HasNextAsync(g1e, 'a');
            await HasNextAsync(g1e, 'd');
            await HasNextAsync(g1e, 'g');
            await HasNextAsync(g1e, 'j');
            await NoNextAsync(g1e);

            Assert.True(await e.MoveNextAsync());
            var g2 = e.Current;
            Assert.Equal(1, g2.Key);
            var g2e = g2.GetAsyncEnumerator();
            await HasNextAsync(g2e, 'b');
            await HasNextAsync(g2e, 'e');
            await HasNextAsync(g2e, 'h');
            await NoNextAsync(g2e);

            Assert.True(await e.MoveNextAsync());
            var g3 = e.Current;
            Assert.Equal(2, g3.Key);
            var g3e = g3.GetAsyncEnumerator();
            await HasNextAsync(g3e, 'c');
            await HasNextAsync(g3e, 'f');
            await HasNextAsync(g3e, 'i');
            await NoNextAsync(g3e);

            await NoNextAsync(e);
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_Sync_Comparer_Simple2()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, x => (char)('a' + x), new EqMod(3));

            var e = ys.GetAsyncEnumerator();

            Assert.True(await e.MoveNextAsync());
            var g1 = e.Current;
            Assert.Equal(0, g1.Key);
            var g1e = g1.GetAsyncEnumerator();
            await HasNextAsync(g1e, 'a');
            await HasNextAsync(g1e, 'd');
            await HasNextAsync(g1e, 'g');
            await HasNextAsync(g1e, 'j');
            await NoNextAsync(g1e);
            await g1e.DisposeAsync();

            Assert.True(await e.MoveNextAsync());
            var g2 = e.Current;
            Assert.Equal(1, g2.Key);
            var g2e = g2.GetAsyncEnumerator();
            await HasNextAsync(g2e, 'b');
            await HasNextAsync(g2e, 'e');
            await HasNextAsync(g2e, 'h');
            await NoNextAsync(g2e);
            await g2e.DisposeAsync();

            Assert.True(await e.MoveNextAsync());
            var g3 = e.Current;
            Assert.Equal(2, g3.Key);
            var g3e = g3.GetAsyncEnumerator();
            await HasNextAsync(g3e, 'c');
            await HasNextAsync(g3e, 'f');
            await HasNextAsync(g3e, 'i');
            await NoNextAsync(g3e);
            await g3e.DisposeAsync();

            await NoNextAsync(e);

            await e.DisposeAsync();
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_Sync_Comparer_Simple3()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, x => (char)('a' + x), new EqMod(3));

            var e = ys.GetAsyncEnumerator();

            Assert.True(await e.MoveNextAsync());
            var g1 = e.Current;
            Assert.Equal(0, g1.Key);
            var g1e = g1.GetAsyncEnumerator();
            await HasNextAsync(g1e, 'a');

            await e.DisposeAsync();

            await HasNextAsync(g1e, 'd');
            await HasNextAsync(g1e, 'g');
            await HasNextAsync(g1e, 'j');
            await NoNextAsync(g1e);
            await g1e.DisposeAsync();

            Assert.False(await e.MoveNextAsync());
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_Sync_Comparer_DisposeEarly()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, x => (char)('a' + x), new EqMod(3));

            var e = ys.GetAsyncEnumerator();
            await e.DisposeAsync();

            Assert.False(await e.MoveNextAsync());
        }

        [Fact]
        public void GroupBy_KeySelector_ResultSelector_Sync_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(default, x => x, (x, ys) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(Return42, default, (x, ys) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy(Return42, x => x, default(Func<int, IAsyncEnumerable<int>, int>)));
        }

        [Fact]
        public void GroupBy_KeySelector_ResultSelector_Sync_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(default, x => x, (x, ys) => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy(Return42, default, (x, ys) => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy(Return42, x => x, default(Func<int, IAsyncEnumerable<int>, int>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task GroupBy_KeySelector_ResultSelector_Sync_Comparer_ToArray()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, (k, cs) => k + " - " + cs.AggregateAsync("", (a, c) => a + c).Result, new EqMod(3));

            var arr = new[] { "0 - 0369", "1 - 147", "2 - 258" };

            Assert.Equal(arr, await ys.ToArrayAsync());
        }

        [Fact]
        public async Task GroupBy_KeySelector_ResultSelector_Sync_Comparer_ToList()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, (k, cs) => k + " - " + cs.AggregateAsync("", (a, c) => a + c).Result, new EqMod(3));

            var arr = new List<string> { "0 - 0369", "1 - 147", "2 - 258" };

            Assert.Equal(arr, await ys.ToListAsync());
        }

        [Fact]
        public async Task GroupBy_KeySelector_ResultSelector_Sync_Comparer_Count()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, (k, cs) => k + " - " + cs.AggregateAsync("", (a, c) => a + c).Result, new EqMod(3));

            Assert.Equal(3, await ys.CountAsync());
        }

        [Fact]
        public async Task GroupBy_KeySelector_ResultSelector_Sync_Comparer_SequenceIdentity()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, (k, cs) => k + " - " + cs.AggregateAsync("", (a, c) => a + c).Result, new EqMod(3));

            await SequenceIdentity(ys);
        }

        [Fact]
        public void GroupBy_KeySelector_ElementSelector_ResultSelector_Sync_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(default, x => x, x => x, (x, ys) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(Return42, default, x => x, (x, ys) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(Return42, x => x, default, (x, ys) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(Return42, x => x, x => x, default));
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_ResultSelector_Sync_Simple1()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x), (k, cs) => k + " - " + cs.AggregateAsync("", (a, c) => a + c).Result);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, "0 - adgj");
            await HasNextAsync(e, "1 - beh");
            await HasNextAsync(e, "2 - cfi");
            await NoNextAsync(e);
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_ResultSelector_Sync_Simple2()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, (k, cs) => k + " - " + cs.AggregateAsync("", (a, c) => a + c).Result);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, "0 - 0369");
            await HasNextAsync(e, "1 - 147");
            await HasNextAsync(e, "2 - 258");
            await NoNextAsync(e);
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_ResultSelector_Sync_ToArray()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x), (k, cs) => k + " - " + cs.AggregateAsync("", (a, c) => a + c).Result);

            var arr = new[] { "0 - adgj", "1 - beh", "2 - cfi" };

            Assert.Equal(arr, await ys.ToArrayAsync());
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_ResultSelector_Sync_ToList()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x), (k, cs) => k + " - " + cs.AggregateAsync("", (a, c) => a + c).Result);

            var arr = new List<string> { "0 - adgj", "1 - beh", "2 - cfi" };

            Assert.Equal(arr, await ys.ToListAsync());
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_ResultSelector_Sync_Count()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x), (k, cs) => k + " - " + cs.AggregateAsync("", (a, c) => a + c).Result);

            Assert.Equal(3, await ys.CountAsync());
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_ResultSelector_Sync_SequenceIdentity()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x), (k, cs) => k + " - " + cs.AggregateAsync("", (a, c) => a + c).Result);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void GroupBy_KeySelector_ElementSelector_ResultSelector_Sync_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(default, x => x, x => x, (x, ys) => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy(Return42, default, x => x, (x, ys) => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(Return42, x => x, default, (x, ys) => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(Return42, x => x, x => x, default, EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_ResultSelector_Sync_Comparer_Simple1()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, x => (char)('a' + x), (k, cs) => k + " - " + cs.AggregateAsync("", (a, c) => a + c).Result, new EqMod(3));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, "0 - adgj");
            await HasNextAsync(e, "1 - beh");
            await HasNextAsync(e, "2 - cfi");
            await NoNextAsync(e);
        }

        [Fact]
        public async Task GroupBy_KeySelector_ElementSelector_ResultSelector_Sync_Comparer_Simple2()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, (k, cs) => k + " - " + cs.AggregateAsync("", (a, c) => a + c).Result, new EqMod(3));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, "0 - 0369");
            await HasNextAsync(e, "1 - 147");
            await HasNextAsync(e, "2 - 258");
            await NoNextAsync(e);
        }

        private sealed class EqMod : IEqualityComparer<int>
        {
            private readonly int _d;

            public EqMod(int d)
            {
                _d = d;
            }

            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(x % _d, y % _d);
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(obj % _d);
            }
        }

        private sealed class Kvp : IEquatable<Kvp>
        {
            public Kvp(string key, int item)
            {
                Key = key;
                Item = item;
            }

            public string Key { get; }
            public int Item { get; }

            public bool Equals(Kvp other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Key, other.Key) && Item == other.Item;
            }

            public override bool Equals(object? obj)
            {
                if (obj is null) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Kvp)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Key != null ? Key.GetHashCode() : 0) * 397) ^ Item;
                }
            }

            public static bool operator ==(Kvp left, Kvp right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(Kvp left, Kvp right)
            {
                return !Equals(left, right);
            }
        }
    }
}

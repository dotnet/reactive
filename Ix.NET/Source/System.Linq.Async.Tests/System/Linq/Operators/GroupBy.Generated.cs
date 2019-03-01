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
    partial class GroupBy
    {
        [Fact]
        public void KeySelector_Sync_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int>(default(IAsyncEnumerable<int>), (int x) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int>(Return42, default(Func<int, int>)));
        }

        [Fact]
        public async Task KeySelector_Sync_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var resS = methodsS.GroupBy(m => m.Name);
            var resA = methodsA.GroupBy(m => m.Name);

            await Groups_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_Sync_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int>(default(IAsyncEnumerable<int>), (int x) => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int>(Return42, default(Func<int, int>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task KeySelector_Sync_Comparer_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var eq = new StringPrefixEqualityComparer(1);

            var resS = methodsS.GroupBy(m => m.Name, eq);
            var resA = methodsA.GroupBy(m => m.Name, eq);

            await Groups_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ElementSelector_Sync_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(default(IAsyncEnumerable<int>), (int x) => x, (int x) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(Return42, default(Func<int, int>), (int x) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(Return42, (int x) => x, default(Func<int, int>)));
        }

        [Fact]
        public async Task KeySelector_ElementSelector_Sync_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var resS = methodsS.GroupBy(m => m.Name, m => m.Name.ToUpper());
            var resA = methodsA.GroupBy(m => m.Name, m => m.Name.ToUpper());

            await Groups_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ElementSelector_Sync_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(default(IAsyncEnumerable<int>), (int x) => x, (int x) => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(Return42, default(Func<int, int>), (int x) => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(default(IAsyncEnumerable<int>), (int x) => x, default(Func<int, int>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task KeySelector_ElementSelector_Sync_Comparer_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var eq = new StringPrefixEqualityComparer(1);

            var resS = methodsS.GroupBy(m => m.Name, m => m.Name.ToUpper(), eq);
            var resA = methodsA.GroupBy(m => m.Name, m => m.Name.ToUpper(), eq);

            await Groups_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ResultSelector_Sync_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(default(IAsyncEnumerable<int>), (int x) => x, (k, g) => 0));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(Return42, default(Func<int, int>), (k, g) => 0));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(Return42, (int x) => x, default(Func<int, IAsyncEnumerable<int>, int>)));
        }

        [Fact]
        public async Task KeySelector_ResultSelector_Sync_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var resS = methodsS.GroupBy(m => m.Name, (k, g) => k + " - " + g.Count());
            var resA = methodsA.GroupBy(m => m.Name, (k, g) => k + " - " + g.CountAsync().Result);

            await Group_Result_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ResultSelector_Sync_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(default(IAsyncEnumerable<int>), (int x) => x, (k, g) => 0, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(Return42, default(Func<int, int>), (k, g) => 0, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(default(IAsyncEnumerable<int>), (int x) => x, default(Func<int, IAsyncEnumerable<int>, int>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task KeySelector_ResultSelector_Sync_Comparer_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var eq = new StringPrefixEqualityComparer(1);

            var resS = methodsS.GroupBy(m => m.Name, (k, g) => k + " - " + g.Count(), eq);
            var resA = methodsA.GroupBy(m => m.Name, (k, g) => k + " - " + g.CountAsync().Result, eq);

            await Group_Result_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ElementSelector_ResultSelector_Sync_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(default(IAsyncEnumerable<int>), (int x) => x, (int x) => x, (k, g) => 0));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(Return42, default(Func<int, int>), (int x) => x, (k, g) => 0));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(Return42, (int x) => x, default(Func<int, int>), (k, g) => 0));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(Return42, (int x) => x, (int x) => x, default(Func<int, IAsyncEnumerable<int>, int>)));
        }

        [Fact]
        public async Task KeySelector_ElementSelector_ResultSelector_Sync_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var resS = methodsS.GroupBy(m => m.Name, m => m.Name.ToUpper(), (k, g) => k + " - " + g.Count());
            var resA = methodsA.GroupBy(m => m.Name, m => m.Name.ToUpper(), (k, g) => k + " - " + g.CountAsync().Result);

            await Group_Result_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ElementSelector_ResultSelector_Sync_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(default(IAsyncEnumerable<int>), (int x) => x, (int x) => x, (k, g) => 0, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(Return42, default(Func<int, int>), (int x) => x, (k, g) => 0, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(default(IAsyncEnumerable<int>), (int x) => x, default(Func<int, int>), (k, g) => 0, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(default(IAsyncEnumerable<int>), (int x) => x, (int x) => x, default(Func<int, IAsyncEnumerable<int>, int>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task KeySelector_ElementSelector_ResultSelector_Sync_Comparer_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var eq = new StringPrefixEqualityComparer(1);

            var resS = methodsS.GroupBy(m => m.Name, m => m.Name.ToUpper(), (k, g) => k + " - " + g.Count(), eq);
            var resA = methodsA.GroupBy(m => m.Name, m => m.Name.ToUpper(), (k, g) => k + " - " + g.CountAsync().Result, eq);

            await Group_Result_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_Async_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int>(default(IAsyncEnumerable<int>), (int x) => new ValueTask<int>(x)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int>(Return42, default(Func<int, ValueTask<int>>)));
        }

        [Fact]
        public async Task KeySelector_Async_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var resS = methodsS.GroupBy(m => m.Name);
            var resA = methodsA.GroupByAwait(m => new ValueTask<string>(m.Name));

            await Groups_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_Async_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int>(default(IAsyncEnumerable<int>), (int x) => new ValueTask<int>(x), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int>(Return42, default(Func<int, ValueTask<int>>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task KeySelector_Async_Comparer_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var eq = new StringPrefixEqualityComparer(1);

            var resS = methodsS.GroupBy(m => m.Name, eq);
            var resA = methodsA.GroupByAwait(m => new ValueTask<string>(m.Name), eq);

            await Groups_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ElementSelector_Async_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int>(default(IAsyncEnumerable<int>), (int x) => new ValueTask<int>(x), (int x) => new ValueTask<int>(x)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int>(Return42, default(Func<int, ValueTask<int>>), (int x) => new ValueTask<int>(x)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int>(Return42, (int x) => new ValueTask<int>(x), default(Func<int, ValueTask<int>>)));
        }

        [Fact]
        public async Task KeySelector_ElementSelector_Async_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var resS = methodsS.GroupBy(m => m.Name, m => m.Name.ToUpper());
            var resA = methodsA.GroupByAwait(m => new ValueTask<string>(m.Name), m => new ValueTask<string>(m.Name.ToUpper()));

            await Groups_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ElementSelector_Async_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int>(default(IAsyncEnumerable<int>), (int x) => new ValueTask<int>(x), (int x) => new ValueTask<int>(x), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int>(Return42, default(Func<int, ValueTask<int>>), (int x) => new ValueTask<int>(x), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int>(default(IAsyncEnumerable<int>), (int x) => new ValueTask<int>(x), default(Func<int, ValueTask<int>>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task KeySelector_ElementSelector_Async_Comparer_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var eq = new StringPrefixEqualityComparer(1);

            var resS = methodsS.GroupBy(m => m.Name, m => m.Name.ToUpper(), eq);
            var resA = methodsA.GroupByAwait(m => new ValueTask<string>(m.Name), m => new ValueTask<string>(m.Name.ToUpper()), eq);

            await Groups_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ResultSelector_Async_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int>(default(IAsyncEnumerable<int>), (int x) => new ValueTask<int>(x), (k, g) => new ValueTask<int>(0)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int>(Return42, default(Func<int, ValueTask<int>>), (k, g) => new ValueTask<int>(0)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int>(Return42, (int x) => new ValueTask<int>(x), default(Func<int, IAsyncEnumerable<int>, ValueTask<int>>)));
        }

        [Fact]
        public async Task KeySelector_ResultSelector_Async_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var resS = methodsS.GroupBy(m => m.Name, (k, g) => k + " - " + g.Count());
            var resA = methodsA.GroupByAwait(m => new ValueTask<string>(m.Name), async (k, g) => k + " - " + await g.CountAsync());

            await Group_Result_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ResultSelector_Async_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int>(default(IAsyncEnumerable<int>), (int x) => new ValueTask<int>(x), (k, g) => new ValueTask<int>(0), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int>(Return42, default(Func<int, ValueTask<int>>), (k, g) => new ValueTask<int>(0), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int>(default(IAsyncEnumerable<int>), (int x) => new ValueTask<int>(x), default(Func<int, IAsyncEnumerable<int>, ValueTask<int>>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task KeySelector_ResultSelector_Async_Comparer_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var eq = new StringPrefixEqualityComparer(1);

            var resS = methodsS.GroupBy(m => m.Name, (k, g) => k + " - " + g.Count(), eq);
            var resA = methodsA.GroupByAwait(m => new ValueTask<string>(m.Name), async (k, g) => k + " - " + await g.CountAsync(), eq);

            await Group_Result_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ElementSelector_ResultSelector_Async_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int, int>(default(IAsyncEnumerable<int>), (int x) => new ValueTask<int>(x), (int x) => new ValueTask<int>(x), (k, g) => new ValueTask<int>(0)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int, int>(Return42, default(Func<int, ValueTask<int>>), (int x) => new ValueTask<int>(x), (k, g) => new ValueTask<int>(0)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int, int>(Return42, (int x) => new ValueTask<int>(x), default(Func<int, ValueTask<int>>), (k, g) => new ValueTask<int>(0)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int, int>(Return42, (int x) => new ValueTask<int>(x), (int x) => new ValueTask<int>(x), default(Func<int, IAsyncEnumerable<int>, ValueTask<int>>)));
        }

        [Fact]
        public async Task KeySelector_ElementSelector_ResultSelector_Async_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var resS = methodsS.GroupBy(m => m.Name, m => m.Name.ToUpper(), (k, g) => k + " - " + g.Count());
            var resA = methodsA.GroupByAwait(m => new ValueTask<string>(m.Name), m => new ValueTask<string>(m.Name.ToUpper()), async (k, g) => k + " - " + await g.CountAsync());

            await Group_Result_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ElementSelector_ResultSelector_Async_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int, int>(default(IAsyncEnumerable<int>), (int x) => new ValueTask<int>(x), (int x) => new ValueTask<int>(x), (k, g) => new ValueTask<int>(0), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int, int>(Return42, default(Func<int, ValueTask<int>>), (int x) => new ValueTask<int>(x), (k, g) => new ValueTask<int>(0), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int, int>(default(IAsyncEnumerable<int>), (int x) => new ValueTask<int>(x), default(Func<int, ValueTask<int>>), (k, g) => new ValueTask<int>(0), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwait<int, int, int, int>(default(IAsyncEnumerable<int>), (int x) => new ValueTask<int>(x), (int x) => new ValueTask<int>(x), default(Func<int, IAsyncEnumerable<int>, ValueTask<int>>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task KeySelector_ElementSelector_ResultSelector_Async_Comparer_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var eq = new StringPrefixEqualityComparer(1);

            var resS = methodsS.GroupBy(m => m.Name, m => m.Name.ToUpper(), (k, g) => k + " - " + g.Count(), eq);
            var resA = methodsA.GroupByAwait(m => new ValueTask<string>(m.Name), m => new ValueTask<string>(m.Name.ToUpper()), async (k, g) => k + " - " + await g.CountAsync(), eq);

            await Group_Result_AssertCore(resS, resA);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public void KeySelector_Async_Cancel_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int>(default(IAsyncEnumerable<int>), (int x, CancellationToken ct) => new ValueTask<int>(x)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int>(Return42, default(Func<int, CancellationToken, ValueTask<int>>)));
        }

        [Fact]
        public async Task KeySelector_Async_Cancel_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var resS = methodsS.GroupBy(m => m.Name);
            var resA = methodsA.GroupByAwaitWithCancellation((m, ct) => new ValueTask<string>(m.Name));

            await Groups_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_Async_Cancel_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int>(default(IAsyncEnumerable<int>), (int x, CancellationToken ct) => new ValueTask<int>(x), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int>(Return42, default(Func<int, CancellationToken, ValueTask<int>>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task KeySelector_Async_Cancel_Comparer_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var eq = new StringPrefixEqualityComparer(1);

            var resS = methodsS.GroupBy(m => m.Name, eq);
            var resA = methodsA.GroupByAwaitWithCancellation((m, ct) => new ValueTask<string>(m.Name), eq);

            await Groups_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ElementSelector_Async_Cancel_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int>(default(IAsyncEnumerable<int>), (int x, CancellationToken ct) => new ValueTask<int>(x), (int x, CancellationToken ct) => new ValueTask<int>(x)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int>(Return42, default(Func<int, CancellationToken, ValueTask<int>>), (int x, CancellationToken ct) => new ValueTask<int>(x)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int>(Return42, (int x, CancellationToken ct) => new ValueTask<int>(x), default(Func<int, CancellationToken, ValueTask<int>>)));
        }

        [Fact]
        public async Task KeySelector_ElementSelector_Async_Cancel_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var resS = methodsS.GroupBy(m => m.Name, m => m.Name.ToUpper());
            var resA = methodsA.GroupByAwaitWithCancellation((m, ct) => new ValueTask<string>(m.Name), (m, ct) => new ValueTask<string>(m.Name.ToUpper()));

            await Groups_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ElementSelector_Async_Cancel_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int>(default(IAsyncEnumerable<int>), (int x, CancellationToken ct) => new ValueTask<int>(x), (int x, CancellationToken ct) => new ValueTask<int>(x), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int>(Return42, default(Func<int, CancellationToken, ValueTask<int>>), (int x, CancellationToken ct) => new ValueTask<int>(x), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int>(default(IAsyncEnumerable<int>), (int x, CancellationToken ct) => new ValueTask<int>(x), default(Func<int, CancellationToken, ValueTask<int>>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task KeySelector_ElementSelector_Async_Cancel_Comparer_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var eq = new StringPrefixEqualityComparer(1);

            var resS = methodsS.GroupBy(m => m.Name, m => m.Name.ToUpper(), eq);
            var resA = methodsA.GroupByAwaitWithCancellation((m, ct) => new ValueTask<string>(m.Name), (m, ct) => new ValueTask<string>(m.Name.ToUpper()), eq);

            await Groups_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ResultSelector_Async_Cancel_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int>(default(IAsyncEnumerable<int>), (int x, CancellationToken ct) => new ValueTask<int>(x), (k, g, ct) => new ValueTask<int>(0)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int>(Return42, default(Func<int, CancellationToken, ValueTask<int>>), (k, g, ct) => new ValueTask<int>(0)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int>(Return42, (int x, CancellationToken ct) => new ValueTask<int>(x), default(Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>)));
        }

        [Fact]
        public async Task KeySelector_ResultSelector_Async_Cancel_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var resS = methodsS.GroupBy(m => m.Name, (k, g) => k + " - " + g.Count());
            var resA = methodsA.GroupByAwaitWithCancellation((m, ct) => new ValueTask<string>(m.Name), async (k, g, ct) => k + " - " + await g.CountAsync(ct));

            await Group_Result_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ResultSelector_Async_Cancel_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int>(default(IAsyncEnumerable<int>), (int x, CancellationToken ct) => new ValueTask<int>(x), (k, g, ct) => new ValueTask<int>(0), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int>(Return42, default(Func<int, CancellationToken, ValueTask<int>>), (k, g, ct) => new ValueTask<int>(0), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int>(default(IAsyncEnumerable<int>), (int x, CancellationToken ct) => new ValueTask<int>(x), default(Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task KeySelector_ResultSelector_Async_Cancel_Comparer_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var eq = new StringPrefixEqualityComparer(1);

            var resS = methodsS.GroupBy(m => m.Name, (k, g) => k + " - " + g.Count(), eq);
            var resA = methodsA.GroupByAwaitWithCancellation((m, ct) => new ValueTask<string>(m.Name), async (k, g, ct) => k + " - " + await g.CountAsync(ct), eq);

            await Group_Result_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ElementSelector_ResultSelector_Async_Cancel_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int, int>(default(IAsyncEnumerable<int>), (int x, CancellationToken ct) => new ValueTask<int>(x), (int x, CancellationToken ct) => new ValueTask<int>(x), (k, g, ct) => new ValueTask<int>(0)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int, int>(Return42, default(Func<int, CancellationToken, ValueTask<int>>), (int x, CancellationToken ct) => new ValueTask<int>(x), (k, g, ct) => new ValueTask<int>(0)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int, int>(Return42, (int x, CancellationToken ct) => new ValueTask<int>(x), default(Func<int, CancellationToken, ValueTask<int>>), (k, g, ct) => new ValueTask<int>(0)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int, int>(Return42, (int x, CancellationToken ct) => new ValueTask<int>(x), (int x, CancellationToken ct) => new ValueTask<int>(x), default(Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>)));
        }

        [Fact]
        public async Task KeySelector_ElementSelector_ResultSelector_Async_Cancel_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var resS = methodsS.GroupBy(m => m.Name, m => m.Name.ToUpper(), (k, g) => k + " - " + g.Count());
            var resA = methodsA.GroupByAwaitWithCancellation((m, ct) => new ValueTask<string>(m.Name), (m, ct) => new ValueTask<string>(m.Name.ToUpper()), async (k, g, ct) => k + " - " + await g.CountAsync(ct));

            await Group_Result_AssertCore(resS, resA);
        }

        [Fact]
        public void KeySelector_ElementSelector_ResultSelector_Async_Cancel_Comparer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int, int>(default(IAsyncEnumerable<int>), (int x, CancellationToken ct) => new ValueTask<int>(x), (int x, CancellationToken ct) => new ValueTask<int>(x), (k, g, ct) => new ValueTask<int>(0), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int, int>(Return42, default(Func<int, CancellationToken, ValueTask<int>>), (int x, CancellationToken ct) => new ValueTask<int>(x), (k, g, ct) => new ValueTask<int>(0), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int, int>(default(IAsyncEnumerable<int>), (int x, CancellationToken ct) => new ValueTask<int>(x), default(Func<int, CancellationToken, ValueTask<int>>), (k, g, ct) => new ValueTask<int>(0), EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupByAwaitWithCancellation<int, int, int, int>(default(IAsyncEnumerable<int>), (int x, CancellationToken ct) => new ValueTask<int>(x), (int x, CancellationToken ct) => new ValueTask<int>(x), default(Func<int, IAsyncEnumerable<int>, CancellationToken, ValueTask<int>>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task KeySelector_ElementSelector_ResultSelector_Async_Cancel_Comparer_All()
        {
            var methodsS = typeof(Enumerable).GetMethods().AsEnumerable();
            var methodsA = methodsS.ToAsyncEnumerable();

            var eq = new StringPrefixEqualityComparer(1);

            var resS = methodsS.GroupBy(m => m.Name, m => m.Name.ToUpper(), (k, g) => k + " - " + g.Count(), eq);
            var resA = methodsA.GroupByAwaitWithCancellation((m, ct) => new ValueTask<string>(m.Name), (m, ct) => new ValueTask<string>(m.Name.ToUpper()), async (k, g, ct) => k + " - " + await g.CountAsync(ct), eq);

            await Group_Result_AssertCore(resS, resA);
        }
#endif

        private async Task Groups_AssertCore<T, K>(IEnumerable<IGrouping<K, T>> resS, IAsyncEnumerable<IAsyncGrouping<K, T>> resA)
        {
            Assert.True(await AsyncEnumerable.SequenceEqualAsync(
                resS.Select(g => g.Key).ToAsyncEnumerable(),
                resA.Select(g => g.Key)
            ));

            // CountAsync

            Assert.Equal(resS.Count(), await resA.CountAsync());

            // ToArrayAsync

            var resArrS = resS.ToArray();
            var resArrA = await resA.ToArrayAsync();

            Assert.Equal(
                resArrS.Select(g => g.Key),
                resArrA.Select(g => g.Key)
            );

            // ToListAsync

            var resLstS = resS.ToList();
            var resLstA = await resA.ToListAsync();

            Assert.Equal(
                resLstS.Select(g => g.Key),
                resLstA.Select(g => g.Key)
            );
        }

        private async Task Group_Result_AssertCore<T>(IEnumerable<T> resS, IAsyncEnumerable<T> resA)
        {
            Assert.True(await AsyncEnumerable.SequenceEqualAsync(
                resS.ToAsyncEnumerable(),
                resA
            ));

            // CountAsync

            Assert.Equal(resS.Count(), await resA.CountAsync());

            // ToArrayAsync

            var resArrS = resS.ToArray();
            var resArrA = await resA.ToArrayAsync();

            Assert.Equal(resArrS, resArrA);

            // ToListAsync

            var resLstS = resS.ToList();
            var resLstA = await resA.ToListAsync();

            Assert.Equal(resLstS, resLstA);
        }

        private sealed class StringPrefixEqualityComparer : IEqualityComparer<string>
        {
            private readonly int _n;

            public StringPrefixEqualityComparer(int n) => _n = n;

            public bool Equals(string s1, string s2) => s1.Substring(0, _n) == s2.Substring(0, _n);

            public int GetHashCode(string s) => s.Substring(0, _n).GetHashCode();
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Select : AsyncEnumerableTests
    {
        [Fact]
        public void Select_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Select<int, int>(default, x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Select<int, int>(default, (x, i) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Select(Return42, default(Func<int, int>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Select(Return42, default(Func<int, int, int>)));
        }

        [Fact]
        public async Task Select_Sync_Simple()
        {
            var xs = ToAsyncEnumerable(new[] { 0, 1, 2 });
            var ys = xs.Select(x => (char)('a' + x));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 'a');
            await HasNextAsync(e, 'b');
            await HasNextAsync(e, 'c');
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Select_Sync_Simple_IList()
        {
            var xs = ToAsyncEnumerableIList(new[] { 0, 1, 2 });
            var ys = xs.Select(x => (char)('a' + x));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 'a');
            await HasNextAsync(e, 'b');
            await HasNextAsync(e, 'c');
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Select_Sync_Simple_AsyncIterator()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(x => (char)('a' + x));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 'a');
            await HasNextAsync(e, 'b');
            await HasNextAsync(e, 'c');
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Select_Sync_Indexed()
        {
            var xs = ToAsyncEnumerable(new[] { 8, 5, 7 });
            var ys = xs.Select((x, i) => (char)('a' + i));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 'a');
            await HasNextAsync(e, 'b');
            await HasNextAsync(e, 'c');
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Select_Sync_Indexed_IList()
        {
            var xs = ToAsyncEnumerableIList(new[] { 8, 5, 7 });
            var ys = xs.Select((x, i) => (char)('a' + i));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 'a');
            await HasNextAsync(e, 'b');
            await HasNextAsync(e, 'c');
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Select_Sync_Indexed_AsyncIterator()
        {
            var xs = new[] { 8, 5, 7 }.ToAsyncEnumerable();
            var ys = xs.Select((x, i) => (char)('a' + i));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 'a');
            await HasNextAsync(e, 'b');
            await HasNextAsync(e, 'c');
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Select_Sync_Throws_Selector()
        {
            var xs = ToAsyncEnumerable(new[] { 0, 1, 2 });
            var ys = xs.Select(x => 1 / x);

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync<DivideByZeroException>(e.MoveNextAsync().AsTask());
        }

        [Fact]
        public async Task Select_Sync_Throws_Selector_IList()
        {
            var xs = ToAsyncEnumerableIList(new[] { 0, 1, 2 });
            var ys = xs.Select(x => 1 / x);

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync<DivideByZeroException>(e.MoveNextAsync().AsTask());
        }

        [Fact]
        public async Task Select_Sync_Throws_Selector_AsyncIterator()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(x => 1 / x);

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync<DivideByZeroException>(e.MoveNextAsync().AsTask());
        }

        [Fact]
        public async Task Select_Sync_Indexed_Throws_Selector()
        {
            var xs = ToAsyncEnumerable(new[] { 8, 5, 7 });
            var ys = xs.Select((x, i) => 1 / i);

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync<DivideByZeroException>(e.MoveNextAsync().AsTask());
        }

        [Fact]
        public async Task Select_Sync_Indexed_Throws_Selector_IList()
        {
            var xs = ToAsyncEnumerableIList(new[] { 8, 5, 7 });
            var ys = xs.Select((x, i) => 1 / i);

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync<DivideByZeroException>(e.MoveNextAsync().AsTask());
        }

        [Fact]
        public async Task Select_Sync_Indexed_Throws_Selector_AsyncIterator()
        {
            var xs = new[] { 8, 5, 7 }.ToAsyncEnumerable();
            var ys = xs.Select((x, i) => 1 / i);

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync<DivideByZeroException>(e.MoveNextAsync().AsTask());
        }

        [Fact]
        public async Task Select_Sync_SelectSelect()
        {
            var xs = ToAsyncEnumerable(new[] { 0, 1, 2 });
            var ys = xs.Select(i => i + 3).Select(x => (char)('a' + x));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 'd');
            await HasNextAsync(e, 'e');
            await HasNextAsync(e, 'f');
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Select_Sync_SelectSelect_IList()
        {
            var xs = ToAsyncEnumerableIList(new[] { 0, 1, 2 });
            var ys = xs.Select(i => i + 3).Select(x => (char)('a' + x));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 'd');
            await HasNextAsync(e, 'e');
            await HasNextAsync(e, 'f');
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Select_Sync_SelectSelect_AsyncIterator()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(i => i + 3).Select(x => (char)('a' + x));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 'd');
            await HasNextAsync(e, 'e');
            await HasNextAsync(e, 'f');
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Select_Sync_SequenceIdentity()
        {
            var xs = ToAsyncEnumerable(new[] { 1, 2, 3 });
            var ys = xs.Select(x => (char)('a' + x));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Select_Sync_SequenceIdentity_IList()
        {
            var xs = ToAsyncEnumerableIList(new[] { 1, 2, 3 });
            var ys = xs.Select(x => (char)('a' + x));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Select_Sync_SequenceIdentity_AsyncIterator()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(x => (char)('a' + x));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Select_Sync_Indexed_SequenceIdentity()
        {
            var xs = ToAsyncEnumerable(new[] { 8, 5, 7 });
            var ys = xs.Select((x, i) => (char)('a' + i));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Select_Sync_Indexed_SequenceIdentity_IList()
        {
            var xs = ToAsyncEnumerableIList(new[] { 8, 5, 7 });
            var ys = xs.Select((x, i) => (char)('a' + i));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Select_Sync_Indexed_SequenceIdentity_AsyncIterator()
        {
            var xs = new[] { 8, 5, 7 }.ToAsyncEnumerable();
            var ys = xs.Select((x, i) => (char)('a' + i));

            await SequenceIdentity(ys);
        }

        private static IAsyncEnumerable<int> ToAsyncEnumerable(int[] xs) => new MyIterator(xs);

        private class MyIterator : IAsyncEnumerable<int>
        {
            private readonly int[] _xs;

            public MyIterator(int[] xs) => _xs = xs;

            public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new Enumerator(this);

            private class Enumerator : IAsyncEnumerator<int>
            {
                private readonly MyIterator _parent;
                private int _i = -1;

                public Enumerator(MyIterator parent) => _parent = parent;

                public int Current => _parent._xs[_i];

                public ValueTask DisposeAsync() => new ValueTask();

                public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(++_i < _parent._xs.Length);
            }
        }

        private static IAsyncEnumerable<int> ToAsyncEnumerableIList(int[] xs) => new MyIteratorIList(xs);

        private class MyIteratorIList : IAsyncEnumerable<int>, IList<int>
        {
            private readonly int[] _xs;

            public MyIteratorIList(int[] xs) => _xs = xs;

            public int this[int index] { get => _xs[index]; set => throw new NotImplementedException(); }

            public int Count => _xs.Length;

            public bool IsReadOnly => true;

            public void Add(int item) => throw new NotImplementedException();

            public void Clear() => throw new NotImplementedException();

            public bool Contains(int item) => Array.IndexOf(_xs, item) >= 0;

            public void CopyTo(int[] array, int arrayIndex) => throw new NotImplementedException();

            public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new Enumerator(this);

            public IEnumerator<int> GetEnumerator() => _xs.AsEnumerable().GetEnumerator();

            public int IndexOf(int item) => Array.IndexOf(_xs, item);

            public void Insert(int index, int item) => throw new NotImplementedException();

            public bool Remove(int item) => throw new NotImplementedException();

            public void RemoveAt(int index) => throw new NotImplementedException();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private class Enumerator : IAsyncEnumerator<int>
            {
                private readonly MyIteratorIList _parent;
                private int _i = -1;

                public Enumerator(MyIteratorIList parent) => _parent = parent;

                public int Current => _parent._xs[_i];

                public ValueTask DisposeAsync() => new ValueTask();

                public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(++_i < _parent._xs.Length);
            }
        }
    }
}

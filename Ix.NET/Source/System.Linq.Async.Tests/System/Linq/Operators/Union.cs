// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Union : AsyncEnumerableTests
    {
        [Fact]
        public void Union_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Union(default, Return42));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Union(default, Return42, new Eq()));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Union(Return42, default, new Eq()));
        }

        [Fact]
        public async Task Union_Simple()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_EqualityComparer()
        {
            var xs = new[] { 1, 2, -3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, -1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys, new Eq());

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, -3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_UnionUnion()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var zs = new[] { 5, 7, 8, 1 }.ToAsyncEnumerable();
            var res = xs.Union(ys).Union(zs);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 8);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_UnionUnionUnion()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var zs = new[] { 5, 7, 8, 1 }.ToAsyncEnumerable();
            var us = new[] { 2, 4, 6, 8 }.ToAsyncEnumerable();
            var res = xs.Union(ys).Union(zs).Union(us);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_UnionOfEmpty2()
        {
            var res = AsyncEnumerable.Empty<int>().Union(AsyncEnumerable.Empty<int>());

            var e = res.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_UnionOfEmpty3()
        {
            var res = AsyncEnumerable.Empty<int>().Union(AsyncEnumerable.Empty<int>()).Union(AsyncEnumerable.Empty<int>());

            var e = res.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_ManyUnionsWithEmpty()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var zs = new[] { 5, 7, 8, 1 }.ToAsyncEnumerable();
            var us = new[] { 2, 4, 6, 8 }.ToAsyncEnumerable();
            var res = AsyncEnumerable.Empty<int>().Union(AsyncEnumerable.Empty<int>()).Union(xs).Union(ys).Union(zs).Union(us);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_Count()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys);

            Assert.Equal(5, await res.CountAsync());
        }

        [Fact]
        public async Task Union_ToArray()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys);

            Assert.Equal([1, 2, 3, 4, 5], (await res.ToArrayAsync()).OrderBy(x => x));
        }

        [Fact]
        public async Task Union_ToList()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys);

            Assert.Equal([1, 2, 3, 4, 5], (await res.ToListAsync()).OrderBy(x => x));
        }


        [Fact]
        public async Task Union_DisposesNotEmpty()
        {
            var e1 = new DisposalDetectingEnumerable(10, 2);
            var e2 = new DisposalDetectingEnumerable(20, 2);
            var res = e1.Union(e2).OrderBy(x => x);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 10);
            await HasNextAsync(e, 11);
            await HasNextAsync(e, 20);
            await HasNextAsync(e, 21);
            await NoNextAsync(e);

            Assert.Single(e1.Enumerators);
            Assert.Single(e2.Enumerators);
            Assert.Equal([true, true], [e1.Enumerators[0].Disposed, e2.Enumerators[0].Disposed]);
        }

        [Fact]
        public async Task Union_DisposesFirstEmpty()
        {
            var e1 = new DisposalDetectingEnumerable(0, 0);
            var e2 = new DisposalDetectingEnumerable(1, 1);
            var res = e1.Union(e2);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await NoNextAsync(e);

            Assert.Single(e1.Enumerators);
            Assert.Single(e2.Enumerators);
            Assert.Equal([true, true], [e1.Enumerators[0].Disposed, e2.Enumerators[0].Disposed]);
        }

        [Fact]
        public async Task Union_DisposesSecondOfTwoEmpty()
        {
            var e1 = new DisposalDetectingEnumerable(1, 1);
            var e2 = new DisposalDetectingEnumerable(0, 0);
            var res = e1.Union(e2);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await NoNextAsync(e);

            Assert.Single(e1.Enumerators);
            Assert.Single(e2.Enumerators);
            Assert.Equal([true, true], [e1.Enumerators[0].Disposed, e2.Enumerators[0].Disposed]);
        }

        [Fact]
        public async Task Union_DisposesSecondOfThreeEmpty()
        {
            var e1 = new DisposalDetectingEnumerable(10, 1);
            var e2 = new DisposalDetectingEnumerable(0, 0);
            var e3 = new DisposalDetectingEnumerable(30, 1);
            var res = e1.Union(e2).Union(e3);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 10);
            await HasNextAsync(e, 30);
            await NoNextAsync(e);

            Assert.Single(e1.Enumerators);
            Assert.Single(e2.Enumerators);
            Assert.Single(e3.Enumerators);
            Assert.Equal([true, true, true], [e1.Enumerators[0].Disposed, e2.Enumerators[0].Disposed, e3.Enumerators[0].Disposed]);
        }

        [Fact]
        public async Task Union_DisposesThirdOfThreeEmpty()
        {
            var e1 = new DisposalDetectingEnumerable(10, 1);
            var e2 = new DisposalDetectingEnumerable(20, 1);
            var e3 = new DisposalDetectingEnumerable(0, 0);
            var res = e1.Union(e2).Union(e3);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 10);
            await HasNextAsync(e, 20);
            await NoNextAsync(e);

            Assert.Single(e1.Enumerators);
            Assert.Single(e2.Enumerators);
            Assert.Single(e3.Enumerators);
            Assert.Equal([true, true, true], [e1.Enumerators[0].Disposed, e2.Enumerators[0].Disposed, e3.Enumerators[0].Disposed]);
        }

        [Fact]
        public async Task Union_DisposesAllOfTwoEmpty()
        {
            var e1 = new DisposalDetectingEnumerable(0, 0);
            var e2 = new DisposalDetectingEnumerable(0, 0);
            var res = e1.Union(e2);

            var e = res.GetAsyncEnumerator();
            await NoNextAsync(e);

            Assert.Single(e1.Enumerators);
            Assert.Single(e2.Enumerators);
            Assert.Equal([true, true], [e1.Enumerators[0].Disposed, e2.Enumerators[0].Disposed]);
        }

        [Fact]
        public async Task Union_DisposesAllOfThreeEmpty()
        {
            var e1 = new DisposalDetectingEnumerable(0, 0);
            var e2 = new DisposalDetectingEnumerable(0, 0);
            var e3 = new DisposalDetectingEnumerable(0, 0);
            var res = e1.Union(e2).Union(e3);

            var e = res.GetAsyncEnumerator();
            await NoNextAsync(e);

            Assert.Single(e1.Enumerators);
            Assert.Single(e2.Enumerators);
            Assert.Single(e3.Enumerators);
            Assert.Equal([true, true, true], [e1.Enumerators[0].Disposed, e2.Enumerators[0].Disposed, e3.Enumerators[0].Disposed]);
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

        private class DisposalDetectingEnumerable : IAsyncEnumerable<int>
        {
            private readonly int _start;
            private readonly int _count;

            public DisposalDetectingEnumerable(int start, int count)
            {
                _start = start;
                _count = count;
            }

            public List<Enumerator> Enumerators { get; } = [];

            public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                Enumerator r = new(_start, _count);
                Enumerators.Add(r);
                return r;
            }

            public sealed class Enumerator : IAsyncEnumerator<int>
            {
                private readonly int _max;

                public Enumerator(int start, int count)
                {
                    Current = start - 1;
                    _max = start + count;
                }

                public int Current { get; private set; }

                public bool Disposed { get; private set; }

                public void Dispose()
                {
                    Disposed = true;
                }

                public ValueTask DisposeAsync()
                {
                    Disposed = true;
                    return new ValueTask();
                }
                public ValueTask<bool> MoveNextAsync()
                {
                    if (++Current < _max)
                    {
                        return new ValueTask<bool>(true);
                    }

                    return new ValueTask<bool>(false);
                }
            }
        }
    }
}

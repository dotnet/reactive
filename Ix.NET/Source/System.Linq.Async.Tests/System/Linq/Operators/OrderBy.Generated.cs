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
    partial class OrderBy
    {
        [Fact]
        public async Task OrderBy_OrderBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2);
            var syncRes = xs.OrderBy(x => x % 2);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2));
            var syncRes = xs.OrderBy(x => x % 2);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2));
            var syncRes = xs.OrderBy(x => x % 2);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2);
            var syncRes = xs.OrderByDescending(x => x % 2);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2));
            var syncRes = xs.OrderByDescending(x => x % 2);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2));
            var syncRes = xs.OrderByDescending(x => x % 2);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenBy(x => x % 3);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescending(x => x % 3);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenBy(x => x % 3);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescending(x => x % 3);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenBy_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenBy_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenBy_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenBy_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenBy_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenBy_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescending_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescending_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescending_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescending_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescending_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescending_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderBy_ThenByDescendingAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderBy(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenBy_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenBy_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenBy_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenBy_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenBy_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenBy_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescending_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescending_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescending_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescending_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescending_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescending_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsync_ThenByDescendingAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenBy_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenBy_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenBy_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenBy_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenBy_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenBy_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescending_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescending_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescending_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescending_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescending_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescending_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByAsyncWithCancellation_ThenByDescendingAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderBy(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenBy_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenBy_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenBy_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenBy_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenBy_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenBy_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescending_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescending_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescending_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescending_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescending_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescending_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescending_ThenByDescendingAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescending(x => x % 2).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenBy_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenBy_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenBy_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenBy_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenBy_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenBy_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescending_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescending_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescending_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescending_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescending_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescending_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsync_ThenByDescendingAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwait(x => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenBy_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenBy_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenBy_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenBy_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenBy_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenBy_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenBy(x => x % 3).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenBy(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescending_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescending_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescending_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescending_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescending_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescending_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescending(x => x % 3).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsync_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsync_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsync_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsync_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsync_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsync_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwait(x => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsyncWithCancellation_ThenBy()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenBy(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsyncWithCancellation_ThenByAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsyncWithCancellation_ThenByAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenBy(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsyncWithCancellation_ThenByDescending()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescending(x => x % 4);
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsyncWithCancellation_ThenByDescendingAsync()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwait(x => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }

        [Fact]
        public async Task OrderBy_OrderByDescendingAsyncWithCancellation_ThenByDescendingAsyncWithCancellation_ThenByDescendingAsyncWithCancellation()
        {
            var rand = new Random(42);
            var xs = Enumerable.Range(0, 100).Select(x => rand.Next(0, 100)).ToArray().Select(x => x);

            var asyncRes = xs.ToAsyncEnumerable().OrderByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 2)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 3)).ThenByDescendingAwaitWithCancellation((x, ct) => new ValueTask<int>(x % 4));
            var syncRes = xs.OrderByDescending(x => x % 2).ThenByDescending(x => x % 3).ThenByDescending(x => x % 4);

            await AssertSorted(asyncRes, syncRes);

            var asyncSegment = asyncRes.AsAsyncEnumerable();
            var syncSegment = syncRes.AsEnumerable();

            foreach (var skipCount in new[] { 3, 7, 2, 5 })
            {
                asyncSegment = asyncSegment.Skip(skipCount);
                syncSegment = syncSegment.Skip(skipCount);
            }

            foreach (var takeCount in new[] { 31, 29, 23 })
            {
                asyncSegment = asyncSegment.Take(takeCount);
                syncSegment = syncSegment.Take(takeCount);
            }

            await AssertSorted(asyncSegment, syncSegment);
        }


        private async Task AssertSorted<T>(IAsyncEnumerable<T> asyncRes, IEnumerable<T> syncRes)
        {
            Assert.True(await syncRes.ToAsyncEnumerable().SequenceEqualAsync(asyncRes));
            Assert.True(syncRes.ToList().SequenceEqual(await asyncRes.ToListAsync()));
            Assert.True(syncRes.ToArray().SequenceEqual(await asyncRes.ToArrayAsync()));

            int syncCount = syncRes.Count();
            int asyncCount = await asyncRes.CountAsync();
            Assert.Equal(syncCount, asyncCount);

            Assert.Equal(syncRes.First(), await asyncRes.FirstAsync());
            Assert.Equal(syncRes.Last(), await asyncRes.LastAsync());

            for (var i = 0; i < syncCount; i++)
            {
                Assert.Equal(syncRes.ElementAt(i), await asyncRes.ElementAtAsync(i));
            }
        }
    }
}

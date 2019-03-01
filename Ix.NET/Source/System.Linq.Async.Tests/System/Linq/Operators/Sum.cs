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
    public class Sum : AsyncEnumerableTests
    {
        [Fact]
        public async Task Sum_Null()
        {
            // Sum(IAE<P>)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<int>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<int?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<long>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<long?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<double>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<double?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<float>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<float?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<decimal>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<decimal?>)).AsTask());

            // Sum(IAE<P>, CT)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<int>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<int?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<long>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<long?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<double>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<double?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<float>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<float?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<decimal>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<decimal?>), CancellationToken.None).AsTask());

            // Sum<T>(IAE<T>, Func<T, P>)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<int>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<int?>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<long>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<long?>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<double>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<double?>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<float>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<float?>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<decimal>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<decimal?>), x => x).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, int>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, int?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, long>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, long?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, double>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, double?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, float>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, float?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>)).AsTask());

            // Sum<T>(IAE<T>, Func<T, P>, CT)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<int>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<int?>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<long>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<long?>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<double>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<double?>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<float>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<float?>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<decimal>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(default(IAsyncEnumerable<decimal?>), x => x, CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, int?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, long>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, long?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, double>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, double?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, float>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, float?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAsync(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>), CancellationToken.None).AsTask());

            // Sum<T>(IAE<T>, Func<T, VT<P>>)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<int>), x => new ValueTask<int>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<int?>), x => new ValueTask<int?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<long>), x => new ValueTask<long>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<long?>), x => new ValueTask<long?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<double>), x => new ValueTask<double>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<double?>), x => new ValueTask<double?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<float>), x => new ValueTask<float>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<float?>), x => new ValueTask<float?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<decimal>), x => new ValueTask<decimal>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<decimal?>), x => new ValueTask<decimal?>(x)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<int?>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<long>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<long?>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<double>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<double?>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<float>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<float?>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<decimal>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<decimal?>>)).AsTask());

            // Sum<T>(IAE<T>, Func<T, VT<P>>, CT)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<int>), x => new ValueTask<int>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<int?>), x => new ValueTask<int?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<long>), x => new ValueTask<long>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<long?>), x => new ValueTask<long?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<double>), x => new ValueTask<double>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<double?>), x => new ValueTask<double?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<float>), x => new ValueTask<float>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<float?>), x => new ValueTask<float?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<decimal>), x => new ValueTask<decimal>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(default(IAsyncEnumerable<decimal?>), x => new ValueTask<decimal?>(x), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<int?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<long>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<long?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<double>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<double?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<float>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<float?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<decimal>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<decimal?>>), CancellationToken.None).AsTask());

#if !NO_DEEP_CANCELLATION
            // Sum<T>(IAE<T>, Func<T, CT, VT<P>>, CT)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(default(IAsyncEnumerable<int>), (x, ct) => new ValueTask<int>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(default(IAsyncEnumerable<int?>), (x, ct) => new ValueTask<int?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(default(IAsyncEnumerable<long>), (x, ct) => new ValueTask<long>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(default(IAsyncEnumerable<long?>), (x, ct) => new ValueTask<long?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(default(IAsyncEnumerable<double>), (x, ct) => new ValueTask<double>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(default(IAsyncEnumerable<double?>), (x, ct) => new ValueTask<double?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(default(IAsyncEnumerable<float>), (x, ct) => new ValueTask<float>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(default(IAsyncEnumerable<float?>), (x, ct) => new ValueTask<float?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(default(IAsyncEnumerable<decimal>), (x, ct) => new ValueTask<decimal>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(default(IAsyncEnumerable<decimal?>), (x, ct) => new ValueTask<decimal?>(x), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<int?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<long>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<long?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<double>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<double?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<float>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<float?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<decimal>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SumAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<decimal?>>), CancellationToken.None).AsTask());
#endif
        }

        [Fact]
        public async Task Sum1Async()
        {
            var xs = new[] { 1, 2, 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), await ys.SumAsync());
            Assert.Equal(xs.Sum(), await ys.SumAsync(x => x));
            Assert.Equal(xs.Sum(), await ys.SumAwaitAsync(x => new ValueTask<int>(x)));
        }

        [Fact]
        public async Task Sum2Async()
        {
            var xs = new[] { 1, default(int?), 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), await ys.SumAsync());
            Assert.Equal(xs.Sum(), await ys.SumAsync(x => x));
            Assert.Equal(xs.Sum(), await ys.SumAwaitAsync(x => new ValueTask<int?>(x)));
        }

        [Fact]
        public async Task Sum3Async()
        {
            var xs = new[] { 1L, 2L, 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), await ys.SumAsync());
            Assert.Equal(xs.Sum(), await ys.SumAsync(x => x));
            Assert.Equal(xs.Sum(), await ys.SumAwaitAsync(x => new ValueTask<long>(x)));
        }

        [Fact]
        public async Task Sum4Async()
        {
            var xs = new[] { 1L, default(long?), 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), await ys.SumAsync());
            Assert.Equal(xs.Sum(), await ys.SumAsync(x => x));
            Assert.Equal(xs.Sum(), await ys.SumAwaitAsync(x => new ValueTask<long?>(x)));
        }

        [Fact]
        public async Task Sum5Async()
        {
            var xs = new[] { 1.0, 2.0, 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), await ys.SumAsync());
            Assert.Equal(xs.Sum(), await ys.SumAsync(x => x));
            Assert.Equal(xs.Sum(), await ys.SumAwaitAsync(x => new ValueTask<double>(x)));
        }

        [Fact]
        public async Task Sum6Async()
        {
            var xs = new[] { 1.0, default(double?), 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), await ys.SumAsync());
            Assert.Equal(xs.Sum(), await ys.SumAsync(x => x));
            Assert.Equal(xs.Sum(), await ys.SumAwaitAsync(x => new ValueTask<double?>(x)));
        }

        [Fact]
        public async Task Sum7Async()
        {
            var xs = new[] { 1.0f, 2.0f, 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), await ys.SumAsync());
            Assert.Equal(xs.Sum(), await ys.SumAsync(x => x));
            Assert.Equal(xs.Sum(), await ys.SumAwaitAsync(x => new ValueTask<float>(x)));
        }

        [Fact]
        public async Task Sum8Async()
        {
            var xs = new[] { 1.0f, default(float?), 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), await ys.SumAsync());
            Assert.Equal(xs.Sum(), await ys.SumAsync(x => x));
            Assert.Equal(xs.Sum(), await ys.SumAwaitAsync(x => new ValueTask<float?>(x)));
        }

        [Fact]
        public async Task Sum9Async()
        {
            var xs = new[] { 1.0m, 2.0m, 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), await ys.SumAsync());
            Assert.Equal(xs.Sum(), await ys.SumAsync(x => x));
            Assert.Equal(xs.Sum(), await ys.SumAwaitAsync(x => new ValueTask<decimal>(x)));
        }

        [Fact]
        public async Task Sum10Async()
        {
            var xs = new[] { 1.0m, default(decimal?), 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), await ys.SumAsync());
            Assert.Equal(xs.Sum(), await ys.SumAsync(x => x));
            Assert.Equal(xs.Sum(), await ys.SumAwaitAsync(x => new ValueTask<decimal?>(x)));
        }
    }
}

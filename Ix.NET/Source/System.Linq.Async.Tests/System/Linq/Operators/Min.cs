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
    public class Min : AsyncEnumerableTests
    {
        [Fact]
        public async Task MinAsync_Null()
        {
            // Min(IAE<P>)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<int>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<int?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<long>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<long?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<double>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<double?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<float>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<float?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<decimal>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<decimal?>)).AsTask());

            // Min(IAE<P>, CT)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<int>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<int?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<long>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<long?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<double>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<double?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<float>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<float?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<decimal>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<decimal?>), CancellationToken.None).AsTask());

            // Min<T>(IAE<T>, Func<T, P>)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<int>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<int?>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<long>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<long?>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<double>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<double?>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<float>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<float?>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<decimal>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<decimal?>), x => x).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, int>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, int?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, long>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, long?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, double>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, double?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, float>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, float?>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>)).AsTask());

            // Min<T>(IAE<T>, Func<T, P>, CT)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<int>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<int?>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<long>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<long?>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<double>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<double?>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<float>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<float?>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<decimal>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<decimal?>), x => x, CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, int?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, long>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, long?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, double>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, double?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, float>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, float?>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>), CancellationToken.None).AsTask());

            // Min<T>(IAE<T>, Func<T, VT<P>>)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<int>), x => new ValueTask<int>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<int?>), x => new ValueTask<int?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<long>), x => new ValueTask<long>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<long?>), x => new ValueTask<long?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<double>), x => new ValueTask<double>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<double?>), x => new ValueTask<double?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<float>), x => new ValueTask<float>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<float?>), x => new ValueTask<float?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<decimal>), x => new ValueTask<decimal>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<decimal?>), x => new ValueTask<decimal?>(x)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<int?>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<long>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<long?>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<double>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<double?>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<float>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<float?>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<decimal>>)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<decimal?>>)).AsTask());

            // Min<T>(IAE<T>, Func<T, VT<P>>, CT)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<int>), x => new ValueTask<int>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<int?>), x => new ValueTask<int?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<long>), x => new ValueTask<long>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<long?>), x => new ValueTask<long?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<double>), x => new ValueTask<double>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<double?>), x => new ValueTask<double?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<float>), x => new ValueTask<float>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<float?>), x => new ValueTask<float?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<decimal>), x => new ValueTask<decimal>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<decimal?>), x => new ValueTask<decimal?>(x), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<int?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<long>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<long?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<double>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<double?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<float>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<float?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<decimal>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<int>(), default(Func<int, ValueTask<decimal?>>), CancellationToken.None).AsTask());

#if !NO_DEEP_CANCELLATION
            // Min<T>(IAE<T>, Func<T, CT, VT<P>>, CT)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(default(IAsyncEnumerable<int>), (x, ct) => new ValueTask<int>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(default(IAsyncEnumerable<int?>), (x, ct) => new ValueTask<int?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(default(IAsyncEnumerable<long>), (x, ct) => new ValueTask<long>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(default(IAsyncEnumerable<long?>), (x, ct) => new ValueTask<long?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(default(IAsyncEnumerable<double>), (x, ct) => new ValueTask<double>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(default(IAsyncEnumerable<double?>), (x, ct) => new ValueTask<double?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(default(IAsyncEnumerable<float>), (x, ct) => new ValueTask<float>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(default(IAsyncEnumerable<float?>), (x, ct) => new ValueTask<float?>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(default(IAsyncEnumerable<decimal>), (x, ct) => new ValueTask<decimal>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(default(IAsyncEnumerable<decimal?>), (x, ct) => new ValueTask<decimal?>(x), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<int?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<long>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<long?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<double>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<double?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<float>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<float?>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<decimal>>), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(AsyncEnumerable.Empty<int>(), default(Func<int, CancellationToken, ValueTask<decimal?>>), CancellationToken.None).AsTask());
#endif

            // Min<T>(IAE<T>)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<DateTime>)).AsTask());

            // Min<T>(IAE<T>, CT)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<DateTime>), CancellationToken.None).AsTask());

            // Min<T>(IAE<T>, Func<T, R>)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<DateTime>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>)).AsTask());

            // Min<T>(IAE<T>, Func<T, R>, CT)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(default(IAsyncEnumerable<DateTime>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAsync(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>), CancellationToken.None).AsTask());

            // Min<T>(IAE<T>, Func<T, VT<R>>)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<DateTime>), x => new ValueTask<DateTime>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, ValueTask<bool>>)).AsTask());

            // Min<T>(IAE<T>, Func<T, VT<R>>, CT)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(default(IAsyncEnumerable<DateTime>), x => new ValueTask<DateTime>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitAsync(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, ValueTask<bool>>), CancellationToken.None).AsTask());

#if !NO_DEEP_CANCELLATION
            // Min<T>(IAE<T>, Func<T, CT, VT<R>>, CT)

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(default(IAsyncEnumerable<DateTime>), (x, ct) => new ValueTask<DateTime>(x), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.MinAwaitWithCancellationAsync(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, CancellationToken, ValueTask<bool>>), CancellationToken.None).AsTask());
#endif
        }

        [Fact]
        public async Task MinAsync_Int32()
        {
            var xs = new[] { 2, 1, 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), await ys.MinAsync());
            Assert.Equal(xs.Min(), await ys.MinAsync(x => x));
            Assert.Equal(xs.Min(), await ys.MinAwaitAsync(x => new ValueTask<int>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Equal(xs.Min(), await ys.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_Nullable_Int32()
        {
            var xs = new[] { 2, default(int?), 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), await ys.MinAsync());
            Assert.Equal(xs.Min(), await ys.MinAsync(x => x));
            Assert.Equal(xs.Min(), await ys.MinAwaitAsync(x => new ValueTask<int?>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Equal(xs.Min(), await ys.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_Int64()
        {
            var xs = new[] { 2L, 1L, 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), await ys.MinAsync());
            Assert.Equal(xs.Min(), await ys.MinAsync(x => x));
            Assert.Equal(xs.Min(), await ys.MinAwaitAsync(x => new ValueTask<long>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Equal(xs.Min(), await ys.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_Nullable_Int64()
        {
            var xs = new[] { 2L, default(long?), 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), await ys.MinAsync());
            Assert.Equal(xs.Min(), await ys.MinAsync(x => x));
            Assert.Equal(xs.Min(), await ys.MinAwaitAsync(x => new ValueTask<long?>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Equal(xs.Min(), await ys.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_Double()
        {
            var xs = new[] { 2.0, 1.0, 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), await ys.MinAsync());
            Assert.Equal(xs.Min(), await ys.MinAsync(x => x));
            Assert.Equal(xs.Min(), await ys.MinAwaitAsync(x => new ValueTask<double>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Equal(xs.Min(), await ys.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_Nullable_Double()
        {
            var xs = new[] { 2.0, default(double?), 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), await ys.MinAsync());
            Assert.Equal(xs.Min(), await ys.MinAsync(x => x));
            Assert.Equal(xs.Min(), await ys.MinAwaitAsync(x => new ValueTask<double?>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Equal(xs.Min(), await ys.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_Single()
        {
            var xs = new[] { 2.0f, 1.0f, 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), await ys.MinAsync());
            Assert.Equal(xs.Min(), await ys.MinAsync(x => x));
            Assert.Equal(xs.Min(), await ys.MinAwaitAsync(x => new ValueTask<float>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Equal(xs.Min(), await ys.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_Nullable_Single()
        {
            var xs = new[] { 2.0f, default(float?), 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), await ys.MinAsync());
            Assert.Equal(xs.Min(), await ys.MinAsync(x => x));
            Assert.Equal(xs.Min(), await ys.MinAwaitAsync(x => new ValueTask<float?>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Equal(xs.Min(), await ys.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_Decimal()
        {
            var xs = new[] { 2.0m, 1.0m, 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), await ys.MinAsync());
            Assert.Equal(xs.Min(), await ys.MinAsync(x => x));
            Assert.Equal(xs.Min(), await ys.MinAwaitAsync(x => new ValueTask<decimal>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Equal(xs.Min(), await ys.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_Nullable_Decimal()
        {
            var xs = new[] { 2.0m, default(decimal?), 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), await ys.MinAsync());
            Assert.Equal(xs.Min(), await ys.MinAsync(x => x));
            Assert.Equal(xs.Min(), await ys.MinAwaitAsync(x => new ValueTask<decimal?>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Equal(xs.Min(), await ys.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_TSource_Value()
        {
            var xs = new[] { DateTime.Now.AddDays(1), DateTime.Now.Subtract(TimeSpan.FromDays(1)), DateTime.Now.AddDays(2), DateTime.Now };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), await ys.MinAsync());
            Assert.Equal(xs.Min(), await ys.MinAsync(x => x));
            Assert.Equal(xs.Min(), await ys.MinAwaitAsync(x => new ValueTask<DateTime>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Equal(xs.Min(), await ys.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<DateTime>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_TSource_NonValue()
        {
            var xs = new[] { "foo", "bar", "qux", "baz", "fred", "wilma" };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), await ys.MinAsync());
            Assert.Equal(xs.Min(), await ys.MinAsync(x => x));
            Assert.Equal(xs.Min(), await ys.MinAwaitAsync(x => new ValueTask<string>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Equal(xs.Min(), await ys.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<string>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_TSource_Value_Empty()
        {
            var xs = new DateTimeOffset[0].ToAsyncEnumerable();
            await AssertThrowsAsync<InvalidOperationException>(xs.MinAsync().AsTask());
            await AssertThrowsAsync<InvalidOperationException>(xs.MinAsync(x => x).AsTask());
            await AssertThrowsAsync<InvalidOperationException>(xs.MinAwaitAsync(x => new ValueTask<DateTimeOffset>(x)).AsTask());
#if !NO_DEEP_CANCELLATION
            await AssertThrowsAsync<InvalidOperationException>(xs.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<DateTimeOffset>(x)).AsTask());
#endif
        }

        [Fact]
        public async Task MinAsync_TSource_NonValue_Empty()
        {
            var xs = new string[0].ToAsyncEnumerable();
            Assert.Null(await xs.MinAsync());
            Assert.Null(await xs.MinAsync(x => x));
            Assert.Null(await xs.MinAwaitAsync(x => new ValueTask<string>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Null(await xs.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<string>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_TSource_NullableValue_Empty()
        {
            var xs = new DateTimeOffset?[0].ToAsyncEnumerable();
            Assert.Null(await xs.MinAsync());
            Assert.Null(await xs.MinAsync(x => x));
            Assert.Null(await xs.MinAwaitAsync(x => new ValueTask<DateTimeOffset?>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Null(await xs.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<DateTimeOffset?>(x)));
#endif
        }

        [Fact]
        public async Task MinAsync_TSource_NullableValue_SomeNull()
        {
            var xs = new DateTimeOffset?[] { null, null, DateTime.Now.AddDays(1), DateTime.Now.Subtract(TimeSpan.FromDays(1)), null, DateTime.Now.AddDays(2), DateTime.Now, null };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), await ys.MinAsync());
            Assert.Equal(xs.Min(), await ys.MinAsync(x => x));
            Assert.Equal(xs.Min(), await ys.MinAwaitAsync(x => new ValueTask<DateTimeOffset?>(x)));
#if !NO_DEEP_CANCELLATION
            Assert.Equal(xs.Min(), await ys.MinAwaitWithCancellationAsync((x, ct) => new ValueTask<DateTimeOffset?>(x)));
#endif
        }
    }
}

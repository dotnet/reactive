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
    public class AverageAsync : AsyncEnumerableTests
    {
        [Fact]
        public async Task AverageAsync_Int32_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<int>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Int32_Empty()
        {
            var ys = new int[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync().AsTask());
        }

        [Fact]
        public async Task AverageAsync_Int32_Many()
        {
            var xs = new int[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Int32_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<int>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(AsyncEnumerable.Empty<object>(), default(Func<object, int>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Selector_Int32_Empty()
        {
            var ys = new int[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => x).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Selector_Int32_Many()
        {
            var xs = new int[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Int32_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(default(IAsyncEnumerable<int>), x => new ValueTask<int>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(AsyncEnumerable.Empty<object>(), default(Func<object, ValueTask<int>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Int32_Empty()
        {
            var ys = new int[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAwaitAsync(x => new ValueTask<int>(x)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Int32_Many()
        {
            var xs = new int[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitAsync(x => new ValueTask<int>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Int32_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(default(IAsyncEnumerable<int>), (x, ct) => new ValueTask<int>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(AsyncEnumerable.Empty<object>(), default(Func<object, CancellationToken, ValueTask<int>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Int32_Empty()
        {
            var ys = new int[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<int>(x), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Int32_Many()
        {
            var xs = new int[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<int>(x), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task AverageAsync_Int32_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<int?>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Int32_Nullable_Empty()
        {
            var ys = new int?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Int32_Nullable_Many()
        {
            var xs = new int?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Int32_Nullable_Empty()
        {
            var ys = new int?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_Selector_Int32_Nullable_Many()
        {
            var xs = new int?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Int32_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(default(IAsyncEnumerable<int?>), x => new ValueTask<int?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(AsyncEnumerable.Empty<object>(), default(Func<object, ValueTask<int?>>)).AsTask());
        }


        [Fact]
        public async Task AverageAwaitAsync_Selector_Int32_Nullable_Empty()
        {
            var ys = new int?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAwaitAsync(x => new ValueTask<int?>(x)));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Int32_Nullable_Many()
        {
            var xs = new int?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitAsync(x => new ValueTask<int?>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Int32_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(default(IAsyncEnumerable<int?>), (x, ct) => new ValueTask<int?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(AsyncEnumerable.Empty<object>(), default(Func<object, CancellationToken, ValueTask<int?>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Int32_Nullable_Empty()
        {
            var ys = new int?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Int32_Nullable_Many()
        {
            var xs = new int?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task AverageAsync_Int64_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<long>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Int64_Empty()
        {
            var ys = new long[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync().AsTask());
        }

        [Fact]
        public async Task AverageAsync_Int64_Many()
        {
            var xs = new long[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Int64_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<long>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(AsyncEnumerable.Empty<object>(), default(Func<object, long>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Selector_Int64_Empty()
        {
            var ys = new long[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => x).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Selector_Int64_Many()
        {
            var xs = new long[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Int64_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(default(IAsyncEnumerable<long>), x => new ValueTask<long>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(AsyncEnumerable.Empty<object>(), default(Func<object, ValueTask<long>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Int64_Empty()
        {
            var ys = new long[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAwaitAsync(x => new ValueTask<long>(x)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Int64_Many()
        {
            var xs = new long[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitAsync(x => new ValueTask<long>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Int64_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(default(IAsyncEnumerable<long>), (x, ct) => new ValueTask<long>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(AsyncEnumerable.Empty<object>(), default(Func<object, CancellationToken, ValueTask<long>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Int64_Empty()
        {
            var ys = new long[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<long>(x), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Int64_Many()
        {
            var xs = new long[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<long>(x), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task AverageAsync_Int64_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<long?>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Int64_Nullable_Empty()
        {
            var ys = new long?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Int64_Nullable_Many()
        {
            var xs = new long?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Int64_Nullable_Empty()
        {
            var ys = new long?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_Selector_Int64_Nullable_Many()
        {
            var xs = new long?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Int64_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(default(IAsyncEnumerable<long?>), x => new ValueTask<long?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(AsyncEnumerable.Empty<object>(), default(Func<object, ValueTask<long?>>)).AsTask());
        }


        [Fact]
        public async Task AverageAwaitAsync_Selector_Int64_Nullable_Empty()
        {
            var ys = new long?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAwaitAsync(x => new ValueTask<long?>(x)));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Int64_Nullable_Many()
        {
            var xs = new long?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitAsync(x => new ValueTask<long?>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Int64_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(default(IAsyncEnumerable<long?>), (x, ct) => new ValueTask<long?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(AsyncEnumerable.Empty<object>(), default(Func<object, CancellationToken, ValueTask<long?>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Int64_Nullable_Empty()
        {
            var ys = new long?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Int64_Nullable_Many()
        {
            var xs = new long?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task AverageAsync_Single_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<float>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Single_Empty()
        {
            var ys = new float[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync().AsTask());
        }

        [Fact]
        public async Task AverageAsync_Single_Many()
        {
            var xs = new float[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Single_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<float>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(AsyncEnumerable.Empty<object>(), default(Func<object, float>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Selector_Single_Empty()
        {
            var ys = new float[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => x).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Selector_Single_Many()
        {
            var xs = new float[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Single_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(default(IAsyncEnumerable<float>), x => new ValueTask<float>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(AsyncEnumerable.Empty<object>(), default(Func<object, ValueTask<float>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Single_Empty()
        {
            var ys = new float[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAwaitAsync(x => new ValueTask<float>(x)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Single_Many()
        {
            var xs = new float[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitAsync(x => new ValueTask<float>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Single_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(default(IAsyncEnumerable<float>), (x, ct) => new ValueTask<float>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(AsyncEnumerable.Empty<object>(), default(Func<object, CancellationToken, ValueTask<float>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Single_Empty()
        {
            var ys = new float[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Single_Many()
        {
            var xs = new float[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task AverageAsync_Single_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<float?>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Single_Nullable_Empty()
        {
            var ys = new float?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Single_Nullable_Many()
        {
            var xs = new float?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Single_Nullable_Empty()
        {
            var ys = new float?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_Selector_Single_Nullable_Many()
        {
            var xs = new float?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Single_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(default(IAsyncEnumerable<float?>), x => new ValueTask<float?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(AsyncEnumerable.Empty<object>(), default(Func<object, ValueTask<float?>>)).AsTask());
        }


        [Fact]
        public async Task AverageAwaitAsync_Selector_Single_Nullable_Empty()
        {
            var ys = new float?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAwaitAsync(x => new ValueTask<float?>(x)));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Single_Nullable_Many()
        {
            var xs = new float?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitAsync(x => new ValueTask<float?>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Single_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(default(IAsyncEnumerable<float?>), (x, ct) => new ValueTask<float?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(AsyncEnumerable.Empty<object>(), default(Func<object, CancellationToken, ValueTask<float?>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Single_Nullable_Empty()
        {
            var ys = new float?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Single_Nullable_Many()
        {
            var xs = new float?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task AverageAsync_Double_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<double>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Double_Empty()
        {
            var ys = new double[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync().AsTask());
        }

        [Fact]
        public async Task AverageAsync_Double_Many()
        {
            var xs = new double[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Double_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<double>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(AsyncEnumerable.Empty<object>(), default(Func<object, double>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Selector_Double_Empty()
        {
            var ys = new double[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => x).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Selector_Double_Many()
        {
            var xs = new double[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Double_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(default(IAsyncEnumerable<double>), x => new ValueTask<double>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(AsyncEnumerable.Empty<object>(), default(Func<object, ValueTask<double>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Double_Empty()
        {
            var ys = new double[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAwaitAsync(x => new ValueTask<double>(x)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Double_Many()
        {
            var xs = new double[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitAsync(x => new ValueTask<double>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Double_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(default(IAsyncEnumerable<double>), (x, ct) => new ValueTask<double>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(AsyncEnumerable.Empty<object>(), default(Func<object, CancellationToken, ValueTask<double>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Double_Empty()
        {
            var ys = new double[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Double_Many()
        {
            var xs = new double[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task AverageAsync_Double_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<double?>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Double_Nullable_Empty()
        {
            var ys = new double?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Double_Nullable_Many()
        {
            var xs = new double?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Double_Nullable_Empty()
        {
            var ys = new double?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_Selector_Double_Nullable_Many()
        {
            var xs = new double?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Double_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(default(IAsyncEnumerable<double?>), x => new ValueTask<double?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(AsyncEnumerable.Empty<object>(), default(Func<object, ValueTask<double?>>)).AsTask());
        }


        [Fact]
        public async Task AverageAwaitAsync_Selector_Double_Nullable_Empty()
        {
            var ys = new double?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAwaitAsync(x => new ValueTask<double?>(x)));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Double_Nullable_Many()
        {
            var xs = new double?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitAsync(x => new ValueTask<double?>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Double_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(default(IAsyncEnumerable<double?>), (x, ct) => new ValueTask<double?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(AsyncEnumerable.Empty<object>(), default(Func<object, CancellationToken, ValueTask<double?>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Double_Nullable_Empty()
        {
            var ys = new double?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Double_Nullable_Many()
        {
            var xs = new double?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task AverageAsync_Decimal_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<decimal>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Decimal_Empty()
        {
            var ys = new decimal[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync().AsTask());
        }

        [Fact]
        public async Task AverageAsync_Decimal_Many()
        {
            var xs = new decimal[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Decimal_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<decimal>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(AsyncEnumerable.Empty<object>(), default(Func<object, decimal>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Selector_Decimal_Empty()
        {
            var ys = new decimal[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => x).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Selector_Decimal_Many()
        {
            var xs = new decimal[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Decimal_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(default(IAsyncEnumerable<decimal>), x => new ValueTask<decimal>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(AsyncEnumerable.Empty<object>(), default(Func<object, ValueTask<decimal>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Decimal_Empty()
        {
            var ys = new decimal[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAwaitAsync(x => new ValueTask<decimal>(x)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Decimal_Many()
        {
            var xs = new decimal[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitAsync(x => new ValueTask<decimal>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Decimal_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(default(IAsyncEnumerable<decimal>), (x, ct) => new ValueTask<decimal>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(AsyncEnumerable.Empty<object>(), default(Func<object, CancellationToken, ValueTask<decimal>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Decimal_Empty()
        {
            var ys = new decimal[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal>(x), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Decimal_Many()
        {
            var xs = new decimal[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal>(x), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task AverageAsync_Decimal_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAsync(default(IAsyncEnumerable<decimal?>)).AsTask());
        }

        [Fact]
        public async Task AverageAsync_Decimal_Nullable_Empty()
        {
            var ys = new decimal?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Decimal_Nullable_Many()
        {
            var xs = new decimal?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Decimal_Nullable_Empty()
        {
            var ys = new decimal?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_Selector_Decimal_Nullable_Many()
        {
            var xs = new decimal?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Decimal_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(default(IAsyncEnumerable<decimal?>), x => new ValueTask<decimal?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitAsync(AsyncEnumerable.Empty<object>(), default(Func<object, ValueTask<decimal?>>)).AsTask());
        }


        [Fact]
        public async Task AverageAwaitAsync_Selector_Decimal_Nullable_Empty()
        {
            var ys = new decimal?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAwaitAsync(x => new ValueTask<decimal?>(x)));
        }

        [Fact]
        public async Task AverageAwaitAsync_Selector_Decimal_Nullable_Many()
        {
            var xs = new decimal?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitAsync(x => new ValueTask<decimal?>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Decimal_Nullable_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(default(IAsyncEnumerable<decimal?>), (x, ct) => new ValueTask<decimal?>(x)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AverageAwaitWithCancellationAsync(AsyncEnumerable.Empty<object>(), default(Func<object, CancellationToken, ValueTask<decimal?>>)).AsTask());
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Decimal_Nullable_Empty()
        {
            var ys = new decimal?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAwaitWithCancellationAsync_Selector_Decimal_Nullable_Many()
        {
            var xs = new decimal?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None));
        }
#endif

    }
}

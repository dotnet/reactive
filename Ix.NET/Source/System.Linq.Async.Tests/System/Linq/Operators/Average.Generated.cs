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
        public async Task AverageAsync_Int32_Empty()
        {
            var ys = new int[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Int32_Many()
        {
            var xs = new int[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Int32_Empty()
        {
            var ys = new int[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_Selector_Int32_Many()
        {
            var xs = new int[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Int32_Empty()
        {
            var ys = new int[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => new ValueTask<int>(x)));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Int32_Many()
        {
            var xs = new int[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => new ValueTask<int>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Int32_Empty()
        {
            var ys = new int[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync((x, ct) => new ValueTask<int>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Int32_Many()
        {
            var xs = new int[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync((x, ct) => new ValueTask<int>(x), CancellationToken.None));
        }
#endif

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
        public async Task AverageAsync_AsyncSelector_Int32_Nullable_Empty()
        {
            var ys = new int?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync(x => new ValueTask<int?>(x)));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Int32_Nullable_Many()
        {
            var xs = new int?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => new ValueTask<int?>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Int32_Nullable_Empty()
        {
            var ys = new int?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Int32_Nullable_Many()
        {
            var xs = new int?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task AverageAsync_Int64_Empty()
        {
            var ys = new long[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Int64_Many()
        {
            var xs = new long[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Int64_Empty()
        {
            var ys = new long[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_Selector_Int64_Many()
        {
            var xs = new long[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Int64_Empty()
        {
            var ys = new long[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => new ValueTask<long>(x)));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Int64_Many()
        {
            var xs = new long[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => new ValueTask<long>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Int64_Empty()
        {
            var ys = new long[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync((x, ct) => new ValueTask<long>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Int64_Many()
        {
            var xs = new long[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync((x, ct) => new ValueTask<long>(x), CancellationToken.None));
        }
#endif

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
        public async Task AverageAsync_AsyncSelector_Int64_Nullable_Empty()
        {
            var ys = new long?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync(x => new ValueTask<long?>(x)));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Int64_Nullable_Many()
        {
            var xs = new long?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => new ValueTask<long?>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Int64_Nullable_Empty()
        {
            var ys = new long?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Int64_Nullable_Many()
        {
            var xs = new long?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task AverageAsync_Single_Empty()
        {
            var ys = new float[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Single_Many()
        {
            var xs = new float[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Single_Empty()
        {
            var ys = new float[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_Selector_Single_Many()
        {
            var xs = new float[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Single_Empty()
        {
            var ys = new float[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => new ValueTask<float>(x)));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Single_Many()
        {
            var xs = new float[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => new ValueTask<float>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Single_Empty()
        {
            var ys = new float[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Single_Many()
        {
            var xs = new float[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None));
        }
#endif

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
        public async Task AverageAsync_AsyncSelector_Single_Nullable_Empty()
        {
            var ys = new float?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync(x => new ValueTask<float?>(x)));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Single_Nullable_Many()
        {
            var xs = new float?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => new ValueTask<float?>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Single_Nullable_Empty()
        {
            var ys = new float?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Single_Nullable_Many()
        {
            var xs = new float?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task AverageAsync_Double_Empty()
        {
            var ys = new double[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Double_Many()
        {
            var xs = new double[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Double_Empty()
        {
            var ys = new double[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_Selector_Double_Many()
        {
            var xs = new double[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Double_Empty()
        {
            var ys = new double[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => new ValueTask<double>(x)));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Double_Many()
        {
            var xs = new double[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => new ValueTask<double>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Double_Empty()
        {
            var ys = new double[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Double_Many()
        {
            var xs = new double[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None));
        }
#endif

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
        public async Task AverageAsync_AsyncSelector_Double_Nullable_Empty()
        {
            var ys = new double?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync(x => new ValueTask<double?>(x)));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Double_Nullable_Many()
        {
            var xs = new double?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => new ValueTask<double?>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Double_Nullable_Empty()
        {
            var ys = new double?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Double_Nullable_Many()
        {
            var xs = new double?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task AverageAsync_Decimal_Empty()
        {
            var ys = new decimal[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Decimal_Many()
        {
            var xs = new decimal[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync());
        }

        [Fact]
        public async Task AverageAsync_Selector_Decimal_Empty()
        {
            var ys = new decimal[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_Selector_Decimal_Many()
        {
            var xs = new decimal[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => x));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Decimal_Empty()
        {
            var ys = new decimal[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync(x => new ValueTask<decimal>(x)));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Decimal_Many()
        {
            var xs = new decimal[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => new ValueTask<decimal>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Decimal_Empty()
        {
            var ys = new decimal[0].ToAsyncEnumerable();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ys.AverageAsync((x, ct) => new ValueTask<decimal>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Decimal_Many()
        {
            var xs = new decimal[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync((x, ct) => new ValueTask<decimal>(x), CancellationToken.None));
        }
#endif

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
        public async Task AverageAsync_AsyncSelector_Decimal_Nullable_Empty()
        {
            var ys = new decimal?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync(x => new ValueTask<decimal?>(x)));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelector_Decimal_Nullable_Many()
        {
            var xs = new decimal?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync(x => new ValueTask<decimal?>(x)));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Decimal_Nullable_Empty()
        {
            var ys = new decimal?[0].ToAsyncEnumerable();
            Assert.Null(await ys.AverageAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None));
        }

        [Fact]
        public async Task AverageAsync_AsyncSelectorWithCancellation_Decimal_Nullable_Many()
        {
            var xs = new decimal?[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Average(), await ys.AverageAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None));
        }
#endif

    }
}

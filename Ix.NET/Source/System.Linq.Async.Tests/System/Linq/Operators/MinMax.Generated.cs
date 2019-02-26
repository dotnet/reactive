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
    public class MinMax : AsyncEnumerableTests
    {
        [Fact]
        public async Task MinAsync_Empty_Int32()
        {
            await AssertThrowsAsync<InvalidOperationException>(new int[0].ToAsyncEnumerable().MinAsync(CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MinAsync_Selector_Empty_Int32()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAsync(_ => default(int), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Empty_Int32()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAwaitAsync(_ => default(ValueTask<int>), CancellationToken.None).AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Empty_Int32()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => default(ValueTask<int>), CancellationToken.None).AsTask());
        }
#endif

        [Fact]
        public async Task MinAsync_Empty_Nullable_Int32()
        {
            Assert.Null(await new int?[0].ToAsyncEnumerable().MinAsync(CancellationToken.None));
        }

        [Fact]
        public async Task MinAsync_Selector_Empty_Nullable_Int32()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAsync(_ => default(int?), CancellationToken.None));
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Empty_Nullable_Int32()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAwaitAsync(_ => default(ValueTask<int?>), CancellationToken.None));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Empty_Nullable_Int32()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => default(ValueTask<int?>), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_1()
        {
            var input = new int[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_1()
        {
            var input = new int[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_1()
        {
            var input = new int[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_1()
        {
            var input = new int[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_Nullable_1_NoNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_Nullable_1_NoNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_Nullable_1_NoNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_Nullable_1_NoNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_Nullable_1_SomeNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_Nullable_1_SomeNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_Nullable_1_SomeNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_Nullable_1_SomeNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_4()
        {
            var input = new int[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_4()
        {
            var input = new int[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_4()
        {
            var input = new int[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_4()
        {
            var input = new int[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_Nullable_4_NoNull()
        {
            var input = new int?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_Nullable_4_NoNull()
        {
            var input = new int?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_Nullable_4_NoNull()
        {
            var input = new int?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_Nullable_4_NoNull()
        {
            var input = new int?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_Nullable_4_SomeNull()
        {
            var input = new int?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_Nullable_4_SomeNull()
        {
            var input = new int?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_Nullable_4_SomeNull()
        {
            var input = new int?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_Nullable_4_SomeNull()
        {
            var input = new int?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_8()
        {
            var input = new int[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_8()
        {
            var input = new int[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_8()
        {
            var input = new int[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_8()
        {
            var input = new int[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_Nullable_8_NoNull()
        {
            var input = new int?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_Nullable_8_NoNull()
        {
            var input = new int?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_Nullable_8_NoNull()
        {
            var input = new int?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_Nullable_8_NoNull()
        {
            var input = new int?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_Nullable_8_SomeNull()
        {
            var input = new int?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_Nullable_8_SomeNull()
        {
            var input = new int?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_Nullable_8_SomeNull()
        {
            var input = new int?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_Nullable_8_SomeNull()
        {
            var input = new int?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_16()
        {
            var input = new int[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_16()
        {
            var input = new int[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_16()
        {
            var input = new int[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_16()
        {
            var input = new int[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_Nullable_16_NoNull()
        {
            var input = new int?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_Nullable_16_NoNull()
        {
            var input = new int?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_Nullable_16_NoNull()
        {
            var input = new int?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_Nullable_16_NoNull()
        {
            var input = new int?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Int32_Nullable_16_SomeNull()
        {
            var input = new int?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int32_Nullable_16_SomeNull()
        {
            var input = new int?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int32_Nullable_16_SomeNull()
        {
            var input = new int?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int32_Nullable_16_SomeNull()
        {
            var input = new int?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Empty_Int64()
        {
            await AssertThrowsAsync<InvalidOperationException>(new long[0].ToAsyncEnumerable().MinAsync(CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MinAsync_Selector_Empty_Int64()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAsync(_ => default(long), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Empty_Int64()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAwaitAsync(_ => default(ValueTask<long>), CancellationToken.None).AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Empty_Int64()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => default(ValueTask<long>), CancellationToken.None).AsTask());
        }
#endif

        [Fact]
        public async Task MinAsync_Empty_Nullable_Int64()
        {
            Assert.Null(await new long?[0].ToAsyncEnumerable().MinAsync(CancellationToken.None));
        }

        [Fact]
        public async Task MinAsync_Selector_Empty_Nullable_Int64()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAsync(_ => default(long?), CancellationToken.None));
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Empty_Nullable_Int64()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAwaitAsync(_ => default(ValueTask<long?>), CancellationToken.None));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Empty_Nullable_Int64()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => default(ValueTask<long?>), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_1()
        {
            var input = new long[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_1()
        {
            var input = new long[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_1()
        {
            var input = new long[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_1()
        {
            var input = new long[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_Nullable_1_NoNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_Nullable_1_NoNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_Nullable_1_NoNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_Nullable_1_NoNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_Nullable_1_SomeNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_Nullable_1_SomeNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_Nullable_1_SomeNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_Nullable_1_SomeNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_4()
        {
            var input = new long[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_4()
        {
            var input = new long[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_4()
        {
            var input = new long[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_4()
        {
            var input = new long[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_Nullable_4_NoNull()
        {
            var input = new long?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_Nullable_4_NoNull()
        {
            var input = new long?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_Nullable_4_NoNull()
        {
            var input = new long?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_Nullable_4_NoNull()
        {
            var input = new long?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_Nullable_4_SomeNull()
        {
            var input = new long?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_Nullable_4_SomeNull()
        {
            var input = new long?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_Nullable_4_SomeNull()
        {
            var input = new long?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_Nullable_4_SomeNull()
        {
            var input = new long?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_8()
        {
            var input = new long[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_8()
        {
            var input = new long[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_8()
        {
            var input = new long[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_8()
        {
            var input = new long[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_Nullable_8_NoNull()
        {
            var input = new long?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_Nullable_8_NoNull()
        {
            var input = new long?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_Nullable_8_NoNull()
        {
            var input = new long?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_Nullable_8_NoNull()
        {
            var input = new long?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_Nullable_8_SomeNull()
        {
            var input = new long?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_Nullable_8_SomeNull()
        {
            var input = new long?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_Nullable_8_SomeNull()
        {
            var input = new long?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_Nullable_8_SomeNull()
        {
            var input = new long?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_16()
        {
            var input = new long[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_16()
        {
            var input = new long[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_16()
        {
            var input = new long[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_16()
        {
            var input = new long[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_Nullable_16_NoNull()
        {
            var input = new long?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_Nullable_16_NoNull()
        {
            var input = new long?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_Nullable_16_NoNull()
        {
            var input = new long?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_Nullable_16_NoNull()
        {
            var input = new long?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Int64_Nullable_16_SomeNull()
        {
            var input = new long?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Int64_Nullable_16_SomeNull()
        {
            var input = new long?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Int64_Nullable_16_SomeNull()
        {
            var input = new long?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Int64_Nullable_16_SomeNull()
        {
            var input = new long?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Empty_Single()
        {
            await AssertThrowsAsync<InvalidOperationException>(new float[0].ToAsyncEnumerable().MinAsync(CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MinAsync_Selector_Empty_Single()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAsync(_ => default(float), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Empty_Single()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAwaitAsync(_ => default(ValueTask<float>), CancellationToken.None).AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Empty_Single()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => default(ValueTask<float>), CancellationToken.None).AsTask());
        }
#endif

        [Fact]
        public async Task MinAsync_Empty_Nullable_Single()
        {
            Assert.Null(await new float?[0].ToAsyncEnumerable().MinAsync(CancellationToken.None));
        }

        [Fact]
        public async Task MinAsync_Selector_Empty_Nullable_Single()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAsync(_ => default(float?), CancellationToken.None));
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Empty_Nullable_Single()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAwaitAsync(_ => default(ValueTask<float?>), CancellationToken.None));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Empty_Nullable_Single()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => default(ValueTask<float?>), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task MinAsync_Single_1()
        {
            var input = new float[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_1()
        {
            var input = new float[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_1()
        {
            var input = new float[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_1()
        {
            var input = new float[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_Nullable_1_NoNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_Nullable_1_NoNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_Nullable_1_NoNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_Nullable_1_NoNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_4()
        {
            var input = new float[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_4()
        {
            var input = new float[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_4()
        {
            var input = new float[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_4()
        {
            var input = new float[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_Nullable_4_NoNull()
        {
            var input = new float?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_Nullable_4_NoNull()
        {
            var input = new float?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_Nullable_4_NoNull()
        {
            var input = new float?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_Nullable_4_NoNull()
        {
            var input = new float?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_8()
        {
            var input = new float[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_8()
        {
            var input = new float[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_8()
        {
            var input = new float[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_8()
        {
            var input = new float[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_Nullable_8_NoNull()
        {
            var input = new float?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_Nullable_8_NoNull()
        {
            var input = new float?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_Nullable_8_NoNull()
        {
            var input = new float?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_Nullable_8_NoNull()
        {
            var input = new float?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_16()
        {
            var input = new float[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_16()
        {
            var input = new float[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_16()
        {
            var input = new float[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_16()
        {
            var input = new float[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_Nullable_16_NoNull()
        {
            var input = new float?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_Nullable_16_NoNull()
        {
            var input = new float?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_Nullable_16_NoNull()
        {
            var input = new float?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_Nullable_16_NoNull()
        {
            var input = new float?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_1()
        {
            var input = new float[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_1()
        {
            var input = new float[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_1()
        {
            var input = new float[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_1()
        {
            var input = new float[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_1_NoNull()
        {
            var input = new float?[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_1_NoNull()
        {
            var input = new float?[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_1_NoNull()
        {
            var input = new float?[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_1_NoNull()
        {
            var input = new float?[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MinAsync_Single_WithNaN_4()
        {
            var input = new float[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_4()
        {
            var input = new float[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_4()
        {
            var input = new float[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_4()
        {
            var input = new float[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_4_NoNull()
        {
            var input = new float?[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_4_NoNull()
        {
            var input = new float?[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_4_NoNull()
        {
            var input = new float?[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_4_NoNull()
        {
            var input = new float?[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MinAsync_Single_WithNaN_8()
        {
            var input = new float[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_8()
        {
            var input = new float[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_8()
        {
            var input = new float[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_8()
        {
            var input = new float[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_8_NoNull()
        {
            var input = new float?[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_8_NoNull()
        {
            var input = new float?[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_8_NoNull()
        {
            var input = new float?[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_8_NoNull()
        {
            var input = new float?[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, float.NaN, -46, null, float.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, float.NaN, -46, null, float.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, float.NaN, -46, null, float.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, float.NaN, -46, null, float.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MinAsync_Single_WithNaN_16()
        {
            var input = new float[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_16()
        {
            var input = new float[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_16()
        {
            var input = new float[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_16()
        {
            var input = new float[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_16_NoNull()
        {
            var input = new float?[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_16_NoNull()
        {
            var input = new float?[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_16_NoNull()
        {
            var input = new float?[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_16_NoNull()
        {
            var input = new float?[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Single_WithNaN_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, float.NaN, 98, null, float.NaN, -100, null, float.NaN, -86, null, float.NaN, 19, null, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Single_WithNaN_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, float.NaN, 98, null, float.NaN, -100, null, float.NaN, -86, null, float.NaN, 19, null, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Single_WithNaN_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, float.NaN, 98, null, float.NaN, -100, null, float.NaN, -86, null, float.NaN, 19, null, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, float.NaN, 98, null, float.NaN, -100, null, float.NaN, -86, null, float.NaN, 19, null, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MinAsync_Empty_Double()
        {
            await AssertThrowsAsync<InvalidOperationException>(new double[0].ToAsyncEnumerable().MinAsync(CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MinAsync_Selector_Empty_Double()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAsync(_ => default(double), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Empty_Double()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAwaitAsync(_ => default(ValueTask<double>), CancellationToken.None).AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Empty_Double()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => default(ValueTask<double>), CancellationToken.None).AsTask());
        }
#endif

        [Fact]
        public async Task MinAsync_Empty_Nullable_Double()
        {
            Assert.Null(await new double?[0].ToAsyncEnumerable().MinAsync(CancellationToken.None));
        }

        [Fact]
        public async Task MinAsync_Selector_Empty_Nullable_Double()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAsync(_ => default(double?), CancellationToken.None));
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Empty_Nullable_Double()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAwaitAsync(_ => default(ValueTask<double?>), CancellationToken.None));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Empty_Nullable_Double()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => default(ValueTask<double?>), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task MinAsync_Double_1()
        {
            var input = new double[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_1()
        {
            var input = new double[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_1()
        {
            var input = new double[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_1()
        {
            var input = new double[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_Nullable_1_NoNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_Nullable_1_NoNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_Nullable_1_NoNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_Nullable_1_NoNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_4()
        {
            var input = new double[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_4()
        {
            var input = new double[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_4()
        {
            var input = new double[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_4()
        {
            var input = new double[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_Nullable_4_NoNull()
        {
            var input = new double?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_Nullable_4_NoNull()
        {
            var input = new double?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_Nullable_4_NoNull()
        {
            var input = new double?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_Nullable_4_NoNull()
        {
            var input = new double?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_8()
        {
            var input = new double[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_8()
        {
            var input = new double[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_8()
        {
            var input = new double[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_8()
        {
            var input = new double[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_Nullable_8_NoNull()
        {
            var input = new double?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_Nullable_8_NoNull()
        {
            var input = new double?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_Nullable_8_NoNull()
        {
            var input = new double?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_Nullable_8_NoNull()
        {
            var input = new double?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_16()
        {
            var input = new double[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_16()
        {
            var input = new double[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_16()
        {
            var input = new double[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_16()
        {
            var input = new double[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_Nullable_16_NoNull()
        {
            var input = new double?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_Nullable_16_NoNull()
        {
            var input = new double?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_Nullable_16_NoNull()
        {
            var input = new double?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_Nullable_16_NoNull()
        {
            var input = new double?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_1()
        {
            var input = new double[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_1()
        {
            var input = new double[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_1()
        {
            var input = new double[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_1()
        {
            var input = new double[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_1_NoNull()
        {
            var input = new double?[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_1_NoNull()
        {
            var input = new double?[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_1_NoNull()
        {
            var input = new double?[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_1_NoNull()
        {
            var input = new double?[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MinAsync_Double_WithNaN_4()
        {
            var input = new double[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_4()
        {
            var input = new double[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_4()
        {
            var input = new double[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_4()
        {
            var input = new double[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_4_NoNull()
        {
            var input = new double?[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_4_NoNull()
        {
            var input = new double?[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_4_NoNull()
        {
            var input = new double?[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_4_NoNull()
        {
            var input = new double?[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MinAsync_Double_WithNaN_8()
        {
            var input = new double[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_8()
        {
            var input = new double[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_8()
        {
            var input = new double[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_8()
        {
            var input = new double[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_8_NoNull()
        {
            var input = new double?[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_8_NoNull()
        {
            var input = new double?[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_8_NoNull()
        {
            var input = new double?[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_8_NoNull()
        {
            var input = new double?[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, double.NaN, -46, null, double.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, double.NaN, -46, null, double.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, double.NaN, -46, null, double.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, double.NaN, -46, null, double.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MinAsync_Double_WithNaN_16()
        {
            var input = new double[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_16()
        {
            var input = new double[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_16()
        {
            var input = new double[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_16()
        {
            var input = new double[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_16_NoNull()
        {
            var input = new double?[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_16_NoNull()
        {
            var input = new double?[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_16_NoNull()
        {
            var input = new double?[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_16_NoNull()
        {
            var input = new double?[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Double_WithNaN_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, double.NaN, 98, null, double.NaN, -100, null, double.NaN, -86, null, double.NaN, 19, null, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Double_WithNaN_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, double.NaN, 98, null, double.NaN, -100, null, double.NaN, -86, null, double.NaN, 19, null, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Double_WithNaN_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, double.NaN, 98, null, double.NaN, -100, null, double.NaN, -86, null, double.NaN, 19, null, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, double.NaN, 98, null, double.NaN, -100, null, double.NaN, -86, null, double.NaN, 19, null, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MinAsync_Empty_Decimal()
        {
            await AssertThrowsAsync<InvalidOperationException>(new decimal[0].ToAsyncEnumerable().MinAsync(CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MinAsync_Selector_Empty_Decimal()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAsync(_ => default(decimal), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Empty_Decimal()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAwaitAsync(_ => default(ValueTask<decimal>), CancellationToken.None).AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Empty_Decimal()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => default(ValueTask<decimal>), CancellationToken.None).AsTask());
        }
#endif

        [Fact]
        public async Task MinAsync_Empty_Nullable_Decimal()
        {
            Assert.Null(await new decimal?[0].ToAsyncEnumerable().MinAsync(CancellationToken.None));
        }

        [Fact]
        public async Task MinAsync_Selector_Empty_Nullable_Decimal()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAsync(_ => default(decimal?), CancellationToken.None));
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Empty_Nullable_Decimal()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAwaitAsync(_ => default(ValueTask<decimal?>), CancellationToken.None));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Empty_Nullable_Decimal()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => default(ValueTask<decimal?>), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_1()
        {
            var input = new decimal[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_1()
        {
            var input = new decimal[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_1()
        {
            var input = new decimal[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_1()
        {
            var input = new decimal[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_Nullable_1_NoNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_Nullable_1_NoNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_Nullable_1_NoNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_Nullable_1_NoNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 1);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_Nullable_1_SomeNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_Nullable_1_SomeNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_Nullable_1_SomeNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_Nullable_1_SomeNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_4()
        {
            var input = new decimal[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_4()
        {
            var input = new decimal[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_4()
        {
            var input = new decimal[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_4()
        {
            var input = new decimal[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_Nullable_4_NoNull()
        {
            var input = new decimal?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_Nullable_4_NoNull()
        {
            var input = new decimal?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_Nullable_4_NoNull()
        {
            var input = new decimal?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_Nullable_4_NoNull()
        {
            var input = new decimal?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 4);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_Nullable_4_SomeNull()
        {
            var input = new decimal?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_Nullable_4_SomeNull()
        {
            var input = new decimal?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_Nullable_4_SomeNull()
        {
            var input = new decimal?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_Nullable_4_SomeNull()
        {
            var input = new decimal?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_8()
        {
            var input = new decimal[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_8()
        {
            var input = new decimal[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_8()
        {
            var input = new decimal[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_8()
        {
            var input = new decimal[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_Nullable_8_NoNull()
        {
            var input = new decimal?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_Nullable_8_NoNull()
        {
            var input = new decimal?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_Nullable_8_NoNull()
        {
            var input = new decimal?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_Nullable_8_NoNull()
        {
            var input = new decimal?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 8);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_Nullable_8_SomeNull()
        {
            var input = new decimal?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_Nullable_8_SomeNull()
        {
            var input = new decimal?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_Nullable_8_SomeNull()
        {
            var input = new decimal?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_Nullable_8_SomeNull()
        {
            var input = new decimal?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_16()
        {
            var input = new decimal[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_16()
        {
            var input = new decimal[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_16()
        {
            var input = new decimal[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_16()
        {
            var input = new decimal[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_Nullable_16_NoNull()
        {
            var input = new decimal?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_Nullable_16_NoNull()
        {
            var input = new decimal?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_Nullable_16_NoNull()
        {
            var input = new decimal?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_Nullable_16_NoNull()
        {
            var input = new decimal?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 16);

            var expected = input.Min();
            var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MinAsync_Decimal_Nullable_16_SomeNull()
        {
            var input = new decimal?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAsync_Selector_Decimal_Nullable_16_SomeNull()
        {
            var input = new decimal?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MinAwaitAsync_Selector_Decimal_Nullable_16_SomeNull()
        {
            var input = new decimal?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MinAwaitWithCancellationAsync_Selector_Decimal_Nullable_16_SomeNull()
        {
            var input = new decimal?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Min();
                var actual = await input.ToAsyncEnumerable().MinAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Empty_Int32()
        {
            await AssertThrowsAsync<InvalidOperationException>(new int[0].ToAsyncEnumerable().MaxAsync(CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MaxAsync_Selector_Empty_Int32()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAsync(_ => default(int), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Empty_Int32()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAwaitAsync(_ => default(ValueTask<int>), CancellationToken.None).AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Empty_Int32()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => default(ValueTask<int>), CancellationToken.None).AsTask());
        }
#endif

        [Fact]
        public async Task MaxAsync_Empty_Nullable_Int32()
        {
            Assert.Null(await new int?[0].ToAsyncEnumerable().MaxAsync(CancellationToken.None));
        }

        [Fact]
        public async Task MaxAsync_Selector_Empty_Nullable_Int32()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAsync(_ => default(int?), CancellationToken.None));
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Empty_Nullable_Int32()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAwaitAsync(_ => default(ValueTask<int?>), CancellationToken.None));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Empty_Nullable_Int32()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => default(ValueTask<int?>), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_1()
        {
            var input = new int[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_1()
        {
            var input = new int[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_1()
        {
            var input = new int[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_1()
        {
            var input = new int[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_Nullable_1_NoNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_Nullable_1_NoNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_Nullable_1_NoNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_Nullable_1_NoNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_Nullable_1_SomeNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_Nullable_1_SomeNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_Nullable_1_SomeNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_Nullable_1_SomeNull()
        {
            var input = new int?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_4()
        {
            var input = new int[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_4()
        {
            var input = new int[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_4()
        {
            var input = new int[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_4()
        {
            var input = new int[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_Nullable_4_NoNull()
        {
            var input = new int?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_Nullable_4_NoNull()
        {
            var input = new int?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_Nullable_4_NoNull()
        {
            var input = new int?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_Nullable_4_NoNull()
        {
            var input = new int?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_Nullable_4_SomeNull()
        {
            var input = new int?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_Nullable_4_SomeNull()
        {
            var input = new int?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_Nullable_4_SomeNull()
        {
            var input = new int?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_Nullable_4_SomeNull()
        {
            var input = new int?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_8()
        {
            var input = new int[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_8()
        {
            var input = new int[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_8()
        {
            var input = new int[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_8()
        {
            var input = new int[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_Nullable_8_NoNull()
        {
            var input = new int?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_Nullable_8_NoNull()
        {
            var input = new int?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_Nullable_8_NoNull()
        {
            var input = new int?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_Nullable_8_NoNull()
        {
            var input = new int?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_Nullable_8_SomeNull()
        {
            var input = new int?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_Nullable_8_SomeNull()
        {
            var input = new int?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_Nullable_8_SomeNull()
        {
            var input = new int?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_Nullable_8_SomeNull()
        {
            var input = new int?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_16()
        {
            var input = new int[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_16()
        {
            var input = new int[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_16()
        {
            var input = new int[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_16()
        {
            var input = new int[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_Nullable_16_NoNull()
        {
            var input = new int?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_Nullable_16_NoNull()
        {
            var input = new int?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_Nullable_16_NoNull()
        {
            var input = new int?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_Nullable_16_NoNull()
        {
            var input = new int?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(int?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Int32_Nullable_16_SomeNull()
        {
            var input = new int?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int32_Nullable_16_SomeNull()
        {
            var input = new int?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int32_Nullable_16_SomeNull()
        {
            var input = new int?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int32_Nullable_16_SomeNull()
        {
            var input = new int?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<int?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Empty_Int64()
        {
            await AssertThrowsAsync<InvalidOperationException>(new long[0].ToAsyncEnumerable().MaxAsync(CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MaxAsync_Selector_Empty_Int64()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAsync(_ => default(long), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Empty_Int64()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAwaitAsync(_ => default(ValueTask<long>), CancellationToken.None).AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Empty_Int64()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => default(ValueTask<long>), CancellationToken.None).AsTask());
        }
#endif

        [Fact]
        public async Task MaxAsync_Empty_Nullable_Int64()
        {
            Assert.Null(await new long?[0].ToAsyncEnumerable().MaxAsync(CancellationToken.None));
        }

        [Fact]
        public async Task MaxAsync_Selector_Empty_Nullable_Int64()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAsync(_ => default(long?), CancellationToken.None));
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Empty_Nullable_Int64()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAwaitAsync(_ => default(ValueTask<long?>), CancellationToken.None));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Empty_Nullable_Int64()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => default(ValueTask<long?>), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_1()
        {
            var input = new long[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_1()
        {
            var input = new long[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_1()
        {
            var input = new long[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_1()
        {
            var input = new long[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_Nullable_1_NoNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_Nullable_1_NoNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_Nullable_1_NoNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_Nullable_1_NoNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_Nullable_1_SomeNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_Nullable_1_SomeNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_Nullable_1_SomeNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_Nullable_1_SomeNull()
        {
            var input = new long?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_4()
        {
            var input = new long[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_4()
        {
            var input = new long[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_4()
        {
            var input = new long[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_4()
        {
            var input = new long[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_Nullable_4_NoNull()
        {
            var input = new long?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_Nullable_4_NoNull()
        {
            var input = new long?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_Nullable_4_NoNull()
        {
            var input = new long?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_Nullable_4_NoNull()
        {
            var input = new long?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_Nullable_4_SomeNull()
        {
            var input = new long?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_Nullable_4_SomeNull()
        {
            var input = new long?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_Nullable_4_SomeNull()
        {
            var input = new long?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_Nullable_4_SomeNull()
        {
            var input = new long?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_8()
        {
            var input = new long[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_8()
        {
            var input = new long[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_8()
        {
            var input = new long[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_8()
        {
            var input = new long[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_Nullable_8_NoNull()
        {
            var input = new long?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_Nullable_8_NoNull()
        {
            var input = new long?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_Nullable_8_NoNull()
        {
            var input = new long?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_Nullable_8_NoNull()
        {
            var input = new long?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_Nullable_8_SomeNull()
        {
            var input = new long?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_Nullable_8_SomeNull()
        {
            var input = new long?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_Nullable_8_SomeNull()
        {
            var input = new long?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_Nullable_8_SomeNull()
        {
            var input = new long?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_16()
        {
            var input = new long[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_16()
        {
            var input = new long[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_16()
        {
            var input = new long[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_16()
        {
            var input = new long[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_Nullable_16_NoNull()
        {
            var input = new long?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_Nullable_16_NoNull()
        {
            var input = new long?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_Nullable_16_NoNull()
        {
            var input = new long?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_Nullable_16_NoNull()
        {
            var input = new long?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(long?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Int64_Nullable_16_SomeNull()
        {
            var input = new long?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Int64_Nullable_16_SomeNull()
        {
            var input = new long?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Int64_Nullable_16_SomeNull()
        {
            var input = new long?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Int64_Nullable_16_SomeNull()
        {
            var input = new long?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<long?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Empty_Single()
        {
            await AssertThrowsAsync<InvalidOperationException>(new float[0].ToAsyncEnumerable().MaxAsync(CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MaxAsync_Selector_Empty_Single()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAsync(_ => default(float), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Empty_Single()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAwaitAsync(_ => default(ValueTask<float>), CancellationToken.None).AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Empty_Single()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => default(ValueTask<float>), CancellationToken.None).AsTask());
        }
#endif

        [Fact]
        public async Task MaxAsync_Empty_Nullable_Single()
        {
            Assert.Null(await new float?[0].ToAsyncEnumerable().MaxAsync(CancellationToken.None));
        }

        [Fact]
        public async Task MaxAsync_Selector_Empty_Nullable_Single()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAsync(_ => default(float?), CancellationToken.None));
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Empty_Nullable_Single()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAwaitAsync(_ => default(ValueTask<float?>), CancellationToken.None));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Empty_Nullable_Single()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => default(ValueTask<float?>), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_1()
        {
            var input = new float[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_1()
        {
            var input = new float[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_1()
        {
            var input = new float[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_1()
        {
            var input = new float[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_Nullable_1_NoNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_Nullable_1_NoNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_Nullable_1_NoNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_Nullable_1_NoNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_4()
        {
            var input = new float[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_4()
        {
            var input = new float[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_4()
        {
            var input = new float[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_4()
        {
            var input = new float[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_Nullable_4_NoNull()
        {
            var input = new float?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_Nullable_4_NoNull()
        {
            var input = new float?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_Nullable_4_NoNull()
        {
            var input = new float?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_Nullable_4_NoNull()
        {
            var input = new float?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_8()
        {
            var input = new float[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_8()
        {
            var input = new float[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_8()
        {
            var input = new float[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_8()
        {
            var input = new float[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_Nullable_8_NoNull()
        {
            var input = new float?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_Nullable_8_NoNull()
        {
            var input = new float?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_Nullable_8_NoNull()
        {
            var input = new float?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_Nullable_8_NoNull()
        {
            var input = new float?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_16()
        {
            var input = new float[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_16()
        {
            var input = new float[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_16()
        {
            var input = new float[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_16()
        {
            var input = new float[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_Nullable_16_NoNull()
        {
            var input = new float?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_Nullable_16_NoNull()
        {
            var input = new float?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_Nullable_16_NoNull()
        {
            var input = new float?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_Nullable_16_NoNull()
        {
            var input = new float?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_1()
        {
            var input = new float[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_1()
        {
            var input = new float[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_1()
        {
            var input = new float[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_1()
        {
            var input = new float[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_1_NoNull()
        {
            var input = new float?[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_1_NoNull()
        {
            var input = new float?[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_1_NoNull()
        {
            var input = new float?[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_1_NoNull()
        {
            var input = new float?[] { float.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_1_SomeNull()
        {
            var input = new float?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MaxAsync_Single_WithNaN_4()
        {
            var input = new float[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_4()
        {
            var input = new float[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_4()
        {
            var input = new float[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_4()
        {
            var input = new float[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_4_NoNull()
        {
            var input = new float?[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_4_NoNull()
        {
            var input = new float?[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_4_NoNull()
        {
            var input = new float?[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_4_NoNull()
        {
            var input = new float?[] { float.NaN, -78, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_4_SomeNull()
        {
            var input = new float?[] { -2, null, float.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MaxAsync_Single_WithNaN_8()
        {
            var input = new float[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_8()
        {
            var input = new float[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_8()
        {
            var input = new float[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_8()
        {
            var input = new float[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_8_NoNull()
        {
            var input = new float?[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_8_NoNull()
        {
            var input = new float?[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_8_NoNull()
        {
            var input = new float?[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_8_NoNull()
        {
            var input = new float?[] { float.NaN, -19, float.NaN, -46, float.NaN, -63, float.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, float.NaN, -46, null, float.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, float.NaN, -46, null, float.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, float.NaN, -46, null, float.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_8_SomeNull()
        {
            var input = new float?[] { -48, null, float.NaN, -46, null, float.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MaxAsync_Single_WithNaN_16()
        {
            var input = new float[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_16()
        {
            var input = new float[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_16()
        {
            var input = new float[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_16()
        {
            var input = new float[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(float.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_16_NoNull()
        {
            var input = new float?[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_16_NoNull()
        {
            var input = new float?[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_16_NoNull()
        {
            var input = new float?[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_16_NoNull()
        {
            var input = new float?[] { float.NaN, 98, float.NaN, 98, float.NaN, 63, float.NaN, 14, float.NaN, -86, float.NaN, -79, float.NaN, -84, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(float?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<float?>(float.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Single_WithNaN_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, float.NaN, 98, null, float.NaN, -100, null, float.NaN, -86, null, float.NaN, 19, null, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Single_WithNaN_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, float.NaN, 98, null, float.NaN, -100, null, float.NaN, -86, null, float.NaN, 19, null, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Single_WithNaN_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, float.NaN, 98, null, float.NaN, -100, null, float.NaN, -86, null, float.NaN, 19, null, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Single_WithNaN_Nullable_16_SomeNull()
        {
            var input = new float?[] { 59, null, float.NaN, 98, null, float.NaN, -100, null, float.NaN, -86, null, float.NaN, 19, null, float.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<float?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MaxAsync_Empty_Double()
        {
            await AssertThrowsAsync<InvalidOperationException>(new double[0].ToAsyncEnumerable().MaxAsync(CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MaxAsync_Selector_Empty_Double()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAsync(_ => default(double), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Empty_Double()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAwaitAsync(_ => default(ValueTask<double>), CancellationToken.None).AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Empty_Double()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => default(ValueTask<double>), CancellationToken.None).AsTask());
        }
#endif

        [Fact]
        public async Task MaxAsync_Empty_Nullable_Double()
        {
            Assert.Null(await new double?[0].ToAsyncEnumerable().MaxAsync(CancellationToken.None));
        }

        [Fact]
        public async Task MaxAsync_Selector_Empty_Nullable_Double()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAsync(_ => default(double?), CancellationToken.None));
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Empty_Nullable_Double()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAwaitAsync(_ => default(ValueTask<double?>), CancellationToken.None));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Empty_Nullable_Double()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => default(ValueTask<double?>), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_1()
        {
            var input = new double[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_1()
        {
            var input = new double[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_1()
        {
            var input = new double[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_1()
        {
            var input = new double[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_Nullable_1_NoNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_Nullable_1_NoNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_Nullable_1_NoNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_Nullable_1_NoNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_4()
        {
            var input = new double[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_4()
        {
            var input = new double[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_4()
        {
            var input = new double[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_4()
        {
            var input = new double[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_Nullable_4_NoNull()
        {
            var input = new double?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_Nullable_4_NoNull()
        {
            var input = new double?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_Nullable_4_NoNull()
        {
            var input = new double?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_Nullable_4_NoNull()
        {
            var input = new double?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_8()
        {
            var input = new double[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_8()
        {
            var input = new double[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_8()
        {
            var input = new double[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_8()
        {
            var input = new double[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_Nullable_8_NoNull()
        {
            var input = new double?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_Nullable_8_NoNull()
        {
            var input = new double?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_Nullable_8_NoNull()
        {
            var input = new double?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_Nullable_8_NoNull()
        {
            var input = new double?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_16()
        {
            var input = new double[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_16()
        {
            var input = new double[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_16()
        {
            var input = new double[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_16()
        {
            var input = new double[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_Nullable_16_NoNull()
        {
            var input = new double?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_Nullable_16_NoNull()
        {
            var input = new double?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_Nullable_16_NoNull()
        {
            var input = new double?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_Nullable_16_NoNull()
        {
            var input = new double?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_1()
        {
            var input = new double[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_1()
        {
            var input = new double[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_1()
        {
            var input = new double[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_1()
        {
            var input = new double[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_1_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_1_NoNull()
        {
            var input = new double?[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_1_NoNull()
        {
            var input = new double?[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_1_NoNull()
        {
            var input = new double?[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_1_NoNull()
        {
            var input = new double?[] { double.NaN }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_1_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_1_SomeNull()
        {
            var input = new double?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MaxAsync_Double_WithNaN_4()
        {
            var input = new double[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_4()
        {
            var input = new double[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_4()
        {
            var input = new double[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_4()
        {
            var input = new double[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_4_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_4_NoNull()
        {
            var input = new double?[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_4_NoNull()
        {
            var input = new double?[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_4_NoNull()
        {
            var input = new double?[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_4_NoNull()
        {
            var input = new double?[] { double.NaN, -78, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_4_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_4_SomeNull()
        {
            var input = new double?[] { -2, null, double.NaN, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MaxAsync_Double_WithNaN_8()
        {
            var input = new double[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_8()
        {
            var input = new double[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_8()
        {
            var input = new double[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_8()
        {
            var input = new double[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_8_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_8_NoNull()
        {
            var input = new double?[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_8_NoNull()
        {
            var input = new double?[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_8_NoNull()
        {
            var input = new double?[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_8_NoNull()
        {
            var input = new double?[] { double.NaN, -19, double.NaN, -46, double.NaN, -63, double.NaN, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_8_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, double.NaN, -46, null, double.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, double.NaN, -46, null, double.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, double.NaN, -46, null, double.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_8_SomeNull()
        {
            var input = new double?[] { -48, null, double.NaN, -46, null, double.NaN, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MaxAsync_Double_WithNaN_16()
        {
            var input = new double[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_16()
        {
            var input = new double[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_16()
        {
            var input = new double[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_16()
        {
            var input = new double[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_16_AllNaN()
        {
            var input = Enumerable.Repeat(double.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_16_NoNull()
        {
            var input = new double?[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_16_NoNull()
        {
            var input = new double?[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_16_NoNull()
        {
            var input = new double?[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_16_NoNull()
        {
            var input = new double?[] { double.NaN, 98, double.NaN, 98, double.NaN, 63, double.NaN, 14, double.NaN, -86, double.NaN, -79, double.NaN, -84, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(double?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_16_AllNaN()
        {
            var input = Enumerable.Repeat<double?>(double.NaN, 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Double_WithNaN_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, double.NaN, 98, null, double.NaN, -100, null, double.NaN, -86, null, double.NaN, 19, null, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Double_WithNaN_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, double.NaN, 98, null, double.NaN, -100, null, double.NaN, -86, null, double.NaN, 19, null, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Double_WithNaN_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, double.NaN, 98, null, double.NaN, -100, null, double.NaN, -86, null, double.NaN, 19, null, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Double_WithNaN_Nullable_16_SomeNull()
        {
            var input = new double?[] { 59, null, double.NaN, 98, null, double.NaN, -100, null, double.NaN, -86, null, double.NaN, 19, null, double.NaN, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<double?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif
        [Fact]
        public async Task MaxAsync_Empty_Decimal()
        {
            await AssertThrowsAsync<InvalidOperationException>(new decimal[0].ToAsyncEnumerable().MaxAsync(CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MaxAsync_Selector_Empty_Decimal()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAsync(_ => default(decimal), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Empty_Decimal()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAwaitAsync(_ => default(ValueTask<decimal>), CancellationToken.None).AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Empty_Decimal()
        {
            await AssertThrowsAsync<InvalidOperationException>(new object[0].ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => default(ValueTask<decimal>), CancellationToken.None).AsTask());
        }
#endif

        [Fact]
        public async Task MaxAsync_Empty_Nullable_Decimal()
        {
            Assert.Null(await new decimal?[0].ToAsyncEnumerable().MaxAsync(CancellationToken.None));
        }

        [Fact]
        public async Task MaxAsync_Selector_Empty_Nullable_Decimal()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAsync(_ => default(decimal?), CancellationToken.None));
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Empty_Nullable_Decimal()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAwaitAsync(_ => default(ValueTask<decimal?>), CancellationToken.None));
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Empty_Nullable_Decimal()
        {
            Assert.Null(await new object[0].ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => default(ValueTask<decimal?>), CancellationToken.None));
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_1()
        {
            var input = new decimal[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_1()
        {
            var input = new decimal[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_1()
        {
            var input = new decimal[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_1()
        {
            var input = new decimal[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_Nullable_1_NoNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_Nullable_1_NoNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_Nullable_1_NoNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_Nullable_1_NoNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_Nullable_1_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 1);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_Nullable_1_SomeNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_Nullable_1_SomeNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_Nullable_1_SomeNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_Nullable_1_SomeNull()
        {
            var input = new decimal?[] { 33 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_4()
        {
            var input = new decimal[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_4()
        {
            var input = new decimal[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_4()
        {
            var input = new decimal[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_4()
        {
            var input = new decimal[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_Nullable_4_NoNull()
        {
            var input = new decimal?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_Nullable_4_NoNull()
        {
            var input = new decimal?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_Nullable_4_NoNull()
        {
            var input = new decimal?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_Nullable_4_NoNull()
        {
            var input = new decimal?[] { -2, -78, -61, -17 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_Nullable_4_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 4);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_Nullable_4_SomeNull()
        {
            var input = new decimal?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_Nullable_4_SomeNull()
        {
            var input = new decimal?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_Nullable_4_SomeNull()
        {
            var input = new decimal?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_Nullable_4_SomeNull()
        {
            var input = new decimal?[] { -2, null, -61, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_8()
        {
            var input = new decimal[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_8()
        {
            var input = new decimal[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_8()
        {
            var input = new decimal[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_8()
        {
            var input = new decimal[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_Nullable_8_NoNull()
        {
            var input = new decimal?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_Nullable_8_NoNull()
        {
            var input = new decimal?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_Nullable_8_NoNull()
        {
            var input = new decimal?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_Nullable_8_NoNull()
        {
            var input = new decimal?[] { -48, -19, 25, -46, 81, -63, -10, -99 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_Nullable_8_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 8);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_Nullable_8_SomeNull()
        {
            var input = new decimal?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_Nullable_8_SomeNull()
        {
            var input = new decimal?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_Nullable_8_SomeNull()
        {
            var input = new decimal?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_Nullable_8_SomeNull()
        {
            var input = new decimal?[] { -48, null, 25, null, 81, null, -10, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_16()
        {
            var input = new decimal[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_16()
        {
            var input = new decimal[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_16()
        {
            var input = new decimal[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_16()
        {
            var input = new decimal[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_Nullable_16_NoNull()
        {
            var input = new decimal?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_Nullable_16_NoNull()
        {
            var input = new decimal?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_Nullable_16_NoNull()
        {
            var input = new decimal?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_Nullable_16_NoNull()
        {
            var input = new decimal?[] { 59, 98, -3, 98, 20, 63, -100, 14, -77, -86, -26, -79, 19, -84, -28, 41 }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_Nullable_16_AllNull()
        {
            var input = Enumerable.Repeat(default(decimal?), 16);

            var expected = input.Max();
            var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
            Assert.Equal(expected, actual);
        }
#endif

        [Fact]
        public async Task MaxAsync_Decimal_Nullable_16_SomeNull()
        {
            var input = new decimal?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAsync_Selector_Decimal_Nullable_16_SomeNull()
        {
            var input = new decimal?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAsync(x => x, CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

        [Fact]
        public async Task MaxAwaitAsync_Selector_Decimal_Nullable_16_SomeNull()
        {
            var input = new decimal?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitAsync(x => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task MaxAwaitWithCancellationAsync_Selector_Decimal_Nullable_16_SomeNull()
        {
            var input = new decimal?[] { 59, null, -3, null, 20, null, -100, null, -77, null, -26, null, 19, null, -28, null }.AsEnumerable();

            for (var i = 0; i < 4; i++)
            {
                var expected = input.Max();
                var actual = await input.ToAsyncEnumerable().MaxAwaitWithCancellationAsync((x, ct) => new ValueTask<decimal?>(x), CancellationToken.None);
                Assert.Equal(expected, actual);

                input = Shuffle(input);
            }
        }
#endif


        private static IEnumerable<T> Shuffle<T>(IEnumerable<T> source)
        {
            var rand = new Random(42);
            return source.OrderBy(x => rand.Next());
        }
    }
}

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
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal?>), x => x));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal?>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal?>), x => x, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>), CancellationToken.None));
        }

        [Fact]
        public void Sum1()
        {
            var xs = new[] { 1, 2, 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum2()
        {
            var xs = new[] { 1, default(int?), 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum3()
        {
            var xs = new[] { 1L, 2L, 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum4()
        {
            var xs = new[] { 1L, default(long?), 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum5()
        {
            var xs = new[] { 1.0, 2.0, 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum6()
        {
            var xs = new[] { 1.0, default(double?), 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum7()
        {
            var xs = new[] { 1.0f, 2.0f, 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum8()
        {
            var xs = new[] { 1.0f, default(float?), 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum9()
        {
            var xs = new[] { 1.0m, 2.0m, 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }

        [Fact]
        public void Sum10()
        {
            var xs = new[] { 1.0m, default(decimal?), 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Sum(), ys.Sum().Result);
            Assert.Equal(xs.Sum(), ys.Sum(x => x).Result);
        }
    }
}

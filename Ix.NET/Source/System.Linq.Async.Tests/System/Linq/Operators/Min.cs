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
        public async Task Min_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal?>), x => x));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal?>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal?>), x => x, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>), CancellationToken.None));
        }

        [Fact]
        public void Min1()
        {
            var xs = new[] { 2, 1, 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min2()
        {
            var xs = new[] { 2, default(int?), 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min3()
        {
            var xs = new[] { 2L, 1L, 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min4()
        {
            var xs = new[] { 2L, default(long?), 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min5()
        {
            var xs = new[] { 2.0, 1.0, 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min6()
        {
            var xs = new[] { 2.0, default(double?), 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min7()
        {
            var xs = new[] { 2.0f, 1.0f, 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min8()
        {
            var xs = new[] { 2.0f, default(float?), 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min9()
        {
            var xs = new[] { 2.0m, 1.0m, 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min10()
        {
            var xs = new[] { 2.0m, default(decimal?), 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }

        [Fact]
        public void Min11()
        {
            var xs = new[] { DateTime.Now.AddDays(1), DateTime.Now.Subtract(TimeSpan.FromDays(1)), DateTime.Now.AddDays(2), DateTime.Now };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Min(), ys.Min().Result);
            Assert.Equal(xs.Min(), ys.Min(x => x).Result);
        }
    }
}

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
    public class Max : AsyncEnumerableTests
    {
        [Fact]
        public async Task Max_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float?>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal?>), x => x));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, int?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, long>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, long?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, double>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, double?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, float>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, float?>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>), x => x));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal?>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float?>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal?>), x => x, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, int?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, long>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, long?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, double>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, double?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, float>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, float?>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>), CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>), x => x, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>), CancellationToken.None));
        }

        [Fact]
        public void Max1()
        {
            var xs = new[] { 2, 7, 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max2()
        {
            var xs = new[] { 2, default(int?), 3, 1 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max3()
        {
            var xs = new[] { 2L, 7L, 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max4()
        {
            var xs = new[] { 2L, default(long?), 3L, 1L };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max5()
        {
            var xs = new[] { 2.0, 7.0, 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max6()
        {
            var xs = new[] { 2.0, default(double?), 3.0, 1.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max7()
        {
            var xs = new[] { 2.0f, 7.0f, 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max8()
        {
            var xs = new[] { 2.0f, default(float?), 3.0f, 1.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max9()
        {
            var xs = new[] { 2.0m, 7.0m, 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max10()
        {
            var xs = new[] { 2.0m, default(decimal?), 3.0m, 1.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }

        [Fact]
        public void Max11()
        {
            var xs = new[] { DateTime.Now.AddDays(1), DateTime.Now.Subtract(TimeSpan.FromDays(1)), DateTime.Now.AddDays(2), DateTime.Now };
            var ys = xs.ToAsyncEnumerable();
            Assert.Equal(xs.Max(), ys.Max().Result);
            Assert.Equal(xs.Max(), ys.Max(x => x).Result);
        }
    }
}

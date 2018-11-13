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
    public class Catch : AsyncEnumerableExTests
    {
        [Fact]
        public void Catch_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Catch<int, Exception>(default, x => default(IAsyncEnumerable<int>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Catch<int, Exception>(Return42, default(Func<Exception, IAsyncEnumerable<int>>)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Catch<int>(default, Return42));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Catch<int>(Return42, default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Catch<int>(default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Catch<int>(default(IEnumerable<IAsyncEnumerable<int>>)));
        }

        [Fact]
        public async Task Catch1Async()
        {
            var err = false;
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = xs.Catch<int, Exception>(ex_ => { err = true; return ys; });

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await NoNextAsync(e);

            Assert.False(err);
        }

        [Fact]
        public async Task Catch2Async()
        {
            var ex = new InvalidOperationException("Bang!");

            var err = false;
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = xs.Catch<int, InvalidOperationException>(ex_ => { err = true; return ys; });

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);

            Assert.False(err);

            await HasNextAsync(e, 4);

            Assert.True(err);

            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Catch3Async()
        {
            var ex = new InvalidOperationException("Bang!");

            var err = false;
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = xs.Catch<int, Exception>(ex_ => { err = true; return ys; });

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);

            Assert.False(err);

            await HasNextAsync(e, 4);

            Assert.True(err);

            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Catch4Async()
        {
            var ex = new DivideByZeroException();

            var err = false;
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = xs.Catch<int, InvalidOperationException>(ex_ => { err = true; return ys; });

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);

            await AssertThrowsAsync(e.MoveNextAsync(), ex);

            Assert.False(err);
        }

        [Fact]
        public async Task Catch5Async()
        {
            var ex = new InvalidOperationException("Bang!");
            var ex2 = new Exception("Oops!");

            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = xs.Catch<int, InvalidOperationException>(ex_ => { if (ex_.Message == "Bang!") throw ex2; return ys; });

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);

            await AssertThrowsAsync(e.MoveNextAsync(), ex2);
        }

        [Fact]
        public async Task Catch6Async()
        {
            var ex = new InvalidOperationException("Bang!");

            var err = false;
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));

            var res = xs.Catch<int, InvalidOperationException>(ex_ => { err = true; return xs; });

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);

            Assert.False(err);

            await HasNextAsync(e, 1);

            Assert.True(err);

            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);

            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Catch7Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.Catch(xs, ys);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Catch8Async()
        {
            var ex = new InvalidOperationException("Bang!");

            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.Catch(xs, ys);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Catch9Async()
        {
            var ex = new InvalidOperationException("Bang!");

            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.Catch(new[] { xs, xs, ys, ys });

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Catch10Async()
        {
            var ex = new Exception("Bang!");
            var res = CatchXss(ex).Catch();

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);

            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        private IEnumerable<IAsyncEnumerable<int>> CatchXss(Exception ex)
        {
            yield return new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(new Exception("!!!")));
            throw ex;
        }

        [Fact]
        public async Task Catch11Async()
        {
            var ex = new InvalidOperationException("Bang!");

            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));

            var res = AsyncEnumerableEx.Catch(new[] { xs, xs });

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);

            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Catch12Async()
        {
            var res = AsyncEnumerableEx.Catch(Enumerable.Empty<IAsyncEnumerable<int>>());

            var e = res.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Catch13()
        {
            var ex = new InvalidOperationException("Bang!");

            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.Catch(new[] { xs, xs, ys, ys });

            await SequenceIdentity(res);
        }

        [Fact]
        public async Task Catch14()
        {
            var err = false;
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();

            var res = xs.Catch<int, Exception>(ex_ => { err = true; return ys; });

            await SequenceIdentity(res);

            Assert.False(err);
        }
    }
}

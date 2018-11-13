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
    public class Using : AsyncEnumerableExTests
    {
        [Fact]
        public void Using_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Using<int, IDisposable>(default, _ => default(IAsyncEnumerable<int>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Using<int, IDisposable>(() => new MyD(null), default));
        }

        [Fact]
        public void Using1()
        {
            var i = 0;
            var d = 0;

            var xs = AsyncEnumerableEx.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { d++; });
                },
                _ => Return42
            );

            Assert.Equal(0, i);
            Assert.Equal(0, d);

            var e = xs.GetAsyncEnumerator();

            Assert.Equal(0, i);
            Assert.Equal(0, d);
        }

        [Fact]
        public async Task Using2()
        {
            var i = 0;
            var d = 0;

            var xs = AsyncEnumerableEx.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { d++; });
                },
                _ => Return42
            );

            Assert.Equal(0, i);
            Assert.Equal(0, d);

            var e = xs.GetAsyncEnumerator();
            Assert.Equal(0, i);
            Assert.Equal(0, d);

            await e.DisposeAsync();

            Assert.Equal(0, i);
            Assert.Equal(0, d);
        }

        [Fact]
        public async Task Using3()
        {
            var ex = new Exception("Bang!");
            var i = 0;
            var d = 0;

            var xs = AsyncEnumerableEx.Using<int, MyD>(
                () =>
                {
                    i++;
                    return new MyD(() => { d++; });
                },
                _ => { throw ex; }
            );

            Assert.Equal(0, i);
            Assert.Equal(0, d);

            var e = xs.GetAsyncEnumerator();

            Assert.Equal(0, i);
            Assert.Equal(0, d);

            await e.DisposeAsync();

            Assert.Equal(0, i);
            Assert.Equal(0, d);
        }

        [Fact]
        public async Task Using4Async()
        {
            var i = 0;
            var disposed = new TaskCompletionSource<bool>();

            var xs = AsyncEnumerableEx.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { disposed.TrySetResult(true); });
                },
                _ => Return42
            );

            Assert.Equal(0, i);

            var e = xs.GetAsyncEnumerator();

            Assert.Equal(0, i);

            await HasNextAsync(e, 42);

            Assert.Equal(1, i);

            await NoNextAsync(e);

            Assert.True(await disposed.Task);
        }

        [Fact]
        public async Task Using5Async()
        {
            var ex = new Exception("Bang!");
            var i = 0;
            var disposed = new TaskCompletionSource<bool>();

            var xs = AsyncEnumerableEx.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { disposed.TrySetResult(true); });
                },
                _ => Throw<int>(ex)
            );

            Assert.Equal(0, i);

            var e = xs.GetAsyncEnumerator();

            Assert.Equal(0, i);

            await AssertThrowsAsync(e.MoveNextAsync(), ex);

            Assert.Equal(1, i);

            Assert.True(await disposed.Task);
        }

        [Fact]
        public async Task Using7()
        {
            var i = 0;
            var d = 0;

            var xs = AsyncEnumerableEx.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { d++; });
                },
                _ => Return42
            );

            await SequenceIdentity(xs);
        }

        private sealed class MyD : IDisposable
        {
            private readonly Action _dispose;

            public MyD(Action dispose)
            {
                _dispose = dispose;
            }

            public void Dispose()
            {
                _dispose();
            }
        }
    }
}

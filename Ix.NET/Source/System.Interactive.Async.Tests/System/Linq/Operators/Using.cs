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
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Using<int, IDisposable>(default, _ => default(IAsyncEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Using<int, IDisposable>(() => new MyD(null), default));
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

            var e = xs.GetAsyncEnumerator();
            Assert.Equal(1, i);
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

            var e = xs.GetAsyncEnumerator();
            Assert.Equal(1, i);

            await e.DisposeAsync();
            Assert.Equal(1, d);
        }

        [Fact]
        public void Using3()
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

            AssertThrows<Exception>(() => xs.GetAsyncEnumerator(), ex_ => ex_ == ex);

            Assert.Equal(1, d);
        }

        [Fact]
        public void Using4()
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
            Assert.Equal(1, i);

            HasNext(e, 42);
            NoNext(e);

            Assert.True(disposed.Task.Result);
        }

        [Fact]
        public void Using5()
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
            Assert.Equal(1, i);

            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));

            Assert.True(disposed.Task.Result);
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

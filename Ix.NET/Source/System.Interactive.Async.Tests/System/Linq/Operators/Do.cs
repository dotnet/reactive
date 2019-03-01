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
    public class Do : AsyncEnumerableExTests
    {
        [Fact]
        public void Do_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(default, x => { }));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(Return42, default(Action<int>)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(default, x => { }, () => { }));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(Return42, default, () => { }));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(Return42, x => { }, default(Action)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(default, x => { }, ex => { }));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(Return42, default, ex => { }));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(Return42, x => { }, default(Action<Exception>)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(default, x => { }, ex => { }, () => { }));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(Return42, default, ex => { }, () => { }));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(Return42, x => { }, default, () => { }));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(Return42, x => { }, ex => { }, default));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(default, new MyObs()));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(Return42, default(IObserver<int>)));
        }

        [Fact]
        public async Task Do1Async()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Do(x => sum += x);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            Assert.Equal(1, sum);
            await HasNextAsync(e, 2);
            Assert.Equal(3, sum);
            await HasNextAsync(e, 3);
            Assert.Equal(6, sum);
            await HasNextAsync(e, 4);
            Assert.Equal(10, sum);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Do2()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Do(x => { throw ex; });

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Do3Async()
        {
            var sum = 0;
            var fail = false;
            var done = false;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Do(x => sum += x, ex => { fail = true; }, () => { done = true; });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            Assert.Equal(1, sum);
            await HasNextAsync(e, 2);
            Assert.Equal(3, sum);
            await HasNextAsync(e, 3);
            Assert.Equal(6, sum);
            await HasNextAsync(e, 4);
            Assert.Equal(10, sum);
            await NoNextAsync(e);

            Assert.False(fail);
            Assert.True(done);
        }

        [Fact]
        public async Task Do4Async()
        {
            var sum = 0;
            var done = false;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Do(x => sum += x, () => { done = true; });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            Assert.Equal(1, sum);
            await HasNextAsync(e, 2);
            Assert.Equal(3, sum);
            await HasNextAsync(e, 3);
            Assert.Equal(6, sum);
            await HasNextAsync(e, 4);
            Assert.Equal(10, sum);
            await NoNextAsync(e);

            Assert.True(done);
        }

        [Fact]
        public async Task Do5()
        {
            var ex = new Exception("Bang");
            var exa = default(Exception);
            var done = false;
            var hasv = false;
            var xs = Throw<int>(ex);
            var ys = xs.Do(x => { hasv = true; }, exx => { exa = exx; }, () => { done = true; });

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);

            Assert.False(hasv);
            Assert.False(done);
            Assert.Same(exa, ex);
        }

        [Fact]
        public async Task Do6()
        {
            var ex = new Exception("Bang");
            var exa = default(Exception);
            var hasv = false;
            var xs = Throw<int>(ex);
            var ys = xs.Do(x => { hasv = true; }, exx => { exa = exx; });

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);

            Assert.False(hasv);
            Assert.Same(exa, ex);
        }

        [Fact]
        public async Task Do7()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Do(x => sum += x);

            await SequenceIdentity(ys);

            Assert.Equal(20, sum);
        }

        private sealed class MyObs : IObserver<int>
        {
            public void OnCompleted()
            {
                throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(int value)
            {
                throw new NotImplementedException();
            }
        }
    }
}

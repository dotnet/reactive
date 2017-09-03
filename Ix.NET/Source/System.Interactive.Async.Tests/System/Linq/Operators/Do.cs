// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
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
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(null, x => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(AsyncEnumerable.Return(42), default(Action<int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(null, x => { }, () => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(AsyncEnumerable.Return(42), default(Action<int>), () => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(AsyncEnumerable.Return(42), x => { }, default(Action)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(null, x => { }, ex => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(AsyncEnumerable.Return(42), default(Action<int>), ex => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(AsyncEnumerable.Return(42), x => { }, default(Action<Exception>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(null, x => { }, ex => { }, () => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(AsyncEnumerable.Return(42), default(Action<int>), ex => { }, () => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(AsyncEnumerable.Return(42), x => { }, default(Action<Exception>), () => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(AsyncEnumerable.Return(42), x => { }, ex => { }, default(Action)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(null, new MyObs()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Do<int>(AsyncEnumerable.Return(42), default(IObserver<int>)));
        }

        [Fact]
        public void Do1()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Do(x => sum += x);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            Assert.Equal(1, sum);
            HasNext(e, 2);
            Assert.Equal(3, sum);
            HasNext(e, 3);
            Assert.Equal(6, sum);
            HasNext(e, 4);
            Assert.Equal(10, sum);
            NoNext(e);
        }

        [Fact]
        public void Do2()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Do(x => { throw ex; });

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Do3()
        {
            var sum = 0;
            var fail = false;
            var done = false;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Do(x => sum += x, ex => { fail = true; }, () => { done = true; });

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            Assert.Equal(1, sum);
            HasNext(e, 2);
            Assert.Equal(3, sum);
            HasNext(e, 3);
            Assert.Equal(6, sum);
            HasNext(e, 4);
            Assert.Equal(10, sum);
            NoNext(e);

            Assert.False(fail);
            Assert.True(done);
        }

        [Fact]
        public void Do4()
        {
            var sum = 0;
            var done = false;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Do(x => sum += x, () => { done = true; });

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 1);
            Assert.Equal(1, sum);
            HasNext(e, 2);
            Assert.Equal(3, sum);
            HasNext(e, 3);
            Assert.Equal(6, sum);
            HasNext(e, 4);
            Assert.Equal(10, sum);
            NoNext(e);

            Assert.True(done);
        }

        [Fact]
        public void Do5()
        {
            var ex = new Exception("Bang");
            var exa = default(Exception);
            var done = false;
            var hasv = false;
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Do(x => { hasv = true; }, exx => { exa = exx; }, () => { done = true; });

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ex_.InnerException == ex);

            Assert.False(hasv);
            Assert.False(done);
            Assert.Same(exa, ex);
        }

        [Fact]
        public void Do6()
        {
            var ex = new Exception("Bang");
            var exa = default(Exception);
            var hasv = false;
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Do(x => { hasv = true; }, exx => { exa = exx; });

            var e = ys.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ex_.InnerException == ex);

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

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
    public partial class AsyncTests
    {
        [Fact]
        public void MoveNextExtension_Null()
        {
            var en = default(IAsyncEnumerator<int>);

            Assert.ThrowsAsync<ArgumentNullException>(() => en.MoveNextAsync());
        }

        [Fact]
        public void SelectWhere2()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(i => i + 2).Where(i => i % 2 == 0);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 2);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void WhereSelect2()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Where(i => i % 2 == 0).Select(i => i + 2);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 2);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void WhereSelect3()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Where(i => i % 2 == 0).Select(i => i + 2).Select(i => i + 2);

            var e = ys.GetAsyncEnumerator();
            HasNext(e, 4);
            HasNext(e, 6);
            NoNext(e);
        }

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

        class MyObs : IObserver<int>
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

        [Fact]
        public async Task ForEachAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(null, x => { }));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(AsyncEnumerable.Return(42), default(Action<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(null, (x, i) => { }));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(AsyncEnumerable.Return(42), default(Action<int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(null, x => { }, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(AsyncEnumerable.Return(42), default(Action<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(null, (x, i) => { }, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(AsyncEnumerable.Return(42), default(Action<int, int>), CancellationToken.None));
        }

        [Fact]
        public void ForEachAsync1()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            xs.ForEachAsync(x => sum += x).Wait(WaitTimeoutMs);
            Assert.Equal(10, sum);
        }

        [Fact]
        public void ForEachAsync2()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            xs.ForEachAsync((x, i) => sum += x * i).Wait(WaitTimeoutMs);
            Assert.Equal(1 * 0 + 2 * 1 + 3 * 2 + 4 * 3, sum);
        }

        [Fact]
        public void ForEachAsync3()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            AssertThrows<Exception>(() => xs.ForEachAsync(x => { throw ex; }).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ForEachAsync4()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            AssertThrows<Exception>(() => xs.ForEachAsync((x, i) => { throw ex; }).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ForEachAsync5()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);

            AssertThrows<Exception>(() => xs.ForEachAsync(x => { throw ex; }).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ForEachAsync6()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);

            AssertThrows<Exception>(() => xs.ForEachAsync((x, i) => { throw ex; }).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void AsAsyncEnumerable_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.AsAsyncEnumerable<int>((IAsyncEnumerable<int>)null));
        }

        [Fact]
        public void AsAsyncEnumerable1()
        {
            var xs = AsyncEnumerable.Return(42);
            var ys = xs.AsAsyncEnumerable();

            Assert.NotSame(xs, ys);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 42);
            NoNext(e);
        }

        [Fact]
        public void RepeatSeq_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Repeat(default(IAsyncEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.Repeat(default(IAsyncEnumerable<int>), 3));
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerableEx.Repeat(AsyncEnumerable.Return(42), -1));
        }

        [Fact]
        public void RepeatSeq1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Repeat();

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
        }

        [Fact]
        public void RepeatSeq2()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Repeat(3);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            NoNext(e);
        }

        [Fact]
        public void RepeatSeq3()
        {
            var i = 0;
            var xs = RepeatXs(() => i++).ToAsyncEnumerable().Repeat(3);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);

            Assert.Equal(3, i);
        }

        [Fact]
        public void RepeatSeq0()
        {
            var i = 0;
            var xs = RepeatXs(() => i++).ToAsyncEnumerable().Repeat(0);

            var e = xs.GetAsyncEnumerator();

            NoNext(e);
        }

        [Fact]
        public async Task RepeatSeq6()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Repeat(3);

            await SequenceIdentity(xs);
        }

        static IEnumerable<int> RepeatXs(Action started)
        {
            started();

            yield return 1;
            yield return 2;
        }

        [Fact]
        public void RepeatSeq4()
        {
            var xs = new FailRepeat().ToAsyncEnumerable().Repeat();

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is NotImplementedException);
        }

        [Fact]
        public void RepeatSeq5()
        {
            var xs = new FailRepeat().ToAsyncEnumerable().Repeat(3);

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is NotImplementedException);
        }

        class FailRepeat : IEnumerable<int>
        {
            public IEnumerator<int> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void IgnoreElements_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.IgnoreElements(default(IAsyncEnumerable<int>)));
        }

        [Fact]
        public void IgnoreElements1()
        {
            var xs = AsyncEnumerable.Empty<int>().IgnoreElements();

            var e = xs.GetAsyncEnumerator();
            NoNext(e);

            AssertThrows<InvalidOperationException>(() => { var ignored = e.Current; });
        }

        [Fact]
        public void IgnoreElements2()
        {
            var xs = AsyncEnumerable.Return(42).IgnoreElements();

            var e = xs.GetAsyncEnumerator();
            NoNext(e);

            AssertThrows<InvalidOperationException>(() => { var ignored = e.Current; });
        }

        [Fact]
        public void IgnoreElements3()
        {
            var xs = AsyncEnumerable.Range(0, 10).IgnoreElements();

            var e = xs.GetAsyncEnumerator();
            NoNext(e);

            AssertThrows<InvalidOperationException>(() => { var ignored = e.Current; });
        }

        [Fact]
        public void IgnoreElements4()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex).IgnoreElements();

            var e = xs.GetAsyncEnumerator();
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task IgnoreElements5()
        {
            var xs = AsyncEnumerable.Range(0, 10).IgnoreElements();

            await SequenceIdentity(xs);
        }

        [Fact]
        public void StartWith_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.StartWith(default(IAsyncEnumerable<int>), new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerableEx.StartWith(AsyncEnumerable.Return(42), null));
        }

        [Fact]
        public void StartWith1()
        {
            var xs = AsyncEnumerable.Empty<int>().StartWith(1, 2);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void StartWith2()
        {
            var xs = AsyncEnumerable.Return<int>(0).StartWith(1, 2);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 0);
            NoNext(e);
        }

        [Fact]
        public void StartWith3()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex).StartWith(1, 2);

            var e = xs.GetAsyncEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            AssertThrows(() => e.MoveNextAsync().Wait(WaitTimeoutMs), (Exception ex_) => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}

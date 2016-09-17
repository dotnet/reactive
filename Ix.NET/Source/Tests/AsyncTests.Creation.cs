// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using System.Threading;

namespace Tests
{
    public partial class AsyncTests
    {

        [Fact]
        public void Create_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.CreateEnumerable<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.CreateEnumerator<int>(null, () => 3, () => {}));
       
        }

        [Fact]
        public void Create_Iterator_Throws()
        {
     
           var iter = AsyncEnumerable.CreateEnumerator<int>(c => Task.FromResult(true), () => 3, () => { });

            var enu = (IAsyncEnumerable<int>)iter;

            AssertThrows<NotSupportedException>(() => enu.GetEnumerator());
        }


        [Fact]
        public void Return()
        {
            var xs = AsyncEnumerable.Return(42);
            HasNext(xs.GetEnumerator(), 42);
        }

        [Fact]
        public void Never()
        {
            var xs = AsyncEnumerable.Never<int>();

            var e = xs.GetEnumerator();
            Assert.False(e.MoveNext().IsCompleted); // Very rudimentary check
            AssertThrows<InvalidOperationException>(() => Nop(e.Current));
            e.Dispose();
        }

        [Fact]
        public async Task Empty_Null()
        {
           await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.IsEmpty<int>(null));
           await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.IsEmpty<int>(null, CancellationToken.None));
        }

        [Fact]
        public void Empty1()
        {
            var xs = AsyncEnumerable.Empty<int>();
            NoNext(xs.GetEnumerator());
        }

        [Fact]
        public void Empty2()
        {
            var xs = AsyncEnumerable.Empty<int>();

            var e = xs.GetEnumerator();
            Assert.False(e.MoveNext().Result);
            AssertThrows<InvalidOperationException>(() => Nop(e.Current));
        }

        [Fact]
        public void Throw_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Throw<int>(null));
        }

        [Fact]
        public void Throw()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);

            var e = xs.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
            AssertThrows<InvalidOperationException>(() => Nop(e.Current));
        }

        private void Nop(object o)
        {
        }

        [Fact]
        public void Range_Null()
        {
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.Range(0, -1));
        }

        [Fact]
        public void Range1()
        {
            var xs = AsyncEnumerable.Range(2, 5);

            var e = xs.GetEnumerator();
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public void Range2()
        {
            var xs = AsyncEnumerable.Range(2, 0);

            var e = xs.GetEnumerator();
            NoNext(e);
        }

        [Fact]
        public void Repeat_Null()
        {
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.Repeat(0, -1));
        }

        [Fact]
        public void Repeat1()
        {
            var xs = AsyncEnumerable.Repeat(2, 5);

            var e = xs.GetEnumerator();
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void Repeat2()
        {
            var xs = AsyncEnumerable.Repeat(2, 0);

            var e = xs.GetEnumerator();
            NoNext(e);
        }

        [Fact]
        public void Repeat3()
        {
            var xs = AsyncEnumerable.Repeat(2);

            var e = xs.GetEnumerator();
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 2);
            e.Dispose();
        }

        [Fact]
        public async Task Repeat4()
        {
            var xs = AsyncEnumerable.Repeat(2).Take(5);

            await SequenceIdentity(xs);
        }

        [Fact]
        public void Defer_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Defer<int>(null));
        }

        [Fact]
        public void Defer1()
        {
            var x = 0;
            var xs = AsyncEnumerable.Defer<int>(() => new[] { x }.ToAsyncEnumerable());

            {
                var e = xs.GetEnumerator();
                HasNext(e, 0);
                NoNext(e);
            }

            {
                x++;
                var e = xs.GetEnumerator();
                HasNext(e, 1);
                NoNext(e);
            }
        }

        [Fact]
        public void Generate_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Generate<int, int>(0, null, x => x, x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Generate<int, int>(0, x => true, null, x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Generate<int, int>(0, x => true, x => x, null));
        }

        [Fact]
        public void Generate1()
        {
            var xs = AsyncEnumerable.Generate(0, x => x < 5, x => x + 1, x => x * x);

            var e = xs.GetEnumerator();
            HasNext(e, 0);
            HasNext(e, 1);
            HasNext(e, 4);
            HasNext(e, 9);
            HasNext(e, 16);
            NoNext(e);
            e.Dispose();
        }

        [Fact]
        public void Generate2()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Generate(0, x => { throw ex; }, x => x + 1, x => x * x);

            var e = xs.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Generate3()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Generate(0, x => true, x => x + 1, x => { if (x == 1) throw ex; return x * x; });

            var e = xs.GetEnumerator();
            HasNext(e, 0);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Generate4()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Generate(0, x => true, x => { throw ex; }, x => x * x);

            var e = xs.GetEnumerator();
            HasNext(e, 0);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task Generate5()
        {
            var xs = AsyncEnumerable.Generate(0, x => x < 5, x => x + 1, x => x * x);

            await SequenceIdentity(xs);
        }

        [Fact]
        public void Using_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Using<int, IDisposable>(null, _ => null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Using<int, IDisposable>(() => new MyD(null), null));
        }

        [Fact]
        public void Using1()
        {
            var i = 0;
            var d = 0;

            var xs = AsyncEnumerable.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { d++; });
                },
                _ => AsyncEnumerable.Return(42)
            );

            Assert.Equal(0, i);

            var e = xs.GetEnumerator();
            Assert.Equal(1, i);
        }

        [Fact]
        public void Using2()
        {
            var i = 0;
            var d = 0;

            var xs = AsyncEnumerable.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { d++; });
                },
                _ => AsyncEnumerable.Return(42)
            );

            Assert.Equal(0, i);

            var e = xs.GetEnumerator();
            Assert.Equal(1, i);

            e.Dispose();
            Assert.Equal(1, d);
        }

        [Fact]
        public void Using3()
        {
            var ex = new Exception("Bang!");
            var i = 0;
            var d = 0;

            var xs = AsyncEnumerable.Using<int, MyD>(
                () =>
                {
                    i++;
                    return new MyD(() => { d++; });
                },
                _ => { throw ex; }
            );

            Assert.Equal(0, i);

            AssertThrows<Exception>(() => xs.GetEnumerator(), ex_ => ex_ == ex);
            
            Assert.Equal(1, d);
        }

        [Fact]
        public void Using4()
        {
            var i = 0;
            var disposed = new TaskCompletionSource<bool>();

            var xs = AsyncEnumerable.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { disposed.TrySetResult(true); });
                },
                _ => AsyncEnumerable.Return(42)
            );

            Assert.Equal(0, i);

            var e = xs.GetEnumerator();
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

            var xs = AsyncEnumerable.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { disposed.TrySetResult(true); });
                },
                _ => AsyncEnumerable.Throw<int>(ex)
            );

            Assert.Equal(0, i);

            var e = xs.GetEnumerator();
            Assert.Equal(1, i);

            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);

            Assert.True(disposed.Task.Result);
        }

        [Fact]
        public async Task Using6()
        {
            var i = 0;
            var disposed = new TaskCompletionSource<bool>();

            var xs = AsyncEnumerable.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { disposed.TrySetResult(true); });
                },
                _ => new CancellationTestAsyncEnumerable(2) // need to use this to verify we actually cancel
            );

            Assert.Equal(0, i);

            var e = xs.GetEnumerator();
            Assert.Equal(1, i);

            HasNext(e, 0);
            HasNext(e, 1);

            var cts = new CancellationTokenSource();
            var t = e.MoveNext(cts.Token);
            cts.Cancel();

            try
            {
                t.Wait(WaitTimeoutMs);
            }
            catch (AggregateException ex)
            {
                ex.Flatten().Handle(inner => inner is TaskCanceledException);
            }

            Assert.True(disposed.Task.IsCompleted);
            Assert.True(await disposed.Task);
        }

        [Fact]
        public async Task Using7()
        {
            var i = 0;
            var d = 0;

            var xs = AsyncEnumerable.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { d++; });
                },
                _ => AsyncEnumerable.Return(42)
            );

            await SequenceIdentity(xs);
        }

        class MyD : IDisposable
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
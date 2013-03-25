// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
#if !NO_TPL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Threading;

namespace Tests
{
    static class Ext
    {
        public static Task<bool> MoveNext<T>(this IAsyncEnumerator<T> enumerator)
        {
            return enumerator.MoveNext(CancellationToken.None);
        }
    }

    public partial class AsyncTests
    {
        [TestMethod]
        public void Return()
        {
            var xs = AsyncEnumerable.Return(42);
            HasNext(xs.GetEnumerator(), 42);
        }

        [TestMethod]
        public void Never()
        {
            var xs = AsyncEnumerable.Never<int>();

            var e = xs.GetEnumerator();
            Assert.IsFalse(e.MoveNext().IsCompleted); // Very rudimentary check
            AssertThrows<InvalidOperationException>(() => Nop(e.Current));
            e.Dispose();
        }

        [TestMethod]
        public void Empty1()
        {
            var xs = AsyncEnumerable.Empty<int>();
            NoNext(xs.GetEnumerator());
        }

        [TestMethod]
        public void Empty2()
        {
            var xs = AsyncEnumerable.Empty<int>();

            var e = xs.GetEnumerator();
            Assert.IsFalse(e.MoveNext().Result);
            AssertThrows<InvalidOperationException>(() => Nop(e.Current));
        }

        [TestMethod]
        public void Throw_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Throw<int>(null));
        }

        [TestMethod]
        public void Throw()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);

            var e = xs.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
            AssertThrows<InvalidOperationException>(() => Nop(e.Current));
        }

        private void Nop(object o)
        {
        }

        [TestMethod]
        public void Range_Null()
        {
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.Range(0, -1));
        }

        [TestMethod]
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

        [TestMethod]
        public void Range2()
        {
            var xs = AsyncEnumerable.Range(2, 0);

            var e = xs.GetEnumerator();
            NoNext(e);
        }

        [TestMethod]
        public void Repeat_Null()
        {
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.Repeat(0, -1));
        }

        [TestMethod]
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

        [TestMethod]
        public void Repeat2()
        {
            var xs = AsyncEnumerable.Repeat(2, 0);

            var e = xs.GetEnumerator();
            NoNext(e);
        }

        [TestMethod]
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

        [TestMethod]
        public void Defer_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Defer<int>(null));
        }

        [TestMethod]
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

        [TestMethod]
        public void Generate_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Generate<int, int>(0, null, x => x, x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Generate<int, int>(0, x => true, null, x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Generate<int, int>(0, x => true, x => x, null));
        }

        [TestMethod]
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

        [TestMethod]
        public void Generate2()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Generate(0, x => { throw ex; }, x => x + 1, x => x * x);

            var e = xs.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Generate3()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Generate(0, x => true, x => x + 1, x => { if (x == 1) throw ex; return x * x; });

            var e = xs.GetEnumerator();
            HasNext(e, 0);
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Generate4()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Generate(0, x => true, x => { throw ex; }, x => x * x);

            var e = xs.GetEnumerator();
            HasNext(e, 0);
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Using_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Using<int, IDisposable>(null, _ => null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Using<int, IDisposable>(() => new MyD(null), null));
        }

        [TestMethod]
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

            Assert.AreEqual(0, i);

            var e = xs.GetEnumerator();
            Assert.AreEqual(1, i);
        }

        [TestMethod]
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

            Assert.AreEqual(0, i);

            var e = xs.GetEnumerator();
            Assert.AreEqual(1, i);

            e.Dispose();
            Assert.AreEqual(1, d);
        }

        [TestMethod]
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

            Assert.AreEqual(0, i);

            AssertThrows<Exception>(() => xs.GetEnumerator(), ex_ => ex_ == ex);
            
            Assert.AreEqual(1, d);
        }

        [TestMethod]
        public void Using4()
        {
            var i = 0;
            var disposed = false;

            var xs = AsyncEnumerable.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { disposed = true; });
                },
                _ => AsyncEnumerable.Return(42)
            );

            Assert.AreEqual(0, i);

            var e = xs.GetEnumerator();
            Assert.AreEqual(1, i);

            HasNext(e, 42);
            NoNext(e);

            Assert.IsTrue(disposed);
        }

        [TestMethod]
        public void Using5()
        {
            var ex = new Exception("Bang!");
            var i = 0;
            var disposed = false;

            var xs = AsyncEnumerable.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { disposed = true; });
                },
                _ => AsyncEnumerable.Throw<int>(ex)
            );

            Assert.AreEqual(0, i);

            var e = xs.GetEnumerator();
            Assert.AreEqual(1, i);

            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);

            Assert.IsTrue(disposed);
        }

        [TestMethod]
        public void Using6()
        {
            var i = 0;
            var disposed = false;

            var xs = AsyncEnumerable.Using(
                () =>
                {
                    i++;
                    return new MyD(() => { disposed = true; });
                },
                _ => AsyncEnumerable.Range(0, 10)
            );

            Assert.AreEqual(0, i);

            var e = xs.GetEnumerator();
            Assert.AreEqual(1, i);

            HasNext(e, 0);
            HasNext(e, 1);

            var cts = new CancellationTokenSource();
            var t = e.MoveNext(cts.Token);
            cts.Cancel();

            t.Wait();

            Assert.IsTrue(disposed);
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

#endif
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
namespace Tests
{

    public partial class AsyncTests
    {
        [Fact]
        public void MoveNextExtension_Null()
        {
            var en = default(IAsyncEnumerator<int>);

            Assert.ThrowsAsync<ArgumentNullException>(() => en.MoveNext());
        }

        [Fact]
        public void MoveNextExtension()
        {
            var enumerable = new CancellationTestAsyncEnumerable();
            var en = enumerable.GetEnumerator();

            en.MoveNext();

            Assert.True(en.MoveNextWasCalled);
            Assert.Equal(CancellationToken.None, en.LastToken);
        }

        [Fact]
        public void Select_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Select<int, int>(null, x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Select<int, int>(null, (x, i) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Select<int, int>(AsyncEnumerable.Return<int>(42), default(Func<int, int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Select<int, int>(AsyncEnumerable.Return<int>(42), default(Func<int, int, int>)));
        }

        [Fact]
        public void Select1()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(x => (char)('a' + x));

            var e = ys.GetEnumerator();
            HasNext(e, 'a');
            HasNext(e, 'b');
            HasNext(e, 'c');
            NoNext(e);
        }

        [Fact]
        public void Select2()
        {
            var xs = new[] { 8, 5, 7 }.ToAsyncEnumerable();
            var ys = xs.Select((x, i) => (char)('a' + i));

            var e = ys.GetEnumerator();
            HasNext(e, 'a');
            HasNext(e, 'b');
            HasNext(e, 'c');
            NoNext(e);
        }

        [Fact]
        public void Select3()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(x => 1 / x);

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is DivideByZeroException);
        }

        [Fact]
        public void Select4()
        {
            var xs = new[] { 8, 5, 7 }.ToAsyncEnumerable();
            var ys = xs.Select((x, i) => 1 / i);

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is DivideByZeroException);
        }

        [Fact]
        public void Select5()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(i => i + 3).Select(x => (char)('a' + x));

            var e = ys.GetEnumerator();
            HasNext(e, 'd');
            HasNext(e, 'e');
            HasNext(e, 'f');
            NoNext(e);
        }

        [Fact]
        public void Select6()
        {
            var xs = new CancellationTestAsyncEnumerable(10);
            var ys = xs.Select(i => i + 3).Select(x => (char)('a' + x));

            var e = ys.GetEnumerator();
            HasNext(e, 'd');
            HasNext(e, 'e');
            HasNext(e, 'f');
        }

        [Fact]
        public async Task Select7()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(x => (char)('a' + x));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Select8()
        {
            var xs = new[] { 8, 5, 7 }.ToAsyncEnumerable();
            var ys = xs.Select((x, i) => (char)('a' + i));

            await SequenceIdentity(ys);
        }


        [Fact]
        public void SelectWhere1()
        {
            var xs = new CancellationTestAsyncEnumerable(10);
            var ys = xs.Select(i => i + 2).Where(i => i % 2 == 0);

            var e = ys.GetEnumerator();
            HasNext(e, 2);
            HasNext(e, 4);
            HasNext(e, 6);
        }

        [Fact]
        public void WhereSelect1()
        {
            var xs = new CancellationTestAsyncEnumerable(10);
            var ys = xs.Where(i => i % 2 == 0).Select(i => i + 2);

            var e = ys.GetEnumerator();
            HasNext(e, 2);
            HasNext(e, 4);
            HasNext(e, 6);
        }


        [Fact]
        public void SelectWhere2()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(i => i + 2).Where(i => i % 2 == 0);

            var e = ys.GetEnumerator();
            HasNext(e, 2);
            HasNext(e, 4);
            NoNext(e);

        }

        [Fact]
        public void WhereSelect2()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Where(i => i % 2 == 0).Select(i => i + 2);

            var e = ys.GetEnumerator();
            HasNext(e, 2);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void WhereSelect3()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Where(i => i % 2 == 0).Select(i => i + 2).Select(i => i + 2);

            var e = ys.GetEnumerator();
            HasNext(e, 4);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public async Task WhereSelect4()
        {
            var xs = new CancellationTestAsyncEnumerable(10).Take(5);
            var ys = xs.Where(i => i % 2 == 0).Select(i => i + 2);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void Where_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Where<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Where<int>(null, (x, i) => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Where<int>(AsyncEnumerable.Return<int>(42), default(Func<int, bool>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Where<int>(AsyncEnumerable.Return<int>(42), default(Func<int, int, bool>)));
        }

        [Fact]
        public void Where1()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => x % 2 == 0);
            var e = ys.GetEnumerator();
            HasNext(e, 8);
            HasNext(e, 4);
            HasNext(e, 6);
            HasNext(e, 2);
            HasNext(e, 0);
            NoNext(e);
        }

        [Fact]
        public void Where2()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((x, i) => i % 2 == 0);

            var e = ys.GetEnumerator();
            HasNext(e, 8);
            HasNext(e, 7);
            HasNext(e, 6);
            HasNext(e, 2);
            HasNext(e, 0);
            NoNext(e);
        }

        [Fact]
        public void Where3()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ex = new Exception("Bang");
            var ys = xs.Where(x => { if (x == 4) throw ex; return true; });

            var e = ys.GetEnumerator();
            HasNext(e, 8);
            HasNext(e, 5);
            HasNext(e, 7);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Where4()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ex = new Exception("Bang");
            var ys = xs.Where((x, i) => { if (i == 3) throw ex; return true; });

            var e = ys.GetEnumerator();
            HasNext(e, 8);
            HasNext(e, 5);
            HasNext(e, 7);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Where5()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Where(x => true);

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Where6()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);

            var ys = xs.Where((x, i) => true);
            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }


        [Fact]
        public void Where7()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => x % 2 == 0).Where(x => x > 5);
            var e = ys.GetEnumerator();
            HasNext(e, 8);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public async Task Where8()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => x % 2 == 0);
            
            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Where9()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((x, i) => i % 2 == 0);

            await SequenceIdentity(ys);
        }


        [Fact]
        public void SelectMany_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int>(null, x => null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int>(null, (x, i) => null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int>(AsyncEnumerable.Return(42), default(Func<int, IAsyncEnumerable<int>>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int>(AsyncEnumerable.Return(42), default(Func<int, int, IAsyncEnumerable<int>>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int, int>(null, x => null, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int, int>(null, (x, i) => null, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int, int>(AsyncEnumerable.Return(42), default(Func<int, IAsyncEnumerable<int>>), (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int, int>(AsyncEnumerable.Return(42), default(Func<int, int, IAsyncEnumerable<int>>), (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int, int>(AsyncEnumerable.Return(42), x => null, default(Func<int, int, int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int, int>(AsyncEnumerable.Return(42), (x, i) => null, default(Func<int, int, int>)));
        }

        [Fact]
        public void SelectMany1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(0, x).ToAsyncEnumerable());

            var e = ys.GetEnumerator();
            HasNext(e, 0);
            HasNext(e, 0);
            HasNext(e, 1);
            HasNext(e, 0);
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void SelectMany2()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x =>
            {
                if (x < 3)
                    return Enumerable.Range(0, x).ToAsyncEnumerable();
                else
                    return AsyncEnumerable.Throw<int>(ex);
            });

            var e = ys.GetEnumerator();
            HasNext(e, 0);
            HasNext(e, 0);
            HasNext(e, 1);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void SelectMany3()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.SelectMany(x => Enumerable.Range(0, x).ToAsyncEnumerable());

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void SelectMany4()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x =>
            {
                if (x < 3)
                    return Enumerable.Range(0, x).ToAsyncEnumerable();
                else
                    throw ex;
            });

            var e = ys.GetEnumerator();
            HasNext(e, 0);
            HasNext(e, 0);
            HasNext(e, 1);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void SelectMany5()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 5, x).ToAsyncEnumerable());

            var e = ys.GetEnumerator();
            HasNext(e, 5);
            HasNext(e, 6);
            HasNext(e, 7);
            HasNext(e, 7);
            HasNext(e, 8);
            HasNext(e, 9);
            NoNext(e);
        }

        [Fact]
        public void SelectMany6()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) =>
            {
                if (i < 2)
                    return Enumerable.Range(0, x).ToAsyncEnumerable();
                else
                    return AsyncEnumerable.Throw<int>(ex);
            });

            var e = ys.GetEnumerator();
            HasNext(e, 0);
            HasNext(e, 0);
            HasNext(e, 1);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void SelectMany7()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.SelectMany((x, i) => Enumerable.Range(0, x).ToAsyncEnumerable());

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void SelectMany8()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) =>
            {
                if (i < 2)
                    return Enumerable.Range(0, x).ToAsyncEnumerable();
                else
                    throw ex;
            });

            var e = ys.GetEnumerator();
            HasNext(e, 0);
            HasNext(e, 0);
            HasNext(e, 1);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void SelectMany9()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(3, x).ToAsyncEnumerable(), (x, y) => x * y);

            var e = ys.GetEnumerator();
            HasNext(e, 3);
            HasNext(e, 6);
            HasNext(e, 8);
            HasNext(e, 9);
            HasNext(e, 12);
            HasNext(e, 15);
            NoNext(e);
        }

        [Fact]
        public void SelectMany10()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 3, x).ToAsyncEnumerable(), (x, y) => x * y);

            var e = ys.GetEnumerator();
            HasNext(e, 3);
            HasNext(e, 8);
            HasNext(e, 10);
            HasNext(e, 15);
            HasNext(e, 18);
            HasNext(e, 21);
            NoNext(e);
        }

        [Fact]
        public void SelectMany11()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(3, x).ToAsyncEnumerable(), (x, y) =>
            {
                if (x * y > 10)
                    throw ex;
                return x * y;
            });

            var e = ys.GetEnumerator();
            HasNext(e, 3);
            HasNext(e, 6);
            HasNext(e, 8);
            HasNext(e, 9);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void SelectMany12()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 3, x).ToAsyncEnumerable(), (x, y) =>
            {
                if (x * y > 10)
                    throw ex;
                return x * y;
            });

            var e = ys.GetEnumerator();
            HasNext(e, 3);
            HasNext(e, 8);
            HasNext(e, 10);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task SelectMany13()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(0, x).ToAsyncEnumerable());

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SelectMany14()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 5, x).ToAsyncEnumerable());

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SelectMany15()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(3, x).ToAsyncEnumerable(), (x, y) => x * y);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SelectMany16()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 3, x).ToAsyncEnumerable(), (x, y) => x * y);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void OfType_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OfType<int>(null));
        }

        [Fact]
        public void OfType()
        {
            var xs = new object[] { 1, 1.2, true, 4, "" }.ToAsyncEnumerable();
            var ys = xs.OfType<int>();

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void Cast_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Cast<int>(null));
        }

        [Fact]
        public void Cast1()
        {
            var xs = new object[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Cast<int>();

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void Cast2()
        {
            var xs = new [] { new EventArgs(), new EventArgs(), new EventArgs() }.ToAsyncEnumerable();
            var ys = xs.Cast<EventArgs>();

            Assert.Same(xs, ys);
        }

        [Fact]
        public void Do_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(null, x => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(AsyncEnumerable.Return(42), default(Action<int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(null, x => { }, () => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(AsyncEnumerable.Return(42), default(Action<int>), () => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(AsyncEnumerable.Return(42), x => { }, default(Action)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(null, x => { }, ex => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(AsyncEnumerable.Return(42), default(Action<int>), ex => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(AsyncEnumerable.Return(42), x => { }, default(Action<Exception>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(null, x => { }, ex => { }, () => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(AsyncEnumerable.Return(42), default(Action<int>), ex => { }, () => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(AsyncEnumerable.Return(42), x => { }, default(Action<Exception>), () => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(AsyncEnumerable.Return(42), x => { }, ex => { }, default(Action)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(null, new MyObs()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Do<int>(AsyncEnumerable.Return(42), default(IObserver<int>)));
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

            var e = ys.GetEnumerator();
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

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Do3()
        {
            var sum = 0;
            var fail = false;
            var done = false;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Do(x => sum += x, ex => { fail = true; }, () => { done = true; });

            var e = ys.GetEnumerator();
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

            var e = ys.GetEnumerator();
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

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ex_.InnerException == ex);

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

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ex_.InnerException == ex);

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
        public void ForEach_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ForEach<int>(null, x => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ForEach<int>(AsyncEnumerable.Return(42), default(Action<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ForEach<int>(null, (x, i) => { }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ForEach<int>(AsyncEnumerable.Return(42), default(Action<int, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ForEach<int>(null, x => { }, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ForEach<int>(AsyncEnumerable.Return(42), default(Action<int>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ForEach<int>(null, (x, i) => { }, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ForEach<int>(AsyncEnumerable.Return(42), default(Action<int, int>), CancellationToken.None));
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
        public void ForEach1()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            xs.ForEach(x => sum += x);
            Assert.Equal(10, sum);
        }

        [Fact]
        public void ForEachAsync2()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            xs.ForEach((x, i) => sum += x * i);
            Assert.Equal(1 * 0 + 2 * 1 + 3 * 2 + 4 * 3, sum);
        }

        [Fact]
        public void ForEach2()
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
        public void ForEach3()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            AssertThrows<Exception>(() => xs.ForEach(x => { throw ex; }), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ForEachAsync4()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            AssertThrows<Exception>(() => xs.ForEachAsync((x, i) => { throw ex; }).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ForEach4()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            AssertThrows<Exception>(() => xs.ForEach((x, i) => { throw ex; }), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ForEachAsync5()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);

            AssertThrows<Exception>(() => xs.ForEachAsync(x => { throw ex; }).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ForEach5()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);

            AssertThrows<Exception>(() => xs.ForEach(x => { throw ex; }), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ForEachAsync6()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);

            AssertThrows<Exception>(() => xs.ForEachAsync((x, i) => { throw ex; }).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ForEach6()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);

            AssertThrows<Exception>(() => xs.ForEach((x, i) => { throw ex; }), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Take_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Take<int>(null, 5));
        }

        [Fact]
        public void Take0()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(-2);

            var e = ys.GetEnumerator();
            NoNext(e);
        }

        [Fact]
        public void Take1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(2);

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void Take2()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(10);

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void Take3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(0);

            var e = ys.GetEnumerator();
            NoNext(e);
        }

        [Fact]
        public void Take4()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Take(2);

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task Take5()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(2);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void TakeWhile_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.TakeWhile<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.TakeWhile<int>(null, (x, i) => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.TakeWhile<int>(AsyncEnumerable.Return(42), default(Func<int, bool>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.TakeWhile<int>(AsyncEnumerable.Return(42), default(Func<int, int, bool>)));
        }

        [Fact]
        public void TakeWhile1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => x < 3);

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void TakeWhile2()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => false);

            var e = ys.GetEnumerator();
            NoNext(e);
        }

        [Fact]
        public void TakeWhile3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => true);

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void TakeWhile4()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 2, 1 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => x < 3);

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void TakeWhile5()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => { throw ex; });

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void TakeWhile6()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile((x, i) => i < 2);

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void TakeWhile7()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile((x, i) => false);

            var e = ys.GetEnumerator();
            NoNext(e);
        }

        [Fact]
        public void TakeWhile8()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile((x, i) => true);

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void TakeWhile9()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile((x, i) => { throw ex; });

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }


        [Fact]
        public async Task TakeWhile10()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => x < 3);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task TakeWhile11()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile((x, i) => i < 2);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void Skip_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Skip<int>(null, 5));
        }

        //[Fact]
        //public void Skip0()
        //{
        //    var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
        //    var ys = xs.Skip(-2);

        //    var e = ys.GetEnumerator();
        //    NoNext(e);
        //}

        [Fact]
        public void Skip1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(2);

            var e = ys.GetEnumerator();
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void Skip2()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(10);

            var e = ys.GetEnumerator();
            NoNext(e);
        }

        [Fact]
        public void Skip3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(0);

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void Skip4()
        {
            var ex = new Exception("Bang");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Skip(2);

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task Skip5()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(2);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void SkipWhile_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SkipWhile<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SkipWhile<int>(null, (x, i) => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SkipWhile<int>(AsyncEnumerable.Return(42), default(Func<int, bool>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SkipWhile<int>(AsyncEnumerable.Return(42), default(Func<int, int, bool>)));
        }

        [Fact]
        public void SkipWhile1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => x < 3);

            var e = ys.GetEnumerator();
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void SkipWhile2()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => false);

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void SkipWhile3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => true);

            var e = ys.GetEnumerator();
            NoNext(e);
        }

        [Fact]
        public void SkipWhile4()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 2, 1 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => x < 3);

            var e = ys.GetEnumerator();
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 3);
            HasNext(e, 2);
            HasNext(e, 1);
            NoNext(e);
        }

        [Fact]
        public void SkipWhile5()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => { throw ex; });

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void SkipWhile6()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => i < 2);

            var e = ys.GetEnumerator();
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void SkipWhile7()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => false);

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void SkipWhile8()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => true);

            var e = ys.GetEnumerator();
            NoNext(e);
        }

        [Fact]
        public void SkipWhile9()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => { throw ex; });

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task SkipWhile10()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => x < 3);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SkipWhile11()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => false);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void DefaultIfEmpty_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.DefaultIfEmpty<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.DefaultIfEmpty<int>(null, 42));
        }

        [Fact]
        public void DefaultIfEmpty1()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty();

            var e = xs.GetEnumerator();
            HasNext(e, 0);
            NoNext(e);
        }

        [Fact]
        public void DefaultIfEmpty2()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty(42);

            var e = xs.GetEnumerator();
            HasNext(e, 42);
            NoNext(e);
        }

        [Fact]
        public void DefaultIfEmpty3()
        {
            var xs = AsyncEnumerable.Return<int>(42).DefaultIfEmpty();

            var e = xs.GetEnumerator();
            HasNext(e, 42);
            NoNext(e);
        }

        [Fact]
        public void DefaultIfEmpty4()
        {
            var xs = AsyncEnumerable.Return<int>(42).DefaultIfEmpty(24);

            var e = xs.GetEnumerator();
            HasNext(e, 42);
            NoNext(e);
        }

        [Fact]
        public void DefaultIfEmpty5()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty();

            var e = xs.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void DefaultIfEmpty6()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            var e = xs.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void DefaultIfEmpty7()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex).DefaultIfEmpty();

            var e = xs.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void DefaultIfEmpty8()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex).DefaultIfEmpty(24);

            var e = xs.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task DefaultIfEmpty9()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty(42);

            var res = new[] { 42 };

            Assert.True(res.SequenceEqual(await xs.ToArray()));
        }

        [Fact]
        public async Task DefaultIfEmpty10()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty(42);

            var res = new List<int> { 42 };

            Assert.True(res.SequenceEqual(await xs.ToList()));
        }

        [Fact]
        public async Task DefaultIfEmpty11()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty(42);
            
            Assert.Equal(1, await xs.Count());
        }


        [Fact]
        public async Task DefaultIfEmpty12()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            var res = new[] { 1, 2, 3, 4 };

            Assert.True(res.SequenceEqual(await xs.ToArray()));
        }

        [Fact]
        public async Task DefaultIfEmpty13()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            var res = new List<int> { 1, 2, 3, 4 };

            Assert.True(res.SequenceEqual(await xs.ToList()));
        }

        [Fact]
        public async Task DefaultIfEmpty14()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            Assert.Equal(4, await xs.Count());
        }

        [Fact]
        public async Task DefaultIfEmpty15()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task DefaultIfEmpty16()
        {
            var xs = new CancellationTestAsyncEnumerable(10)
                .Take(5)
                .Reverse() // so we have an ilist provider 
                .DefaultIfEmpty(24)
                .Append(5); // for the onlyIsCheap to true

            var r = new[] { 4, 3, 2, 1, 0, 5 };

            Assert.Equal(r, await xs.ToArray());
        }

        [Fact]
        public async Task DefaultIfEmpty17()
        {
            var xs = new CancellationTestAsyncEnumerable(10)
                .Take(5)
                .DefaultIfEmpty(24)
                .Append(5); // for the onlyIsCheap to true

            var r = new[] { 0, 1, 2, 3, 4, 5};

            Assert.Equal(r, await xs.ToArray());
        }


        [Fact]
        public void Distinct_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Distinct<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Distinct<int>(null, new Eq()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Distinct<int>(AsyncEnumerable.Return(42), null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Distinct<int, int>(AsyncEnumerable.Return(42), (Func<int, int>)null));
        }

        [Fact]
        public void Distinct1()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();

            var e = xs.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 5);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void Distinct2()
        {
            var xs = new[] { 1, -2, -1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(new Eq());

            var e = xs.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, -2);
            HasNext(e, 3);
            HasNext(e, 5);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public async Task Distinct3()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();
            
            var res = new [] { 1, 2, 3, 5, 4 };
            Assert.True(res.SequenceEqual(await xs.ToArray()));
        }

        [Fact]
        public async Task Distinct4()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();

            var res = new List<int> { 1, 2, 3, 5, 4 };
            Assert.True(res.SequenceEqual(await xs.ToList()));
        }

        [Fact]
        public async Task Distinct5()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();
            
            Assert.Equal(5, await xs.Count());
        }

        [Fact]
        public async Task Distinct6()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(k => k);

            var res = new[] { 1, 2, 3, 5, 4 };
            Assert.True(res.SequenceEqual(await xs.ToArray()));
        }

        [Fact]
        public async Task Distinct7()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(k => k);

            var res = new List<int> { 1, 2, 3, 5, 4 };
            Assert.True(res.SequenceEqual(await xs.ToList()));
        }

        [Fact]
        public async Task Distinct8()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(k => k);

            Assert.Equal(5, await xs.Count());
        }

        [Fact]
        public async Task Distinct9()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(k => k);

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task Distinct10()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();

            await SequenceIdentity(xs);
        }

        [Fact]
        public void Distinct11()
        {
            var xs = AsyncEnumerable.Empty<int>().Distinct(k => k);

            var e = xs.GetEnumerator();
            
            NoNext(e);
        }

        [Fact]
        public void Distinct12()
        {
            var xs = AsyncEnumerable.Empty<int>().Distinct();

            var e = xs.GetEnumerator();

            NoNext(e);
        }

        [Fact]
        public void Reverse_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Reverse<int>(null));
        }

        [Fact]
        public void Reverse1()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var ys = xs.Reverse();

            var e = ys.GetEnumerator();
            NoNext(e);
        }

        [Fact]
        public void Reverse2()
        {
            var xs = AsyncEnumerable.Return(42);
            var ys = xs.Reverse();

            var e = ys.GetEnumerator();
            HasNext(e, 42);
            NoNext(e);
        }

        [Fact]
        public void Reverse3()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            var e = ys.GetEnumerator();
            HasNext(e, 3);
            HasNext(e, 2);
            HasNext(e, 1);
            NoNext(e);
        }

        [Fact]
        public void Reverse4()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Reverse();

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task Reverse5()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            Assert.Equal(new[] { 3, 2, 1 }, await ys.ToArray());
        }

        [Fact]
        public async Task Reverse6()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            Assert.Equal(new[] { 3, 2, 1 }, await ys.ToList());
        }

        [Fact]
        public async Task Reverse7()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            Assert.Equal(3, await ys.Count());
        }

        [Fact]
        public async Task Reverse8()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Reverse9()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse().Prepend(4); // to trigger onlyIfCheap

            Assert.Equal(new[] { 4, 3, 2, 1 }, await ys.ToArray());
        }

        [Fact]
        public async Task Reverse10()
        {
            var xs = new CancellationTestAsyncEnumerable(10).Skip(1).Take(3);
            var ys = xs.Reverse().Prepend(4); // to trigger onlyIfCheap

            Assert.Equal(new[] { 4, 3, 2, 1 }, await ys.ToArray());
        }

        [Fact]
        public async Task Reverse11()
        {
            var xs = new CancellationTestAsyncEnumerable(10).Skip(1).Take(3);
            var ys = xs.Reverse().Prepend(4); // to trigger onlyIfCheap

            Assert.Equal(new[] { 4, 3, 2, 1 }, await ys.ToList());
        }


        [Fact]
        public async Task Reverse12()
        {
            var xs = new CancellationTestAsyncEnumerable(10).Skip(1).Take(3);
            var ys = xs.Reverse().Prepend(4).Prepend(5); // to trigger onlyIfCheap

            Assert.Equal(new[] { 5, 4, 3, 2, 1 }, await ys.ToArray());
        }


        [Fact]
        public void OrderBy_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderBy<int, int>(null, x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderBy<int, int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderBy<int, int>(null, x => x, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderBy<int, int>(AsyncEnumerable.Return(42), null, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderBy<int, int>(AsyncEnumerable.Return(42), x => x, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending<int, int>(null, x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending<int, int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending<int, int>(null, x => x, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending<int, int>(AsyncEnumerable.Return(42), null, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.OrderByDescending<int, int>(AsyncEnumerable.Return(42), x => x, null));

            var xs = AsyncEnumerable.Return(42).OrderBy(x => x);

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenBy<int, int>(null, x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenBy<int, int>(xs, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenBy<int, int>(null, x => x, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenBy<int, int>(xs, null, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenBy<int, int>(xs, x => x, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending<int, int>(null, x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending<int, int>(xs, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending<int, int>(null, x => x, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending<int, int>(xs, null, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ThenByDescending<int, int>(xs, x => x, null));
        }

        [Fact]
        public void OrderBy1()
        {
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy(x => x);

            var e = ys.GetEnumerator();
            for (var i = 0; i < 10; i++)
                HasNext(e, i);
            NoNext(e);
        }

        [Fact]
        public void OrderBy2()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy<int, int>(x => { throw ex; });

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task OrderBy3()
        {
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy(x => x);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void ThenBy2()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy(x => x).ThenBy<int, int>(x => { throw ex; });

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void OrderByDescending1()
        {
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderByDescending(x => x);

            var e = ys.GetEnumerator();
            for (var i = 9; i >= 0; i--)
                HasNext(e, i);
            NoNext(e);
        }

        [Fact]
        public void OrderByDescending2()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderByDescending<int, int>(x => { throw ex; });

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task OrderByDescending3()
        {
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderByDescending(x => x);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void ThenByDescending2()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 6, 1, 5, 7, 8, 9, 3, 4, 0 }.ToAsyncEnumerable();
            var ys = xs.OrderBy<int, int>(x => x).ThenByDescending<int, int>(x => { throw ex; });

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void OrderByThenBy1()
        {
            var xs = new[] {
                new { Name = "Bart", Age = 27 },
                new { Name = "John", Age = 62 },
                new { Name = "Eric", Age = 27 },
                new { Name = "Lisa", Age = 14 },
                new { Name = "Brad", Age = 27 },
                new { Name = "Lisa", Age = 23 },
                new { Name = "Eric", Age = 42 },
            };

            var ys = xs.ToAsyncEnumerable();

            var ress = xs.OrderBy(x => x.Name).ThenBy(x => x.Age);
            var resa = ys.OrderBy(x => x.Name).ThenBy(x => x.Age);

            Assert.True(ress.SequenceEqual(resa.ToEnumerable()));
        }

        [Fact]
        public void OrderByThenBy2()
        {
            var xs = new[] {
                new { Name = "Bart", Age = 27 },
                new { Name = "John", Age = 62 },
                new { Name = "Eric", Age = 27 },
                new { Name = "Lisa", Age = 14 },
                new { Name = "Brad", Age = 27 },
                new { Name = "Lisa", Age = 23 },
                new { Name = "Eric", Age = 42 },
            };

            var ys = xs.ToAsyncEnumerable();

            var ress = xs.OrderBy(x => x.Name).ThenByDescending(x => x.Age);
            var resa = ys.OrderBy(x => x.Name).ThenByDescending(x => x.Age);

            Assert.True(ress.SequenceEqual(resa.ToEnumerable()));
        }

        [Fact]
        public void OrderByThenBy3()
        {
            var xs = new[] {
                new { Name = "Bart", Age = 27 },
                new { Name = "John", Age = 62 },
                new { Name = "Eric", Age = 27 },
                new { Name = "Lisa", Age = 14 },
                new { Name = "Brad", Age = 27 },
                new { Name = "Lisa", Age = 23 },
                new { Name = "Eric", Age = 42 },
            };

            var ys = xs.ToAsyncEnumerable();

            var ress = xs.OrderByDescending(x => x.Name).ThenBy(x => x.Age);
            var resa = ys.OrderByDescending(x => x.Name).ThenBy(x => x.Age);

            Assert.True(ress.SequenceEqual(resa.ToEnumerable()));
        }

        [Fact]
        public void OrderByThenBy4()
        {
            var xs = new[] {
                new { Name = "Bart", Age = 27 },
                new { Name = "John", Age = 62 },
                new { Name = "Eric", Age = 27 },
                new { Name = "Lisa", Age = 14 },
                new { Name = "Brad", Age = 27 },
                new { Name = "Lisa", Age = 23 },
                new { Name = "Eric", Age = 42 },
            };

            var ys = xs.ToAsyncEnumerable();

            var ress = xs.OrderByDescending(x => x.Name).ThenByDescending(x => x.Age);
            var resa = ys.OrderByDescending(x => x.Name).ThenByDescending(x => x.Age);

            Assert.True(ress.SequenceEqual(resa.ToEnumerable()));
        }

        [Fact]
        public void GroupBy_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int>(null, x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int>(AsyncEnumerable.Return(42), default(Func<int, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int>(null, x => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int>(AsyncEnumerable.Return(42), default(Func<int, int>), EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int>(AsyncEnumerable.Return(42), x => x, default(IEqualityComparer<int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(null, x => x, x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(AsyncEnumerable.Return(42), default(Func<int, int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(AsyncEnumerable.Return(42), x => x, default(Func<int, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(null, x => x, x => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(AsyncEnumerable.Return(42), default(Func<int, int>), x => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(AsyncEnumerable.Return(42), x => x, default(Func<int, int>), EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(AsyncEnumerable.Return(42), x => x, x => x, default(IEqualityComparer<int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(null, x => x, (x, ys) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(AsyncEnumerable.Return(42), default(Func<int, int>), (x, ys) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(AsyncEnumerable.Return(42), x => x, default(Func<int, IAsyncEnumerable<int>, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(null, x => x, (x, ys) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(AsyncEnumerable.Return(42), default(Func<int, int>), (x, ys) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(AsyncEnumerable.Return(42), x => x, default(Func<int, IAsyncEnumerable<int>, int>), EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int>(AsyncEnumerable.Return(42), x => x, (x, ys) => x, default(IEqualityComparer<int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(null, x => x, x => x, (x, ys) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(AsyncEnumerable.Return(42), default(Func<int, int>), x => x, (x, ys) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(AsyncEnumerable.Return(42), x => x, default(Func<int, int>), (x, ys) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(AsyncEnumerable.Return(42), x => x, x => x, default(Func<int, IAsyncEnumerable<int>, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(null, x => x, x => x, (x, ys) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(AsyncEnumerable.Return(42), default(Func<int, int>), x => x, (x, ys) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(AsyncEnumerable.Return(42), x => x, default(Func<int, int>), (x, ys) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(AsyncEnumerable.Return(42), x => x, x => x, default(Func<int, IAsyncEnumerable<int>, int>), EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupBy<int, int, int, int>(AsyncEnumerable.Return(42), x => x, x => x, (x, ys) => x, default(IEqualityComparer<int>)));
        }

        [Fact]
        public void GroupBy1()
        {
            var xs = new[] {
                new { Name = "Bart", Age = 27 },
                new { Name = "John", Age = 62 },
                new { Name = "Eric", Age = 27 },
                new { Name = "Lisa", Age = 14 },
                new { Name = "Brad", Age = 27 },
                new { Name = "Lisa", Age = 23 },
                new { Name = "Eric", Age = 42 },
            };

            var ys = xs.ToAsyncEnumerable();

            var res = ys.GroupBy(x => x.Age / 10);

            var e = res.GetEnumerator();

            Assert.True(e.MoveNext().Result);
            Assert.Equal(2, e.Current.Key);
            var g1 = e.Current.GetEnumerator();
            HasNext(g1, xs[0]);
            HasNext(g1, xs[2]);
            HasNext(g1, xs[4]);
            HasNext(g1, xs[5]);
            NoNext(g1);

            Assert.True(e.MoveNext().Result);
            Assert.Equal(6, e.Current.Key);
            var g2 = e.Current.GetEnumerator();
            HasNext(g2, xs[1]);
            NoNext(g2);

            Assert.True(e.MoveNext().Result);
            Assert.Equal(1, e.Current.Key);
            var g3 = e.Current.GetEnumerator();
            HasNext(g3, xs[3]);
            NoNext(g3);

            Assert.True(e.MoveNext().Result);
            Assert.Equal(4, e.Current.Key);
            var g4 = e.Current.GetEnumerator();
            HasNext(g4, xs[6]);
            NoNext(g4);

            NoNext(e);
        }

        [Fact]
        public void GroupBy2()
        {
            var xs = new[] {
                new { Name = "Bart", Age = 27 },
                new { Name = "John", Age = 62 },
                new { Name = "Eric", Age = 27 },
                new { Name = "Lisa", Age = 14 },
                new { Name = "Brad", Age = 27 },
                new { Name = "Lisa", Age = 23 },
                new { Name = "Eric", Age = 42 },
            };

            var ys = xs.ToAsyncEnumerable();

            var res = ys.GroupBy(x => x.Age / 10);

            var e = res.GetEnumerator();

            Assert.True(e.MoveNext().Result);
            var g1 = e.Current;
            Assert.Equal(2, g1.Key);

            Assert.True(e.MoveNext().Result);
            var g2 = e.Current;
            Assert.Equal(6, g2.Key);

            Assert.True(e.MoveNext().Result);
            var g3 = e.Current;
            Assert.Equal(1, g3.Key);

            Assert.True(e.MoveNext().Result);
            var g4 = e.Current;
            Assert.Equal(4, g4.Key);

            NoNext(e);

            var g1e = g1.GetEnumerator();
            HasNext(g1e, xs[0]);
            HasNext(g1e, xs[2]);
            HasNext(g1e, xs[4]);
            HasNext(g1e, xs[5]);
            NoNext(g1e);

            var g2e = g2.GetEnumerator();
            HasNext(g2e, xs[1]);
            NoNext(g2e);

            var g3e = g3.GetEnumerator();
            HasNext(g3e, xs[3]);
            NoNext(g3e);

            var g4e = g4.GetEnumerator();
            HasNext(g4e, xs[6]);
            NoNext(g4e);
        }

        [Fact]
        public void GroupBy3()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var ys = xs.GroupBy(x => x);

            var e = ys.GetEnumerator();
            NoNext(e);
        }

        [Fact]
        public void GroupBy4()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.GroupBy(x => x);

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void GroupBy5()
        {
            var xs = GetXs().ToAsyncEnumerable();
            var ys = xs.GroupBy(x => x);

            var e = ys.GetEnumerator();

            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single().Message == "Bang!");

            //Assert.True(e.MoveNext().Result);
            //var g1 = e.Current;
            //Assert.Equal(g1.Key, 42);
            //var g1e = g1.GetEnumerator();
            //HasNext(g1e, 42);

            //Assert.True(e.MoveNext().Result);
            //var g2 = e.Current;
            //Assert.Equal(g2.Key, 43);
            //var g2e = g2.GetEnumerator();
            //HasNext(g2e, 43);

            
            //AssertThrows<Exception>(() => g1e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single().Message == "Bang!");
            //AssertThrows<Exception>(() => g2e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single().Message == "Bang!");
        }

        [Fact]
        public void GroupBy6()
        {
            var xs = GetXs().ToAsyncEnumerable();
            var ys = xs.GroupBy(x => x);

            var e = ys.GetEnumerator();

            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single().Message == "Bang!");

            //Assert.True(e.MoveNext().Result);
            //var g1 = e.Current;
            //Assert.Equal(g1.Key, 42);
            //var g1e = g1.GetEnumerator();
            //HasNext(g1e, 42);
            //AssertThrows<Exception>(() => g1e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single().Message == "Bang!");

                    
        }

        static IEnumerable<int> GetXs()
        {
            yield return 42;
            yield return 43;
            throw new Exception("Bang!");
        }

        [Fact]
        public void GroupBy7()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Return(42);
            var ys = xs.GroupBy<int, int>(x => { throw ex; });

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void GroupBy8()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.GroupBy<int, int>(x => { if (x == 3) throw ex; return x; });

            var e = ys.GetEnumerator();

            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);

            //Assert.True(e.MoveNext().Result);
            //var g1 = e.Current;
            //Assert.Equal(g1.Key, 1);
            //var g1e = g1.GetEnumerator();
            //HasNext(g1e, 1);

            //Assert.True(e.MoveNext().Result);
            //var g2 = e.Current;
            //Assert.Equal(g2.Key, 2);
            //var g2e = g2.GetEnumerator();
            //HasNext(g2e, 2);

           
            //AssertThrows<Exception>(() => g1e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
            //AssertThrows<Exception>(() => g2e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void GroupBy9()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x));

            var e = ys.GetEnumerator();

            Assert.True(e.MoveNext().Result);
            var g1 = e.Current;
            Assert.Equal(0, g1.Key);
            var g1e = g1.GetEnumerator();
            HasNext(g1e, 'a');
            HasNext(g1e, 'd');
            HasNext(g1e, 'g');
            HasNext(g1e, 'j');
            NoNext(g1e);

            Assert.True(e.MoveNext().Result);
            var g2 = e.Current;
            Assert.Equal(1, g2.Key);
            var g2e = g2.GetEnumerator();
            HasNext(g2e, 'b');
            HasNext(g2e, 'e');
            HasNext(g2e, 'h');
            NoNext(g2e);

            Assert.True(e.MoveNext().Result);
            var g3 = e.Current;
            Assert.Equal(2, g3.Key);
            var g3e = g3.GetEnumerator();
            HasNext(g3e, 'c');
            HasNext(g3e, 'f');
            HasNext(g3e, 'i');
            NoNext(g3e);

            NoNext(e);
        }

        [Fact]
        public void GroupBy10()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x), (k, cs) => k + " - " + cs.Aggregate("", (a, c) => a + c).Result);

            var e = ys.GetEnumerator();
            HasNext(e, "0 - adgj");
            HasNext(e, "1 - beh");
            HasNext(e, "2 - cfi");
            NoNext(e);
        }

        [Fact]
        public void GroupBy11()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, (k, cs) => k + " - " + cs.Aggregate("", (a, c) => a + c).Result);

            var e = ys.GetEnumerator();
            HasNext(e, "0 - 0369");
            HasNext(e, "1 - 147");
            HasNext(e, "2 - 258");
            NoNext(e);
        }

        [Fact]
        public void GroupBy12()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, new EqMod(3));

            var e = ys.GetEnumerator();

            Assert.True(e.MoveNext().Result);
            var g1 = e.Current;
            Assert.Equal(0, g1.Key);
            var g1e = g1.GetEnumerator();
            HasNext(g1e, 0);
            HasNext(g1e, 3);
            HasNext(g1e, 6);
            HasNext(g1e, 9);
            NoNext(g1e);

            Assert.True(e.MoveNext().Result);
            var g2 = e.Current;
            Assert.Equal(1, g2.Key);
            var g2e = g2.GetEnumerator();
            HasNext(g2e, 1);
            HasNext(g2e, 4);
            HasNext(g2e, 7);
            NoNext(g2e);

            Assert.True(e.MoveNext().Result);
            var g3 = e.Current;
            Assert.Equal(2, g3.Key);
            var g3e = g3.GetEnumerator();
            HasNext(g3e, 2);
            HasNext(g3e, 5);
            HasNext(g3e, 8);
            NoNext(g3e);

            NoNext(e);
        }

        [Fact]
        public void GroupBy13()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, x => (char)('a' + x), new EqMod(3));

            var e = ys.GetEnumerator();

            Assert.True(e.MoveNext().Result);
            var g1 = e.Current;
            Assert.Equal(0, g1.Key);
            var g1e = g1.GetEnumerator();
            HasNext(g1e, 'a');
            HasNext(g1e, 'd');
            HasNext(g1e, 'g');
            HasNext(g1e, 'j');
            NoNext(g1e);

            Assert.True(e.MoveNext().Result);
            var g2 = e.Current;
            Assert.Equal(1, g2.Key);
            var g2e = g2.GetEnumerator();
            HasNext(g2e, 'b');
            HasNext(g2e, 'e');
            HasNext(g2e, 'h');
            NoNext(g2e);

            Assert.True(e.MoveNext().Result);
            var g3 = e.Current;
            Assert.Equal(2, g3.Key);
            var g3e = g3.GetEnumerator();
            HasNext(g3e, 'c');
            HasNext(g3e, 'f');
            HasNext(g3e, 'i');
            NoNext(g3e);

            NoNext(e);
        }

        [Fact]
        public void GroupBy14()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, x => (char)('a' + x), (k, cs) => k + " - " + cs.Aggregate("", (a, c) => a + c).Result, new EqMod(3));

            var e = ys.GetEnumerator();
            HasNext(e, "0 - adgj");
            HasNext(e, "1 - beh");
            HasNext(e, "2 - cfi");
            NoNext(e);
        }

        [Fact]
        public void GroupBy15()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, (k, cs) => k + " - " + cs.Aggregate("", (a, c) => a + c).Result, new EqMod(3));

            var e = ys.GetEnumerator();
            HasNext(e, "0 - 0369");
            HasNext(e, "1 - 147");
            HasNext(e, "2 - 258");
            NoNext(e);
        }

        [Fact]
        public void GroupBy16()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, x => (char)('a' + x), new EqMod(3));

            var e = ys.GetEnumerator();

            Assert.True(e.MoveNext().Result);
            var g1 = e.Current;
            Assert.Equal(0, g1.Key);
            var g1e = g1.GetEnumerator();
            HasNext(g1e, 'a');
            HasNext(g1e, 'd');
            HasNext(g1e, 'g');
            HasNext(g1e, 'j');
            NoNext(g1e);
            g1e.Dispose();

            Assert.True(e.MoveNext().Result);
            var g2 = e.Current;
            Assert.Equal(1, g2.Key);
            var g2e = g2.GetEnumerator();
            HasNext(g2e, 'b');
            HasNext(g2e, 'e');
            HasNext(g2e, 'h');
            NoNext(g2e);
            g2e.Dispose();

            Assert.True(e.MoveNext().Result);
            var g3 = e.Current;
            Assert.Equal(2, g3.Key);
            var g3e = g3.GetEnumerator();
            HasNext(g3e, 'c');
            HasNext(g3e, 'f');
            HasNext(g3e, 'i');
            NoNext(g3e);
            g3e.Dispose();

            NoNext(e);

            e.Dispose();
        }

        [Fact]
        public void GroupBy17()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, x => (char)('a' + x), new EqMod(3));

            var e = ys.GetEnumerator();
            e.Dispose();

            Assert.False(e.MoveNext().Result);
        }

        [Fact]
        public void GroupBy18()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, x => (char)('a' + x), new EqMod(3));

            var e = ys.GetEnumerator();

            Assert.True(e.MoveNext().Result);
            var g1 = e.Current;
            Assert.Equal(0, g1.Key);
            var g1e = g1.GetEnumerator();
            HasNext(g1e, 'a');

            e.Dispose();

            HasNext(g1e, 'd');
            HasNext(g1e, 'g');
            HasNext(g1e, 'j');
            NoNext(g1e);
            g1e.Dispose();

            Assert.False(e.MoveNext().Result);
        }

        [Fact]
        public async Task GroupBy19()
        {
            // We're using Kvp here because the types will eval as equal for this test
            var xs = new[]
            {
                new Kvp("Bart", 27),
                new Kvp("John", 62),
                new Kvp("Eric", 27),
                new Kvp("Lisa", 14),
                new Kvp("Brad", 27),
                new Kvp("Lisa", 23),
                new Kvp("Eric", 42)
            };

            var ys = xs.ToAsyncEnumerable();

            var res = ys.GroupBy(x => x.Item/10);

            await SequenceIdentity(res);
        }


        [Fact]
        public async Task GroupBy20()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x), (k, cs) => k + " - " + cs.Aggregate("", (a, c) => a + c).Result);

            var arr = new[] { "0 - adgj", "1 - beh", "2 - cfi" };

            Assert.Equal(arr, await ys.ToArray());
        }

        [Fact]
        public async Task GroupBy21()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x), (k, cs) => k + " - " + cs.Aggregate("", (a, c) => a + c).Result);

            var arr = new List<string> { "0 - adgj", "1 - beh", "2 - cfi" };

            Assert.Equal(arr, await ys.ToList());
        }

        [Fact]
        public async Task GroupBy22()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x), (k, cs) => k + " - " + cs.Aggregate("", (a, c) => a + c).Result);
            
            Assert.Equal(3, await ys.Count());
        }

        [Fact]
        public async Task GroupBy23()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x), (k, cs) => k + " - " + cs.Aggregate("", (a, c) => a + c).Result);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task GroupBy24()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x));

            var g1a = new[] { 'a', 'd', 'g', 'j' };
            var g2a = new[] { 'b', 'e', 'h' };
            var g3a = new[] { 'c', 'f', 'i' };

            var gar = await ys.ToArray();

            Assert.Equal(3, gar.Length);

            var gg1 = gar[0];
            var gg1a = await gg1.ToArray();
            
            Assert.Equal(g1a, gg1a);

            var gg2 = gar[1];
            var gg2a = await gg2.ToArray();

            Assert.Equal(g2a, gg2a);

            var gg3 = gar[2];
            var gg3a = await gg3.ToArray();
            Assert.Equal(g3a, gg3a); 
        }

        [Fact]
        public async Task GroupBy25()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x));

            var g1a = new List<char> { 'a', 'd', 'g', 'j' };
            var g2a = new List<char> { 'b', 'e', 'h' };
            var g3a = new List<char> { 'c', 'f', 'i' };

            var gar = await ys.ToList();

            Assert.Equal(3, gar.Count);

            var gg1 = gar[0];
            var gg1a = await gg1.ToList();
            Assert.Equal(g1a, gg1a);

            var gg2 = gar[1];
            var gg2a = await gg2.ToList();
            Assert.Equal(g2a, gg2a);

            var gg3 = gar[2];
            var gg3a = await gg3.ToList();
            Assert.Equal(g3a, gg3a);
        }

        [Fact]
        public async Task GroupBy26()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x));
            
            var gar = await ys.ToList();

            Assert.Equal(3, gar.Count);

            var gg1 = gar[0];
            var gg1a = await gg1.Count();
            Assert.Equal(4, gg1a);

            var gg2 = gar[1];
            var gg2a = await gg2.Count();
            Assert.Equal(3, gg2a);

            var gg3 = gar[2];
            var gg3a = await gg3.Count();
            Assert.Equal(3, gg3a);
        }

        [Fact]
        public async Task GroupBy27()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x));

            var gar = await ys.Count();

            Assert.Equal(3, gar);
        }

        [Fact]
        public async Task GroupBy28()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x % 3, x => (char)('a' + x));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task GroupBy29()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, new EqMod(3));

            var g1a = new List<int> { 0, 3, 6, 9 };
            var g2a = new List<int> { 1, 4, 7 };
            var g3a = new List<int> { 2, 5, 8 };

            var gar = await ys.ToList();

            Assert.Equal(3, gar.Count);

            var gg1 = gar[0];
            var gg1a = await gg1.ToList();
            Assert.Equal(g1a, gg1a);

            var gg2 = gar[1];
            var gg2a = await gg2.ToList();
            Assert.Equal(g2a, gg2a);

            var gg3 = gar[2];
            var gg3a = await gg3.ToList();
            Assert.Equal(g3a, gg3a);
        }

        [Fact]
        public async Task GroupBy30()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, new EqMod(3));


            var gar = await ys.ToList();

            Assert.Equal(3, gar.Count);

            var gg1 = gar[0];
            var gg1a = await gg1.Count();
            Assert.Equal(4, gg1a);

            var gg2 = gar[1];
            var gg2a = await gg2.Count();
            Assert.Equal(3, gg2a);

            var gg3 = gar[2];
            var gg3a = await gg3.Count();
            Assert.Equal(3, gg3a);
        }

        [Fact]
        public async Task GroupBy31()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, new EqMod(3));

            var g1a = new[] { 0, 3, 6, 9 };
            var g2a = new[] { 1, 4, 7 };
            var g3a = new[] { 2, 5, 8 };

            var gar = await ys.ToArray();

            Assert.Equal(3, gar.Length);

            var gg1 = gar[0];
            var gg1a = await gg1.ToArray();

            Assert.Equal(g1a, gg1a);

            var gg2 = gar[1];
            var gg2a = await gg2.ToArray();

            Assert.Equal(g2a, gg2a);

            var gg3 = gar[2];
            var gg3a = await gg3.ToArray();
            Assert.Equal(g3a, gg3a);
        }

        [Fact]
        public async Task GroupBy32()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, new EqMod(3));

            var gar = await ys.Count();

            Assert.Equal(3, gar);
        }

        [Fact]
        public async Task GroupBy33()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, new EqMod(3));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task GroupBy34()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, (k, cs) => k + " - " + cs.Aggregate("", (a, c) => a + c).Result, new EqMod(3));

            var arr = new[] { "0 - 0369", "1 - 147", "2 - 258" };

            Assert.Equal(arr, await ys.ToArray());
        }

        [Fact]
        public async Task GroupBy35()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, (k, cs) => k + " - " + cs.Aggregate("", (a, c) => a + c).Result, new EqMod(3));

            var arr = new List<string> { "0 - 0369", "1 - 147", "2 - 258" };

            Assert.Equal(arr, await ys.ToList());
        }

        [Fact]
        public async Task GroupBy36()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, (k, cs) => k + " - " + cs.Aggregate("", (a, c) => a + c).Result, new EqMod(3));

            Assert.Equal(3, await ys.Count());
        }

        [Fact]
        public async Task GroupBy37()
        {
            var xs = AsyncEnumerable.Range(0, 10);
            var ys = xs.GroupBy(x => x, (k, cs) => k + " - " + cs.Aggregate("", (a, c) => a + c).Result, new EqMod(3));

            await SequenceIdentity(ys);
        }

        class Kvp : IEquatable<Kvp>
        {
            public bool Equals(Kvp other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Key, other.Key) && Item == other.Item;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Kvp)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Key != null ? Key.GetHashCode() : 0)*397) ^ Item;
                }
            }

            public static bool operator ==(Kvp left, Kvp right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(Kvp left, Kvp right)
            {
                return !Equals(left, right);
            }


            public string Key { get; }
            public int Item { get; }

            public Kvp(string key, int item)
            {
                Key = key;
                Item = item;
            }
        }


        class EqMod : IEqualityComparer<int>
        {
            private readonly int _d;

            public EqMod(int d)
            {
                _d = d;
            }

            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(x % _d, y % _d);
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(obj % _d);
            }
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

            var e = xs.GetEnumerator();
            HasNext(e, 42);
            NoNext(e);
        }

        [Fact]
        public void RepeatSeq_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Repeat(default(IAsyncEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Repeat(default(IAsyncEnumerable<int>), 3));
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.Repeat(AsyncEnumerable.Return(42), -1));
        }

        [Fact]
        public void RepeatSeq1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Repeat();

            var e = xs.GetEnumerator();
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

            var e = xs.GetEnumerator();
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

            var e = xs.GetEnumerator();
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

            var e = xs.GetEnumerator();

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

            var e = xs.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is NotImplementedException);
        }

        [Fact]
        public void RepeatSeq5()
        {
            var xs = new FailRepeat().ToAsyncEnumerable().Repeat(3);

            var e = xs.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is NotImplementedException);
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
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.IgnoreElements(default(IAsyncEnumerable<int>)));
        }

        [Fact]
        public void IgnoreElements1()
        {
            var xs = AsyncEnumerable.Empty<int>().IgnoreElements();

            var e = xs.GetEnumerator();
            NoNext(e);

            AssertThrows<InvalidOperationException>(() => { var ignored = e.Current; });
        }

        [Fact]
        public void IgnoreElements2()
        {
            var xs = AsyncEnumerable.Return(42).IgnoreElements();

            var e = xs.GetEnumerator();
            NoNext(e);

            AssertThrows<InvalidOperationException>(() => { var ignored = e.Current; });
        }

        [Fact]
        public void IgnoreElements3()
        {
            var xs = AsyncEnumerable.Range(0, 10).IgnoreElements();

            var e = xs.GetEnumerator();
            NoNext(e);

            AssertThrows<InvalidOperationException>(() => { var ignored = e.Current; });
        }

        [Fact]
        public void IgnoreElements4()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex).IgnoreElements();

            var e = xs.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
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
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.StartWith(default(IAsyncEnumerable<int>), new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.StartWith(AsyncEnumerable.Return(42), null));
        }

        [Fact]
        public void StartWith1()
        {
            var xs = AsyncEnumerable.Empty<int>().StartWith(1, 2);

            var e = xs.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void StartWith2()
        {
            var xs = AsyncEnumerable.Return<int>(0).StartWith(1, 2);

            var e = xs.GetEnumerator();
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

            var e = xs.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Buffer_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Buffer(default(IAsyncEnumerable<int>), 1));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Buffer(default(IAsyncEnumerable<int>), 1, 1));

            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.Buffer(AsyncEnumerable.Return(42), -1));
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.Buffer(AsyncEnumerable.Return(42), -1, 1));
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.Buffer(AsyncEnumerable.Return(42), 1, -1));
        }

        [Fact]
        public void Buffer1()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Buffer(2);

            var e = xs.GetEnumerator();

            Assert.True(e.MoveNext().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 1, 2 }));

            Assert.True(e.MoveNext().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 3, 4 }));

            Assert.True(e.MoveNext().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 5 }));

            Assert.False(e.MoveNext().Result);
        }

        [Fact]
        public void Buffer2()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Buffer(3, 2);

            var e = xs.GetEnumerator();

            Assert.True(e.MoveNext().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 1, 2, 3 }));

            Assert.True(e.MoveNext().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 3, 4, 5 }));

            Assert.True(e.MoveNext().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 5 }));

            Assert.False(e.MoveNext().Result);
        }

        [Fact]
        public void Buffer3()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Buffer(2, 3);

            var e = xs.GetEnumerator();

            Assert.True(e.MoveNext().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 1, 2 }));

            Assert.True(e.MoveNext().Result);
            Assert.True(e.Current.SequenceEqual(new[] { 4, 5 }));

            Assert.False(e.MoveNext().Result);
        }

        [Fact]
        public async Task Buffer4()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Buffer(3, 2);

            await SequenceIdentity(xs);
        }

        [Fact]
        public void DistinctUntilChanged_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.DistinctUntilChanged(default(IAsyncEnumerable<int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.DistinctUntilChanged(default(IAsyncEnumerable<int>), EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.DistinctUntilChanged(AsyncEnumerable.Return(42), default(IEqualityComparer<int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.DistinctUntilChanged(default(IAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.DistinctUntilChanged(AsyncEnumerable.Return(42), default(Func<int, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.DistinctUntilChanged(default(IAsyncEnumerable<int>), x => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.DistinctUntilChanged(AsyncEnumerable.Return(42), default(Func<int, int>), EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.DistinctUntilChanged(AsyncEnumerable.Return(42), x => x, default(IEqualityComparer<int>)));
        }

        [Fact]
        public void DistinctUntilChanged1()
        {
            var xs = new[] { 1, 2, 2, 3, 4, 4, 4, 4, 5, 6, 6, 7, 3, 2, 2, 1, 1 }.ToAsyncEnumerable().DistinctUntilChanged();

            var e = xs.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            HasNext(e, 7);
            HasNext(e, 3);
            HasNext(e, 2);
            HasNext(e, 1);
            NoNext(e);
        }

        [Fact]
        public void DistinctUntilChanged2()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 5, 2 }.ToAsyncEnumerable().DistinctUntilChanged(x => (x + 1) / 2);

            var e = xs.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 3);
            HasNext(e, 5);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void DistinctUntilChanged3()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4, 3, 5, 2 }.ToAsyncEnumerable().DistinctUntilChanged(x => { if (x == 4) throw ex; return x; });

            var e = xs.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task DistinctUntilChanged4()
        {
            var xs = new[] { 1, 2, 2, 3, 4, 4, 4, 4, 5, 6, 6, 7, 3, 2, 2, 1, 1 }.ToAsyncEnumerable().DistinctUntilChanged();

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task DistinctUntilChanged5()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 5, 2 }.ToAsyncEnumerable().DistinctUntilChanged(x => (x + 1) / 2);

            await SequenceIdentity(xs);
        }

        [Fact]
        public void Expand_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Expand(default(IAsyncEnumerable<int>), x => null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Expand(AsyncEnumerable.Return(42), null));
        }

        [Fact]
        public void Expand1()
        {
            var xs = new[] { 2, 3 }.ToAsyncEnumerable().Expand(x => AsyncEnumerable.Return(x - 1).Repeat(x - 1));

            var e = xs.GetEnumerator();
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 2);
            HasNext(e, 1);
            HasNext(e, 1);
            NoNext(e);
        }

        [Fact]
        public void Expand2()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 2, 3 }.ToAsyncEnumerable().Expand(x => { throw ex; });

            var e = xs.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Expand3()
        {
            var xs = new[] { 2, 3 }.ToAsyncEnumerable().Expand(x => null);

            var e = xs.GetEnumerator();
            HasNext(e, 2);
            HasNext(e, 3);
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is NullReferenceException);
        }

        [Fact]
        public async Task Expand4()
        {
            var xs = new[] { 2, 3 }.ToAsyncEnumerable().Expand(x => AsyncEnumerable.Return(x - 1).Repeat(x - 1));

            await SequenceIdentity(xs);
        }

        [Fact]
        public void Scan_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Scan(default(IAsyncEnumerable<int>), 3, (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Scan(AsyncEnumerable.Return(42), 3, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Scan(default(IAsyncEnumerable<int>), (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Scan(AsyncEnumerable.Return(42), null));
        }

        [Fact]
        public void Scan1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan(8, (x, y) => x + y);

            var e = xs.GetEnumerator();
            HasNext(e, 9);
            HasNext(e, 11);
            HasNext(e, 14);
            NoNext(e);
        }

        [Fact]
        public void Scan2()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan((x, y) => x + y);

            var e = xs.GetEnumerator();
            HasNext(e, 3);
            HasNext(e, 6);
            NoNext(e);
        }

        [Fact]
        public void Scan3()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan(8, (x, y) => { throw ex; });

            var e = xs.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void Scan4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan((x, y) => { throw ex; });

            var e = xs.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public async Task Scan5()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan(8, (x, y) => x + y);

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task Scan6()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Scan((x, y) => x + y);

            await SequenceIdentity(xs);
        }

        [Fact]
        public void DistinctKey_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Distinct(default(IAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Distinct(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Distinct(default(IAsyncEnumerable<int>), x => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Distinct(AsyncEnumerable.Return(42), null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Distinct(AsyncEnumerable.Return(42), x => x, null));
        }

        [Fact]
        public void DistinctKey1()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Distinct(x => x / 2);

            var e = xs.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void TakeLast_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.TakeLast(default(IAsyncEnumerable<int>), 5));
        }

        [Fact]
        public void TakeLast0()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(-2);

            var e = xs.GetEnumerator();
            NoNext(e);
        }

        [Fact]
        public void TakeLast1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(2);

            var e = xs.GetEnumerator();
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public void TakeLast2()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(5);

            var e = xs.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }

        [Fact]
        public async Task TakeLast3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(2);

            await SequenceIdentity(xs);
        }

        [Fact]
        public void TakeLast_BugFix_TakeLast_Zero_TakesForever()
        {
            var isSet = false;
            new int[] { 1, 2, 3, 4 }.ToAsyncEnumerable()
                .TakeLast(0)
                .ForEachAsync(_ => { isSet = true; })
                .Wait(WaitTimeoutMs);

            Assert.False(isSet);

            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(0);

            var e = xs.GetEnumerator();
            
            NoNext(e);
        }

        [Fact]
        public void SkipLast_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SkipLast(default(IAsyncEnumerable<int>), 5));
        }

        [Fact]
        public void SkipLast1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().SkipLast(2);

            var e = xs.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);
        }

        [Fact]
        public void SkipLast2()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().SkipLast(5);

            var e = xs.GetEnumerator();
            NoNext(e);
        }

        [Fact]
        public async Task SkipLast3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().SkipLast(2);

            await SequenceIdentity(xs);
        }

        [Fact]
        public void SkipLast4()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().SkipLast(0);

            var e = xs.GetEnumerator();

            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }
    }
}
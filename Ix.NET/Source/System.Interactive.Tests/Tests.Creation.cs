// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Collections;
using System.Threading;

namespace Tests
{
    public partial class Tests
    {
        [Fact]
        public void Create_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Create<int>(default(Func<IEnumerator<int>>)));
        }

        [Fact]
        public void Create1()
        {
            var hot = false;
            var res = EnumerableEx.Create<int>(() =>
            {
                hot = true;
                return MyEnumerator();
            });

            Assert.False(hot);

            var e = res.GetEnumerator();
            Assert.True(hot);

            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);

            hot = false;
            var f = ((IEnumerable)res).GetEnumerator();
            Assert.True(hot);
        }

        [Fact]
        public void CreateYield()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var xs = EnumerableEx.Create<int>(async yield => {
                var i = 0;
                while (i < 10)
                {
                    await yield.Return(i++);
                }
            });

            var j = 0;
            foreach (var elem in xs)
            {
                Assert.Equal(j, elem);
                j++;
            }

            Assert.Equal(10, j);
        }

        [Fact]
        public void CreateYieldBreak()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var xs = EnumerableEx.Create<int>(async yield => {
                var i = 0;
                while (true)
                {
                    if (i == 10)
                    {
                        await yield.Break();
                        return;
                    }

                    await yield.Return(i++);
                }
            });

            var j = 0;
            foreach (var elem in xs)
            {
                Assert.Equal(elem, j);
                j++;
            }

            Assert.Equal(10, j);
        }

        [Fact]
        public void YielderNoReset()
        {
            var xs = EnumerableEx.Create<int>(async yield => {
               await yield.Break();
            });

            AssertThrows<NotSupportedException>(() => xs.GetEnumerator().Reset());
        }


        private static IEnumerator<int> MyEnumerator()
        {
            yield return 1;
            yield return 2;
        }

        [Fact]
        public void Return()
        {
            Assert.Equal(42, EnumerableEx.Return(42).Single());
        }

        [Fact]
        public void Throw_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Throw<int>(null));
        }

        [Fact]
        public void Throw()
        {
            var ex = new MyException();
            var xs = EnumerableEx.Throw<int>(ex);

            var e = xs.GetEnumerator();
            AssertThrows<MyException>(() => e.MoveNext());
        }

        [Fact]
        public void Defer_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Defer<int>(null));
        }

        [Fact]
        public void Defer1()
        {
            var i = 0;
            var n = 5;
            var xs = EnumerableEx.Defer(() =>
            {
                i++;
                return Enumerable.Range(0, n);
            });

            Assert.Equal(0, i);

            Assert.True(Enumerable.SequenceEqual(xs, Enumerable.Range(0, n)));
            Assert.Equal(1, i);

            n = 3;
            Assert.True(Enumerable.SequenceEqual(xs, Enumerable.Range(0, n)));
            Assert.Equal(2, i);
        }

        [Fact]
        public void Defer2()
        {
            var xs = EnumerableEx.Defer<int>(() =>
            {
                throw new MyException();
            });

            AssertThrows<MyException>(() => xs.GetEnumerator().MoveNext());
        }

        [Fact]
        public void Generate_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Generate<int, int>(0, null, _ => _, _ => _));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Generate<int, int>(0, _ => true, null, _ => _));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Generate<int, int>(0, _ => true, _ => _, null));
        }

        [Fact]
        public void Generate()
        {
            var res = EnumerableEx.Generate(0, x => x < 5, x => x + 1, x => x * x).ToList();
            Assert.True(Enumerable.SequenceEqual(res, new[] { 0, 1, 4, 9, 16 }));
        }

        [Fact]
        public void Using_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Using<int, MyDisposable>(null, d => new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Using<int, MyDisposable>(() => new MyDisposable(), null));
        }

        [Fact]
        public void Using1()
        {
            var d = default(MyDisposable);

            var xs = EnumerableEx.Using(() => d = new MyDisposable(), d_ => new[] { 1 });
            Assert.Null(d);

            var d1 = default(MyDisposable);
            xs.ForEach(_ => { d1 = d; Assert.NotNull(d1); Assert.False(d1.Done); });
            Assert.True(d1.Done);

            var d2 = default(MyDisposable);
            xs.ForEach(_ => { d2 = d; Assert.NotNull(d2); Assert.False(d2.Done); });
            Assert.True(d2.Done);

            Assert.NotSame(d1, d2);
        }

        [Fact]
        public void Using2()
        {
            var d = default(MyDisposable);

            var xs = EnumerableEx.Using(() => d = new MyDisposable(), d_ => EnumerableEx.Throw<int>(new MyException()));
            Assert.Null(d);

            AssertThrows<MyException>(() => xs.ForEach(_ => { }));
            Assert.True(d.Done);
        }

        [Fact]
        public void Using3()
        {
            var d = default(MyDisposable);

            var xs = EnumerableEx.Using<int, MyDisposable>(() => d = new MyDisposable(), d_ => { throw new MyException(); });
            Assert.Null(d);

            AssertThrows<MyException>(() => xs.ForEach(_ => { }));
            Assert.True(d.Done);
        }

        class MyDisposable : IDisposable
        {
            public bool Done;

            public void Dispose()
            {
                Done = true;
            }
        }

        [Fact]
        public void RepeatElementInfinite()
        {
            var xs = EnumerableEx.Repeat(42).Take(1000);
            Assert.True(xs.All(x => x == 42));
            Assert.True(xs.Count() == 1000);
        }

        [Fact]
        public void RepeatSequence_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Repeat<int>(null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Repeat<int>(null, 5));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Repeat<int>(new[] { 1 }, -1));
        }

        [Fact]
        public void RepeatSequence1()
        {
            var i = 0;
            var xs = new[] { 1, 2 }.Do(_ => i++).Repeat();

            var res = xs.Take(10).ToList();
            Assert.Equal(10, res.Count);
            Assert.True(res.Buffer(2).Select(b => b.Sum()).All(x => x == 3));
            Assert.Equal(10, i);
        }

        [Fact]
        public void RepeatSequence2()
        {
            var i = 0;
            var xs = new[] { 1, 2 }.Do(_ => i++).Repeat(5);

            var res = xs.ToList();
            Assert.Equal(10, res.Count);
            Assert.True(res.Buffer(2).Select(b => b.Sum()).All(x => x == 3));
            Assert.Equal(10, i);
        }
    }
}

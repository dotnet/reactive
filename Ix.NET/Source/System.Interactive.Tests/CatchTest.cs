// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace Tests
{
    public class CatchTest : Tests
    {
        [Fact]
        public void Catch_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Catch<int>(null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Catch<int>(new[] { 1 }, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Catch<int>(default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Catch<int>(default(IEnumerable<IEnumerable<int>>)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Catch<int, Exception>(null, ex => new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Catch<int, Exception>(new[] { 1 }, null));
        }

        [Fact]
        public void Catch1()
        {
            var ex = new MyException();
            var res = EnumerableEx.Throw<int>(ex).Catch<int, MyException>(e => { Assert.Same(ex, e); return new[] { 42 }; }).Single();
            Assert.Equal(42, res);
        }

        [Fact]
        public void Catch2()
        {
            var ex = new MyException();
            var res = EnumerableEx.Throw<int>(ex).Catch<int, Exception>(e => { Assert.Same(ex, e); return new[] { 42 }; }).Single();
            Assert.Equal(42, res);
        }

        [Fact]
        public void Catch3()
        {
            var ex = new MyException();
            AssertThrows<MyException>(() =>
            {
                EnumerableEx.Throw<int>(ex).Catch<int, InvalidOperationException>(e => { Assert.True(false); return new[] { 42 }; }).Single();
            });
        }

        [Fact]
        public void Catch4()
        {
            var xs = Enumerable.Range(0, 10);
            var res = xs.Catch<int, MyException>(e => { Assert.True(false); return new[] { 42 }; });
            Assert.True(xs.SequenceEqual(res));
        }

        [Fact]
        public void Catch5()
        {
            var xss = new[] { Enumerable.Range(0, 5), Enumerable.Range(5, 5) };
            var res = EnumerableEx.Catch(xss);
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 5)));
        }

        [Fact]
        public void Catch6()
        {
            var xss = new[] { Enumerable.Range(0, 5), Enumerable.Range(5, 5) };
            var res = xss.Catch();
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 5)));
        }

        [Fact]
        public void Catch7()
        {
            var xss = new[] { Enumerable.Range(0, 5), Enumerable.Range(5, 5) };
            var res = xss[0].Catch(xss[1]);
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 5)));
        }

        [Fact]
        public void Catch8()
        {
            var xss = new[] { Enumerable.Range(0, 5).Concat(EnumerableEx.Throw<int>(new MyException())), Enumerable.Range(5, 5) };
            var res = EnumerableEx.Catch(xss);
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void Catch9()
        {
            var xss = new[] { Enumerable.Range(0, 5).Concat(EnumerableEx.Throw<int>(new MyException())), Enumerable.Range(5, 5) };
            var res = xss.Catch();
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void Catch10()
        {
            var xss = new[] { Enumerable.Range(0, 5).Concat(EnumerableEx.Throw<int>(new MyException())), Enumerable.Range(5, 5) };
            var res = xss[0].Catch(xss[1]);
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void Catch11()
        {
            var e1 = new MyException();
            var ex1 = EnumerableEx.Throw<int>(e1);

            var e2 = new MyException();
            var ex2 = EnumerableEx.Throw<int>(e2);

            var e3 = new MyException();
            var ex3 = EnumerableEx.Throw<int>(e3);

            var xss = new[] { Enumerable.Range(0, 2).Concat(ex1), Enumerable.Range(2, 2).Concat(ex2), ex3 };
            var res = xss.Catch();

            var e = res.GetEnumerator();
            HasNext(e, 0);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            AssertThrows<MyException>(() => e.MoveNext(), ex => ex == e3);
        }

        [Fact]
        public void Catch4_Array()
        {
            var xs = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var res = xs.Catch<int, MyException>(e => { Assert.False(true); return new[] { 42 }; });
            Assert.True(xs.SequenceEqual(res));
        }

        [Fact]
        public void Catch5_Array()
        {
            var xss = new[] { new[] { 0, 1, 2, 3, 4 }, new[] { 5, 6, 7, 8, 9 } };
            var res = EnumerableEx.Catch(xss);
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 5)));
        }

        [Fact]
        public void Catch6_Array()
        {
            var xss = new[] { new[] { 0, 1, 2, 3, 4 }, new[] { 5, 6, 7, 8, 9 } };
            var res = xss.Catch();
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 5)));
        }

        [Fact]
        public void Catch7_Array()
        {
            var xss = new[] { new[] { 0, 1, 2, 3, 4 }, new[] { 5, 6, 7, 8, 9 } };
            var res = xss[0].Catch(xss[1]);
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 5)));
        }

        [Fact]
        public void Catch8_Array()
        {
            var xss = new[] { new[] { 0, 1, 2, 3, 4 }.Concat(EnumerableEx.Throw<int>(new MyException())), new[] { 5, 6, 7, 8, 9 } };
            var res = EnumerableEx.Catch(xss);
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void Catch9_Array()
        {
            var xss = new[] { new[] { 0, 1, 2, 3, 4 }.Concat(EnumerableEx.Throw<int>(new MyException())), new[] { 5, 6, 7, 8, 9 } };
            var res = xss.Catch();
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void Catch10_Array()
        {
            var xss = new[] { new[] { 0, 1, 2, 3, 4 }.Concat(EnumerableEx.Throw<int>(new MyException())), new[] { 5, 6, 7, 8, 9 } };
            var res = xss[0].Catch(xss[1]);
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void Catch11_Array()
        {
            var e1 = new MyException();
            var ex1 = EnumerableEx.Throw<int>(e1);

            var e2 = new MyException();
            var ex2 = EnumerableEx.Throw<int>(e2);

            var e3 = new MyException();
            var ex3 = EnumerableEx.Throw<int>(e3);

            var xss = new[] { new[] { 0, 1 }.Concat(ex1), new[] { 2, 3 }.Concat(ex2), ex3 };
            var res = xss.Catch();

            var e = res.GetEnumerator();
            HasNext(e, 0);
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            AssertThrows<MyException>(() => e.MoveNext(), ex => ex == e3);
        }
    }
}

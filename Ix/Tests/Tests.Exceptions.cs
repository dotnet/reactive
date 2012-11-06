// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    public partial class Tests
    {
        [TestMethod]
        public void Catch_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Catch<int>(null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Catch<int>(new[] { 1 }, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Catch<int>(default(IEnumerable<int>[])));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Catch<int>(default(IEnumerable<IEnumerable<int>>)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Catch<int, Exception>(null, ex => new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Catch<int, Exception>(new[] { 1 }, null));
        }

        [TestMethod]
        public void Catch1()
        {
            var ex = new MyException();
            var res = EnumerableEx.Throw<int>(ex).Catch<int, MyException>(e => { Assert.AreSame(ex, e); return new[] { 42 }; }).Single();
            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void Catch2()
        {
            var ex = new MyException();
            var res = EnumerableEx.Throw<int>(ex).Catch<int, Exception>(e => { Assert.AreSame(ex, e); return new[] { 42 }; }).Single();
            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void Catch3()
        {
            var ex = new MyException();
            AssertThrows<MyException>(() =>
            {
                EnumerableEx.Throw<int>(ex).Catch<int, InvalidOperationException>(e => { Assert.Fail(); return new[] { 42 }; }).Single();
            });
        }

        [TestMethod]
        public void Catch4()
        {
            var xs = Enumerable.Range(0, 10);
            var res = xs.Catch<int, MyException>(e => { Assert.Fail(); return new[] { 42 }; });
            Assert.IsTrue(xs.SequenceEqual(res));
        }

        [TestMethod]
        public void Catch5()
        {
            var xss = new[] { Enumerable.Range(0, 5), Enumerable.Range(5, 5) };
            var res = EnumerableEx.Catch(xss);
            Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 5)));
        }

        [TestMethod]
        public void Catch6()
        {
            var xss = new[] { Enumerable.Range(0, 5), Enumerable.Range(5, 5) };
            var res = xss.Catch();
            Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 5)));
        }

        [TestMethod]
        public void Catch7()
        {
            var xss = new[] { Enumerable.Range(0, 5), Enumerable.Range(5, 5) };
            var res = xss[0].Catch(xss[1]);
            Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 5)));
        }

        [TestMethod]
        public void Catch8()
        {
            var xss = new[] { Enumerable.Range(0, 5).Concat(EnumerableEx.Throw<int>(new MyException())), Enumerable.Range(5, 5) };
            var res = EnumerableEx.Catch(xss);
            Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [TestMethod]
        public void Catch9()
        {
            var xss = new[] { Enumerable.Range(0, 5).Concat(EnumerableEx.Throw<int>(new MyException())), Enumerable.Range(5, 5) };
            var res = xss.Catch();
            Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [TestMethod]
        public void Catch10()
        {
            var xss = new[] { Enumerable.Range(0, 5).Concat(EnumerableEx.Throw<int>(new MyException())), Enumerable.Range(5, 5) };
            var res = xss[0].Catch(xss[1]);
            Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [TestMethod]
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

        [TestMethod]
        public void Finally_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Finally<int>(null, () => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Finally<int>(new[] { 1 }, null));
        }

        [TestMethod]
        public void Finally1()
        {
            var done = false;

            var xs = Enumerable.Range(0, 2).Finally(() => done = true);
            Assert.IsFalse(done);

            var e = xs.GetEnumerator();
            Assert.IsFalse(done);

            HasNext(e, 0);
            Assert.IsFalse(done);

            HasNext(e, 1);
            Assert.IsFalse(done);

            NoNext(e);
            Assert.IsTrue(done);
        }

        [TestMethod]
        public void Finally2()
        {
            var done = false;

            var xs = Enumerable.Range(0, 2).Finally(() => done = true);
            Assert.IsFalse(done);

            var e = xs.GetEnumerator();
            Assert.IsFalse(done);

            HasNext(e, 0);
            Assert.IsFalse(done);

            e.Dispose();
            Assert.IsTrue(done);
        }

        [TestMethod]
        public void Finally3()
        {
            var done = false;

            var ex = new MyException();
            var xs = EnumerableEx.Throw<int>(ex).Finally(() => done = true);
            Assert.IsFalse(done);

            var e = xs.GetEnumerator();
            Assert.IsFalse(done);

            try
            {
                HasNext(e, 0);
                Assert.Fail();
            }
            catch (MyException ex_)
            {
                Assert.AreSame(ex, ex_);
            }

            Assert.IsTrue(done);
        }

        [TestMethod]
        public void OnErrorResumeNext_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.OnErrorResumeNext<int>(null, new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.OnErrorResumeNext<int>(new[] { 1 }, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.OnErrorResumeNext<int>(default(IEnumerable<int>[])));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.OnErrorResumeNext<int>(default(IEnumerable<IEnumerable<int>>)));
        }

        [TestMethod]
        public void OnErrorResumeNext1()
        {
            var xs = new[] { 1, 2 };
            var ys = new[] { 3, 4 };

            var res = xs.OnErrorResumeNext(ys);
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 1, 2, 3, 4 }));
        }

        [TestMethod]
        public void OnErrorResumeNext2()
        {
            var xs = new[] { 1, 2 }.Concat(EnumerableEx.Throw<int>(new MyException()));
            var ys = new[] { 3, 4 };

            var res = xs.OnErrorResumeNext(ys);
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 1, 2, 3, 4 }));
        }

        [TestMethod]
        public void OnErrorResumeNext3()
        {
            var xs = new[] { 1, 2 };
            var ys = new[] { 3, 4 };
            var zs = new[] { 5, 6 };

            var res = EnumerableEx.OnErrorResumeNext(xs, ys, zs);
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 1, 2, 3, 4, 5, 6 }));
        }

        [TestMethod]
        public void OnErrorResumeNext4()
        {
            var xs = new[] { 1, 2 }.Concat(EnumerableEx.Throw<int>(new MyException()));
            var ys = new[] { 3, 4 };
            var zs = new[] { 5, 6 };

            var res = EnumerableEx.OnErrorResumeNext(xs, ys, zs);
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 1, 2, 3, 4, 5, 6 }));
        }

        [TestMethod]
        public void OnErrorResumeNext5()
        {
            var xs = new[] { 1, 2 };
            var ys = new[] { 3, 4 };

            var res = new[] { xs, ys }.OnErrorResumeNext();
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 1, 2, 3, 4 }));
        }

        [TestMethod]
        public void OnErrorResumeNext6()
        {
            var xs = new[] { 1, 2 }.Concat(EnumerableEx.Throw<int>(new MyException()));
            var ys = new[] { 3, 4 };

            var res = new[] { xs, ys }.OnErrorResumeNext();
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 1, 2, 3, 4 }));
        }

        [TestMethod]
        public void Retry_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Retry<int>(null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Retry<int>(null, 5));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Retry<int>(new[] { 1 }, -1));
        }

        [TestMethod]
        public void Retry1()
        {
            var xs = Enumerable.Range(0, 10);

            var res = xs.Retry();
            Assert.IsTrue(Enumerable.SequenceEqual(res, xs));
        }

        [TestMethod]
        public void Retry2()
        {
            var xs = Enumerable.Range(0, 10);

            var res = xs.Retry(2);
            Assert.IsTrue(Enumerable.SequenceEqual(res, xs));
        }

        [TestMethod]
        public void Retry3()
        {
            var ex = new MyException();
            var xs = Enumerable.Range(0, 2).Concat(EnumerableEx.Throw<int>(ex));

            var res = xs.Retry(2);
            var e = res.GetEnumerator();
            HasNext(e, 0);
            HasNext(e, 1);
            HasNext(e, 0);
            HasNext(e, 1);
            AssertThrows<MyException>(() => e.MoveNext(), ex_ => ex == ex_);
        }
    }
}

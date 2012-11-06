// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace Tests
{
    public partial class Tests
    {
        [TestMethod]
        public void Create_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Create<int>(default(Func<IEnumerator<int>>)));
        }

        [TestMethod]
        public void Create1()
        {
            var hot = false;
            var res = EnumerableEx.Create<int>(() =>
            {
                hot = true;
                return MyEnumerator();
            });

            Assert.IsFalse(hot);

            var e = res.GetEnumerator();
            Assert.IsTrue(hot);

            HasNext(e, 1);
            HasNext(e, 2);
            NoNext(e);

            hot = false;
            var f = ((IEnumerable)res).GetEnumerator();
            Assert.IsTrue(hot);
        }

        private static IEnumerator<int> MyEnumerator()
        {
            yield return 1;
            yield return 2;
        }

        [TestMethod]
        public void Return()
        {
            Assert.AreEqual(42, EnumerableEx.Return(42).Single());
        }

        [TestMethod]
        public void Throw_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Throw<int>(null));
        }

        [TestMethod]
        public void Throw()
        {
            var ex = new MyException();
            var xs = EnumerableEx.Throw<int>(ex);

            var e = xs.GetEnumerator();
            AssertThrows<MyException>(() => e.MoveNext());
        }

        [TestMethod]
        public void Defer_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Defer<int>(null));
        }

        [TestMethod]
        public void Defer1()
        {
            var i = 0;
            var n = 5;
            var xs = EnumerableEx.Defer(() =>
            {
                i++;
                return Enumerable.Range(0, n);
            });

            Assert.AreEqual(0, i);

            Assert.IsTrue(Enumerable.SequenceEqual(xs, Enumerable.Range(0, n)));
            Assert.AreEqual(1, i);

            n = 3;
            Assert.IsTrue(Enumerable.SequenceEqual(xs, Enumerable.Range(0, n)));
            Assert.AreEqual(2, i);
        }

        [TestMethod]
        public void Defer2()
        {
            var xs = EnumerableEx.Defer<int>(() =>
            {
                throw new MyException();
            });

            AssertThrows<MyException>(() => xs.GetEnumerator().MoveNext());
        }

        [TestMethod]
        public void Generate_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Generate<int, int>(0, null, _ => _, _ => _));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Generate<int, int>(0, _ => true, null, _ => _));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Generate<int, int>(0, _ => true, _ => _, null));
        }

        [TestMethod]
        public void Generate()
        {
            var res = EnumerableEx.Generate(0, x => x < 5, x => x + 1, x => x * x).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 0, 1, 4, 9, 16 }));
        }

        [TestMethod]
        public void Using_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Using<int, MyDisposable>(null, d => new[] { 1 }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Using<int, MyDisposable>(() => new MyDisposable(), null));
        }

        [TestMethod]
        public void Using1()
        {
            var d = default(MyDisposable);

            var xs = EnumerableEx.Using(() => d = new MyDisposable(), d_ => new[] { 1 });
            Assert.IsNull(d);

            var d1 = default(MyDisposable);
            xs.ForEach(_ => { d1 = d; Assert.IsNotNull(d1); Assert.IsFalse(d1.Done); });
            Assert.IsTrue(d1.Done);

            var d2 = default(MyDisposable);
            xs.ForEach(_ => { d2 = d; Assert.IsNotNull(d2); Assert.IsFalse(d2.Done); });
            Assert.IsTrue(d2.Done);

            Assert.AreNotSame(d1, d2);
        }

        [TestMethod]
        public void Using2()
        {
            var d = default(MyDisposable);

            var xs = EnumerableEx.Using(() => d = new MyDisposable(), d_ => EnumerableEx.Throw<int>(new MyException()));
            Assert.IsNull(d);

            AssertThrows<MyException>(() => xs.ForEach(_ => { }));
            Assert.IsTrue(d.Done);
        }

        [TestMethod]
        public void Using3()
        {
            var d = default(MyDisposable);

            var xs = EnumerableEx.Using<int, MyDisposable>(() => d = new MyDisposable(), d_ => { throw new MyException(); });
            Assert.IsNull(d);

            AssertThrows<MyException>(() => xs.ForEach(_ => { }));
            Assert.IsTrue(d.Done);
        }

        class MyDisposable : IDisposable
        {
            public bool Done;

            public void Dispose()
            {
                Done = true;
            }
        }

        [TestMethod]
        public void RepeatElementInfinite()
        {
            var xs = EnumerableEx.Repeat(42).Take(1000);
            Assert.IsTrue(xs.All(x => x == 42));
            Assert.IsTrue(xs.Count() == 1000);
        }

        [TestMethod]
        public void RepeatSequence_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Repeat<int>(null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Repeat<int>(null, 5));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Repeat<int>(new[] { 1 }, -1));
        }

        [TestMethod]
        public void RepeatSequence1()
        {
            var i = 0;
            var xs = new[] { 1, 2 }.Do(_ => i++).Repeat();

            var res = xs.Take(10).ToList();
            Assert.AreEqual(10, res.Count);
            Assert.IsTrue(res.Buffer(2).Select(b => b.Sum()).All(x => x == 3));
            Assert.AreEqual(10, i);
        }

        [TestMethod]
        public void RepeatSequence2()
        {
            var i = 0;
            var xs = new[] { 1, 2 }.Do(_ => i++).Repeat(5);

            var res = xs.ToList();
            Assert.AreEqual(10, res.Count);
            Assert.IsTrue(res.Buffer(2).Select(b => b.Sum()).All(x => x == 3));
            Assert.AreEqual(10, i);
        }
    }
}

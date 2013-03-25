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
        public void Share_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Share<int>(null));
        }

        [TestMethod]
        public void Share1()
        {
            var rng = Enumerable.Range(0, 5).Share();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);
            HasNext(e1, 3);
            HasNext(e1, 4);
            NoNext(e1);
        }

        [TestMethod]
        public void Share2()
        {
            var rng = Enumerable.Range(0, 5).Share();

            var e1 = rng.GetEnumerator();
            var e2 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e2, 1);
            HasNext(e1, 2);
            HasNext(e2, 3);
            HasNext(e1, 4);
            NoNext(e2);
            NoNext(e1);
        }

        [TestMethod]
        public void Share3()
        {
            var rng = Enumerable.Range(0, 5).Share();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);

            var e2 = rng.GetEnumerator();
            HasNext(e2, 3);
            HasNext(e2, 4);
            NoNext(e2);
            NoNext(e1);
        }

        //[TestMethod]
        public void Share4()
        {
            var rng = Enumerable.Range(0, 5).Share();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);

            e1.Dispose();
            Assert.IsFalse(e1.MoveNext());
        }

        [TestMethod]
        public void Share5()
        {
            var rng = Enumerable.Range(0, 5).Share();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);

            rng.Dispose();
            AssertThrows<ObjectDisposedException>(() => e1.MoveNext());
            AssertThrows<ObjectDisposedException>(() => rng.GetEnumerator());
            AssertThrows<ObjectDisposedException>(() => ((IEnumerable)rng).GetEnumerator());
        }

        [TestMethod]
        public void Share6()
        {
            var rng = Enumerable.Range(0, 5).Share();

            var e1 = ((IEnumerable)rng).GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual(0, (int)e1.Current);
        }

        [TestMethod]
        public void Publish_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Publish<int>(null));
        }

        [TestMethod]
        public void Publish0()
        {
            var n = 0;
            var rng = Tick(i => n += i).Publish();

            var e1 = rng.GetEnumerator();
            var e2 = rng.GetEnumerator();

            HasNext(e1, 0);
            Assert.AreEqual(0, n);

            HasNext(e1, 1);
            Assert.AreEqual(1, n);

            HasNext(e1, 2);
            Assert.AreEqual(3, n);
            HasNext(e2, 0);
            Assert.AreEqual(3, n);

            HasNext(e1, 3);
            Assert.AreEqual(6, n);
            HasNext(e2, 1);
            Assert.AreEqual(6, n);

            HasNext(e2, 2);
            Assert.AreEqual(6, n);
            HasNext(e2, 3);
            Assert.AreEqual(6, n);

            HasNext(e2, 4);
            Assert.AreEqual(10, n);
            HasNext(e1, 4);
            Assert.AreEqual(10, n);
        }

        static IEnumerable<int> Tick(Action<int> t)
        {
            var i = 0;
            while (true)
            {
                t(i);
                yield return i++;
            }
        }

        [TestMethod]
        public void Publish1()
        {
            var rng = Enumerable.Range(0, 5).Publish();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);
            HasNext(e1, 3);
            HasNext(e1, 4);
            NoNext(e1);
        }

        [TestMethod]
        public void Publish2()
        {
            var rng = Enumerable.Range(0, 5).Publish();

            var e1 = rng.GetEnumerator();
            var e2 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e2, 0);
            HasNext(e1, 1);
            HasNext(e2, 1);
            HasNext(e1, 2);
            HasNext(e2, 2);
            HasNext(e1, 3);
            HasNext(e2, 3);
            HasNext(e1, 4);
            HasNext(e2, 4);
            NoNext(e1);
            NoNext(e2);
        }

        [TestMethod]
        public void Publish3()
        {
            var rng = Enumerable.Range(0, 5).Publish();

            var e1 = rng.GetEnumerator();
            var e2 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);
            HasNext(e1, 3);
            HasNext(e1, 4);
            HasNext(e2, 0);
            HasNext(e2, 1);
            HasNext(e2, 2);
            HasNext(e2, 3);
            HasNext(e2, 4);
            NoNext(e1);
            NoNext(e2);
        }

        [TestMethod]
        public void Publish4()
        {
            var rng = Enumerable.Range(0, 5).Publish();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);
            var e2 = rng.GetEnumerator();
            HasNext(e1, 3);
            HasNext(e1, 4);
            HasNext(e2, 3);
            HasNext(e2, 4);
            NoNext(e1);
            NoNext(e2);
        }

        [TestMethod]
        public void Publish5()
        {
            var rng = Enumerable.Range(0, 5).Publish();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);
            e1.Dispose();

            var e2 = rng.GetEnumerator();
            HasNext(e2, 3);
            HasNext(e2, 4);
            NoNext(e2);
        }

        [TestMethod]
        public void Publish6()
        {
            var ex = new MyException();
            var rng = Enumerable.Range(0, 2).Concat(EnumerableEx.Throw<int>(ex)).Publish();

            var e1 = rng.GetEnumerator();
            var e2 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            AssertThrows<MyException>(() => e1.MoveNext());

            HasNext(e2, 0);
            HasNext(e2, 1);
            AssertThrows<MyException>(() => e2.MoveNext());
        }

        class MyException : Exception
        {
        }

        [TestMethod]
        public void Publish7()
        {
            var rng = Enumerable.Range(0, 5).Publish();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);
            
            var e2 = rng.GetEnumerator();
            HasNext(e2, 3);
            HasNext(e2, 4);
            NoNext(e2);

            HasNext(e1, 3);
            HasNext(e1, 4);
            NoNext(e2);

            var e3 = rng.GetEnumerator();
            NoNext(e3);
        }

        [TestMethod]
        public void Publish8()
        {
            var rng = Enumerable.Range(0, 5).Publish();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);

            rng.Dispose();
            AssertThrows<ObjectDisposedException>(() => e1.MoveNext());
            AssertThrows<ObjectDisposedException>(() => rng.GetEnumerator());
            AssertThrows<ObjectDisposedException>(() => ((IEnumerable)rng).GetEnumerator());
        }

        [TestMethod]
        public void Publish9()
        {
            var rng = Enumerable.Range(0, 5).Publish();

            var e1 = ((IEnumerable)rng).GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual(0, (int)e1.Current);
        }

        [TestMethod]
        public void Publish10()
        {
            var rnd = Rand().Take(1000).Publish();
            Assert.IsTrue(rnd.Zip(rnd, (l, r) => l == r).All(x => x));
        }

        [TestMethod]
        public void Memoize_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Memoize<int>(null));
        }

        [TestMethod]
        public void MemoizeLimited_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Memoize<int>(null, 2));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Memoize<int>(new[] { 1 }, 0));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Memoize<int>(new[] { 1 }, -1));
        }

        [TestMethod]
        public void Memoize0()
        {
            var n = 0;
            var rng = Tick(i => n += i).Memoize();

            var e1 = rng.GetEnumerator();
            var e2 = rng.GetEnumerator();

            HasNext(e1, 0);
            Assert.AreEqual(0, n);

            HasNext(e1, 1);
            Assert.AreEqual(1, n);

            HasNext(e1, 2);
            Assert.AreEqual(3, n);
            HasNext(e2, 0);
            Assert.AreEqual(3, n);

            HasNext(e1, 3);
            Assert.AreEqual(6, n);
            HasNext(e2, 1);
            Assert.AreEqual(6, n);

            HasNext(e2, 2);
            Assert.AreEqual(6, n);
            HasNext(e2, 3);
            Assert.AreEqual(6, n);

            HasNext(e2, 4);
            Assert.AreEqual(10, n);
            HasNext(e1, 4);
            Assert.AreEqual(10, n);
        }

        [TestMethod]
        public void Publish11()
        {
            var rng = Enumerable.Range(0, 5).Publish();

            var e1 = rng.GetEnumerator();
            var e2 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);
            e1.Dispose();

            HasNext(e2, 0);
            HasNext(e2, 1);
            e2.Dispose();

            var e3 = rng.GetEnumerator();
            HasNext(e3, 3);
            HasNext(e3, 4);
            NoNext(e3);
        }

        [TestMethod]
        public void Memoize1()
        {
            var rng = Enumerable.Range(0, 5).Memoize();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);
            HasNext(e1, 3);
            HasNext(e1, 4);
            NoNext(e1);
        }

        [TestMethod]
        public void Memoize2()
        {
            var rng = Enumerable.Range(0, 5).Memoize();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);
            HasNext(e1, 3);
            HasNext(e1, 4);
            NoNext(e1);

            var e2 = rng.GetEnumerator();
            HasNext(e2, 0);
            HasNext(e2, 1);
            HasNext(e2, 2);
            HasNext(e2, 3);
            HasNext(e2, 4);
            NoNext(e2);
        }

        [TestMethod]
        public void Memoize3()
        {
            var rng = Enumerable.Range(0, 5).Memoize();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);

            var e2 = rng.GetEnumerator();
            HasNext(e1, 3);
            HasNext(e2, 0);
            HasNext(e2, 1);
            HasNext(e1, 4);
            HasNext(e2, 2);
            NoNext(e1);

            HasNext(e2, 3);
            HasNext(e2, 4);
            NoNext(e2);
        }

        [TestMethod]
        public void Memoize4()
        {
            var rng = Enumerable.Range(0, 5).Memoize(2);

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);

            var e2 = rng.GetEnumerator();
            HasNext(e2, 0);
            HasNext(e2, 1);
            HasNext(e2, 2);

            var e3 = rng.GetEnumerator();
            AssertThrows<InvalidOperationException>(() => e3.MoveNext());
        }

        [TestMethod]
        public void Memoize6()
        {
            var ex = new MyException();
            var rng = Enumerable.Range(0, 2).Concat(EnumerableEx.Throw<int>(ex)).Memoize();

            var e1 = rng.GetEnumerator();
            var e2 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            AssertThrows<MyException>(() => e1.MoveNext());

            HasNext(e2, 0);
            HasNext(e2, 1);
            AssertThrows<MyException>(() => e2.MoveNext());
        }

        [TestMethod]
        public void Memoize7()
        {
            var rng = Enumerable.Range(0, 5).Memoize();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);
            e1.Dispose();

            var e2 = rng.GetEnumerator();
            HasNext(e2, 0);
            HasNext(e2, 1);
            e2.Dispose();

            var e3 = rng.GetEnumerator();
            HasNext(e3, 0);
            HasNext(e3, 1);
            HasNext(e3, 2);
            HasNext(e3, 3);
            HasNext(e3, 4);
            NoNext(e3);
        }

        [TestMethod]
        public void Memoize8()
        {
            var rng = Enumerable.Range(0, 5).Memoize();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);

            rng.Dispose();
            AssertThrows<ObjectDisposedException>(() => e1.MoveNext());
            AssertThrows<ObjectDisposedException>(() => rng.GetEnumerator());
            AssertThrows<ObjectDisposedException>(() => ((IEnumerable)rng).GetEnumerator());
        }

        [TestMethod]
        public void Memoize9()
        {
            var rng = Enumerable.Range(0, 5).Memoize();

            var e1 = ((IEnumerable)rng).GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual(0, (int)e1.Current);
        }

        [TestMethod]
        public void Memoize10()
        {
            var rnd = Rand().Take(1000).Memoize();
            Assert.IsTrue(rnd.Zip(rnd, (l, r) => l == r).All(x => x));
        }

        static Random s_rand = new Random();

        static IEnumerable<int> Rand()
        {
            while (true)
                yield return s_rand.Next();
        }

        [TestMethod]
        public void ShareLambda_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Share<int, int>(null, xs => xs));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Share<int, int>(new[] { 1 }, null));
        }

        [TestMethod]
        public void ShareLambda()
        {
            var n = 0;
            var res = Enumerable.Range(0, 10).Do(_ => n++).Share(xs => xs.Zip(xs, (l, r) => l + r).Take(4)).ToList();
            Assert.IsTrue(res.SequenceEqual(new[] { 0 + 1, 2 + 3, 4 + 5, 6 + 7 }));
            Assert.AreEqual(8, n);
        }

        [TestMethod]
        public void PublishLambda_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Publish<int, int>(null, xs => xs));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Publish<int, int>(new[] { 1 }, null));
        }

        [TestMethod]
        public void PublishLambda()
        {
            var n = 0;
            var res = Enumerable.Range(0, 10).Do(_ => n++).Publish(xs => xs.Zip(xs, (l, r) => l + r).Take(4)).ToList();
            Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 4).Select(x => x * 2)));
            Assert.AreEqual(4, n);
        }

        [TestMethod]
        public void MemoizeLambda_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Memoize<int, int>(null, xs => xs));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Memoize<int, int>(new[] { 1 }, null));
        }

        [TestMethod]
        public void MemoizeLambda()
        {
            var n = 0;
            var res = Enumerable.Range(0, 10).Do(_ => n++).Memoize(xs => xs.Zip(xs, (l, r) => l + r).Take(4)).ToList();
            Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 4).Select(x => x * 2)));
            Assert.AreEqual(4, n);
        }

        [TestMethod]
        public void MemoizeLimitedLambda_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Memoize<int, int>(null, 2, xs => xs));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Memoize<int, int>(new[] { 1 }, 2, null));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Memoize<int, int>(new[] { 1 }, 0, xs => xs));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Memoize<int, int>(new[] { 1 }, -1, xs => xs));
        }

        [TestMethod]
        public void MemoizeLimitedLambda()
        {
            var n = 0;
            var res = Enumerable.Range(0, 10).Do(_ => n++).Memoize(2, xs => xs.Zip(xs, (l, r) => l + r).Take(4)).ToList();
            Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 4).Select(x => x * 2)));
            Assert.AreEqual(4, n);
        }
    }
}

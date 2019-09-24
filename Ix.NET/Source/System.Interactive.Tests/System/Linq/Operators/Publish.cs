// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Publish : Tests
    {
        [Fact]
        public void Publish_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Publish<int>(null));
        }

        [Fact]
        public void Publish0()
        {
            var n = 0;
            var rng = Tick(i => n += i).Publish();

            var e1 = rng.GetEnumerator();
            var e2 = rng.GetEnumerator();

            HasNext(e1, 0);
            Assert.Equal(0, n);

            HasNext(e1, 1);
            Assert.Equal(1, n);

            HasNext(e1, 2);
            Assert.Equal(3, n);
            HasNext(e2, 0);
            Assert.Equal(3, n);

            HasNext(e1, 3);
            Assert.Equal(6, n);
            HasNext(e2, 1);
            Assert.Equal(6, n);

            HasNext(e2, 2);
            Assert.Equal(6, n);
            HasNext(e2, 3);
            Assert.Equal(6, n);

            HasNext(e2, 4);
            Assert.Equal(10, n);
            HasNext(e1, 4);
            Assert.Equal(10, n);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        private class MyException : Exception
        {
        }

        [Fact]
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

        [Fact]
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

        [Fact]
        public void Publish9()
        {
            var rng = Enumerable.Range(0, 5).Publish();

            var e1 = ((IEnumerable)rng).GetEnumerator();
            Assert.True(e1.MoveNext());
            Assert.Equal(0, (int)e1.Current);
        }

        [Fact]
        public void Publish10()
        {
            var rnd = Rand().Take(1000).Publish();
            Assert.True(rnd.Zip(rnd, (l, r) => l == r).All(x => x));
        }

        [Fact]
        public void PublishLambda_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Publish<int, int>(null, xs => xs));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Publish<int, int>(new[] { 1 }, null));
        }

        [Fact]
        public void PublishLambda()
        {
            var n = 0;
            var res = Enumerable.Range(0, 10).Do(_ => n++).Publish(xs => xs.Zip(xs, (l, r) => l + r).Take(4)).ToList();
            Assert.True(res.SequenceEqual(Enumerable.Range(0, 4).Select(x => x * 2)));
            Assert.Equal(4, n);
        }

        private static IEnumerable<int> Tick(Action<int> t)
        {
            var i = 0;
            while (true)
            {
                t(i);
                yield return i++;
            }
        }

        private static readonly Random s_rand = new Random();

        private static IEnumerable<int> Rand()
        {
            while (true)
                yield return s_rand.Next();
        }
    }
}

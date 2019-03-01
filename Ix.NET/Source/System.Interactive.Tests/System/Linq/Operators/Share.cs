// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Share : Tests
    {
        [Fact]
        public void Share_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Share<int>(null));
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        //[Fact]
        public void Share4()
        {
            var rng = Enumerable.Range(0, 5).Share();

            var e1 = rng.GetEnumerator();
            HasNext(e1, 0);
            HasNext(e1, 1);
            HasNext(e1, 2);

            e1.Dispose();
            Assert.False(e1.MoveNext());
        }

        [Fact]
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

        [Fact]
        public void Share6()
        {
            var rng = Enumerable.Range(0, 5).Share();

            var e1 = ((IEnumerable)rng).GetEnumerator();
            Assert.True(e1.MoveNext());
            Assert.Equal(0, (int)e1.Current);
        }

        [Fact]
        public void ShareLambda_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Share<int, int>(null, xs => xs));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Share<int, int>(new[] { 1 }, null));
        }

        [Fact]
        public void ShareLambda()
        {
            var n = 0;
            var res = Enumerable.Range(0, 10).Do(_ => n++).Share(xs => xs.Zip(xs, (l, r) => l + r).Take(4)).ToList();
            Assert.True(res.SequenceEqual(new[] { 0 + 1, 2 + 3, 4 + 5, 6 + 7 }));
            Assert.Equal(8, n);
        }
    }
}

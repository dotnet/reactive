// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
#if !NO_TPL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Tests
{
    public partial class AsyncTests
    {
        [TestMethod]
        public void Concat_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Concat<int>(null, AsyncEnumerable.Return(42)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Concat<int>(AsyncEnumerable.Return(42), null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Concat<int>(default(IAsyncEnumerable<int>[])));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Concat<int>(default(IEnumerable<IAsyncEnumerable<int>>)));
        }

        [TestMethod]
        public void Concat1()
        {
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(new[] { 4, 5, 6 }.ToAsyncEnumerable());

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            NoNext(e);
        }

        [TestMethod]
        public void Concat2()
        {
            var ex = new Exception("Bang");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(AsyncEnumerable.Throw<int>(ex));

            var e = ys.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Concat3()
        {
            var ex = new Exception("Bang");
            var ys = AsyncEnumerable.Throw<int>(ex).Concat(new[] { 4, 5, 6 }.ToAsyncEnumerable());

            var e = ys.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Concat4()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = new[] { 6, 7, 8 }.ToAsyncEnumerable();

            var res = AsyncEnumerable.Concat(xs, ys, zs);

            var e = res.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            HasNext(e, 6);
            HasNext(e, 7);
            HasNext(e, 8);
            NoNext(e);
        }

        [TestMethod]
        public void Concat5()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = AsyncEnumerable.Throw<int>(ex);

            var res = AsyncEnumerable.Concat(xs, ys, zs);

            var e = res.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Concat6()
        {
            var res = AsyncEnumerable.Concat(ConcatXss());

            var e = res.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 5);
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single().Message == "Bang!");
        }

        static IEnumerable<IAsyncEnumerable<int>> ConcatXss()
        {
            yield return new[] { 1, 2, 3 }.ToAsyncEnumerable();
            yield return new[] { 4, 5 }.ToAsyncEnumerable();
            throw new Exception("Bang!");
        }

        [TestMethod]
        public void Zip_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Zip<int, int, int>(null, AsyncEnumerable.Return(42), (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Zip<int, int, int>(AsyncEnumerable.Return(42), null, (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Zip<int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null));
        }

        [TestMethod]
        public void Zip1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetEnumerator();
            HasNext(e, 1 * 4);
            HasNext(e, 2 * 5);
            HasNext(e, 3 * 6);
            NoNext(e);
        }

        [TestMethod]
        public void Zip2()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6, 7 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetEnumerator();
            HasNext(e, 1 * 4);
            HasNext(e, 2 * 5);
            HasNext(e, 3 * 6);
            NoNext(e);
        }

        [TestMethod]
        public void Zip3()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetEnumerator();
            HasNext(e, 1 * 4);
            HasNext(e, 2 * 5);
            HasNext(e, 3 * 6);
            NoNext(e);
        }

        [TestMethod]
        public void Zip4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = AsyncEnumerable.Throw<int>(ex);
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Zip5()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Zip6()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => { if (x > 0) throw ex; return x * y; });

            var e = res.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Union_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Union<int>(null, AsyncEnumerable.Return(42)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Union<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Union<int>(null, AsyncEnumerable.Return(42), new Eq()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Union<int>(AsyncEnumerable.Return(42), null, new Eq()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Union<int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null));
        }

        [TestMethod]
        public void Union1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys);

            var e = res.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, 3);
            HasNext(e, 5);
            HasNext(e, 4);
            NoNext(e);
        }

        [TestMethod]
        public void Union2()
        {
            var xs = new[] { 1, 2, -3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, -1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys, new Eq());

            var e = res.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 2);
            HasNext(e, -3);
            HasNext(e, 5);
            HasNext(e, 4);
            NoNext(e);
        }

        [TestMethod]
        public void Intersect_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Intersect<int>(null, AsyncEnumerable.Return(42)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Intersect<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Intersect<int>(null, AsyncEnumerable.Return(42), new Eq()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Intersect<int>(AsyncEnumerable.Return(42), null, new Eq()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Intersect<int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null));
        }

        [TestMethod]
        public void Intersect1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Intersect(ys);

            var e = res.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, 3);
            NoNext(e);
        }

        [TestMethod]
        public void Intersect2()
        {
            var xs = new[] { 1, 2, -3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, -1, 4 }.ToAsyncEnumerable();
            var res = xs.Intersect(ys, new Eq());

            var e = res.GetEnumerator();
            HasNext(e, 1);
            HasNext(e, -3);
            NoNext(e);
        }

        [TestMethod]
        public void Except_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Except<int>(null, AsyncEnumerable.Return(42)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Except<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Except<int>(null, AsyncEnumerable.Return(42), new Eq()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Except<int>(AsyncEnumerable.Return(42), null, new Eq()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Except<int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null));
        }

        [TestMethod]
        public void Except1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Except(ys);

            var e = res.GetEnumerator();
            HasNext(e, 2);
            NoNext(e);
        }

        [TestMethod]
        public void Except2()
        {
            var xs = new[] { 1, 2, -3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, -1, 4 }.ToAsyncEnumerable();
            var res = xs.Except(ys, new Eq());

            var e = res.GetEnumerator();
            HasNext(e, 2);
            NoNext(e);
        }

        [TestMethod]
        public void SequenceEqual_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual<int>(null, AsyncEnumerable.Return(42)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual<int>(null, AsyncEnumerable.Return(42), new Eq()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual<int>(AsyncEnumerable.Return(42), null, new Eq()));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual<int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual<int>(null, AsyncEnumerable.Return(42), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual<int>(null, AsyncEnumerable.Return(42), new Eq(), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual<int>(AsyncEnumerable.Return(42), null, new Eq(), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual<int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [TestMethod]
        public void SequenceEqual1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(xs);
            Assert.IsTrue(res.Result);
        }

        [TestMethod]
        public void SequenceEqual2()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.SequenceEqual(xs);
            Assert.IsTrue(res.Result);
        }

        [TestMethod]
        public void SequenceEqual3()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 3, 2 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys);
            Assert.IsFalse(res.Result);
        }

        [TestMethod]
        public void SequenceEqual4()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys);
            Assert.IsFalse(res.Result);
        }

        [TestMethod]
        public void SequenceEqual5()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys);
            Assert.IsFalse(res.Result);
        }

        [TestMethod]
        public void SequenceEqual6()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = AsyncEnumerable.Throw<int>(ex);
            var res = xs.SequenceEqual(ys);

            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void SequenceEqual7()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys);

            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void SequenceEqual8()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(xs, new Eq());
            Assert.IsTrue(res.Result);
        }

        [TestMethod]
        public void SequenceEqual9()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.SequenceEqual(xs, new Eq());
            Assert.IsTrue(res.Result);
        }

        [TestMethod]
        public void SequenceEqual10()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 3, 2 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys, new Eq());
            Assert.IsFalse(res.Result);
        }

        [TestMethod]
        public void SequenceEqual11()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys, new Eq());
            Assert.IsFalse(res.Result);
        }

        [TestMethod]
        public void SequenceEqual12()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys, new Eq());
            Assert.IsFalse(res.Result);
        }

        [TestMethod]
        public void SequenceEqual13()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = AsyncEnumerable.Throw<int>(ex);
            var res = xs.SequenceEqual(ys, new Eq());

            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void SequenceEqual14()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys, new Eq());

            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void SequenceEqual15()
        {
            var xs = new[] { 1, 2, -3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, -2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys, new Eq());
            Assert.IsTrue(res.Result);
        }

        [TestMethod]
        public void SequenceEqual16()
        {
            var xs = new[] { 1, 2, -3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, -2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys, new EqEx());
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is NotImplementedException);
        }

        class EqEx : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(int obj)
            {
                throw new NotImplementedException();
            }
        }
        
        [TestMethod]
        public void GroupJoin_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(null, AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), null, x => x, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, null, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(null, AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), null, x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, null, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x, default(IEqualityComparer<int>)));
        }

        [TestMethod]
        public void GroupJoin1()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 4, 7, 6, 2, 3, 4, 8, 9 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetEnumerator();
            HasNext(e, "0 - 639");
            HasNext(e, "1 - 474");
            HasNext(e, "2 - 28");
            NoNext(e);
        }

        [TestMethod]
        public void GroupJoin2()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetEnumerator();
            HasNext(e, "0 - 36");
            HasNext(e, "1 - 4");
            HasNext(e, "2 - ");
            NoNext(e);
        }

        [TestMethod]
        public void GroupJoin3()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void GroupJoin4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = AsyncEnumerable.Throw<int>(ex);

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void GroupJoin5()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => { throw ex; }, y => y % 3, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void GroupJoin6()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => { throw ex; }, (x, i) => x + " - " + i.Aggregate("", (s, j) => s + j).Result);

            var e = res.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void GroupJoin7()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => {
                if (x == 1)
                    throw ex;
                return x + " - " + i.Aggregate("", (s, j) => s + j).Result;
            });

            var e = res.GetEnumerator();
            HasNext(e, "0 - 36");
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Join_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(null, AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), null, x => x, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null, x => x, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, null, (x, y) => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(null, AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), null, x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), null, x => x, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, null, (x, y) => x, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Join<int, int, int, int>(AsyncEnumerable.Return(42), AsyncEnumerable.Return(42), x => x, x => x, (x, y) => x, default(IEqualityComparer<int>)));
        }

        [TestMethod]
        public void Join1()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetEnumerator();
            HasNext(e, 0 + 3);
            HasNext(e, 0 + 6);
            HasNext(e, 1 + 4);
            NoNext(e);
        }

        [TestMethod]
        public void Join2()
        {
            var xs = new[] { 3, 6, 4 }.ToAsyncEnumerable();
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetEnumerator();
            HasNext(e, 3 + 0);
            HasNext(e, 6 + 0);
            HasNext(e, 4 + 1);
            NoNext(e);
        }

        [TestMethod]
        public void Join3()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetEnumerator();
            HasNext(e, 0 + 3);
            HasNext(e, 0 + 6);
            NoNext(e);
        }

        [TestMethod]
        public void Join4()
        {
            var xs = new[] { 3, 6 }.ToAsyncEnumerable();
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetEnumerator();
            HasNext(e, 3 + 0);
            HasNext(e, 6 + 0);
            NoNext(e);
        }

        [TestMethod]
        public void Join5()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Join6()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = AsyncEnumerable.Throw<int>(ex);

            var res = xs.Join(ys, x => x % 3, y => y % 3, (x, y) => x + y);

            var e = res.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Join7()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => { throw ex; }, y => y, (x, y) => x + y);

            var e = res.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Join8()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join(ys, x => x, y => { throw ex; }, (x, y) => x + y);

            var e = res.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Join9()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 0, 1, 2 }.ToAsyncEnumerable();

            var res = xs.Join<int, int, int, int>(ys, x => x, y => y, (x, y) => { throw ex; });

            var e = res.GetEnumerator();
            AssertThrows<Exception>(() => e.MoveNext().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void SelectManyMultiple_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int>(default(IAsyncEnumerable<int>), AsyncEnumerable.Return(42)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SelectMany<int, int>(AsyncEnumerable.Return(42), default(IAsyncEnumerable<int>)));
        }

        [TestMethod]
        public void SelectManyMultiple1()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 4 }.ToAsyncEnumerable();

            var res = xs.SelectMany(ys);

            var e = res.GetEnumerator();
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 3);
            HasNext(e, 4);
            HasNext(e, 3);
            HasNext(e, 4);
            NoNext(e);
        }
    }
}

#endif
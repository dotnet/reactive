// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
#if DESKTOPCLR40

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Threading;

namespace Tests
{
    public partial class AsyncTests
    {
        [TestMethod]
        public void Aggregate_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int>(null, (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int>(null, 0, (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int>(AsyncEnumerable.Return(42), 0, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(null, 0, (x, y) => x + y, z => z));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(AsyncEnumerable.Return(42), 0, null, z => z));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(AsyncEnumerable.Return(42), 0, (x, y) => x + y, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int>(null, (x, y) => x + y, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int>(null, 0, (x, y) => x + y, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int>(AsyncEnumerable.Return(42), 0, null, CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(null, 0, (x, y) => x + y, z => z, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(AsyncEnumerable.Return(42), 0, null, z => z, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Aggregate<int, int, int>(AsyncEnumerable.Return(42), 0, (x, y) => x + y, null, CancellationToken.None));
        }

        [TestMethod]
        public void Aggregate1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate((x, y) => x * y);
            Assert.AreEqual(ys.Result, 24);
        }

        [TestMethod]
        public void Aggregate2()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.Aggregate((x, y) => x * y);
            AssertThrows<Exception>(() => ys.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void Aggregate3()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Aggregate((x, y) => x * y);
            AssertThrows<Exception>(() => ys.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Aggregate4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate((x, y) => { throw ex; });
            AssertThrows<Exception>(() => ys.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Aggregate5()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => x * y);
            Assert.AreEqual(ys.Result, 24);
        }

        [TestMethod]
        public void Aggregate6()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => x * y);
            Assert.AreEqual(ys.Result, 1);
        }

        [TestMethod]
        public void Aggregate7()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Aggregate(1, (x, y) => x * y);
            AssertThrows<Exception>(() => ys.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Aggregate8()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => { throw ex; });
            AssertThrows<Exception>(() => ys.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Aggregate9()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => x * y, x => x + 1);
            Assert.AreEqual(ys.Result, 25);
        }

        [TestMethod]
        public void Aggregate10()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => x * y, x => x + 1);
            Assert.AreEqual(ys.Result, 2);
        }

        [TestMethod]
        public void Aggregate11()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerable.Throw<int>(ex);
            var ys = xs.Aggregate(1, (x, y) => x * y, x => x + 1);
            AssertThrows<Exception>(() => ys.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Aggregate12()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate(1, (x, y) => { throw ex; }, x => x + 1);
            AssertThrows<Exception>(() => ys.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Aggregate13()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Aggregate<int, int, int>(1, (x, y) => x * y, x => { throw ex; });
            AssertThrows<Exception>(() => ys.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Count_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Count<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Count<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Count<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Count<int>(null, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Count<int>(null, x => true, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Count<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [TestMethod]
        public void Count1()
        {
            Assert.AreEqual(new int[0].ToAsyncEnumerable().Count().Result, 0);
            Assert.AreEqual(new[] { 1, 2, 3 }.ToAsyncEnumerable().Count().Result, 3);
            AssertThrows<AggregateException>(() => AsyncEnumerable.Throw<int>(new Exception("Bang!")).Count().Wait());
        }

        [TestMethod]
        public void Count2()
        {
            Assert.AreEqual(new int[0].ToAsyncEnumerable().Count(x => x < 3).Result, 0);
            Assert.AreEqual(new[] { 1, 2, 3 }.ToAsyncEnumerable().Count(x => x < 3).Result, 2);
            AssertThrows<AggregateException>(() => AsyncEnumerable.Throw<int>(new Exception("Bang!")).Count(x => x < 3).Wait());
        }

        [TestMethod]
        public void Count3()
        {
            var ex = new Exception("Bang!");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().Count(x => { throw ex; });
            AssertThrows<Exception>(() => ys.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void LongCount_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(null, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(null, x => true, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [TestMethod]
        public void LongCount1()
        {
            Assert.AreEqual(new int[0].ToAsyncEnumerable().LongCount().Result, 0);
            Assert.AreEqual(new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCount().Result, 3);
            AssertThrows<AggregateException>(() => AsyncEnumerable.Throw<int>(new Exception("Bang!")).LongCount().Wait());
        }

        [TestMethod]
        public void LongCount2()
        {
            Assert.AreEqual(new int[0].ToAsyncEnumerable().LongCount(x => x < 3).Result, 0);
            Assert.AreEqual(new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCount(x => x < 3).Result, 2);
            AssertThrows<AggregateException>(() => AsyncEnumerable.Throw<int>(new Exception("Bang!")).LongCount(x => x < 3).Wait());
        }

        [TestMethod]
        public void LongCount3()
        {
            var ex = new Exception("Bang!");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCount(x => { throw ex; });
            AssertThrows<Exception>(() => ys.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void All_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.All<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.All<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.All<int>(null, x => true, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.All<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [TestMethod]
        public void All1()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().All(x => x % 2 == 0);
            Assert.IsFalse(res.Result);
        }

        [TestMethod]
        public void All2()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().All(x => x % 2 == 0);
            Assert.IsTrue(res.Result);
        }

        [TestMethod]
        public void All3()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).All(x => x % 2 == 0);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void All4()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().All(x => { throw ex; });
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Any_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Any<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Any<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Any<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Any<int>(null, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Any<int>(null, x => true, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Any<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [TestMethod]
        public void Any1()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Any(x => x % 2 == 0);
            Assert.IsTrue(res.Result);
        }

        [TestMethod]
        public void Any2()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().Any(x => x % 2 != 0);
            Assert.IsFalse(res.Result);
        }

        [TestMethod]
        public void Any3()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Any(x => x % 2 == 0);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Any4()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().Any(x => { throw ex; });
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Any5()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Any();
            Assert.IsTrue(res.Result);
        }

        [TestMethod]
        public void Any6()
        {
            var res = new int[0].ToAsyncEnumerable().Any();
            Assert.IsFalse(res.Result);
        }

        [TestMethod]
        public void Contains_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(null, 42));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(null, 42, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(AsyncEnumerable.Return(42), 42, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(null, 42, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(null, 42, EqualityComparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(AsyncEnumerable.Return(42), 42, null, CancellationToken.None));
        }

        [TestMethod]
        public void Contains1()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Contains(3);
            Assert.IsTrue(ys.Result);
        }

        [TestMethod]
        public void Contains2()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Contains(6);
            Assert.IsFalse(ys.Result);
        }

        [TestMethod]
        public void Contains3()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Contains(-3, new Eq());
            Assert.IsTrue(ys.Result);
        }

        [TestMethod]
        public void Contains4()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Contains(-6, new Eq());
            Assert.IsFalse(ys.Result);
        }

        class Eq : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(Math.Abs(x), Math.Abs(y));
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(Math.Abs(obj));
            }
        }

        [TestMethod]
        public void First_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.First<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.First<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.First<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.First<int>(null, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.First<int>(null, x => true, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.First<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [TestMethod]
        public void First1()
        {
            var res = AsyncEnumerable.Empty<int>().First();
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void First2()
        {
            var res = AsyncEnumerable.Empty<int>().First(x => true);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void First3()
        {
            var res = AsyncEnumerable.Return(42).First(x => x % 2 != 0);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void First4()
        {
            var res = AsyncEnumerable.Return(42).First();
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void First5()
        {
            var res = AsyncEnumerable.Return(42).First(x => x % 2 == 0);
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void First6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).First();
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void First7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).First(x => true);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void First8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().First();
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void First9()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().First(x => x % 2 != 0);
            Assert.AreEqual(45, res.Result);
        }

        [TestMethod]
        public void FirstOrDefault_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefault<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefault<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefault<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefault<int>(null, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefault<int>(null, x => true, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefault<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [TestMethod]
        public void FirstOrDefault1()
        {
            var res = AsyncEnumerable.Empty<int>().FirstOrDefault();
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void FirstOrDefault2()
        {
            var res = AsyncEnumerable.Empty<int>().FirstOrDefault(x => true);
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void FirstOrDefault3()
        {
            var res = AsyncEnumerable.Return(42).FirstOrDefault(x => x % 2 != 0);
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void FirstOrDefault4()
        {
            var res = AsyncEnumerable.Return(42).FirstOrDefault();
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void FirstOrDefault5()
        {
            var res = AsyncEnumerable.Return(42).FirstOrDefault(x => x % 2 == 0);
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void FirstOrDefault6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).FirstOrDefault();
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void FirstOrDefault7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).FirstOrDefault(x => true);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void FirstOrDefault8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstOrDefault();
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void FirstOrDefault9()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstOrDefault(x => x % 2 != 0);
            Assert.AreEqual(45, res.Result);
        }

        [TestMethod]
        public void FirstOrDefault10()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstOrDefault(x => x < 10);
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void Last_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Last<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Last<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Last<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Last<int>(null, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Last<int>(null, x => true, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Last<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [TestMethod]
        public void Last1()
        {
            var res = AsyncEnumerable.Empty<int>().Last();
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void Last2()
        {
            var res = AsyncEnumerable.Empty<int>().Last(x => true);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void Last3()
        {
            var res = AsyncEnumerable.Return(42).Last(x => x % 2 != 0);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void Last4()
        {
            var res = AsyncEnumerable.Return(42).Last();
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void Last5()
        {
            var res = AsyncEnumerable.Return(42).Last(x => x % 2 == 0);
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void Last6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Last();
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Last7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Last(x => true);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Last8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().Last();
            Assert.AreEqual(90, res.Result);
        }

        [TestMethod]
        public void Last9()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().Last(x => x % 2 != 0);
            Assert.AreEqual(45, res.Result);
        }

        [TestMethod]
        public void LastOrDefault_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(null, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(null, x => true, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [TestMethod]
        public void LastOrDefault1()
        {
            var res = AsyncEnumerable.Empty<int>().LastOrDefault();
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void LastOrDefault2()
        {
            var res = AsyncEnumerable.Empty<int>().LastOrDefault(x => true);
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void LastOrDefault3()
        {
            var res = AsyncEnumerable.Return(42).LastOrDefault(x => x % 2 != 0);
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void LastOrDefault4()
        {
            var res = AsyncEnumerable.Return(42).LastOrDefault();
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void LastOrDefault5()
        {
            var res = AsyncEnumerable.Return(42).LastOrDefault(x => x % 2 == 0);
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void LastOrDefault6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).LastOrDefault();
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void LastOrDefault7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).LastOrDefault(x => true);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void LastOrDefault8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastOrDefault();
            Assert.AreEqual(90, res.Result);
        }

        [TestMethod]
        public void LastOrDefault9()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().LastOrDefault(x => x % 2 != 0);
            Assert.AreEqual(45, res.Result);
        }

        [TestMethod]
        public void LastOrDefault10()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastOrDefault(x => x < 10);
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void Single_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Single<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Single<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Single<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Single<int>(null, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Single<int>(null, x => true, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Single<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [TestMethod]
        public void Single1()
        {
            var res = AsyncEnumerable.Empty<int>().Single();
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void Single2()
        {
            var res = AsyncEnumerable.Empty<int>().Single(x => true);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void Single3()
        {
            var res = AsyncEnumerable.Return(42).Single(x => x % 2 != 0);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void Single4()
        {
            var res = AsyncEnumerable.Return(42).Single();
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void Single5()
        {
            var res = AsyncEnumerable.Return(42).Single(x => x % 2 == 0);
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void Single6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Single();
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Single7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).Single(x => true);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void Single8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().Single();
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void Single9()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().Single(x => x % 2 != 0);
            Assert.AreEqual(45, res.Result);
        }

        [TestMethod]
        public void Single10()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().Single(x => x % 2 != 0);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void SingleOrDefault_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(null, x => true));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(null, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(null, x => true, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefault<int>(AsyncEnumerable.Return(42), null, CancellationToken.None));
        }

        [TestMethod]
        public void SingleOrDefault1()
        {
            var res = AsyncEnumerable.Empty<int>().SingleOrDefault();
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void SingleOrDefault2()
        {
            var res = AsyncEnumerable.Empty<int>().SingleOrDefault(x => true);
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void SingleOrDefault3()
        {
            var res = AsyncEnumerable.Return(42).SingleOrDefault(x => x % 2 != 0);
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void SingleOrDefault4()
        {
            var res = AsyncEnumerable.Return(42).SingleOrDefault();
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void SingleOrDefault5()
        {
            var res = AsyncEnumerable.Return(42).SingleOrDefault(x => x % 2 == 0);
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void SingleOrDefault6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).SingleOrDefault();
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void SingleOrDefault7()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).SingleOrDefault(x => true);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void SingleOrDefault8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefault();
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void SingleOrDefault9()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefault(x => x % 2 != 0);
            Assert.AreEqual(45, res.Result);
        }

        [TestMethod]
        public void SingleOrDefault10()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefault(x => x < 10);
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void SingleOrDefault11()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefault(x => true);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void ElementAt_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ElementAt<int>(null, 0));
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAt<int>(AsyncEnumerable.Return(42), -1));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ElementAt<int>(null, 0, CancellationToken.None));
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAt<int>(AsyncEnumerable.Return(42), -1, CancellationToken.None));
        }

        [TestMethod]
        public void ElementAt1()
        {
            var res = AsyncEnumerable.Empty<int>().ElementAt(0);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentOutOfRangeException);
        }

        [TestMethod]
        public void ElementAt2()
        {
            var res = AsyncEnumerable.Return<int>(42).ElementAt(0);
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void ElementAt3()
        {
            var res = AsyncEnumerable.Return<int>(42).ElementAt(1);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentOutOfRangeException);
        }

        [TestMethod]
        public void ElementAt4()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAt(1);
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void ElementAt5()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAt(7);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentOutOfRangeException);
        }

        [TestMethod]
        public void ElementAt6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).ElementAt(15);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void ElementAtOrDefault_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ElementAtOrDefault<int>(null, 0));
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAtOrDefault<int>(AsyncEnumerable.Return(42), -1));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ElementAtOrDefault<int>(null, 0, CancellationToken.None));
            AssertThrows<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAtOrDefault<int>(AsyncEnumerable.Return(42), -1, CancellationToken.None));
        }

        [TestMethod]
        public void ElementAtOrDefault1()
        {
            var res = AsyncEnumerable.Empty<int>().ElementAtOrDefault(0);
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void ElementAtOrDefault2()
        {
            var res = AsyncEnumerable.Return<int>(42).ElementAtOrDefault(0);
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void ElementAtOrDefault3()
        {
            var res = AsyncEnumerable.Return<int>(42).ElementAtOrDefault(1);
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void ElementAtOrDefault4()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAtOrDefault(1);
            Assert.AreEqual(42, res.Result);
        }

        [TestMethod]
        public void ElementAtOrDefault5()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAtOrDefault(7);
            Assert.AreEqual(0, res.Result);
        }

        [TestMethod]
        public void ElementAtOrDefault6()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).ElementAtOrDefault(15);
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void ToList_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToList<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToList<int>(null, CancellationToken.None));
        }

        [TestMethod]
        public void ToList1()
        {
            var xs = new[] { 42, 25, 39 };
            var res = xs.ToAsyncEnumerable().ToList();
            Assert.IsTrue(res.Result.SequenceEqual(xs));
        }

        [TestMethod]
        public void ToList2()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.ToList();
            Assert.IsTrue(res.Result.Count == 0);
        }

        [TestMethod]
        public void ToList3()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).ToList();
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void ToArray_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToArray<int>(null));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToArray<int>(null, CancellationToken.None));
        }

        [TestMethod]
        public void ToArray1()
        {
            var xs = new[] { 42, 25, 39 };
            var res = xs.ToAsyncEnumerable().ToArray();
            Assert.IsTrue(res.Result.SequenceEqual(xs));
        }

        [TestMethod]
        public void ToArray2()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.ToArray();
            Assert.IsTrue(res.Result.Length == 0);
        }

        [TestMethod]
        public void ToArray3()
        {
            var ex = new Exception("Bang!");
            var res = AsyncEnumerable.Throw<int>(ex).ToArray();
            AssertThrows<Exception>(() => res.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void ToDictionary_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(null, x => 0));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(null, x => 0, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(AsyncEnumerable.Return(42), null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(AsyncEnumerable.Return(42), x => 0, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(null, x => 0, x => 0));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), null, x => 0));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), x => 0, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(null, x => 0, x => 0, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), null, x => 0, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), x => 0, null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), x => 0, x => 0, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(null, x => 0, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(AsyncEnumerable.Return(42), null, CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(null, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(AsyncEnumerable.Return(42), null, EqualityComparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int>(AsyncEnumerable.Return(42), x => 0, null, CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(null, x => 0, x => 0, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), null, x => 0, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), x => 0, null, CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(null, x => 0, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), null, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), x => 0, null, EqualityComparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToDictionary<int, int, int>(AsyncEnumerable.Return(42), x => 0, x => 0, null, CancellationToken.None));
        }

        [TestMethod]
        public void ToDictionary1()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToDictionary(x => x % 2).Result;
            Assert.IsTrue(res[0] == 4);
            Assert.IsTrue(res[1] == 1);
        }

        [TestMethod]
        public void ToDictionary2()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            AssertThrows<Exception>(() => xs.ToDictionary(x => x % 2).Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentException);
        }

        [TestMethod]
        public void ToDictionary3()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToDictionary(x => x % 2, x => x + 1).Result;
            Assert.IsTrue(res[0] == 5);
            Assert.IsTrue(res[1] == 2);
        }

        [TestMethod]
        public void ToDictionary4()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            AssertThrows<Exception>(() => xs.ToDictionary(x => x % 2, x => x + 1).Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentException);
        }

        [TestMethod]
        public void ToDictionary5()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToDictionary(x => x % 2, new Eq()).Result;
            Assert.IsTrue(res[0] == 4);
            Assert.IsTrue(res[1] == 1);
        }

        [TestMethod]
        public void ToDictionary6()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            AssertThrows<Exception>(() => xs.ToDictionary(x => x % 2, new Eq()).Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is ArgumentException);
        }

        [TestMethod]
        public void ToDictionary7()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToDictionary(x => x % 2, x => x, new Eq()).Result;
            Assert.IsTrue(res[0] == 4);
            Assert.IsTrue(res[1] == 1);
        }

        [TestMethod]
        public void ToLookup_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(null, x => 0));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(AsyncEnumerable.Return(42), null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(null, x => 0, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(AsyncEnumerable.Return(42), null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(AsyncEnumerable.Return(42), x => 0, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(null, x => 0, x => 0));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), null, x => 0));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), x => 0, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(null, x => 0, x => 0, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), null, x => 0, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), x => 0, null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), x => 0, x => 0, null));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(null, x => 0, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(AsyncEnumerable.Return(42), null, CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(null, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(AsyncEnumerable.Return(42), null, EqualityComparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int>(AsyncEnumerable.Return(42), x => 0, null, CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(null, x => 0, x => 0, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), null, x => 0, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), x => 0, null, CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(null, x => 0, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), null, x => 0, EqualityComparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), x => 0, null, EqualityComparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.ToLookup<int, int, int>(AsyncEnumerable.Return(42), x => 0, x => 0, null, CancellationToken.None));
        }

        [TestMethod]
        public void ToLookup1()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2).Result;
            Assert.IsTrue(res.Contains(0));
            Assert.IsTrue(res.Contains(1));
            Assert.IsTrue(res[0].Contains(4));
            Assert.IsTrue(res[1].Contains(1));
            Assert.IsTrue(res.Count == 2);
        }

        [TestMethod]
        public void ToLookup2()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2).Result;
            Assert.IsTrue(res.Contains(0));
            Assert.IsTrue(res.Contains(1));
            Assert.IsTrue(res[0].Contains(4));
            Assert.IsTrue(res[0].Contains(2));
            Assert.IsTrue(res[1].Contains(1));
            Assert.IsTrue(res.Count == 2);
        }

        [TestMethod]
        public void ToLookup3()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, x => x + 1).Result;
            Assert.IsTrue(res.Contains(0));
            Assert.IsTrue(res.Contains(1));
            Assert.IsTrue(res[0].Contains(5));
            Assert.IsTrue(res[1].Contains(2));
            Assert.IsTrue(res.Count == 2);
        }

        [TestMethod]
        public void ToLookup4()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, x => x + 1).Result;
            Assert.IsTrue(res.Contains(0));
            Assert.IsTrue(res.Contains(1));
            Assert.IsTrue(res[0].Contains(5));
            Assert.IsTrue(res[0].Contains(3));
            Assert.IsTrue(res[1].Contains(2));
            Assert.IsTrue(res.Count == 2);
        }

        [TestMethod]
        public void ToLookup5()
        {
            var xs = new[] { 1, 4 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, new Eq()).Result;
            Assert.IsTrue(res.Contains(0));
            Assert.IsTrue(res.Contains(1));
            Assert.IsTrue(res[0].Contains(4));
            Assert.IsTrue(res[1].Contains(1));
            Assert.IsTrue(res.Count == 2);
        }

        [TestMethod]
        public void ToLookup6()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, new Eq()).Result;
            Assert.IsTrue(res.Contains(0));
            Assert.IsTrue(res.Contains(1));
            Assert.IsTrue(res[0].Contains(4));
            Assert.IsTrue(res[0].Contains(2));
            Assert.IsTrue(res[1].Contains(1));
            Assert.IsTrue(res.Count == 2);
        }

        [TestMethod]
        public void ToLookup7()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2).Result;
            foreach (var g in res)
                Assert.IsTrue(g.Key == 0 || g.Key == 1);
        }

        [TestMethod]
        public void ToLookup8()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2).Result;
            foreach (IGrouping<int, int> g in (IEnumerable)res)
                Assert.IsTrue(g.Key == 0 || g.Key == 1);
        }

        [TestMethod]
        public void ToLookup9()
        {
            var xs = new[] { 1, 4, 2 }.ToAsyncEnumerable();
            var res = xs.ToLookup(x => x % 2, x => x, new Eq()).Result;
            Assert.IsTrue(res.Contains(0));
            Assert.IsTrue(res.Contains(1));
            Assert.IsTrue(res[0].Contains(4));
            Assert.IsTrue(res[0].Contains(2));
            Assert.IsTrue(res[1].Contains(1));
            Assert.IsTrue(res.Count == 2);
        }

        [TestMethod]
        public void Average_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal?>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal?>), x => x));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, int?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, long>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, long?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, double>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, double?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, float>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, float?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal?>), CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<int?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<long?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<double?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<float?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(default(IAsyncEnumerable<decimal?>), x => x, CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, int>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, int?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, long>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, long?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, double>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, double?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, float>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, float?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Average(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>), CancellationToken.None));
        }

        [TestMethod]
        public void Average1()
        {
            var xs = new[] { 1, 2, 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Average(), ys.Average().Result);
            Assert.AreEqual(xs.Average(), ys.Average(x => x).Result);
        }

        [TestMethod]
        public void Average2()
        {
            var xs = new[] { 1, default(int?), 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Average(), ys.Average().Result);
            Assert.AreEqual(xs.Average(), ys.Average(x => x).Result);
        }

        [TestMethod]
        public void Average3()
        {
            var xs = new[] { 1L, 2L, 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Average(), ys.Average().Result);
            Assert.AreEqual(xs.Average(), ys.Average(x => x).Result);
        }

        [TestMethod]
        public void Average4()
        {
            var xs = new[] { 1L, default(long?), 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Average(), ys.Average().Result);
            Assert.AreEqual(xs.Average(), ys.Average(x => x).Result);
        }

        [TestMethod]
        public void Average5()
        {
            var xs = new[] { 1.0, 2.0, 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Average(), ys.Average().Result);
            Assert.AreEqual(xs.Average(), ys.Average(x => x).Result);
        }

        [TestMethod]
        public void Average6()
        {
            var xs = new[] { 1.0, default(double?), 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Average(), ys.Average().Result);
            Assert.AreEqual(xs.Average(), ys.Average(x => x).Result);
        }

        [TestMethod]
        public void Average7()
        {
            var xs = new[] { 1.0f, 2.0f, 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Average(), ys.Average().Result);
            Assert.AreEqual(xs.Average(), ys.Average(x => x).Result);
        }

        [TestMethod]
        public void Average8()
        {
            var xs = new[] { 1.0f, default(float?), 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Average(), ys.Average().Result);
            Assert.AreEqual(xs.Average(), ys.Average(x => x).Result);
        }

        [TestMethod]
        public void Average9()
        {
            var xs = new[] { 1.0m, 2.0m, 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Average(), ys.Average().Result);
            Assert.AreEqual(xs.Average(), ys.Average(x => x).Result);
        }

        [TestMethod]
        public void Average10()
        {
            var xs = new[] { 1.0m, default(decimal?), 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Average(), ys.Average().Result);
            Assert.AreEqual(xs.Average(), ys.Average(x => x).Result);
        }

        [TestMethod]
        public void Average11()
        {
            var xs = new int[0];
            var ys = xs.ToAsyncEnumerable();
            AssertThrows<Exception>(() => ys.Average().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void Average12()
        {
            var xs = new int?[0];
            var ys = xs.ToAsyncEnumerable();
            Assert.IsNull(ys.Average().Result);
        }

        [TestMethod]
        public void Average13()
        {
            var xs = new long[0];
            var ys = xs.ToAsyncEnumerable();
            AssertThrows<Exception>(() => ys.Average().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void Average14()
        {
            var xs = new long?[0];
            var ys = xs.ToAsyncEnumerable();
            Assert.IsNull(ys.Average().Result);
        }

        [TestMethod]
        public void Average15()
        {
            var xs = new double[0];
            var ys = xs.ToAsyncEnumerable();
            AssertThrows<Exception>(() => ys.Average().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void Average16()
        {
            var xs = new double?[0];
            var ys = xs.ToAsyncEnumerable();
            Assert.IsNull(ys.Average().Result);
        }

        [TestMethod]
        public void Average17()
        {
            var xs = new float[0];
            var ys = xs.ToAsyncEnumerable();
            AssertThrows<Exception>(() => ys.Average().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void Average18()
        {
            var xs = new float?[0];
            var ys = xs.ToAsyncEnumerable();
            Assert.IsNull(ys.Average().Result);
        }

        [TestMethod]
        public void Average19()
        {
            var xs = new decimal[0];
            var ys = xs.ToAsyncEnumerable();
            AssertThrows<Exception>(() => ys.Average().Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void Average20()
        {
            var xs = new decimal?[0];
            var ys = xs.ToAsyncEnumerable();
            Assert.IsNull(ys.Average().Result);
        }

        [TestMethod]
        public void Min_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal?>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal?>), x => x));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, int?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, long>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, long?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, double>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, double?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, float>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, float?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal?>), CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<int?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<long?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<double?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<float?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<decimal?>), x => x, CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, int>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, int?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, long>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, long?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, double>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, double?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, float>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, float?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>), CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>), CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Min(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>), CancellationToken.None));
        }

        [TestMethod]
        public void Min1()
        {
            var xs = new[] { 2, 1, 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Min(), ys.Min().Result);
            Assert.AreEqual(xs.Min(), ys.Min(x => x).Result);
        }

        [TestMethod]
        public void Min2()
        {
            var xs = new[] { 2, default(int?), 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Min(), ys.Min().Result);
            Assert.AreEqual(xs.Min(), ys.Min(x => x).Result);
        }

        [TestMethod]
        public void Min3()
        {
            var xs = new[] { 2L, 1L, 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Min(), ys.Min().Result);
            Assert.AreEqual(xs.Min(), ys.Min(x => x).Result);
        }

        [TestMethod]
        public void Min4()
        {
            var xs = new[] { 2L, default(long?), 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Min(), ys.Min().Result);
            Assert.AreEqual(xs.Min(), ys.Min(x => x).Result);
        }

        [TestMethod]
        public void Min5()
        {
            var xs = new[] { 2.0, 1.0, 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Min(), ys.Min().Result);
            Assert.AreEqual(xs.Min(), ys.Min(x => x).Result);
        }

        [TestMethod]
        public void Min6()
        {
            var xs = new[] { 2.0, default(double?), 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Min(), ys.Min().Result);
            Assert.AreEqual(xs.Min(), ys.Min(x => x).Result);
        }

        [TestMethod]
        public void Min7()
        {
            var xs = new[] { 2.0f, 1.0f, 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Min(), ys.Min().Result);
            Assert.AreEqual(xs.Min(), ys.Min(x => x).Result);
        }

        [TestMethod]
        public void Min8()
        {
            var xs = new[] { 2.0f, default(float?), 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Min(), ys.Min().Result);
            Assert.AreEqual(xs.Min(), ys.Min(x => x).Result);
        }

        [TestMethod]
        public void Min9()
        {
            var xs = new[] { 2.0m, 1.0m, 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Min(), ys.Min().Result);
            Assert.AreEqual(xs.Min(), ys.Min(x => x).Result);
        }

        [TestMethod]
        public void Min10()
        {
            var xs = new[] { 2.0m, default(decimal?), 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Min(), ys.Min().Result);
            Assert.AreEqual(xs.Min(), ys.Min(x => x).Result);
        }

        [TestMethod]
        public void Min11()
        {
            var xs = new[] { DateTime.Now.AddDays(1), DateTime.Now.Subtract(TimeSpan.FromDays(1)), DateTime.Now.AddDays(2), DateTime.Now };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Min(), ys.Min().Result);
            Assert.AreEqual(xs.Min(), ys.Min(x => x).Result);
        }

        [TestMethod]
        public void Max_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal?>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal?>), x => x));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, int?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, long>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, long?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, double>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, double?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, float>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, float?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal?>), CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<int?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<long?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<double?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<float?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<decimal?>), x => x, CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, int>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, int?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, long>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, long?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, double>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, double?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, float>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, float?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>), CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<DateTime>(), default(Func<DateTime, bool>), CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(default(IAsyncEnumerable<DateTime>), Comparer<DateTime>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Max(AsyncEnumerable.Empty<DateTime>(), default(IComparer<DateTime>), CancellationToken.None));
        }

        [TestMethod]
        public void Max1()
        {
            var xs = new[] { 2, 7, 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Max(), ys.Max().Result);
            Assert.AreEqual(xs.Max(), ys.Max(x => x).Result);
        }

        [TestMethod]
        public void Max2()
        {
            var xs = new[] { 2, default(int?), 3, 1 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Max(), ys.Max().Result);
            Assert.AreEqual(xs.Max(), ys.Max(x => x).Result);
        }

        [TestMethod]
        public void Max3()
        {
            var xs = new[] { 2L, 7L, 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Max(), ys.Max().Result);
            Assert.AreEqual(xs.Max(), ys.Max(x => x).Result);
        }

        [TestMethod]
        public void Max4()
        {
            var xs = new[] { 2L, default(long?), 3L, 1L };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Max(), ys.Max().Result);
            Assert.AreEqual(xs.Max(), ys.Max(x => x).Result);
        }

        [TestMethod]
        public void Max5()
        {
            var xs = new[] { 2.0, 7.0, 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Max(), ys.Max().Result);
            Assert.AreEqual(xs.Max(), ys.Max(x => x).Result);
        }

        [TestMethod]
        public void Max6()
        {
            var xs = new[] { 2.0, default(double?), 3.0, 1.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Max(), ys.Max().Result);
            Assert.AreEqual(xs.Max(), ys.Max(x => x).Result);
        }

        [TestMethod]
        public void Max7()
        {
            var xs = new[] { 2.0f, 7.0f, 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Max(), ys.Max().Result);
            Assert.AreEqual(xs.Max(), ys.Max(x => x).Result);
        }

        [TestMethod]
        public void Max8()
        {
            var xs = new[] { 2.0f, default(float?), 3.0f, 1.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Max(), ys.Max().Result);
            Assert.AreEqual(xs.Max(), ys.Max(x => x).Result);
        }

        [TestMethod]
        public void Max9()
        {
            var xs = new[] { 2.0m, 7.0m, 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Max(), ys.Max().Result);
            Assert.AreEqual(xs.Max(), ys.Max(x => x).Result);
        }

        [TestMethod]
        public void Max10()
        {
            var xs = new[] { 2.0m, default(decimal?), 3.0m, 1.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Max(), ys.Max().Result);
            Assert.AreEqual(xs.Max(), ys.Max(x => x).Result);
        }

        [TestMethod]
        public void Max11()
        {
            var xs = new[] { DateTime.Now.AddDays(1), DateTime.Now.Subtract(TimeSpan.FromDays(1)), DateTime.Now.AddDays(2), DateTime.Now };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Max(), ys.Max().Result);
            Assert.AreEqual(xs.Max(), ys.Max(x => x).Result);
        }

        [TestMethod]
        public void Sum_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal?>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float?>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal?>), x => x));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, int>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, int?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, long>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, long?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, double>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, double?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, float>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, float?>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>)));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal?>), CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<int?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<long?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<double?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<float?>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(default(IAsyncEnumerable<decimal?>), x => x, CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, int>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, int?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, long>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, long?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, double>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, double?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, float>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, float?>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, decimal>), CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.Sum(AsyncEnumerable.Empty<int>(), default(Func<int, decimal?>), CancellationToken.None));
        }

        [TestMethod]
        public void Sum1()
        {
            var xs = new[] { 1, 2, 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Sum(), ys.Sum().Result);
            Assert.AreEqual(xs.Sum(), ys.Sum(x => x).Result);
        }

        [TestMethod]
        public void Sum2()
        {
            var xs = new[] { 1, default(int?), 3 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Sum(), ys.Sum().Result);
            Assert.AreEqual(xs.Sum(), ys.Sum(x => x).Result);
        }

        [TestMethod]
        public void Sum3()
        {
            var xs = new[] { 1L, 2L, 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Sum(), ys.Sum().Result);
            Assert.AreEqual(xs.Sum(), ys.Sum(x => x).Result);
        }

        [TestMethod]
        public void Sum4()
        {
            var xs = new[] { 1L, default(long?), 3L };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Sum(), ys.Sum().Result);
            Assert.AreEqual(xs.Sum(), ys.Sum(x => x).Result);
        }

        [TestMethod]
        public void Sum5()
        {
            var xs = new[] { 1.0, 2.0, 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Sum(), ys.Sum().Result);
            Assert.AreEqual(xs.Sum(), ys.Sum(x => x).Result);
        }

        [TestMethod]
        public void Sum6()
        {
            var xs = new[] { 1.0, default(double?), 3.0 };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Sum(), ys.Sum().Result);
            Assert.AreEqual(xs.Sum(), ys.Sum(x => x).Result);
        }

        [TestMethod]
        public void Sum7()
        {
            var xs = new[] { 1.0f, 2.0f, 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Sum(), ys.Sum().Result);
            Assert.AreEqual(xs.Sum(), ys.Sum(x => x).Result);
        }

        [TestMethod]
        public void Sum8()
        {
            var xs = new[] { 1.0f, default(float?), 3.0f };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Sum(), ys.Sum().Result);
            Assert.AreEqual(xs.Sum(), ys.Sum(x => x).Result);
        }

        [TestMethod]
        public void Sum9()
        {
            var xs = new[] { 1.0m, 2.0m, 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Sum(), ys.Sum().Result);
            Assert.AreEqual(xs.Sum(), ys.Sum(x => x).Result);
        }

        [TestMethod]
        public void Sum10()
        {
            var xs = new[] { 1.0m, default(decimal?), 3.0m };
            var ys = xs.ToAsyncEnumerable();
            Assert.AreEqual(xs.Sum(), ys.Sum().Result);
            Assert.AreEqual(xs.Sum(), ys.Sum(x => x).Result);
        }

        [TestMethod]
        public void MinBy_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MinBy(default(IAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MinBy(AsyncEnumerable.Return(42), default(Func<int, int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MinBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MinBy(AsyncEnumerable.Return(42), default(Func<int, int>), Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MinBy(AsyncEnumerable.Return(42), x => x, default(IComparer<int>)));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MinBy(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MinBy(AsyncEnumerable.Return(42), default(Func<int, int>), CancellationToken.None));

            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MinBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MinBy(AsyncEnumerable.Return(42), default(Func<int, int>), Comparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MinBy(AsyncEnumerable.Return(42), x => x, default(IComparer<int>), CancellationToken.None));
        }

        [TestMethod]
        public void MinBy1()
        {
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinBy(x => x / 2);
            var res = xs.Result;

            Assert.IsTrue(res.SequenceEqual(new[] { 3, 2 }));
        }

        [TestMethod]
        public void MinBy2()
        {
            var xs = new int[0].ToAsyncEnumerable().MinBy(x => x / 2);

            AssertThrows<Exception>(() => xs.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void MinBy3()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinBy(x => { if (x == 3) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void MinBy4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinBy(x => { if (x == 4) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void MinBy5()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().Concat(AsyncEnumerable.Throw<int>(ex)).MinBy(x => x, Comparer<int>.Default);

            AssertThrows<Exception>(() => xs.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void MaxBy_Null()
        {
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MaxBy(default(IAsyncEnumerable<int>), x => x));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MaxBy(AsyncEnumerable.Return(42), default(Func<int, int>)));
                                                                      
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MaxBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MaxBy(AsyncEnumerable.Return(42), default(Func<int, int>), Comparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MaxBy(AsyncEnumerable.Return(42), x => x, default(IComparer<int>)));
                                                                      
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MaxBy(default(IAsyncEnumerable<int>), x => x, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MaxBy(AsyncEnumerable.Return(42), default(Func<int, int>), CancellationToken.None));
                                                                      
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MaxBy(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MaxBy(AsyncEnumerable.Return(42), default(Func<int, int>), Comparer<int>.Default, CancellationToken.None));
            AssertThrows<ArgumentNullException>(() => AsyncEnumerable.MaxBy(AsyncEnumerable.Return(42), x => x, default(IComparer<int>), CancellationToken.None));
        }

        [TestMethod]
        public void MaxBy1()
        {
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MaxBy(x => x / 2);
            var res = xs.Result;

            Assert.IsTrue(res.SequenceEqual(new[] { 7, 6 }));
        }

        [TestMethod]
        public void MaxBy2()
        {
            var xs = new int[0].ToAsyncEnumerable().MaxBy(x => x / 2);

            AssertThrows<Exception>(() => xs.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [TestMethod]
        public void MaxBy3()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MaxBy(x => { if (x == 3) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void MaxBy4()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MaxBy(x => { if (x == 4) throw ex; return x; });

            AssertThrows<Exception>(() => xs.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [TestMethod]
        public void MaxBy5()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().Concat(AsyncEnumerable.Throw<int>(ex)).MaxBy(x => x, Comparer<int>.Default);

            AssertThrows<Exception>(() => xs.Wait(), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}

#endif
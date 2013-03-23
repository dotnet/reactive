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
        public void Hide_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Hide<int>(null));
        }

        [TestMethod]
        public void Hide()
        {
            var xs = new List<int> { 1, 2, 3 };
            var ys = xs.Hide();
            Assert.IsFalse(ys is List<int>);
            Assert.IsTrue(xs.SequenceEqual(ys));
        }

        [TestMethod]
        public void ForEach_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.ForEach<int>(null, x => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.ForEach<int>(new[] { 1 }, default(Action<int>)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.ForEach<int>(null, (x, i) => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.ForEach<int>(new[] { 1 }, default(Action<int, int>)));
        }

        [TestMethod]
        public void ForEach1()
        {
            var n = 0;
            Enumerable.Range(5, 3).ForEach(x => n += x);
            Assert.AreEqual(5 + 6 + 7, n);
        }

        [TestMethod]
        public void ForEach2()
        {
            var n = 0;
            Enumerable.Range(5, 3).ForEach((x, i) => n += x * i);
            Assert.AreEqual(5 * 0 + 6 * 1 + 7 * 2, n);
        }

        [TestMethod]
        public void Buffer_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Buffer<int>(null, 5));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Buffer<int>(null, 5, 3));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Buffer<int>(new[] { 1 }, 0));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Buffer<int>(new[] { 1 }, 5, 0));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.Buffer<int>(new[] { 1 }, 0, 3));
        }

        [TestMethod]
        public void Buffer1()
        {
            var rng = Enumerable.Range(0, 10);

            var res = rng.Buffer(3).ToList();
            Assert.AreEqual(4, res.Count);

            Assert.IsTrue(res[0].SequenceEqual(new[] { 0, 1, 2 }));
            Assert.IsTrue(res[1].SequenceEqual(new[] { 3, 4, 5 }));
            Assert.IsTrue(res[2].SequenceEqual(new[] { 6, 7, 8 }));
            Assert.IsTrue(res[3].SequenceEqual(new[] { 9 }));
        }

        [TestMethod]
        public void Buffer2()
        {
            var rng = Enumerable.Range(0, 10);

            var res = rng.Buffer(5).ToList();
            Assert.AreEqual(2, res.Count);

            Assert.IsTrue(res[0].SequenceEqual(new[] { 0, 1, 2, 3, 4 }));
            Assert.IsTrue(res[1].SequenceEqual(new[] { 5, 6, 7, 8, 9 }));
        }

        [TestMethod]
        public void Buffer3()
        {
            var rng = Enumerable.Empty<int>();

            var res = rng.Buffer(5).ToList();
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void Buffer4()
        {
            var rng = Enumerable.Range(0, 10);

            var res = rng.Buffer(3, 2).ToList();
            Assert.AreEqual(5, res.Count);

            Assert.IsTrue(res[0].SequenceEqual(new[] { 0, 1, 2 }));
            Assert.IsTrue(res[1].SequenceEqual(new[] { 2, 3, 4 }));
            Assert.IsTrue(res[2].SequenceEqual(new[] { 4, 5, 6 }));
            Assert.IsTrue(res[3].SequenceEqual(new[] { 6, 7, 8 }));
            Assert.IsTrue(res[4].SequenceEqual(new[] { 8, 9 }));
        }

        [TestMethod]
        public void Buffer5()
        {
            var rng = Enumerable.Range(0, 10);

            var res = rng.Buffer(3, 4).ToList();
            Assert.AreEqual(3, res.Count);

            Assert.IsTrue(res[0].SequenceEqual(new[] { 0, 1, 2 }));
            Assert.IsTrue(res[1].SequenceEqual(new[] { 4, 5, 6 }));
            Assert.IsTrue(res[2].SequenceEqual(new[] { 8, 9 }));
        }

        [TestMethod]
        public void Do_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(null, _ => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(null, _ => { }, () => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(null, _ => { }, _ => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(null, _ => { }, _ => { }, () => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, default(Action<int>)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, default(Action<int>), () => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, _ => { }, default(Action)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, default(Action<int>), _ => { }, () => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, _ => { }, default(Action<Exception>), () => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, _ => { }, _ => { }, default(Action)));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, default(Action<int>), _ => { }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, _ => { }, default(Action<Exception>)));
#if !NO_RXINTERFACES
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(null, new MyObserver()));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Do<int>(new[] { 1 }, default(IObserver<int>)));
#endif
        }

        [TestMethod]
        public void Do1()
        {
            var n = 0;
            Enumerable.Range(0, 10).Do(x => n += x).ForEach(_ => { });
            Assert.AreEqual(45, n);
        }

        [TestMethod]
        public void Do2()
        {
            var n = 0;
            Enumerable.Range(0, 10).Do(x => n += x, () => n *= 2).ForEach(_ => { });
            Assert.AreEqual(90, n);
        }

        [TestMethod]
        public void Do3()
        {
            var ex = new MyException();
            var ok = false;
            AssertThrows<MyException>(() =>
                EnumerableEx.Throw<int>(ex).Do(x => { Assert.Fail(); }, e => { Assert.AreEqual(ex, e); ok = true; }).ForEach(_ => { })
            );
            Assert.IsTrue(ok);
        }

#if !NO_RXINTERFACES
        [TestMethod]
        public void Do4()
        {
            var obs = new MyObserver();
            Enumerable.Range(0, 10).Do(obs).ForEach(_ => { });

            Assert.IsTrue(obs.Done);
            Assert.AreEqual(45, obs.Sum);
        }

        class MyObserver : IObserver<int>
        {
            public int Sum;
            public bool Done;

            public void OnCompleted()
            {
                Done = true;
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(int value)
            {
                Sum += value;
            }
        }
#endif

        [TestMethod]
        public void Do5()
        {
            var sum = 0;
            var done = false;
            Enumerable.Range(0, 10).Do(x => sum += x, ex => { throw ex; }, () => done = true).ForEach(_ => { });

            Assert.IsTrue(done);
            Assert.AreEqual(45, sum);
        }

        [TestMethod]
        public void StartWith_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.StartWith<int>(null, 5));
        }

        [TestMethod]
        public void StartWith1()
        {
            var e = Enumerable.Range(1, 5);
            var r = e.StartWith(0).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(r, Enumerable.Range(0, 6)));
        }

        [TestMethod]
        public void StartWith2()
        {
            var oops = false;
            var e = Enumerable.Range(1, 5).Do(_ => oops = true);
            var r = e.StartWith(0).Take(1).ToList();
            Assert.IsFalse(oops);
        }

        [TestMethod]
        public void Expand_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Expand<int>(null, _ => new[] { _ }));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Expand<int>(new[] { 1 }, null));
        }

        [TestMethod]
        public void Expand1()
        {
            var res = new[] { 0 }.Expand(x => new[] { x + 1 }).Take(10).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(res, Enumerable.Range(0, 10)));
        }

        [TestMethod]
        public void Expand2()
        {
            var res = new[] { 3 }.Expand(x => Enumerable.Range(0, x)).ToList();
            var exp = new[] {
                3,
                0, 1, 2,
                0,
                0, 1,
                0
            };
            Assert.IsTrue(Enumerable.SequenceEqual(res, exp));
        }

        [TestMethod]
        public void Distinct_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Distinct<int, int>(null, _ => _));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Distinct<int, int>(new[] { 1 }, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Distinct<int, int>(null, _ => _, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Distinct<int, int>(new[] { 1 }, null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Distinct<int, int>(new[] { 1 }, _ => _, null));
        }

        [TestMethod]
        public void Distinct1()
        {
            var res = Enumerable.Range(0, 10).Distinct(x => x % 5).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(res, Enumerable.Range(0, 5)));
        }

        [TestMethod]
        public void Distinct2()
        {
            var res = Enumerable.Range(0, 10).Distinct(x => x % 5, new MyEqualityComparer()).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 0, 1 }));
        }

        class MyEqualityComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x % 2 == y % 2;
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(obj % 2);
            }
        }

        [TestMethod]
        public void DistinctUntilChanged_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int>(null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int>(null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int>(new[] { 1 }, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int, int>(null, _ => _));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int, int>(new[] { 1 }, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int, int>(null, _ => _, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int, int>(new[] { 1 }, null, EqualityComparer<int>.Default));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.DistinctUntilChanged<int, int>(new[] { 1 }, _ => _, null));
        }

        [TestMethod]
        public void DistinctUntilChanged1()
        {
            var res = new[] { 1, 2, 2, 3, 3, 3, 2, 2, 1 }.DistinctUntilChanged().ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 1, 2, 3, 2, 1 }));
        }

        [TestMethod]
        public void DistinctUntilChanged2()
        {
            var res = new[] { 1, 1, 2, 3, 4, 5, 5, 6, 7 }.DistinctUntilChanged(x => x / 2).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 1, 2, 4, 6 }));
        }

        [TestMethod]
        public void IgnoreElements_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.IgnoreElements<int>(null));
        }

        [TestMethod]
        public void IgnoreElements()
        {
            var n = 0;
            Enumerable.Range(0, 10).Do(_ => n++).IgnoreElements().Take(5).ForEach(_ => { });
            Assert.AreEqual(10, n);
        }

        [TestMethod]
        public void TakeLast_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.TakeLast<int>(null, 5));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.TakeLast<int>(new[] { 1 }, -1));
        }

        [TestMethod]
        public void TakeLast_Empty()
        {
            var e = Enumerable.Empty<int>();
            var r = e.TakeLast(1).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(r, e));
        }

        [TestMethod]
        public void TakeLast_All()
        {
            var e = Enumerable.Range(0, 5);
            var r = e.TakeLast(5).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(r, e));
        }

        [TestMethod]
        public void TakeLast_Part()
        {
            var e = Enumerable.Range(0, 5);
            var r = e.TakeLast(3).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(r, e.Skip(2)));
        }

        [TestMethod]
        public void SkipLast_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.SkipLast<int>(null, 5));
            AssertThrows<ArgumentOutOfRangeException>(() => EnumerableEx.SkipLast<int>(new[] { 1 }, -1));
        }

        [TestMethod]
        public void SkipLast_Empty()
        {
            var e = Enumerable.Empty<int>();
            var r = e.SkipLast(1).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(r, e));
        }

        [TestMethod]
        public void SkipLast_All()
        {
            var e = Enumerable.Range(0, 5);
            var r = e.SkipLast(0).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(r, e));
        }

        [TestMethod]
        public void SkipLast_Part()
        {
            var e = Enumerable.Range(0, 5);
            var r = e.SkipLast(3).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(r, e.Take(2)));
        }

        [TestMethod]
        public void Scan_Arguments()
        {
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Scan<int>(null, (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Scan<int>(new[] { 1 }, null));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Scan<int, int>(null, 0, (x, y) => x + y));
            AssertThrows<ArgumentNullException>(() => EnumerableEx.Scan<int, int>(new[] { 1 }, 0, null));
        }

        [TestMethod]
        public void Scan1()
        {
            var res = Enumerable.Range(0, 5).Scan((n, x) => n + x).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 1, 3, 6, 10 }));
        }

        [TestMethod]
        public void Scan2()
        {
            var res = Enumerable.Range(0, 5).Scan(10, (n, x) => n - x).ToList();
            Assert.IsTrue(Enumerable.SequenceEqual(res, new[] { 10, 9, 7, 4, 0 }));
        }
    }
}

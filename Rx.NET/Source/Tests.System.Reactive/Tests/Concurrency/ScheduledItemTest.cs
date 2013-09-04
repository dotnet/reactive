// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests
{
    [TestClass]
    public class ScheduledItemTest : ReactiveTest
    {
        [TestMethod]
        public void ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledItem<DateTimeOffset, int>(default(IScheduler), 42, (x, y) => Disposable.Empty, DateTimeOffset.Now));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledItem<DateTimeOffset, int>(Scheduler.Default, 42, default(Func<IScheduler, int, IDisposable>), DateTimeOffset.Now));

            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledItem<DateTimeOffset, int>(default(IScheduler), 42, (x, y) => Disposable.Empty, DateTimeOffset.Now, Comparer<DateTimeOffset>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledItem<DateTimeOffset, int>(Scheduler.Default, 42, default(Func<IScheduler, int, IDisposable>), DateTimeOffset.Now, Comparer<DateTimeOffset>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledItem<DateTimeOffset, int>(Scheduler.Default, 42, (x, y) => Disposable.Empty, DateTimeOffset.Now, default(IComparer<DateTimeOffset>)));
        }

        [TestMethod]
        public void Inequalities()
        {
            var si1 = new SI(42);
            var si2 = new SI(43);
            var si3 = new SI(42);

            Assert.IsTrue(si1 < si2);
            Assert.IsFalse(si1 < si3);
            Assert.IsTrue(si1 <= si2);
            Assert.IsTrue(si1 <= si3);
            Assert.IsTrue(si2 > si1);
            Assert.IsFalse(si3 > si1);
            Assert.IsTrue(si2 >= si1);
            Assert.IsTrue(si3 >= si1);

            Assert.IsTrue(si1.CompareTo(si2) < 0);
            Assert.IsTrue(si2.CompareTo(si1) > 0);
            Assert.IsTrue(si1.CompareTo(si1) == 0);
            Assert.IsTrue(si1.CompareTo(si3) == 0);

            Assert.IsTrue(si2 > null);
            Assert.IsTrue(si2 >= null);
            Assert.IsFalse(si2 < null);
            Assert.IsFalse(si2 <= null);
            Assert.IsTrue(null < si1);
            Assert.IsTrue(null <= si1);
            Assert.IsFalse(null > si1);
            Assert.IsFalse(null >= si1);

            Assert.IsTrue(si1.CompareTo(null) > 0);

            var si4 = new SI2(43, -1);
            var si5 = new SI2(44, -1);

            Assert.IsTrue(si4 > si1);
            Assert.IsTrue(si4 >= si1);
            Assert.IsTrue(si1 < si4);
            Assert.IsTrue(si1 <= si4);
            Assert.IsFalse(si4 > si2);
            Assert.IsTrue(si4 >= si2);
            Assert.IsFalse(si2 < si4);
            Assert.IsTrue(si2 <= si4);

            Assert.IsTrue(si5 > si4);
            Assert.IsTrue(si5 >= si4);
            Assert.IsFalse(si4 > si5);
            Assert.IsFalse(si4 >= si5);
            Assert.IsTrue(si4 < si5);
            Assert.IsTrue(si4 <= si5);
            Assert.IsFalse(si5 < si4);
            Assert.IsFalse(si5 <= si4);
        }

        [TestMethod]
        public void Equalities()
        {
            var si1 = new SI2(42, 123);
            var si2 = new SI2(42, 123);
            var si3 = new SI2(42, 321);
            var si4 = new SI2(43, 123);

#pragma warning disable 1718
            Assert.IsFalse(si1 != si1);
            Assert.IsTrue(si1 == si1);
#pragma warning restore 1718
            Assert.IsTrue(si1.Equals(si1));

            Assert.IsTrue(si1 != si2);
            Assert.IsFalse(si1 == si2);
            Assert.IsFalse(si1.Equals(si2));

            Assert.IsTrue(si1 != null);
            Assert.IsTrue(null != si1);
            Assert.IsFalse(si1 == null);
            Assert.IsFalse(null == si1);

            Assert.AreEqual(si1.GetHashCode(), si1.GetHashCode());
            Assert.AreNotEqual(si1.GetHashCode(), si2.GetHashCode());
            Assert.AreNotEqual(si1.GetHashCode(), si3.GetHashCode());
        }

        class SI : ScheduledItem<int>
        {
            public SI(int dueTime)
                : base(dueTime, Comparer<int>.Default)
            {
            }

            protected override IDisposable InvokeCore()
            {
                throw new NotImplementedException();
            }
        }

        class SI2 : ScheduledItem<int>
        {
            private readonly int _value;

            public SI2(int dueTime, int value)
                : base(dueTime, Comparer<int>.Default)
            {
                _value = value;
            }

            protected override IDisposable InvokeCore()
            {
                throw new NotImplementedException();
            }
        }
    }
}

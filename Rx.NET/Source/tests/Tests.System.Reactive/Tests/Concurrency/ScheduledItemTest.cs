// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests
{

    public class ScheduledItemTest : ReactiveTest
    {
        [Fact]
        public void ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledItem<DateTimeOffset, int>(default, 42, (x, y) => Disposable.Empty, DateTimeOffset.Now));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledItem<DateTimeOffset, int>(Scheduler.Default, 42, default, DateTimeOffset.Now));

            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledItem<DateTimeOffset, int>(default, 42, (x, y) => Disposable.Empty, DateTimeOffset.Now, Comparer<DateTimeOffset>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledItem<DateTimeOffset, int>(Scheduler.Default, 42, default, DateTimeOffset.Now, Comparer<DateTimeOffset>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ScheduledItem<DateTimeOffset, int>(Scheduler.Default, 42, (x, y) => Disposable.Empty, DateTimeOffset.Now, default));
        }

        [Fact]
        public void Inequalities()
        {
            var si1 = new SI(42);
            var si2 = new SI(43);
            var si3 = new SI(42);

            Assert.True(si1 < si2);
            Assert.False(si1 < si3);
            Assert.True(si1 <= si2);
            Assert.True(si1 <= si3);
            Assert.True(si2 > si1);
            Assert.False(si3 > si1);
            Assert.True(si2 >= si1);
            Assert.True(si3 >= si1);

            Assert.True(si1.CompareTo(si2) < 0);
            Assert.True(si2.CompareTo(si1) > 0);
            Assert.True(si1.CompareTo(si1) == 0);
            Assert.True(si1.CompareTo(si3) == 0);

            Assert.True(si2 > null);
            Assert.True(si2 >= null);
            Assert.False(si2 < null);
            Assert.False(si2 <= null);
            Assert.True(null < si1);
            Assert.True(null <= si1);
            Assert.False(null > si1);
            Assert.False(null >= si1);

            Assert.True(si1.CompareTo(null) > 0);

            var si4 = new SI2(43, -1);
            var si5 = new SI2(44, -1);

            Assert.True(si4 > si1);
            Assert.True(si4 >= si1);
            Assert.True(si1 < si4);
            Assert.True(si1 <= si4);
            Assert.False(si4 > si2);
            Assert.True(si4 >= si2);
            Assert.False(si2 < si4);
            Assert.True(si2 <= si4);

            Assert.True(si5 > si4);
            Assert.True(si5 >= si4);
            Assert.False(si4 > si5);
            Assert.False(si4 >= si5);
            Assert.True(si4 < si5);
            Assert.True(si4 <= si5);
            Assert.False(si5 < si4);
            Assert.False(si5 <= si4);
        }

        [Fact]
        public void Equalities()
        {
            var si1 = new SI2(42, 123);
            var si2 = new SI2(42, 123);
            var si3 = new SI2(42, 321);
            var si4 = new SI2(43, 123);

#pragma warning disable 1718
            Assert.False(si1 != si1);
            Assert.True(si1 == si1);
#pragma warning restore 1718
            Assert.True(si1.Equals(si1));

            Assert.True(si1 != si2);
            Assert.False(si1 == si2);
            Assert.False(si1.Equals(si2));

            Assert.True(si1 != null);
            Assert.True(null != si1);
            Assert.False(si1 == null);
            Assert.False(null == si1);

            Assert.Equal(si1.GetHashCode(), si1.GetHashCode());
            Assert.NotEqual(si1.GetHashCode(), si2.GetHashCode());
            Assert.NotEqual(si1.GetHashCode(), si3.GetHashCode());
        }

        private class SI : ScheduledItem<int>
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

        private class SI2 : ScheduledItem<int>
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

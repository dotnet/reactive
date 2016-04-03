// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;


// https://github.com/Reactive-Extensions/Rx.NET/issues/134

namespace ReactiveTests.Tests
{
    [TestClass]
    public class WeakObservableTest : ReactiveTest
    {
        [TestMethod]
        public void ToWeakObservable_Complete()
        {
            var scheduler = new TestScheduler();

            long lastItem = -1;
            var xs = Observable.Interval(TimeSpan.FromSeconds(1), scheduler);
            xs.ToWeakObservable()
                .Subscribe(m => lastItem = m);

            scheduler.AdvanceBy(TimeSpan.FromSeconds(2));

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            scheduler.AdvanceBy(TimeSpan.FromSeconds(2));

            Assert.AreEqual(1, lastItem);
        }
    }
}

// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ObservableExtensionsTest : ReactiveTest
    {
        #region Subscribe

        [TestMethod]
        public void Subscribe_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), _ => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, default(Action<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), _ => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, default(Action<int>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, _ => { }, default(Action)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), _ => { }, (Exception _) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, default(Action<int>), (Exception _) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, _ => { }, default(Action<Exception>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default(IObservable<int>), _ => { }, (Exception _) => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, default(Action<int>), (Exception _) => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, _ => { }, default(Action<Exception>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(someObservable, _ => { }, (Exception _) => { }, default(Action)));
        }

        [TestMethod]
        public void Subscribe_None_Return()
        {
            Observable.Return(1, Scheduler.Immediate).Subscribe();
        }

        [TestMethod]
        public void Subscribe_None_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            ReactiveAssert.Throws(ex, () => xs.Subscribe());
        }

        [TestMethod]
        public void Subscribe_None_Empty()
        {
            Observable.Empty<int>(Scheduler.Immediate).Subscribe((int _) => { Assert.Fail(); });
        }

        [TestMethod]
        public void Subscribe_OnNext_Return()
        {
            int _x = -1;
            Observable.Return(42, Scheduler.Immediate).Subscribe((int x) => { _x = x; });
            Assert.AreEqual(42, _x);
        }

        [TestMethod]
        public void Subscribe_OnNext_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            ReactiveAssert.Throws(ex, () => xs.Subscribe(_ => { Assert.Fail(); }));
        }

        [TestMethod]
        public void Subscribe_OnNext_Empty()
        {
            Observable.Empty<int>(Scheduler.Immediate).Subscribe((int _) => { Assert.Fail(); });
        }

        [TestMethod]
        public void Subscribe_OnNextOnCompleted_Return()
        {
            bool finished = false;
            int _x = -1;
            Observable.Return(42, Scheduler.Immediate).Subscribe((int x) => { _x = x; }, () => { finished = true; });
            Assert.AreEqual(42, _x);
            Assert.IsTrue(finished);
        }

        [TestMethod]
        public void Subscribe_OnNextOnCompleted_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            ReactiveAssert.Throws(ex, () => xs.Subscribe(_ => { Assert.Fail(); }, () => { Assert.Fail(); }));
        }

        [TestMethod]
        public void Subscribe_OnNextOnCompleted_Empty()
        {
            bool finished = false;
            Observable.Empty<int>(Scheduler.Immediate).Subscribe((int _) => { Assert.Fail(); }, () => { finished = true; });
            Assert.IsTrue(finished);
        }

        #endregion
    }
}

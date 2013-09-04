// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Reactive.Testing;
using System.Reactive;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class AnonymousTest
    {
        [TestMethod]
        public void AnonymousObservable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObservable<int>(null));
        }

        [TestMethod]
        public void AnonymousObserver_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(default(Action<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(default(Action<int>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(x => { }, default(Action)));

            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(default(Action<int>), ex => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(x => { }, default(Action<Exception>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(default(Action<int>), ex => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(x => { }, default(Action<Exception>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(x => { }, ex => { }, default(Action)));
        }

        [TestMethod]
        public void AnonymousObserver_Error_Null()
        {
            var observer = new AnonymousObserver<int>(_ => { }, e => { }, () => { });
            ReactiveAssert.Throws<ArgumentNullException>(() => observer.OnError(null));
        }
    }
}

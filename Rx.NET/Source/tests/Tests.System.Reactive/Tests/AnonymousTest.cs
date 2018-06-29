// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public class AnonymousTest
    {
        [Fact]
        public void AnonymousObservable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObservable<int>(null));
        }

        [Fact]
        public void AnonymousObserver_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(default));

            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(default, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(x => { }, default(Action)));

            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(default, ex => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(x => { }, default(Action<Exception>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(default, ex => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(x => { }, default, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => new AnonymousObserver<int>(x => { }, ex => { }, default));
        }

        [Fact]
        public void AnonymousObserver_Error_Null()
        {
            var observer = new AnonymousObserver<int>(_ => { }, e => { }, () => { });
            ReactiveAssert.Throws<ArgumentNullException>(() => observer.OnError(null));
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
#pragma warning disable IDE0039 // Use local function
    [TestClass]
    public class CombineLatestTest : ReactiveTest
    {

        #region ArgumentChecking

        [TestMethod]
        public void CombineLatest_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest<int, int, int>(DummyObservable<int>.Instance, DummyObservable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest<int, int, int>(null, DummyObservable<int>.Instance, (_, __) => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest<int, int, int>(DummyObservable<int>.Instance, null, (_, __) => 0));
        }

        [TestMethod]
        public void CombineLatest_ArgumentCheckingHighArity()
        {
            var xs = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, (_0, _1) => _0 + _1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), (_0, _1) => _0 + _1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(Func<int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, (_0, _1, _2) => _0 + _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, (_0, _1, _2) => _0 + _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), (_0, _1, _2) => _0 + _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(Func<int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(Func<int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>)));
        }

        #endregion

        #region Never

        [TestMethod]
        public void CombineLatest_NeverN()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest([e0, e1, e2], xs => xs.Sum())
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, (_0, _1) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never2Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, (_0, _1, _2) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never3Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never4Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never5Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never6Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never7Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never8Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never9Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never10Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never11Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never12Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never13Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never14Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never15Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e15 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [TestMethod]
        public void CombineLatest_Never16Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1)]);
            var e15 = scheduler.CreateHotObservable([OnNext(150, 1)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        #endregion

        #region Never/Empty

        [TestMethod]
        public void CombineLatest_NeverEmpty()
        {
            var scheduler = new TestScheduler();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var res = scheduler.Start(() =>
                n.CombineLatest(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void CombineLatest_EmptyNever()
        {
            var scheduler = new TestScheduler();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                e.CombineLatest(n, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        #endregion

        #region Empty

        [TestMethod]
        public void CombineLatest_EmptyN()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest([e0, e1, e2], xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, (_0, _1) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(220)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty2Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int)>(220)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, (_0, _1, _2) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty3Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int)>(230)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(240)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty4Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int)>(240)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty5Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int)>(250)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(260)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty6Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int)>(260)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(270)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty7Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int)>(270)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(280)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty8Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int)>(280)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(290)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty9Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int)>(290)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty10Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int)>(300)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(310)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty11Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int, int)>(310)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(320)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty12Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int, int, int)>(320)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(330)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty13Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int, int, int, int)>(330)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(340)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty14Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int, int, int, int, int)>(340)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(350)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty15Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int)>(350)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
            var e15 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(360)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(360)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        [TestMethod]
        public void CombineLatest_Empty16Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(210)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(220)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(230)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(240)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
            var e15 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(360)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int)>(360)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            }
        }

        #endregion

        #region Empty/Return

        [TestMethod]
        public void CombineLatest_EmptyReturn()
        {
            var scheduler = new TestScheduler();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                e.CombineLatest(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(215)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [TestMethod]
        public void CombineLatest_ReturnEmpty()
        {
            var scheduler = new TestScheduler();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                o.CombineLatest(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(215)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        #endregion

        #region Never/Return

        [TestMethod]
        public void CombineLatest_NeverReturn()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                n.CombineLatest(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void CombineLatest_ReturnNever()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                o.CombineLatest(n, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        #endregion

        #region Return/Return

        [TestMethod]
        public void CombineLatest_ReturnReturn()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2 + 3),
                OnCompleted<int>(240)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        #endregion

        #region Empty/Error

        [TestMethod]
        public void CombineLatest_EmptyError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_ErrorEmpty()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var f = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                f.CombineLatest(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Return/Throw

        [TestMethod]
        public void CombineLatest_ReturnThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_ThrowReturn()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Throw/Throw

        [TestMethod]
        public void CombineLatest_ThrowThrow()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex2)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex1)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_ErrorThrow()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(220, ex1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex2)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex1)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_ThrowError()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(220, ex1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex2)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex1)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Never/Throw

        [TestMethod]
        public void CombineLatest_NeverThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_ThrowNever()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Some/Throw

        [TestMethod]
        public void CombineLatest_SomeThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_ThrowSome()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region ThrowAfterCompleted

        [TestMethod]
        public void CombineLatest_ThrowAfterCompleteLeft()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void CombineLatest_ThrowAfterCompleteRight()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        #endregion

        #region Basics

        [TestMethod]
        public void CombineLatest_InterleavedWithTail()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnNext(230, 5),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2 + 3),
                OnNext(225, 3 + 4),
                OnNext(230, 4 + 5),
                OnNext(235, 4 + 6),
                OnNext(240, 4 + 7),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void CombineLatest_Consecutive()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(235, 4 + 6),
                OnNext(240, 4 + 7),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void CombineLatest_ConsecutiveEndWithErrorLeft()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void CombineLatest_ConsecutiveEndWithErrorRight()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(250)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnError<int>(245, ex)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(235, 4 + 6),
                OnNext(240, 4 + 7),
                OnError<int>(245, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 245)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 245)
            );
        }

        #endregion

        #region SelectorThrows

        [TestMethod]
        public void CombineLatest_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(240)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                o2.CombineLatest<int, int, int>(o1, (x, y) => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_SelectorThrowsN()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<IList<int>, int> f = xs => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest([e0, e1, e2], f)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            var es = new[] { e0, e1, e2 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, (_0, _1) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            var es = new[] { e0, e1 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, (_0, _1, _2) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            var es = new[] { e0, e1, e2 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, (_0, _1, _2, _3) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(240, ex)
            );

            var es = new[] { e0, e1, e2, e3 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(260, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(270, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(280, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(290, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(310, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(320, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(330, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(340, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(350, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400)]);
            var e15 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(360, 16), OnCompleted<int>(400)]);

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(360, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            }
        }

        #endregion

        #region AllEmptyButOne

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombineN()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest([e0, e1, e2], xs => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, (_0, _1) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine2Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, (_0, _1, _2) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine3Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine4Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine5Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine6Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine7Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine8Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine9Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine10Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine11Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine12Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(360)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine13Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(360)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int, int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(360)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(370)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine14Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(360)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(370)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int, int, int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(360)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(370)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(380)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine15Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(360)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(370)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(380)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(360)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(370)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(380)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(390)]);
            var e15 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine16Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(250)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(260)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(270)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(280)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(290)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(300)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(310)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(320)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(330)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(340)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(350)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(360)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(370)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(380)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnCompleted<int>(390)]);
            var e15 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15)
            );

            res.Messages.AssertEqual(
                OnCompleted<(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int)>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        #endregion

        #region Typical

        [TestMethod]
        public void CombineLatest_TypicalN()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 4), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 5), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 6), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest([e0, e1, e2], xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnNext(230, 6),
                OnNext(410, 9),
                OnNext(420, 12),
                OnNext(430, 15),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 3), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 4), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, (_0, _1) => _0 + _1)
            );

            res.Messages.AssertEqual(
                OnNext(220, 3),
                OnNext(410, 5),
                OnNext(420, 7),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical2Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 3), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 4), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1)
            );

            res.Messages.AssertEqual(
                OnNext(220, (1, 2)),
                OnNext(410, (3, 2)),
                OnNext(420, (3, 4)),
                OnCompleted<(int First, int Second)>(800)
            );

            foreach (var e in new[] { e0, e1 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 4), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 5), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 6), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, (_0, _1, _2) => _0 + _1 + _2)
            );

            res.Messages.AssertEqual(
                OnNext(230, 6),
                OnNext(410, 9),
                OnNext(420, 12),
                OnNext(430, 15),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical3Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 4), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 5), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 6), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnNext(230, (1, 2, 3)),
                OnNext(410, (4, 2, 3)),
                OnNext(420, (4, 5, 3)),
                OnNext(430, (4, 5, 6)),
                OnCompleted<(int First, int Second, int Third)>(800)
            );

            foreach (var e in new[] { e0, e1, e2 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 5), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 6), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 7), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 8), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, (_0, _1, _2, _3) => _0 + _1 + _2 + _3)
            );

            res.Messages.AssertEqual(
                OnNext(240, 10),
                OnNext(410, 14),
                OnNext(420, 18),
                OnNext(430, 22),
                OnNext(440, 26),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical4Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 5), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 6), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 7), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 8), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3)
            );

            res.Messages.AssertEqual(
                OnNext(240, (1, 2, 3, 4)),
                OnNext(410, (5, 2, 3, 4)),
                OnNext(420, (5, 6, 3, 4)),
                OnNext(430, (5, 6, 7, 4)),
                OnNext(440, (5, 6, 7, 8)),
                OnCompleted<(int First, int Second, int Third, int Fourth)>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 6), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 7), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 8), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 9), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 10), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4)
            );

            res.Messages.AssertEqual(
                OnNext(250, 15),
                OnNext(410, 20),
                OnNext(420, 25),
                OnNext(430, 30),
                OnNext(440, 35),
                OnNext(450, 40),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical5Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 6), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 7), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 8), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 9), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 10), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4)
            );

            res.Messages.AssertEqual(
                OnNext(250, (1, 2, 3, 4, 5)),
                OnNext(410, (6, 2, 3, 4, 5)),
                OnNext(420, (6, 7, 3, 4, 5)),
                OnNext(430, (6, 7, 8, 4, 5)),
                OnNext(440, (6, 7, 8, 9, 5)),
                OnNext(450, (6, 7, 8, 9, 10)),
                OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth)>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 7), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 8), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 9), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 10), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 11), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 12), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5)
            );

            res.Messages.AssertEqual(
                OnNext(260, 21),
                OnNext(410, 27),
                OnNext(420, 33),
                OnNext(430, 39),
                OnNext(440, 45),
                OnNext(450, 51),
                OnNext(460, 57),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical6Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 7), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 8), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 9), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 10), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 11), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 12), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5)
            );

            res.Messages.AssertEqual(
                OnNext(260, (1, 2, 3, 4, 5, 6)),
                OnNext(410, (7, 2, 3, 4, 5, 6)),
                OnNext(420, (7, 8, 3, 4, 5, 6)),
                OnNext(430, (7, 8, 9, 4, 5, 6)),
                OnNext(440, (7, 8, 9, 10, 5, 6)),
                OnNext(450, (7, 8, 9, 10, 11, 6)),
                OnNext(460, (7, 8, 9, 10, 11, 12)),
                OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth, int Sixth)>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 8), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 9), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 10), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 11), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 12), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 13), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 14), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6)
            );

            res.Messages.AssertEqual(
                OnNext(270, 28),
                OnNext(410, 35),
                OnNext(420, 42),
                OnNext(430, 49),
                OnNext(440, 56),
                OnNext(450, 63),
                OnNext(460, 70),
                OnNext(470, 77),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical7Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 8), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 9), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 10), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 11), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 12), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 13), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 14), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6)
            );

            res.Messages.AssertEqual(
                OnNext(270, (1, 2, 3, 4, 5, 6, 7)),
                OnNext(410, (8, 2, 3, 4, 5, 6, 7)),
                OnNext(420, (8, 9, 3, 4, 5, 6, 7)),
                OnNext(430, (8, 9, 10, 4, 5, 6, 7)),
                OnNext(440, (8, 9, 10, 11, 5, 6, 7)),
                OnNext(450, (8, 9, 10, 11, 12, 6, 7)),
                OnNext(460, (8, 9, 10, 11, 12, 13, 7)),
                OnNext(470, (8, 9, 10, 11, 12, 13, 14)),
                OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh)>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 9), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 10), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 11), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 12), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 13), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 14), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 15), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 16), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7)
            );

            res.Messages.AssertEqual(
                OnNext(280, 36),
                OnNext(410, 44),
                OnNext(420, 52),
                OnNext(430, 60),
                OnNext(440, 68),
                OnNext(450, 76),
                OnNext(460, 84),
                OnNext(470, 92),
                OnNext(480, 100),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical8Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 9), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 10), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 11), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 12), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 13), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 14), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 15), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 16), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7)
            );

            res.Messages.AssertEqual(
                OnNext(280, (1, 2, 3, 4, 5, 6, 7, 8)),
                OnNext(410, (9, 2, 3, 4, 5, 6, 7, 8)),
                OnNext(420, (9, 10, 3, 4, 5, 6, 7, 8)),
                OnNext(430, (9, 10, 11, 4, 5, 6, 7, 8)),
                OnNext(440, (9, 10, 11, 12, 5, 6, 7, 8)),
                OnNext(450, (9, 10, 11, 12, 13, 6, 7, 8)),
                OnNext(460, (9, 10, 11, 12, 13, 14, 7, 8)),
                OnNext(470, (9, 10, 11, 12, 13, 14, 15, 8)),
                OnNext(480, (9, 10, 11, 12, 13, 14, 15, 16)),
                OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh, int Eighth)>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 10), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 11), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 12), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 13), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 14), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 15), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 16), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 17), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 18), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8)
            );

            res.Messages.AssertEqual(
                OnNext(290, 45),
                OnNext(410, 54),
                OnNext(420, 63),
                OnNext(430, 72),
                OnNext(440, 81),
                OnNext(450, 90),
                OnNext(460, 99),
                OnNext(470, 108),
                OnNext(480, 117),
                OnNext(490, 126),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical9Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 10), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 11), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 12), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 13), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 14), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 15), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 16), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 17), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 18), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8)
            );

            res.Messages.AssertEqual(
                OnNext(290, (1, 2, 3, 4, 5, 6, 7, 8, 9)),
                OnNext(410, (10, 2, 3, 4, 5, 6, 7, 8, 9)),
                OnNext(420, (10, 11, 3, 4, 5, 6, 7, 8, 9)),
                OnNext(430, (10, 11, 12, 4, 5, 6, 7, 8, 9)),
                OnNext(440, (10, 11, 12, 13, 5, 6, 7, 8, 9)),
                OnNext(450, (10, 11, 12, 13, 14, 6, 7, 8, 9)),
                OnNext(460, (10, 11, 12, 13, 14, 15, 7, 8, 9)),
                OnNext(470, (10, 11, 12, 13, 14, 15, 16, 8, 9)),
                OnNext(480, (10, 11, 12, 13, 14, 15, 16, 17, 9)),
                OnNext(490, (10, 11, 12, 13, 14, 15, 16, 17, 18)),
                OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh, int Eighth, int Ninth)>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 11), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 12), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 13), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 14), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 15), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 16), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 17), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 18), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 19), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 20), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9)
            );

            res.Messages.AssertEqual(
                OnNext(300, 55),
                OnNext(410, 65),
                OnNext(420, 75),
                OnNext(430, 85),
                OnNext(440, 95),
                OnNext(450, 105),
                OnNext(460, 115),
                OnNext(470, 125),
                OnNext(480, 135),
                OnNext(490, 145),
                OnNext(500, 155),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical10Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 11), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 12), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 13), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 14), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 15), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 16), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 17), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 18), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 19), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 20), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9)
            );

            res.Messages.AssertEqual(
                OnNext(300, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10)),
                OnNext(410, (11, 2, 3, 4, 5, 6, 7, 8, 9, 10)),
                OnNext(420, (11, 12, 3, 4, 5, 6, 7, 8, 9, 10)),
                OnNext(430, (11, 12, 13, 4, 5, 6, 7, 8, 9, 10)),
                OnNext(440, (11, 12, 13, 14, 5, 6, 7, 8, 9, 10)),
                OnNext(450, (11, 12, 13, 14, 15, 6, 7, 8, 9, 10)),
                OnNext(460, (11, 12, 13, 14, 15, 16, 7, 8, 9, 10)),
                OnNext(470, (11, 12, 13, 14, 15, 16, 17, 8, 9, 10)),
                OnNext(480, (11, 12, 13, 14, 15, 16, 17, 18, 9, 10)),
                OnNext(490, (11, 12, 13, 14, 15, 16, 17, 18, 19, 10)),
                OnNext(500, (11, 12, 13, 14, 15, 16, 17, 18, 19, 20)),
                OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh, int Eighth, int Ninth, int Tenth)>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 12), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 13), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 14), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 15), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 16), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 17), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 18), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 19), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 20), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 21), OnCompleted<int>(800)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnNext(510, 22), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10)
            );

            res.Messages.AssertEqual(
                OnNext(310, 66),
                OnNext(410, 77),
                OnNext(420, 88),
                OnNext(430, 99),
                OnNext(440, 110),
                OnNext(450, 121),
                OnNext(460, 132),
                OnNext(470, 143),
                OnNext(480, 154),
                OnNext(490, 165),
                OnNext(500, 176),
                OnNext(510, 187),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical11Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 12), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 13), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 14), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 15), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 16), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 17), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 18), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 19), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 20), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 21), OnCompleted<int>(800)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnNext(510, 22), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10)
            );

            res.Messages.AssertEqual(
                OnNext(310, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11)),
                OnNext(410, (12, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11)),
                OnNext(420, (12, 13, 3, 4, 5, 6, 7, 8, 9, 10, 11)),
                OnNext(430, (12, 13, 14, 4, 5, 6, 7, 8, 9, 10, 11)),
                OnNext(440, (12, 13, 14, 15, 5, 6, 7, 8, 9, 10, 11)),
                OnNext(450, (12, 13, 14, 15, 16, 6, 7, 8, 9, 10, 11)),
                OnNext(460, (12, 13, 14, 15, 16, 17, 7, 8, 9, 10, 11)),
                OnNext(470, (12, 13, 14, 15, 16, 17, 18, 8, 9, 10, 11)),
                OnNext(480, (12, 13, 14, 15, 16, 17, 18, 19, 9, 10, 11)),
                OnNext(490, (12, 13, 14, 15, 16, 17, 18, 19, 20, 10, 11)),
                OnNext(500, (12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 11)),
                OnNext(510, (12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22)),
                OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh, int Eighth, int Ninth, int Tenth, int Eleventh)>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 13), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 14), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 15), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 16), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 17), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 18), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 19), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 20), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 21), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 22), OnCompleted<int>(800)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnNext(510, 23), OnCompleted<int>(800)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnNext(520, 24), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11)
            );

            res.Messages.AssertEqual(
                OnNext(320, 78),
                OnNext(410, 90),
                OnNext(420, 102),
                OnNext(430, 114),
                OnNext(440, 126),
                OnNext(450, 138),
                OnNext(460, 150),
                OnNext(470, 162),
                OnNext(480, 174),
                OnNext(490, 186),
                OnNext(500, 198),
                OnNext(510, 210),
                OnNext(520, 222),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical12Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 13), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 14), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 15), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 16), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 17), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 18), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 19), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 20), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 21), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 22), OnCompleted<int>(800)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnNext(510, 23), OnCompleted<int>(800)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnNext(520, 24), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11)
            );

            res.Messages.AssertEqual(
                OnNext(320, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)),
                OnNext(410, (13, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)),
                OnNext(420, (13, 14, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)),
                OnNext(430, (13, 14, 15, 4, 5, 6, 7, 8, 9, 10, 11, 12)),
                OnNext(440, (13, 14, 15, 16, 5, 6, 7, 8, 9, 10, 11, 12)),
                OnNext(450, (13, 14, 15, 16, 17, 6, 7, 8, 9, 10, 11, 12)),
                OnNext(460, (13, 14, 15, 16, 17, 18, 7, 8, 9, 10, 11, 12)),
                OnNext(470, (13, 14, 15, 16, 17, 18, 19, 8, 9, 10, 11, 12)),
                OnNext(480, (13, 14, 15, 16, 17, 18, 19, 20, 9, 10, 11, 12)),
                OnNext(490, (13, 14, 15, 16, 17, 18, 19, 20, 21, 10, 11, 12)),
                OnNext(500, (13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 11, 12)),
                OnNext(510, (13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 12)),
                OnNext(520, (13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24)),
                OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh, int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth)>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 14), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 15), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 16), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 17), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 18), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 19), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 20), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 21), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 22), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 23), OnCompleted<int>(800)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnNext(510, 24), OnCompleted<int>(800)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnNext(520, 25), OnCompleted<int>(800)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(330, 13), OnNext(530, 26), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12)
            );

            res.Messages.AssertEqual(
                OnNext(330, 91),
                OnNext(410, 104),
                OnNext(420, 117),
                OnNext(430, 130),
                OnNext(440, 143),
                OnNext(450, 156),
                OnNext(460, 169),
                OnNext(470, 182),
                OnNext(480, 195),
                OnNext(490, 208),
                OnNext(500, 221),
                OnNext(510, 234),
                OnNext(520, 247),
                OnNext(530, 260),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical13Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 14), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 15), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 16), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 17), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 18), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 19), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 20), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 21), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 22), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 23), OnCompleted<int>(800)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnNext(510, 24), OnCompleted<int>(800)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnNext(520, 25), OnCompleted<int>(800)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(330, 13), OnNext(530, 26), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12)
            );

            res.Messages.AssertEqual(
                OnNext(330, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)),
                OnNext(410, (14, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)),
                OnNext(420, (14, 15, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)),
                OnNext(430, (14, 15, 16, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)),
                OnNext(440, (14, 15, 16, 17, 5, 6, 7, 8, 9, 10, 11, 12, 13)),
                OnNext(450, (14, 15, 16, 17, 18, 6, 7, 8, 9, 10, 11, 12, 13)),
                OnNext(460, (14, 15, 16, 17, 18, 19, 7, 8, 9, 10, 11, 12, 13)),
                OnNext(470, (14, 15, 16, 17, 18, 19, 20, 8, 9, 10, 11, 12, 13)),
                OnNext(480, (14, 15, 16, 17, 18, 19, 20, 21, 9, 10, 11, 12, 13)),
                OnNext(490, (14, 15, 16, 17, 18, 19, 20, 21, 22, 10, 11, 12, 13)),
                OnNext(500, (14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 11, 12, 13)),
                OnNext(510, (14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 12, 13)),
                OnNext(520, (14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 13)),
                OnNext(530, (14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26)),
                OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh, int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth, int Thirteenth)>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 15), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 16), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 17), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 18), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 19), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 20), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 21), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 22), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 23), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 24), OnCompleted<int>(800)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnNext(510, 25), OnCompleted<int>(800)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnNext(520, 26), OnCompleted<int>(800)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(330, 13), OnNext(530, 27), OnCompleted<int>(800)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(340, 14), OnNext(540, 28), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13)
            );

            res.Messages.AssertEqual(
                OnNext(340, 105),
                OnNext(410, 119),
                OnNext(420, 133),
                OnNext(430, 147),
                OnNext(440, 161),
                OnNext(450, 175),
                OnNext(460, 189),
                OnNext(470, 203),
                OnNext(480, 217),
                OnNext(490, 231),
                OnNext(500, 245),
                OnNext(510, 259),
                OnNext(520, 273),
                OnNext(530, 287),
                OnNext(540, 301),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical14Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 15), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 16), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 17), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 18), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 19), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 20), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 21), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 22), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 23), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 24), OnCompleted<int>(800)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnNext(510, 25), OnCompleted<int>(800)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnNext(520, 26), OnCompleted<int>(800)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(330, 13), OnNext(530, 27), OnCompleted<int>(800)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(340, 14), OnNext(540, 28), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13)
            );

            res.Messages.AssertEqual(
                OnNext(340, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)),
                OnNext(410, (15, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)),
                OnNext(420, (15, 16, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)),
                OnNext(430, (15, 16, 17, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)),
                OnNext(440, (15, 16, 17, 18, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)),
                OnNext(450, (15, 16, 17, 18, 19, 6, 7, 8, 9, 10, 11, 12, 13, 14)),
                OnNext(460, (15, 16, 17, 18, 19, 20, 7, 8, 9, 10, 11, 12, 13, 14)),
                OnNext(470, (15, 16, 17, 18, 19, 20, 21, 8, 9, 10, 11, 12, 13, 14)),
                OnNext(480, (15, 16, 17, 18, 19, 20, 21, 22, 9, 10, 11, 12, 13, 14)),
                OnNext(490, (15, 16, 17, 18, 19, 20, 21, 22, 23, 10, 11, 12, 13, 14)),
                OnNext(500, (15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 11, 12, 13, 14)),
                OnNext(510, (15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 12, 13, 14)),
                OnNext(520, (15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 13, 14)),
                OnNext(530, (15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 14)),
                OnNext(540, (15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28)),
                OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh, int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth, int Thirteenth, int Fourteenth)>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 16), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 17), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 18), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 19), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 20), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 21), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 22), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 23), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 24), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 25), OnCompleted<int>(800)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnNext(510, 26), OnCompleted<int>(800)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnNext(520, 27), OnCompleted<int>(800)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(330, 13), OnNext(530, 28), OnCompleted<int>(800)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(340, 14), OnNext(540, 29), OnCompleted<int>(800)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(350, 15), OnNext(550, 30), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14)
            );

            res.Messages.AssertEqual(
                OnNext(350, 120),
                OnNext(410, 135),
                OnNext(420, 150),
                OnNext(430, 165),
                OnNext(440, 180),
                OnNext(450, 195),
                OnNext(460, 210),
                OnNext(470, 225),
                OnNext(480, 240),
                OnNext(490, 255),
                OnNext(500, 270),
                OnNext(510, 285),
                OnNext(520, 300),
                OnNext(530, 315),
                OnNext(540, 330),
                OnNext(550, 345),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical15Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 16), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 17), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 18), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 19), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 20), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 21), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 22), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 23), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 24), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 25), OnCompleted<int>(800)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnNext(510, 26), OnCompleted<int>(800)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnNext(520, 27), OnCompleted<int>(800)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(330, 13), OnNext(530, 28), OnCompleted<int>(800)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(340, 14), OnNext(540, 29), OnCompleted<int>(800)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(350, 15), OnNext(550, 30), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14)
            );

            res.Messages.AssertEqual(
                OnNext(350, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)),
                OnNext(410, (16, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)),
                OnNext(420, (16, 17, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)),
                OnNext(430, (16, 17, 18, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)),
                OnNext(440, (16, 17, 18, 19, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)),
                OnNext(450, (16, 17, 18, 19, 20, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)),
                OnNext(460, (16, 17, 18, 19, 20, 21, 7, 8, 9, 10, 11, 12, 13, 14, 15)),
                OnNext(470, (16, 17, 18, 19, 20, 21, 22, 8, 9, 10, 11, 12, 13, 14, 15)),
                OnNext(480, (16, 17, 18, 19, 20, 21, 22, 23, 9, 10, 11, 12, 13, 14, 15)),
                OnNext(490, (16, 17, 18, 19, 20, 21, 22, 23, 24, 10, 11, 12, 13, 14, 15)),
                OnNext(500, (16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 11, 12, 13, 14, 15)),
                OnNext(510, (16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 12, 13, 14, 15)),
                OnNext(520, (16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 13, 14, 15)),
                OnNext(530, (16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 14, 15)),
                OnNext(540, (16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 15)),
                OnNext(550, (16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30)),
                OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh, int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth, int Thirteenth, int Fourteenth, int Fifteenth)>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 17), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 18), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 19), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 20), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 21), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 22), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 23), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 24), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 25), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 26), OnCompleted<int>(800)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnNext(510, 27), OnCompleted<int>(800)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnNext(520, 28), OnCompleted<int>(800)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(330, 13), OnNext(530, 29), OnCompleted<int>(800)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(340, 14), OnNext(540, 30), OnCompleted<int>(800)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(350, 15), OnNext(550, 31), OnCompleted<int>(800)]);
            var e15 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(360, 16), OnNext(560, 32), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15)
            );

            res.Messages.AssertEqual(
                OnNext(360, 136),
                OnNext(410, 152),
                OnNext(420, 168),
                OnNext(430, 184),
                OnNext(440, 200),
                OnNext(450, 216),
                OnNext(460, 232),
                OnNext(470, 248),
                OnNext(480, 264),
                OnNext(490, 280),
                OnNext(500, 296),
                OnNext(510, 312),
                OnNext(520, 328),
                OnNext(530, 344),
                OnNext(540, 360),
                OnNext(550, 376),
                OnNext(560, 392),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        [TestMethod]
        public void CombineLatest_Typical16Tuple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(410, 17), OnCompleted<int>(800)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(420, 18), OnCompleted<int>(800)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(430, 19), OnCompleted<int>(800)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnNext(440, 20), OnCompleted<int>(800)]);
            var e4 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(250, 5), OnNext(450, 21), OnCompleted<int>(800)]);
            var e5 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(260, 6), OnNext(460, 22), OnCompleted<int>(800)]);
            var e6 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(270, 7), OnNext(470, 23), OnCompleted<int>(800)]);
            var e7 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(280, 8), OnNext(480, 24), OnCompleted<int>(800)]);
            var e8 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(290, 9), OnNext(490, 25), OnCompleted<int>(800)]);
            var e9 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(300, 10), OnNext(500, 26), OnCompleted<int>(800)]);
            var e10 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(310, 11), OnNext(510, 27), OnCompleted<int>(800)]);
            var e11 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(320, 12), OnNext(520, 28), OnCompleted<int>(800)]);
            var e12 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(330, 13), OnNext(530, 29), OnCompleted<int>(800)]);
            var e13 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(340, 14), OnNext(540, 30), OnCompleted<int>(800)]);
            var e14 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(350, 15), OnNext(550, 31), OnCompleted<int>(800)]);
            var e15 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(360, 16), OnNext(560, 32), OnCompleted<int>(800)]);

            var res = scheduler.Start(() =>
                ObservableEx.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15)
            );

            res.Messages.AssertEqual(
                OnNext(360, (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)),
                OnNext(410, (17, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)),
                OnNext(420, (17, 18, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)),
                OnNext(430, (17, 18, 19, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)),
                OnNext(440, (17, 18, 19, 20, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)),
                OnNext(450, (17, 18, 19, 20, 21, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)),
                OnNext(460, (17, 18, 19, 20, 21, 22, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)),
                OnNext(470, (17, 18, 19, 20, 21, 22, 23, 8, 9, 10, 11, 12, 13, 14, 15, 16)),
                OnNext(480, (17, 18, 19, 20, 21, 22, 23, 24, 9, 10, 11, 12, 13, 14, 15, 16)),
                OnNext(490, (17, 18, 19, 20, 21, 22, 23, 24, 25, 10, 11, 12, 13, 14, 15, 16)),
                OnNext(500, (17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 11, 12, 13, 14, 15, 16)),
                OnNext(510, (17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 12, 13, 14, 15, 16)),
                OnNext(520, (17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 13, 14, 15, 16)),
                OnNext(530, (17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 14, 15, 16)),
                OnNext(540, (17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 15, 16)),
                OnNext(550, (17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 16)),
                OnNext(560, (17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32)),
                OnCompleted<(int First, int Second, int Third, int Fourth, int Fifth, int Sixth, int Seventh, int Eighth, int Ninth, int Tenth, int Eleventh, int Twelfth, int Thirteenth, int Fourteenth, int Fifteenth, int Sixteenth)>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
            }
        }

        #endregion

        #region NAry

        [TestMethod]
        public void CombineLatest_List_Regular()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(240, 4), OnCompleted<int>(270)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(250, 5), OnCompleted<int>(280)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(290)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(new IObservable<int>[] { e0, e1, e2 }.AsEnumerable())
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual([1, 2, 3])),
                OnNext<IList<int>>(240, l => l.SequenceEqual([4, 2, 3])),
                OnNext<IList<int>>(250, l => l.SequenceEqual([4, 5, 3])),
                OnNext<IList<int>>(260, l => l.SequenceEqual([4, 5, 6])),
                OnCompleted<IList<int>>(290)
            );
        }

        [TestMethod]
        public void CombineLatest_NAry_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IEnumerable<IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IEnumerable<IObservable<int>>), _ => 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest([Observable.Return(42)], default(Func<IList<int>, string>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>[])));
        }

        [TestMethod]
        public void CombineLatest_NAry_Symmetric()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(420)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual([1, 2, 3])),
                OnNext<IList<int>>(240, l => l.SequenceEqual([1, 5, 3])),
                OnNext<IList<int>>(250, l => l.SequenceEqual([4, 5, 3])),
                OnNext<IList<int>>(260, l => l.SequenceEqual([4, 5, 6])),
                OnCompleted<IList<int>>(420)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void CombineLatest_NAry_Symmetric_Selector()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(420)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest([e0, e1, e2], xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnNext(230, new[] { 1, 2, 3 }.Sum()),
                OnNext(240, new[] { 1, 5, 3 }.Sum()),
                OnNext(250, new[] { 4, 5, 3 }.Sum()),
                OnNext(260, new[] { 4, 5, 6 }.Sum()),
                OnCompleted<int>(420)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void CombineLatest_NAry_Asymmetric()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(270)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnNext(290, 7), OnNext(310, 9), OnCompleted<int>(410)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnNext(280, 8), OnCompleted<int>(300)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual([1, 2, 3])),
                OnNext<IList<int>>(240, l => l.SequenceEqual([1, 5, 3])),
                OnNext<IList<int>>(250, l => l.SequenceEqual([4, 5, 3])),
                OnNext<IList<int>>(260, l => l.SequenceEqual([4, 5, 6])),
                OnNext<IList<int>>(280, l => l.SequenceEqual([4, 5, 8])),
                OnNext<IList<int>>(290, l => l.SequenceEqual([4, 7, 8])),
                OnNext<IList<int>>(310, l => l.SequenceEqual([4, 9, 8])),
                OnCompleted<IList<int>>(410)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void CombineLatest_NAry_Asymmetric_Selector()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(270)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnNext(290, 7), OnNext(310, 9), OnCompleted<int>(410)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnNext(280, 8), OnCompleted<int>(300)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest([e0, e1, e2], xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnNext(230, new[] { 1, 2, 3 }.Sum()),
                OnNext(240, new[] { 1, 5, 3 }.Sum()),
                OnNext(250, new[] { 4, 5, 3 }.Sum()),
                OnNext(260, new[] { 4, 5, 6 }.Sum()),
                OnNext(280, new[] { 4, 5, 8 }.Sum()),
                OnNext(290, new[] { 4, 7, 8 }.Sum()),
                OnNext(310, new[] { 4, 9, 8 }.Sum()),
                OnCompleted<int>(410)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void CombineLatest_NAry_Error()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnError<int>(250, ex)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual([1, 2, 3])),
                OnNext<IList<int>>(240, l => l.SequenceEqual([1, 5, 3])),
                OnError<IList<int>>(250, ex)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void CombineLatest_NAry_Error_Selector()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnError<int>(250, ex)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest([e0, e1, e2], xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnNext(230, new[] { 1, 2, 3 }.Sum()),
                OnNext(240, new[] { 1, 5, 3 }.Sum()),
                OnError<int>(250, ex)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        #endregion

        #region AtLeastOneThrows

        [TestMethod]
        public void CombineLatest_AtLeastOneThrows4()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();
            var e0 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400)]);
            var e1 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400)]);
            var e2 = scheduler.CreateHotObservable([OnNext(150, 1), OnError<int>(230, ex)]);
            var e3 = scheduler.CreateHotObservable([OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400)]);

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            e0.Subscriptions.AssertEqual(Subscribe(200, 230));
            e1.Subscriptions.AssertEqual(Subscribe(200, 230));
            e2.Subscriptions.AssertEqual(Subscribe(200, 230));
            e3.Subscriptions.AssertEqual(Subscribe(200, 230));
        }

        #endregion

    }
#pragma warning restore IDE0039 // Use local function
}

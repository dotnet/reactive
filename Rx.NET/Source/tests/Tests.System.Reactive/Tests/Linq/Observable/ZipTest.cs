// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
#pragma warning disable IDE0039 // Use local function
    public class ZipTest : ReactiveTest
    {

        #region ArgumentChecking

        [Fact]
        public void Zip_ArgumentChecking()
        {
            var someObservable = DummyObservable<int>.Instance;
            var someEnumerable = DummyEnumerable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip<int, int, int>(someObservable, someObservable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip<int, int, int>(null, someObservable, (_, __) => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(someObservable, default(IObservable<int>), (_, __) => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip<int, int, int>(someObservable, someEnumerable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip<int, int, int>(null, someEnumerable, (_, __) => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(someObservable, default(IEnumerable<int>), (_, __) => 0));
        }

        [Fact]
        public void Zip_ArgumentCheckingHighArity()
        {
            var xs = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, (_0, _1) => _0 + _1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), (_0, _1) => _0 + _1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(Func<int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, (_0, _1, _2) => _0 + _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, (_0, _1, _2) => _0 + _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), (_0, _1, _2) => _0 + _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(Func<int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(Func<int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
        }

        #endregion

        #region Never/Never

        [Fact]
        public void Zip_Never2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, (_0, _1) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, (_0, _1, _2) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(
                () => Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            }
        }

        [Fact]
        public void Zip_Never16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
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

        [Fact]
        public void Zip_NeverEmpty()
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
                n.Zip(e, (x, y) => x + y)
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

        [Fact]
        public void Zip_EmptyNever()
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
                e.Zip(n, (x, y) => x + y)
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

        #region Empty/Empty

        [Fact]
        public void Zip_EmptyEmpty()
        {
            var scheduler = new TestScheduler();

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var e2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var res = scheduler.Start(() =>
                e1.Zip(e2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Zip_Empty2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, (_0, _1) => 42)
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

        [Fact]
        public void Zip_Empty3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, (_0, _1, _2) => 42)
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

        [Fact]
        public void Zip_Empty4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
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

        [Fact]
        public void Zip_Empty5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
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

        [Fact]
        public void Zip_Empty6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
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

        [Fact]
        public void Zip_Empty7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
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

        [Fact]
        public void Zip_Empty8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
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

        [Fact]
        public void Zip_Empty9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
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

        [Fact]
        public void Zip_Empty10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
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

        [Fact]
        public void Zip_Empty11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
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

        [Fact]
        public void Zip_Empty12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
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

        [Fact]
        public void Zip_Empty13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
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

        [Fact]
        public void Zip_Empty14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
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

        [Fact]
        public void Zip_Empty15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
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

        [Fact]
        public void Zip_Empty16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(360) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
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

        #endregion

        #region Empty/Some

        [Fact]
        public void Zip_EmptyNonEmpty()
        {
            var scheduler = new TestScheduler();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2), // Intended behavior - will only know here there was no error and we can complete gracefully
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                e.Zip(o, (x, y) => x + y)
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

        [Fact]
        public void Zip_NonEmptyEmpty()
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
                o.Zip(e, (x, y) => x + y)
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

        #region Never/Some

        [Fact]
        public void Zip_NeverNonEmpty()
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
                n.Zip(o, (x, y) => x + y)
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

        [Fact]
        public void Zip_NonEmptyNever()
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
                o.Zip(n, (x, y) => x + y)
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

        #region Some/Some

        [Fact]
        public void Zip_NonEmptyNonEmpty()
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
                OnCompleted<int>(240) // Intended behavior - will only know here there was no error and we can complete gracefully
            );

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
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

        [Fact]
        public void Zip_EmptyError()
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
                e.Zip(f, (x, y) => x + y)
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

        [Fact]
        public void Zip_ErrorEmpty()
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
                f.Zip(e, (x, y) => x + y)
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

        #region Never/Error

        [Fact]
        public void Zip_NeverError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var f = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                n.Zip(f, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void Zip_ErrorNever()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var f = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                f.Zip(n, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Error/Error

        [Fact]
        public void Zip_ErrorError()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var f1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex1)
            );

            var f2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex2)
            );

            var res = scheduler.Start(() =>
                f1.Zip(f2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex2)
            );

            f1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            f2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Some/Error

        [Fact]
        public void Zip_SomeError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void Zip_ErrorSome()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                e.Zip(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Simple

        [Fact]
        public void Zip_LeftCompletesFirst()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(220)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 4),
                OnCompleted<int>(225)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(215, 6),
                OnCompleted<int>(225)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void Zip_RightCompletesFirst()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 4),
                OnCompleted<int>(225)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(215, 6),
                OnCompleted<int>(225)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void Zip_LeftTriggersSelectorError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 4)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => { if (x == y) { return 42; } throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void Zip_RightTriggersSelectorError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 4)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => { if (x == y) { return 42; } throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region SymmetricReturn

        [Fact]
        public void Zip_SymmetricReturn2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, (_0, _1) => _0 + _1)
            );

            res.Messages.AssertEqual(
                OnNext(220, 3),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, (_0, _1, _2) => _0 + _1 + _2)
            );

            res.Messages.AssertEqual(
                OnNext(230, 6),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => _0 + _1 + _2 + _3)
            );

            res.Messages.AssertEqual(
                OnNext(240, 10),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4)
            );

            res.Messages.AssertEqual(
                OnNext(250, 15),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5)
            );

            res.Messages.AssertEqual(
                OnNext(260, 21),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6)
            );

            res.Messages.AssertEqual(
                OnNext(270, 28),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7)
            );

            res.Messages.AssertEqual(
                OnNext(280, 36),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8)
            );

            res.Messages.AssertEqual(
                OnNext(290, 45),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9)
            );

            res.Messages.AssertEqual(
                OnNext(300, 55),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10)
            );

            res.Messages.AssertEqual(
                OnNext(310, 66),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11)
            );

            res.Messages.AssertEqual(
                OnNext(320, 78),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12)
            );

            res.Messages.AssertEqual(
                OnNext(330, 91),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13)
            );

            res.Messages.AssertEqual(
                OnNext(340, 105),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14)
            );

            res.Messages.AssertEqual(
                OnNext(350, 120),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        [Fact]
        public void Zip_SymmetricReturn16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(360, 16), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15)
            );

            res.Messages.AssertEqual(
                OnNext(360, 136),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
            }
        }

        #endregion

        #region Various

        [Fact]
        public void Zip_SomeDataAsymmetric1()
        {
            var scheduler = new TestScheduler();

            var msgs1 = Enumerable.Range(0, 5).Select((x, i) => OnNext((ushort)(205 + i * 5), x)).ToArray();
            var msgs2 = Enumerable.Range(0, 10).Select((x, i) => OnNext((ushort)(202 + i * 8), x)).ToArray();

            var len = Math.Min(msgs1.Length, msgs2.Length);

            var o1 = scheduler.CreateHotObservable(msgs1);
            var o2 = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
            );

            Assert.True(len == res.Messages.Count, "length");
            for (var i = 0; i < len; i++)
            {
                var sum = msgs1[i].Value.Value + msgs2[i].Value.Value;
                var time = Math.Max(msgs1[i].Time, msgs2[i].Time);

                Assert.True(res.Messages[i].Time == time);
                Assert.True(res.Messages[i].Value.Kind == NotificationKind.OnNext);
                Assert.True(res.Messages[i].Value.Value == sum, i.ToString());
            }
        }

        [Fact]
        public void Zip_SomeDataAsymmetric2()
        {
            var scheduler = new TestScheduler();

            var msgs1 = Enumerable.Range(0, 10).Select((x, i) => OnNext((ushort)(205 + i * 5), x)).ToArray();
            var msgs2 = Enumerable.Range(0, 5).Select((x, i) => OnNext((ushort)(202 + i * 8), x)).ToArray();

            var len = Math.Min(msgs1.Length, msgs2.Length);

            var o1 = scheduler.CreateHotObservable(msgs1);
            var o2 = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
            );

            Assert.True(len == res.Messages.Count, "length");
            for (var i = 0; i < len; i++)
            {
                var sum = msgs1[i].Value.Value + msgs2[i].Value.Value;
                var time = Math.Max(msgs1[i].Time, msgs2[i].Time);

                Assert.True(res.Messages[i].Time == time);
                Assert.True(res.Messages[i].Value.Kind == NotificationKind.OnNext);
                Assert.True(res.Messages[i].Value.Value == sum, i.ToString());
            }
        }

        [Fact]
        public void Zip_SomeDataSymmetric()
        {
            var scheduler = new TestScheduler();

            var msgs1 = Enumerable.Range(0, 10).Select((x, i) => OnNext((ushort)(205 + i * 5), x)).ToArray();
            var msgs2 = Enumerable.Range(0, 10).Select((x, i) => OnNext((ushort)(202 + i * 8), x)).ToArray();

            var len = Math.Min(msgs1.Length, msgs2.Length);

            var o1 = scheduler.CreateHotObservable(msgs1);
            var o2 = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
            );

            Assert.True(len == res.Messages.Count, "length");
            for (var i = 0; i < len; i++)
            {
                var sum = msgs1[i].Value.Value + msgs2[i].Value.Value;
                var time = Math.Max(msgs1[i].Time, msgs2[i].Time);

                Assert.True(res.Messages[i].Time == time);
                Assert.True(res.Messages[i].Value.Kind == NotificationKind.OnNext);
                Assert.True(res.Messages[i].Value.Value == sum, i.ToString());
            }
        }

        #endregion

        #region SelectorThrows

        [Fact]
        public void Zip_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(240)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnNext(230, 5), //!
                OnCompleted<int>(250)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) =>
                {
                    if (y == 5)
                    {
                        throw ex;
                    }

                    return x + y;
                })
            );

            res.Messages.AssertEqual(
                OnNext(220, 2 + 3),
                OnError<int>(230, ex)
            );
        }

        [Fact]
        public void Zip_SelectorThrows2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, (_0, _1) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            var es = new[] { e0, e1 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, (_0, _1, _2) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            var es = new[] { e0, e1, e2 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(240, ex)
            );

            var es = new[] { e0, e1, e2, e3 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(260, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(270, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(280, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(290, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(310, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(320, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(330, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(340, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(350, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        [Fact]
        public void Zip_SelectorThrows16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(360, 16), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(360, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };
            foreach (var e in es)
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
            }
        }

        #endregion

        #region GetEnumeratorThrows

        [Fact]
        public void Zip_GetEnumeratorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 42),
                OnNext(220, 43),
                OnCompleted<int>(230)
            );

            var ys = new RogueEnumerable<int>(ex);

            var res = scheduler.Start(() =>
                xs.Zip(ys, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        #endregion

        #region AllCompleted

        [Fact]
        public void Zip_AllCompleted2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, (_0, _1) => _0 + _1)
            );

            res.Messages.AssertEqual(
                OnNext(210, 10),
                OnCompleted<int>(220)
            );

            var es = new[] { e0, e1 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, (_0, _1, _2) => _0 + _1 + _2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 15),
                OnCompleted<int>(230)
            );

            var es = new[] { e0, e1, e2 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => _0 + _1 + _2 + _3)
            );

            res.Messages.AssertEqual(
                OnNext(210, 20),
                OnCompleted<int>(240)
            );

            var es = new[] { e0, e1, e2, e3 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4)
            );

            res.Messages.AssertEqual(
                OnNext(210, 25),
                OnCompleted<int>(250)
            );

            var es = new[] { e0, e1, e2, e3, e4 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5)
            );

            res.Messages.AssertEqual(
                OnNext(210, 30),
                OnCompleted<int>(260)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6)
            );

            res.Messages.AssertEqual(
                OnNext(210, 35),
                OnCompleted<int>(270)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7)
            );

            res.Messages.AssertEqual(
                OnNext(210, 40),
                OnCompleted<int>(280)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8)
            );

            res.Messages.AssertEqual(
                OnNext(210, 45),
                OnCompleted<int>(290)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9)
            );

            res.Messages.AssertEqual(
                OnNext(210, 50),
                OnCompleted<int>(300)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnCompleted<int>(320) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10)
            );

            res.Messages.AssertEqual(
                OnNext(210, 55),
                OnCompleted<int>(310)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnCompleted<int>(320) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnCompleted<int>(330) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11)
            );

            res.Messages.AssertEqual(
                OnNext(210, 60),
                OnCompleted<int>(320)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnCompleted<int>(320) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnCompleted<int>(330) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnCompleted<int>(340) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12)
            );

            res.Messages.AssertEqual(
                OnNext(210, 65),
                OnCompleted<int>(330)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnCompleted<int>(320) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnCompleted<int>(330) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnCompleted<int>(340) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnNext(340, 18), OnCompleted<int>(350) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13)
            );

            res.Messages.AssertEqual(
                OnNext(210, 70),
                OnCompleted<int>(340)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnCompleted<int>(320) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnCompleted<int>(330) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnCompleted<int>(340) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnNext(340, 18), OnCompleted<int>(350) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnNext(340, 18), OnNext(350, 19), OnCompleted<int>(360) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14)
            );

            res.Messages.AssertEqual(
                OnNext(210, 75),
                OnCompleted<int>(350)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [Fact]
        public void Zip_AllCompleted16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnCompleted<int>(320) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnCompleted<int>(330) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnCompleted<int>(340) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnNext(340, 18), OnCompleted<int>(350) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnNext(340, 18), OnNext(350, 19), OnCompleted<int>(360) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnNext(340, 18), OnNext(350, 19), OnNext(360, 20), OnCompleted<int>(370) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15)
            );

            res.Messages.AssertEqual(
                OnNext(210, 80),
                OnCompleted<int>(360)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
            {
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));
            }

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        #endregion

        #region ZipWithEnumerable

        [Fact]
        public void ZipWithEnumerable_NeverNever()
        {
            var evt = new ManualResetEvent(false);
            var scheduler = new TestScheduler();

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var n2 = new MockEnumerable<int>(scheduler,
                EnumerableNever(evt)
            );

            var res = scheduler.Start(() =>
                n1.Zip(n2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            n2.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            evt.Set();
        }

        [Fact]
        public void ZipWithEnumerable_NeverEmpty()
        {
            var scheduler = new TestScheduler();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var e = new MockEnumerable<int>(scheduler,
                Enumerable.Empty<int>()
            );

            var res = scheduler.Start(() =>
                n.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void ZipWithEnumerable_EmptyNever()
        {
            var evt = new ManualResetEvent(false);

            var scheduler = new TestScheduler();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var n = new MockEnumerable<int>(scheduler,
                EnumerableNever(evt)
            );

            var res = scheduler.Start(() =>
                e.Zip(n, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            evt.Set();
        }

        [Fact]
        public void ZipWithEnumerable_EmptyEmpty()
        {
            var scheduler = new TestScheduler();

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var e2 = new MockEnumerable<int>(scheduler,
                Enumerable.Empty<int>()
            );

            var res = scheduler.Start(() =>
                e1.Zip(e2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void ZipWithEnumerable_EmptyNonEmpty()
        {
            var scheduler = new TestScheduler();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var o = new MockEnumerable<int>(scheduler,
                new[] { 2 }
            );

            var res = scheduler.Start(() =>
                e.Zip(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void ZipWithEnumerable_NonEmptyEmpty()
        {
            var scheduler = new TestScheduler();

            var e = new MockEnumerable<int>(scheduler,
                Enumerable.Empty<int>()
            );

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(215)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [Fact]
        public void ZipWithEnumerable_NeverNonEmpty()
        {
            var scheduler = new TestScheduler();

            var o = new MockEnumerable<int>(scheduler,
                new[] { 2 }
            );

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                n.Zip(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void ZipWithEnumerable_NonEmptyNonEmpty()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var o2 = new MockEnumerable<int>(scheduler,
                new[] { 3 }
            );

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(215, 2 + 3),
                OnCompleted<int>(230)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void ZipWithEnumerable_EmptyError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var f = new MockEnumerable<int>(scheduler,
                ThrowEnumerable(false, ex)
            );

            var res = scheduler.Start(() =>
                e.Zip(f, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void ZipWithEnumerable_ErrorEmpty()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var e = new MockEnumerable<int>(scheduler,
                Enumerable.Empty<int>()
            );

            var f = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                f.Zip(e, (x, y) => x + y)
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

        [Fact]
        public void ZipWithEnumerable_NeverError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var f = new MockEnumerable<int>(scheduler,
                ThrowEnumerable(false, ex)
            );

            var res = scheduler.Start(() =>
                n.Zip(f, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void ZipWithEnumerable_ErrorNever()
        {
            var evt = new ManualResetEvent(false);

            var scheduler = new TestScheduler();

            var ex = new Exception();

            var n = new MockEnumerable<int>(scheduler,
                EnumerableNever(evt)
            );

            var f = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                f.Zip(n, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            evt.Set();
        }

        [Fact]
        public void ZipWithEnumerable_ErrorError()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var f1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex1)
            );

            var f2 = new MockEnumerable<int>(scheduler,
                ThrowEnumerable(false, ex2)
            );

            var res = scheduler.Start(() =>
                f1.Zip(f2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex1)
            );

            f1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            f2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void ZipWithEnumerable_SomeError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var e = new MockEnumerable<int>(scheduler,
                ThrowEnumerable(false, ex)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(215, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [Fact]
        public void ZipWithEnumerable_ErrorSome()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = new MockEnumerable<int>(scheduler,
                new[] { 2 }
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                e.Zip(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void ZipWithEnumerable_SomeDataBothSides()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = new MockEnumerable<int>(scheduler,
                new[] { 5, 4, 3, 2 }
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5)
            );

            var res = scheduler.Start(() =>
                e.Zip(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(210, 7),
                OnNext(220, 7),
                OnNext(230, 7),
                OnNext(240, 7)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void ZipWithEnumerable_EnumeratorThrowsMoveNext()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(240)
            );

            var o2 = new MockEnumerable<int>(scheduler,
                new MyEnumerable(false, ex)
            );

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(215, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [Fact]
        public void ZipWithEnumerable_EnumeratorThrowsCurrent()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(240)
            );

            var o2 = new MockEnumerable<int>(scheduler,
                new MyEnumerable(true, ex)
            );

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(215, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [Fact]
        public void ZipWithEnumerable_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(240)
            );

            var o2 = new MockEnumerable<int>(scheduler,
                new[] { 3, 5 }
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) =>
                {
                    if (y == 5)
                    {
                        throw ex;
                    }

                    return x + y;
                })
            );

            res.Messages.AssertEqual(
                OnNext(215, 2 + 3),
                OnError<int>(225, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void ZipWithEnumerable_NoAsyncDisposeOnMoveNext()
        {
            var source = new Subject<int>();

            var disposable = new SingleAssignmentDisposable();

            var other = new MoveNextDisposeDetectEnumerable(disposable, true);

            disposable.Disposable = source.Zip(other, (a, b) => a + b).Subscribe();

            source.OnNext(1);

            Assert.True(other.IsDisposed);
            Assert.False(other.DisposedWhileMoveNext);
            Assert.False(other.DisposedWhileCurrent);
        }

        [Fact]
        public void ZipWithEnumerable_NoAsyncDisposeOnCurrent()
        {
            var source = new Subject<int>();

            var disposable = new SingleAssignmentDisposable();

            var other = new MoveNextDisposeDetectEnumerable(disposable, false);

            disposable.Disposable = source.Zip(other, (a, b) => a + b).Subscribe();

            source.OnNext(1);

            Assert.True(other.IsDisposed);
            Assert.False(other.DisposedWhileMoveNext);
            Assert.False(other.DisposedWhileCurrent);
        }

        private class MoveNextDisposeDetectEnumerable : IEnumerable<int>, IEnumerator<int>
        {
            readonly IDisposable _disposable;

            readonly bool _disposeOnMoveNext;

            private bool _moveNextRunning;

            private bool _currentRunning;

            internal bool DisposedWhileMoveNext;

            internal bool DisposedWhileCurrent;

            internal bool IsDisposed;

            internal MoveNextDisposeDetectEnumerable(IDisposable disposable, bool disposeOnMoveNext)
            {
                _disposable = disposable;
                _disposeOnMoveNext = disposeOnMoveNext;
            }
            public int Current
            {
                get
                {
                    _currentRunning = true;
                    if (!_disposeOnMoveNext)
                    {
                        _disposable.Dispose();
                    }
                    _currentRunning = false;
                    return 0;
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                DisposedWhileMoveNext = _moveNextRunning;
                DisposedWhileCurrent = _currentRunning;
                IsDisposed = true;
            }

            public IEnumerator<int> GetEnumerator()
            {
                return this;
            }

            public bool MoveNext()
            {
                _moveNextRunning = true;
                if (_disposeOnMoveNext)
                {
                    _disposable.Dispose();
                }
                _moveNextRunning = false;
                return true;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this;
            }
        }

        private IEnumerable<int> EnumerableNever(ManualResetEvent evt)
        {
            evt.WaitOne();
            yield break;
        }

        private IEnumerable<int> ThrowEnumerable(bool b, Exception ex)
        {
            if (!b)
            {
                throw ex;
            }

            yield break;
        }

        private class MyEnumerable : IEnumerable<int>
        {
            private readonly bool _throwInCurrent;
            private readonly Exception _ex;

            public MyEnumerable(bool throwInCurrent, Exception ex)
            {
                _throwInCurrent = throwInCurrent;
                _ex = ex;
            }

            public IEnumerator<int> GetEnumerator()
            {
                return new MyEnumerator(_throwInCurrent, _ex);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private class MyEnumerator : IEnumerator<int>
            {
                private readonly bool _throwInCurrent;
                private readonly Exception _ex;

                public MyEnumerator(bool throwInCurrent, Exception ex)
                {
                    _throwInCurrent = throwInCurrent;
                    _ex = ex;
                }

                public int Current
                {
                    get
                    {
                        if (_throwInCurrent)
                        {
                            throw _ex;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }

                public void Dispose()
                {
                }

                object System.Collections.IEnumerator.Current
                {
                    get { return Current; }
                }

                public bool MoveNext()
                {
                    if (!_throwInCurrent)
                    {
                        throw _ex;
                    }

                    return true;
                }

                public void Reset()
                {
                }
            }
        }

        #endregion

        #region NAry

        [Fact]
        public void Zip_NAry_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IEnumerable<IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IEnumerable<IObservable<int>>), _ => 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(new[] { Observable.Return(42) }, default(Func<IList<int>, string>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>[])));
        }

        [Fact]
        public void Zip_NAry_Symmetric()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(420) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual(new[] { 1, 2, 3 })),
                OnNext<IList<int>>(260, l => l.SequenceEqual(new[] { 4, 5, 6 })),
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

        [Fact]
        public void Zip_NAry_Symmetric_Selector()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(420) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(new[] { e0, e1, e2 }, xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnNext(230, new[] { 1, 2, 3 }.Sum()),
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

        [Fact]
        public void Zip_NAry_Asymmetric()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(270) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnNext(290, 7), OnNext(310, 9), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnNext(280, 8), OnCompleted<int>(300) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual(new[] { 1, 2, 3 })),
                OnNext<IList<int>>(260, l => l.SequenceEqual(new[] { 4, 5, 6 })),
                OnCompleted<IList<int>>(310)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void Zip_NAry_Asymmetric_Selector()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(270) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnNext(290, 7), OnNext(310, 9), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnNext(280, 8), OnCompleted<int>(300) });

            var res = scheduler.Start(() =>
                Observable.Zip(new[] { e0, e1, e2 }, xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnNext(230, new[] { 1, 2, 3 }.Sum()),
                OnNext(260, new[] { 4, 5, 6 }.Sum()),
                OnCompleted<int>(310)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void Zip_NAry_Error()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnError<int>(250, ex) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual(new[] { 1, 2, 3 })),
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

        [Fact]
        public void Zip_NAry_Error_Selector()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnError<int>(250, ex) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(new[] { e0, e1, e2 }, xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnNext(230, new[] { 1, 2, 3 }.Sum()),
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

        [Fact]
        public void Zip_NAry_Enumerable_Simple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(420) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var started = default(long);
            var xss = GetSources(() => started = scheduler.Clock, e0, e1, e2).Select(xs => (IObservable<int>)xs);

            var res = scheduler.Start(() =>
                Observable.Zip(xss)
            );

            Assert.Equal(200, started);

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual(new[] { 1, 2, 3 })),
                OnNext<IList<int>>(260, l => l.SequenceEqual(new[] { 4, 5, 6 })),
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

        [Fact]
        public void Zip_NAry_Enumerable_Throws()
        {
            var ex = new Exception();
            var xss = GetSources(ex, Observable.Return(42));
            var res = Observable.Zip(xss);

            ReactiveAssert.Throws(ex, () => res.Subscribe(_ => { }));
        }

        private IEnumerable<ITestableObservable<int>> GetSources(Action start, params ITestableObservable<int>[] sources)
        {
            start();

            foreach (var xs in sources)
            {
                yield return xs;
            }
        }

        private IEnumerable<IObservable<T>> GetSources<T>(Exception ex, params IObservable<T>[] sources)
        {
            foreach (var xs in sources)
            {
                yield return xs;
            }

            throw ex;
        }

        #endregion

        #region AtLeastOneThrows

        [Fact]
        public void Zip_AtLeastOneThrows4()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnError<int>(230, ex) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
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

        [Fact]
        public void Zip2WithImmediateReturn()
        {
            Observable.Zip<Unit, Unit, Unit>(
                Observable.Return(Unit.Default), 
                Observable.Return(Unit.Default), 
                (_, __) => Unit.Default
            )
            .Subscribe(_ => {  });
        }

        [Fact]
        public void Zip3WithImmediateReturn()
        {
            int result = 0;

            Observable.Zip<int, int, int, int>(
                Observable.Return(1),
                Observable.Return(2),
                Observable.Return(4),
                (a, b, c) => a + b + c
            )
            .Subscribe(v => result = v);

            Assert.Equal(7, result);
        }

        [Fact]
        public void ZipEnumerableWithImmediateReturn()
        {
            Enumerable.Range(0, 100)
                .Select(_ => Observable.Return(Unit.Default))
                .Zip()
                .Subscribe(_ =>
                {

                }
                );
        }
    }
#pragma warning restore IDE0039 // Use local function
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class LetTest : ReactiveTest
    {
        #region Let

        [Fact]
        public void Let_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Let(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Let<int, int>(someObservable, null));
        }

        [Fact]
        public void Let_CallsFunctionImmediately()
        {
            var called = false;
            Observable.Empty<int>().Let(x => { called = true; return x; });
            Assert.True(called);
        }

        #endregion

    }
}

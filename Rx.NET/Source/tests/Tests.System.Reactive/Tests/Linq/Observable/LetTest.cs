// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class LetTest : ReactiveTest
    {
        #region Let

        [TestMethod]
        public void Let_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Let(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Let<int, int>(someObservable, null));
        }

        [TestMethod]
        public void Let_CallsFunctionImmediately()
        {
            var called = false;
            Observable.Empty<int>().Let(x => { called = true; return x; });
            Assert.True(called);
        }

        #endregion

    }
}

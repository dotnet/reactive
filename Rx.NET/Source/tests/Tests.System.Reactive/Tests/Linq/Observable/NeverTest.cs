// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;
using ReactiveTests.Dummies;
using System.Reflection;
using System.Threading;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace ReactiveTests.Tests
{
    public class NeverTest : ReactiveTest
    {

        [Fact]
        public void Never_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Never<int>().Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Never<int>(42).Subscribe(null));
        }

        [Fact]
        public void Never_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never<int>();

            var res = scheduler.CreateObserver<int>();

            xs.Subscribe(res);

            scheduler.Start();

            res.Messages.AssertEqual(
            );
        }

        [Fact]
        public void Never_Basic_Witness()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never<int>(42);

            var res = scheduler.CreateObserver<int>();

            xs.Subscribe(res);

            scheduler.Start();

            res.Messages.AssertEqual(
            );
        }

    }
}

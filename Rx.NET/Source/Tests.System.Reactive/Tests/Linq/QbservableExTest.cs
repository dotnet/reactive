// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !SILVERLIGHTM7

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class QbservableExTest : ReactiveTest
    {
        private IQbservable<int> _qbNull = null;
        private IQbservable<int> _qbMy = new MyQbservable<int>();
        private IQbservableProvider _qbp = new MyQbservableProvider();

        [TestMethod]
        public void ForkJoin_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(_qbNull, _qbMy, (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(_qbMy, _qbNull, (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(_qbMy, _qbMy, default(Expression<Func<int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(default(IQbservableProvider), _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(_qbp, default(IQbservable<int>[])));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(default(IQbservableProvider), new MyQueryable<IObservable<int>>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(_qbp, default(IQueryable<IObservable<int>>)));
        }

        [TestMethod]
        public void ForkJoin()
        {
            _qbMy.ForkJoin(_qbMy, (a, b) => a + b);
            _qbp.ForkJoin(_qbMy, _qbMy);
            _qbp.ForkJoin(new MyQueryable<IObservable<int>>());
        }

        [TestMethod]
        public void Create_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.Create<int>(default(IQbservableProvider), _ => new IObservable<object>[0]));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.Create<int>(_qbp, default(Expression<Func<IObserver<int>, IEnumerable<IObservable<object>>>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.Create(default(IQbservableProvider), () => new IObservable<object>[0]));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.Create(_qbp, null));
        }

        [TestMethod]
        public void Create()
        {
            _qbp.Create<int>(obs => new IObservable<object>[0]);
            _qbp.Create(() => new IObservable<object>[0]);
        }

        [TestMethod]
        public void Let_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.Let(_qbNull, xs => xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.Let(_qbMy, default(Expression<Func<IObservable<int>, IObservable<int>>>)));
        }

        [TestMethod]
        public void Let()
        {
            _qbMy.Let(xs => xs);
        }
    }
}

#endif
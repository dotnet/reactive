// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public class QbservableExTest : ReactiveTest
    {
        private readonly IQbservable<int> _qbNull = null;
        private IQbservable<int> _qbMy = new MyQbservable<int>();
        private IQbservableProvider _qbp = new MyQbservableProvider();

        [Fact]
        public void ForkJoin_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(_qbNull, _qbMy, (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(_qbMy, _qbNull, (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(_qbMy, _qbMy, default(Expression<Func<int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(default, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(_qbp, default(IQbservable<int>[])));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(default, new MyQueryable<IObservable<int>>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.ForkJoin(_qbp, default(IQueryable<IObservable<int>>)));
        }

        [Fact]
        public void ForkJoin()
        {
            _qbMy.ForkJoin(_qbMy, (a, b) => a + b);
            _qbp.ForkJoin(_qbMy, _qbMy);
            _qbp.ForkJoin(new MyQueryable<IObservable<int>>());
        }

        [Fact]
        public void Create_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.Create<int>(default, _ => new IObservable<object>[0]));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.Create<int>(_qbp, default));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.Create(default, () => new IObservable<object>[0]));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.Create(_qbp, null));
        }

        [Fact]
        public void Create()
        {
            _qbp.Create<int>(obs => new IObservable<object>[0]);
            _qbp.Create(() => new IObservable<object>[0]);
        }

        [Fact]
        public void Let_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.Let(_qbNull, xs => xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => QbservableEx.Let(_qbMy, default(Expression<Func<IObservable<int>, IObservable<int>>>)));
        }

        [Fact]
        public void Let()
        {
            _qbMy.Let(xs => xs);
        }
    }
}

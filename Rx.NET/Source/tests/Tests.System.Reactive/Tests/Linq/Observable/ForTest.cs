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
    public class ForTest : ReactiveTest
    {

        [Fact]
        public void For_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.For(DummyEnumerable<int>.Instance, default(Func<int, IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.For(null, DummyFunc<int, IObservable<int>>.Instance));
        }

        [Fact]
        public void For_Basic()
        {
            var scheduler = new TestScheduler();

            var results = scheduler.Start(() => Observable.For(new[] { 1, 2, 3 }, x => scheduler.CreateColdObservable(
                OnNext<int>((ushort)(x * 100 + 10), x * 10 + 1),
                OnNext<int>((ushort)(x * 100 + 20), x * 10 + 2),
                OnNext<int>((ushort)(x * 100 + 30), x * 10 + 3),
                OnCompleted<int>((ushort)(x * 100 + 40)))));

            results.Messages.AssertEqual(
                OnNext(310, 11),
                OnNext(320, 12),
                OnNext(330, 13),
                OnNext(550, 21),
                OnNext(560, 22),
                OnNext(570, 23),
                OnNext(890, 31),
                OnNext(900, 32),
                OnNext(910, 33),
                OnCompleted<int>(920)
            );
        }

        IEnumerable<int> For_Error_Core(Exception ex)
        {
            yield return 1;
            yield return 2;
            yield return 3;
            throw ex;
        }

        [Fact]
        public void For_Error_Iterator()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.For(For_Error_Core(ex), x => scheduler.CreateColdObservable(
                OnNext<int>((ushort)(x * 100 + 10), x * 10 + 1),
                OnNext<int>((ushort)(x * 100 + 20), x * 10 + 2),
                OnNext<int>((ushort)(x * 100 + 30), x * 10 + 3),
                OnCompleted<int>((ushort)(x * 100 + 40)))));

            results.Messages.AssertEqual(
                OnNext(310, 11),
                OnNext(320, 12),
                OnNext(330, 13),
                OnNext(550, 21),
                OnNext(560, 22),
                OnNext(570, 23),
                OnNext(890, 31),
                OnNext(900, 32),
                OnNext(910, 33),
                OnError<int>(920, ex)
            );
        }

        [Fact]
        public void For_Error_Source()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.For(new[] { 1, 2, 3 }, x => Observable.Throw<int>(ex)));

            results.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [Fact]
        public void For_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.For(new[] { 1, 2, 3 }, x => Throw<IObservable<int>>(ex)));

            results.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        static T Throw<T>(Exception ex)
        {
            throw ex;
        }
    }
}

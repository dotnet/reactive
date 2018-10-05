// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class MinTest : ReactiveTest
    {

        [Fact]
        public void Min_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<long?>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(double)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(float)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(decimal)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(long)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(int?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(double?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(float?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(decimal?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => default(long?)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, long?>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default, Comparer<DateTime>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(IComparer<DateTime>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default(Func<DateTime, string>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(default(IObservable<DateTime>), _ => "", Comparer<string>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), default, Comparer<string>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Min(Observable.Empty<DateTime>(), _ => "", default));
        }

        [Fact]
        public void Min_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<int>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Int32_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 3),
                OnNext(220, 2),
                OnNext(230, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Min_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Min_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<long>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 2L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Int64_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 3L),
                OnNext(220, 2L),
                OnNext(230, 4L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnError<long>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<long>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Min_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Min_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<float>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 2f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Float_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 3f),
                OnNext(220, 2f),
                OnNext(230, 4f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnError<float>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<float>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Min_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Min_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<double>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 2.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Double_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 3.0),
                OnNext(220, 2.0),
                OnNext(230, 4.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnError<double>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<double>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Min_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Min_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<decimal>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 2m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Decimal_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 3m),
                OnNext(220, 2m),
                OnNext(230, 4m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnError<decimal>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<decimal>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Min_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Min_Nullable_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(int?)),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Int32_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)null),
                OnNext(220, (int?)2),
                OnNext(230, (int?)4),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Int32_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)null),
                OnNext(220, (int?)4),
                OnNext(230, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_GeneralNullableMinTest_LhsNull()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)null),
                OnNext(220, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_GeneralNullableMinTest_RhsNull()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnNext(220, (int?)null),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_GeneralNullableMinTest_Less()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnNext(220, (int?)3),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_GeneralNullableMinTest_Greater()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)3),
                OnNext(220, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)2),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnError<int?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<int?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Min_Nullable_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Min_Nullable_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(long?)),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)2L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)2L),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Int64_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)null),
                OnNext(220, (long?)2L),
                OnNext(230, (long?)4L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)2L),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Int64_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)null),
                OnNext(220, (long?)4L),
                OnNext(230, (long?)2L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)2L),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnError<long?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<long?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Min_Nullable_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Min_Nullable_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(float?)),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)2f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)2f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Float_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)null),
                OnNext(220, (float?)2f),
                OnNext(230, (float?)4f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)2f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Float_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)null),
                OnNext(220, (float?)4f),
                OnNext(230, (float?)2f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)2f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnError<float?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<float?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Min_Nullable_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Min_Nullable_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(double?)),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)2.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Double_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)null),
                OnNext(220, (double?)2.0),
                OnNext(230, (double?)4.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Double_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)null),
                OnNext(220, (double?)4.0),
                OnNext(230, (double?)2.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnError<double?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<double?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Min_Nullable_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Min_Nullable_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, default(decimal?)),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Decimal_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)null),
                OnNext(220, (decimal?)2m),
                OnNext(230, (decimal?)4m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Decimal_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)null),
                OnNext(220, (decimal?)4m),
                OnNext(230, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Min_Nullable_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnError<decimal?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<decimal?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Min_Nullable_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void MinOfT_Reference_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            res.Messages.AssertEqual(
                OnNext(250, default(string)),
                OnCompleted<string>(250)
            );
#else
            res.Messages.AssertEqual(
                OnError<string>(250, e => e is InvalidOperationException)
            );
#endif

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MinOfT_Value_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MinOfT_Reference_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, "a"),
                OnCompleted<string>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MinOfT_Value_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MinOfT_Reference_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "b"),
                OnNext(220, "c"),
                OnNext(230, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, "a"),
                OnCompleted<string>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MinOfT_Value_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1993, 2, 11)),
                OnNext(220, new DateTime(2003, 2, 11)),
                OnNext(230, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnNext(250, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MinOfT_Reference_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnError<string>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<string>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void MinOfT_Value_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnError<DateTime>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void MinOfT_Reference_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z")
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void MinOfT_Value_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now)
            );

            var res = scheduler.Start(() =>
                xs.Min()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void MinOfT_Reference_Comparer_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<string>(Comparer<string>.Default))
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            res.Messages.AssertEqual(
                OnNext(250, default(string)),
                OnCompleted<string>(250)
            );
#else
            res.Messages.AssertEqual(
                OnError<string>(250, e => e is InvalidOperationException)
            );
#endif

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MinOfT_Value_Comparer_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MinOfT_Reference_Comparer_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<string>(Comparer<string>.Default))
            );

            res.Messages.AssertEqual(
                OnNext(250, "a"),
                OnCompleted<string>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MinOfT_Value_Comparer_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
                OnNext(250, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MinOfT_Reference_Comparer_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "b"),
                OnNext(220, "c"),
                OnNext(230, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<string>(Comparer<string>.Default))
            );

            res.Messages.AssertEqual(
                OnNext(250, "c"),
                OnCompleted<string>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MinOfT_Value_Comparer_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1993, 2, 11)),
                OnNext(220, new DateTime(2003, 2, 11)),
                OnNext(230, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
                OnNext(250, new DateTime(2003, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void MinOfT_Reference_Comparer_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnError<string>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<string>(Comparer<string>.Default))
            );

            res.Messages.AssertEqual(
                OnError<string>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void MinOfT_Value_Comparer_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnError<DateTime>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void MinOfT_Reference_Comparer_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z")
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<string>(Comparer<string>.Default))
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void MinOfT_Value_Comparer_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ReverseComparer<DateTime>(Comparer<DateTime>.Default))
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void MinOfT_Reference_ComparerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, "z"),
                OnNext(210, "b"),
                OnNext(220, "c"),
                OnNext(230, "a"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ThrowingComparer<string>(ex))
            );

            res.Messages.AssertEqual(
                OnError<string>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void MinOfT_Value_ComparerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, DateTime.Now),
                OnNext(210, new DateTime(1993, 2, 11)),
                OnNext(220, new DateTime(2003, 2, 11)),
                OnNext(230, new DateTime(1983, 2, 11)),
                OnCompleted<DateTime>(250)
            );

            var res = scheduler.Start(() =>
                xs.Min(new ThrowingComparer<DateTime>(ex))
            );

            res.Messages.AssertEqual(
                OnError<DateTime>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void Min_Selector_Regular_Int32()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 1),
                OnCompleted<int>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Min_Selector_Regular_Int64()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => (long)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 1L),
                OnCompleted<long>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Min_Selector_Regular_Single()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => (float)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 1.0f),
                OnCompleted<float>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Min_Selector_Regular_Double()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => (double)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 1.0),
                OnCompleted<double>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Min_Selector_Regular_Decimal()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => (decimal)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 1.0m),
                OnCompleted<decimal>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Min_Selector_Regular_Int32_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => x == "fo" ? default(int?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (int?)1),
                OnCompleted<int?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Min_Selector_Regular_Int64_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => x == "fo" ? default(long?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (long?)1.0),
                OnCompleted<long?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Min_Selector_Regular_Single_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => x == "fo" ? default(float?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (float?)1.0),
                OnCompleted<float?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Min_Selector_Regular_Double_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => x == "fo" ? default(double?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (double?)1.0),
                OnCompleted<double?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Min_Selector_Regular_Decimal_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => x == "fo" ? default(decimal?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (decimal?)1.0),
                OnCompleted<decimal?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void MinOfT_Selector_Regular()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "qux"),
                OnNext(220, "foo"),
                OnNext(230, "bar"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => new string(x.ToCharArray().Reverse().ToArray())));

            res.Messages.AssertEqual(
                OnNext(240, "oof"),
                OnCompleted<string>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void MinOfT_Selector_Regular_Comparer()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "qux"),
                OnNext(220, "foo"),
                OnNext(230, "bar"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Min(x => new string(x.ToCharArray().Reverse().ToArray()), new ReverseComparer<string>(Comparer<string>.Default)));

            res.Messages.AssertEqual(
                OnNext(240, "xuq"),
                OnCompleted<string>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

    }
}

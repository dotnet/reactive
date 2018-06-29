// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class AverageTest : ReactiveTest
    {
        [Fact]
        public void Average_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<long?>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(double)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(float)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(decimal)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(long)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(int?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(double?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(float?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(decimal?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(default(IObservable<DateTime>), _ => default(long?)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Average(Observable.Empty<DateTime>(), default(Func<DateTime, long?>)));
        }

        [Fact]
        public void Average_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
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
        public void Average_Int32_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 3),
                OnNext(220, 4),
                OnNext(230, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Average_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Average_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 2L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
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
        public void Average_Int64_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 3L),
                OnNext(220, 4L),
                OnNext(230, 2L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnError<long>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Average_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Average_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 2.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
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
        public void Average_Double_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 3.0),
                OnNext(220, 4.0),
                OnNext(230, 2.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnError<double>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Average_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Average_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<float>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 2f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
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
        public void Average_Float_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 3f),
                OnNext(220, 4f),
                OnNext(230, 2f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnError<float>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<float>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Average_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Average_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<decimal>(250, e => e is InvalidOperationException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 2m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
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
        public void Average_Decimal_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 3m),
                OnNext(220, 4m),
                OnNext(230, 2m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, 3m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnError<decimal>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<decimal>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Average_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Average_Nullable_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)null),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Nullable_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
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
        public void Average_Nullable_Int32_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)3),
                OnNext(220, (int?)null),
                OnNext(230, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.5),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Nullable_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnError<int?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Average_Nullable_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Average_Nullable_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)null),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Nullable_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)2L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
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
        public void Average_Nullable_Int64_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)3L),
                OnNext(220, (long?)null),
                OnNext(230, (long?)2L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.5),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Nullable_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnError<long?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Average_Nullable_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Average_Nullable_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)null),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Nullable_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)2.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
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
        public void Average_Nullable_Double_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)3.0),
                OnNext(220, (double?)null),
                OnNext(230, (double?)2.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)2.5),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Nullable_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnError<double?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<double?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Average_Nullable_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Average_Nullable_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)null),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Nullable_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)2f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
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
        public void Average_Nullable_Float_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)3f),
                OnNext(220, (float?)null),
                OnNext(230, (float?)2f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)2.5f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Nullable_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnError<float?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<float?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Average_Nullable_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Average_Nullable_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)null),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Nullable_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
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
        public void Average_Nullable_Decimal_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)3m),
                OnNext(220, (decimal?)null),
                OnNext(230, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)2.5m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Average_Nullable_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnError<decimal?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
                OnError<decimal?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Average_Nullable_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m)
            );

            var res = scheduler.Start(() =>
                xs.Average()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

#if !NO_PERF
#if !NO_THREAD
        [Fact]
        public void Average_InjectOverflow_Int32()
        {
            var xs = Observable.Return(42, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<int>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [Fact]
        public void Average_InjectOverflow_Int64()
        {
            var xs = Observable.Return(42L, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<long>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [Fact]
        public void Average_InjectOverflow_Double()
        {
            var xs = Observable.Return(42.0, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<double>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [Fact]
        public void Average_InjectOverflow_Single()
        {
            var xs = Observable.Return(42.0f, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<float>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [Fact]
        public void Average_InjectOverflow_Decimal()
        {
            var xs = Observable.Return(42.0m, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<decimal>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [Fact]
        public void Average_InjectOverflow_Int32_Nullable()
        {
            var xs = Observable.Return((int?)42, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<int?>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [Fact]
        public void Average_InjectOverflow_Int64_Nullable()
        {
            var xs = Observable.Return((long?)42L, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<long?>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [Fact]
        public void Average_InjectOverflow_Double_Nullable()
        {
            var xs = Observable.Return((double?)42.0, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<double?>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [Fact]
        public void Average_InjectOverflow_Single_Nullable()
        {
            var xs = Observable.Return((float?)42.0f, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<float?>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }

        [Fact]
        public void Average_InjectOverflow_Decimal_Nullable()
        {
            var xs = Observable.Return((decimal?)42.0m, ThreadPoolScheduler.Instance);

            var res = new OverflowInjection<decimal?>(xs, long.MaxValue).Average();

            ReactiveAssert.Throws<OverflowException>(() => res.ForEach(_ => { }));
        }
#endif
#if !CRIPPLED_REFLECTION || NETCOREAPP1_1 || NETCOREAPP1_0
        private class OverflowInjection<T> : IObservable<T>
        {
            private readonly IObservable<T> _source;
            private readonly object _initialCount;

            public OverflowInjection(IObservable<T> source, object initialCount)
            {
                _source = source;
                _initialCount = initialCount;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var f = observer.GetType().GetField("_count", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                f.SetValue(observer, _initialCount);

                return _source.Subscribe(observer);
            }
        }
#endif
#endif

        [Fact]
        public void Average_Selector_Regular_Int32()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 2.0),
                OnCompleted<double>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Average_Selector_Regular_Int64()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => (long)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 2.0),
                OnCompleted<double>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Average_Selector_Regular_Single()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => (float)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 2.0f),
                OnCompleted<float>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Average_Selector_Regular_Double()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => (double)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 2.0),
                OnCompleted<double>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Average_Selector_Regular_Decimal()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => (decimal)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 2.0m),
                OnCompleted<decimal>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Average_Selector_Regular_Int32_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(int?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (double?)2.0),
                OnCompleted<double?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Average_Selector_Regular_Int64_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(long?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (double?)2.0),
                OnCompleted<double?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Average_Selector_Regular_Single_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(float?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (float?)2.0),
                OnCompleted<float?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Average_Selector_Regular_Double_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(double?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (double?)2.0),
                OnCompleted<double?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Average_Selector_Regular_Decimal_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "b"),
                OnNext(220, "fo"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Average(x => x == "fo" ? default(decimal?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (decimal?)2.0),
                OnCompleted<decimal?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

    }
}

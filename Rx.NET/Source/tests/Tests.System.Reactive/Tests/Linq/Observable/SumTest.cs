// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class SumTest : ReactiveTest
    {
        [Fact]
        public void Sum_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<long?>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(double)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(float)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(decimal)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(long)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(int?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(double?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(float?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(decimal?)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(default(IObservable<DateTime>), _ => default(long?)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, long>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sum(Observable.Empty<DateTime>(), default(Func<DateTime, long?>)));
        }

        [Fact]
        public void Sum_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
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
        public void Sum_Int32_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2 + 3 + 4),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Int32_Overflow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, int.MaxValue),
                OnNext(220, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<int>(220, e => e is OverflowException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void Sum_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Sum_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Sum_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 2L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
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
        public void Sum_Int64_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, 2L),
                OnNext(220, 3L),
                OnNext(230, 4L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2L + 3L + 4L),
                OnCompleted<long>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Int64_Overflow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnNext(210, long.MaxValue),
                OnNext(220, 1L),
                OnCompleted<long>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<long>(220, e => e is OverflowException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void Sum_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L),
                OnError<long>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<long>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Sum_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1L)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Sum_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 2f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
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
        public void Sum_Float_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnNext(210, 2f),
                OnNext(220, 3f),
                OnNext(230, 4f),
                OnCompleted<float>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2f + 3f + 4f),
                OnCompleted<float>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f),
                OnError<float>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<float>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Sum_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1f)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Sum_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 2.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
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
        public void Sum_Double_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnNext(210, 2.0),
                OnNext(220, 3.0),
                OnNext(230, 4.0),
                OnCompleted<double>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2.0 + 3.0 + 4.0),
                OnCompleted<double>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0),
                OnError<double>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<double>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Sum_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1.0)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Sum_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 0m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 2m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
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
        public void Sum_Decimal_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnNext(210, 2m),
                OnNext(220, 3m),
                OnNext(230, 4m),
                OnCompleted<decimal>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, 2m + 3m + 4m),
                OnCompleted<decimal>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m),
                OnError<decimal>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<decimal>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Sum_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1m)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Sum_Nullable_Int32_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)0),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Nullable_Int32_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
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
        public void Sum_Nullable_Int32_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)2),
                OnNext(220, (int?)null),
                OnNext(230, (int?)4),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (int?)(2 + 4)),
                OnCompleted<int?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Nullable_Int32_Overflow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnNext(210, (int?)int.MaxValue),
                OnNext(220, (int?)1),
                OnCompleted<int?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<int?>(220, e => e is OverflowException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void Sum_Nullable_Int32_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1),
                OnError<int?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<int?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Sum_Nullable_Int32_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (int?)1)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Sum_Nullable_Int64_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)0L),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Nullable_Int64_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)2L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
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
        public void Sum_Nullable_Int64_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)2L),
                OnNext(220, (long?)null),
                OnNext(230, (long?)4L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (long?)(2L + 4L)),
                OnCompleted<long?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Nullable_Int64_Overflow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnNext(210, (long?)long.MaxValue),
                OnNext(220, (long?)1L),
                OnCompleted<long?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<long?>(220, e => e is OverflowException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void Sum_Nullable_Int64_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L),
                OnError<long?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<long?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Sum_Nullable_Int64_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (long?)1L)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Sum_Nullable_Float_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)0f),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Nullable_Float_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)2f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
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
        public void Sum_Nullable_Float_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnNext(210, (float?)2f),
                OnNext(220, (float?)null),
                OnNext(230, (float?)4f),
                OnCompleted<float?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (float?)(2f + 4f)),
                OnCompleted<float?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Nullable_Float_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f),
                OnError<float?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<float?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Sum_Nullable_Float_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (float?)1f)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Sum_Nullable_Double_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)0.0),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Nullable_Double_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)2.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
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
        public void Sum_Nullable_Double_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnNext(210, (double?)2.0),
                OnNext(220, (double?)null),
                OnNext(230, (double?)4.0),
                OnCompleted<double?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (double?)(2.0 + 4.0)),
                OnCompleted<double?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Nullable_Double_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0),
                OnError<double?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<double?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Sum_Nullable_Double_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (double?)1.0)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Sum_Nullable_Decimal_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)0m),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Nullable_Decimal_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)2m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
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
        public void Sum_Nullable_Decimal_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnNext(210, (decimal?)2m),
                OnNext(220, (decimal?)null),
                OnNext(230, (decimal?)4m),
                OnCompleted<decimal?>(250)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnNext(250, (decimal?)(2m + 4m)),
                OnCompleted<decimal?>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Sum_Nullable_Decimal_Throw()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m),
                OnError<decimal?>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
                OnError<decimal?>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void Sum_Nullable_Decimal_Never()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, (decimal?)1m)
            );

            var res = scheduler.Start(() =>
                xs.Sum()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Sum_Selector_Regular_Int32()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 6),
                OnCompleted<int>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Sum_Selector_Regular_Int64()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => (long)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 6L),
                OnCompleted<long>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Sum_Selector_Regular_Single()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => (float)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 6.0f),
                OnCompleted<float>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Sum_Selector_Regular_Double()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => (double)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 6.0),
                OnCompleted<double>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Sum_Selector_Regular_Decimal()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => (decimal)x.Length));

            res.Messages.AssertEqual(
                OnNext(240, 6.0m),
                OnCompleted<decimal>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Sum_Selector_Regular_Int32_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => x == "fo" ? default(int?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (int?)4),
                OnCompleted<int?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Sum_Selector_Regular_Int64_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => x == "fo" ? default(long?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (long?)4.0),
                OnCompleted<long?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Sum_Selector_Regular_Single_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => x == "fo" ? default(float?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (float?)4.0),
                OnCompleted<float?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Sum_Selector_Regular_Double_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => x == "fo" ? default(double?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (double?)4.0),
                OnCompleted<double?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [Fact]
        public void Sum_Selector_Regular_Decimal_Nullable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, "fo"),
                OnNext(220, "b"),
                OnNext(230, "qux"),
                OnCompleted<string>(240)
            );

            var res = scheduler.Start(() => xs.Sum(x => x == "fo" ? default(decimal?) : x.Length));

            res.Messages.AssertEqual(
                OnNext(240, (decimal?)4.0),
                OnCompleted<decimal?>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

    }
}

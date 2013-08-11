// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !SILVERLIGHTM7

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Joins;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Reactive.Testing;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class QbservableTest
    {
        private IQbservable<int> _qbNull = null;
        private IQbservable<int> _qbMy = new MyQbservable<int>();
        private IQbservableProvider _qbp = new MyQbservableProvider();

        [TestMethod]
        public void LocalQueryMethodImplementationTypeAttribute()
        {
            var t = typeof(string);

            var attr = new LocalQueryMethodImplementationTypeAttribute(t);

            Assert.AreSame(t, attr.TargetType);
        }

        [TestMethod]
        public void Aggregate_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Aggregate(_qbNull, (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Aggregate(_qbMy, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Aggregate(_qbNull, 1, (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Aggregate(_qbMy, 1, null));
        }

        [TestMethod]
        public void Aggregate()
        {
            _qbMy.Aggregate((a, b) => a + b);
            _qbMy.Aggregate("", (a, b) => a + b);
        }

        [TestMethod]
        public void All_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.All(_qbNull, a => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.All(_qbMy, null));
        }

        [TestMethod]
        public void All()
        {
            _qbMy.All(a => true);
        }

        [TestMethod]
        public void Amb_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Amb(_qbNull, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Amb(_qbMy, _qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Amb(default(IQbservableProvider), _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Amb(_qbp, default(IQbservable<int>[])));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Amb(default(IQbservableProvider), new MyQueryable<IObservable<int>>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Amb(_qbp, default(IQueryable<IObservable<int>>)));
        }

        [TestMethod]
        public void Amb()
        {
            _qbMy.Amb(_qbMy);
            _qbp.Amb(_qbMy, _qbMy);
            _qbp.Amb(new MyQueryable<IObservable<int>>());
        }

        [TestMethod]
        public void And_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.And(_qbNull, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbNull));
#if !NO_LARGEARITY
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbNull));
#endif
        }

        [TestMethod]
        public void And()
        {
            _qbMy.And(_qbMy);
        }

        [TestMethod]
        public void Any_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Any(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Any(_qbNull, a => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Any(_qbMy, null));
        }

        [TestMethod]
        public void Any()
        {
            _qbMy.Any();
            _qbMy.Any(a => true);
        }

        [TestMethod]
        public void Average_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Average(default(IQbservable<decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Average(default(IQbservable<decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Average(default(IQbservable<double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Average(default(IQbservable<double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Average(default(IQbservable<float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Average(default(IQbservable<float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Average(default(IQbservable<int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Average(default(IQbservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Average(default(IQbservable<long?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Average(default(IQbservable<long>)));
        }

        [TestMethod]
        public void Average()
        {
            new MyQbservable<decimal?>().Average();
            new MyQbservable<decimal>().Average();
            new MyQbservable<double?>().Average();
            new MyQbservable<double>().Average();
            new MyQbservable<float?>().Average();
            new MyQbservable<float>().Average();
            new MyQbservable<int?>().Average();
            new MyQbservable<int>().Average();
            new MyQbservable<long?>().Average();
            new MyQbservable<long>().Average();
        }

        [TestMethod]
        public void BufferWithCount_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Window(_qbNull, 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Window(_qbNull, 1, 1));
        }

        [TestMethod]
        public void BufferWithCount()
        {
            _qbMy.Window(1);
            _qbMy.Window(1, 1);
        }

        [TestMethod]
        public void BufferWithTime_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Buffer(_qbNull, TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Buffer(_qbNull, TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Buffer(_qbMy, TimeSpan.Zero, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Buffer(_qbNull, TimeSpan.Zero, TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Buffer(_qbNull, TimeSpan.Zero, TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Buffer(_qbMy, TimeSpan.Zero, TimeSpan.Zero, default(IScheduler)));
        }

        [TestMethod]
        public void BufferWithTime()
        {
            _qbMy.Buffer(TimeSpan.Zero);
            _qbMy.Buffer(TimeSpan.Zero, Scheduler.Immediate);
            _qbMy.Buffer(TimeSpan.Zero, TimeSpan.Zero);
            _qbMy.Buffer(TimeSpan.Zero, TimeSpan.Zero, Scheduler.Immediate);
        }

        [TestMethod]
        public void Case_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Case(null, () => 1, new Dictionary<int, IObservable<int>>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Case(_qbp, default(Expression<Func<int>>), new Dictionary<int, IObservable<int>>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Case(_qbp, () => 1, default(Dictionary<int, IObservable<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Case(null, () => 1, new Dictionary<int, IObservable<int>>(), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Case(_qbp, default(Expression<Func<int>>), new Dictionary<int, IObservable<int>>(), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Case(_qbp, () => 1, default(Dictionary<int, IObservable<int>>), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Case(_qbp, () => 1, new Dictionary<int, IObservable<int>>(), default(IScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Case(null, () => 1, new Dictionary<int, IObservable<int>>(), _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Case(_qbp, default(Expression<Func<int>>), new Dictionary<int, IObservable<int>>(), _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Case(_qbp, () => 1, default(Dictionary<int, IObservable<int>>), _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Case(_qbp, () => 1, new Dictionary<int, IObservable<int>>(), default(IQbservable<int>)));
        }

        [TestMethod]
        public void Case()
        {
            _qbp.Case(() => 1, new Dictionary<int, IObservable<int>>());
            _qbp.Case(() => 1, new Dictionary<int, IObservable<int>>(), Scheduler.Immediate);
            _qbp.Case(() => 1, new Dictionary<int, IObservable<int>>(), _qbMy);
        }

        [TestMethod]
        public void Cast_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Cast<int>(default(MyQbservable<object>)));
        }

        [TestMethod]
        public void Cast()
        {
            Qbservable.Cast<int>(new MyQbservable<object>());
        }

        [TestMethod]
        public void Catch_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Catch<int, Exception>(_qbMy, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Catch<int, Exception>(_qbNull, (Exception ex) => null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Catch<int>(_qbMy, _qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Catch<int>(_qbNull, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Catch(default(IQbservableProvider), _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Catch(_qbp, default(IQbservable<int>[])));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Catch(default(IQbservableProvider), new MyQueryable<IObservable<int>>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Catch(_qbp, default(IQueryable<IObservable<int>>)));
        }

        [TestMethod]
        public void Catch()
        {
            _qbMy.Catch((Exception ex) => _qbMy);
            _qbMy.Catch(_qbMy);
            _qbp.Catch(_qbMy, _qbMy);
            _qbp.Catch(new MyQueryable<IObservable<int>>());
        }

        [TestMethod]
        public void CombineLatest_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.CombineLatest(_qbNull, _qbMy, (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.CombineLatest(_qbMy, _qbNull, (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.CombineLatest(_qbMy, _qbMy, default(Expression<Func<int, int, int>>)));
        }

        [TestMethod]
        public void CombineLatest()
        {
            _qbMy.CombineLatest(_qbMy, (a, b) => a + b);
        }

        [TestMethod]
        public void Contains_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Contains(_qbNull, 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Contains(_qbNull, 1, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Contains(_qbMy, 1, null));
        }

        [TestMethod]
        public void Contains()
        {
            _qbMy.Contains(1);
            _qbMy.Contains(1, EqualityComparer<int>.Default);
        }

        [TestMethod]
        public void Count_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Count(_qbNull));
        }

        [TestMethod]
        public void Count()
        {
            _qbMy.Count();
        }

        [TestMethod]
        public void Concat_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Concat(_qbNull, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Concat(_qbMy, _qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Concat(default(IQbservableProvider), _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Concat(_qbp, default(IQbservable<int>[])));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Concat(default(IQbservableProvider), new MyQueryable<IObservable<int>>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Concat(_qbp, default(IQueryable<IObservable<int>>)));
        }

        [TestMethod]
        public void Concat()
        {
            _qbMy.Concat(_qbMy);
            _qbp.Concat(_qbMy, _qbMy);
            _qbp.Concat(new MyQueryable<IObservable<int>>());
        }

        [TestMethod]
        public void Create_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Create<int>(null, o => default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Create<int>(_qbp, default(Expression<Func<IObserver<int>, Action>>)));
        }

        [TestMethod]
        public void Create()
        {
            _qbp.Create<int>(o => default(Action));
        }

        [TestMethod]
        public void CreateWithDisposable_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Create<int>(null, o => default(IDisposable)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Create<int>(_qbp, default(Expression<Func<IObserver<int>, IDisposable>>)));
        }

        [TestMethod]
        public void CreateWithDisposable()
        {
            _qbp.Create<int>(o => default(IDisposable));
        }

        [TestMethod]
        public void Defer_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Defer(null, () => _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Defer<int>(_qbp, default(Expression<Func<IObservable<int>>>)));
        }

        [TestMethod]
        public void Defer()
        {
            _qbp.Defer<int>(() => _qbMy);
        }

        [TestMethod]
        public void Delay_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Delay(_qbNull, DateTimeOffset.Now));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Delay(_qbNull, DateTimeOffset.Now, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Delay(_qbMy, DateTimeOffset.Now, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Delay(_qbNull, TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Delay(_qbNull, TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Delay(_qbMy, TimeSpan.Zero, default(IScheduler)));
        }

        [TestMethod]
        public void Delay()
        {
            _qbMy.Delay(DateTimeOffset.Now);
            _qbMy.Delay(TimeSpan.Zero);
            _qbMy.Delay(DateTimeOffset.Now, Scheduler.Immediate);
            _qbMy.Delay(TimeSpan.Zero, Scheduler.Immediate);
        }

        [TestMethod]
        public void Dematerialize_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Dematerialize(default(IQbservable<Notification<int>>)));
        }

        [TestMethod]
        public void Dematerialize()
        {
            new MyQbservable<Notification<int>>().Dematerialize();
        }

        [TestMethod]
        public void DistinctUntilChanged_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.DistinctUntilChanged(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.DistinctUntilChanged(_qbNull, a => a));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.DistinctUntilChanged(_qbMy, default(Expression<Func<int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.DistinctUntilChanged(_qbNull, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.DistinctUntilChanged(_qbMy, default(IEqualityComparer<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.DistinctUntilChanged(_qbNull, a => a, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.DistinctUntilChanged(_qbMy, default(Expression<Func<int, int>>), EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.DistinctUntilChanged(_qbMy, a => a, default(IEqualityComparer<int>)));
        }

        [TestMethod]
        public void DistinctUntilChanged()
        {
            _qbMy.DistinctUntilChanged();
            _qbMy.DistinctUntilChanged(a => a);
            _qbMy.DistinctUntilChanged(EqualityComparer<int>.Default);
            _qbMy.DistinctUntilChanged(a => a, EqualityComparer<int>.Default);
        }

        [TestMethod]
        public void Do_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbNull, i => Console.WriteLine(i)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbMy, default(Expression<Action<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbNull, i => Console.WriteLine(i), ex => Console.WriteLine(ex.Message)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbMy, default(Expression<Action<int>>), ex => Console.WriteLine(ex.Message)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbMy, i => Console.WriteLine(i), default(Expression<Action<Exception>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbNull, i => Console.WriteLine(i), () => Console.WriteLine()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbMy, default(Expression<Action<int>>), () => Console.WriteLine()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbMy, i => Console.WriteLine(i), default(Expression<Action>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbNull, i => Console.WriteLine(i), ex => Console.WriteLine(ex.Message), () => Console.WriteLine()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbMy, default(Expression<Action<int>>), ex => Console.WriteLine(ex.Message), () => Console.WriteLine()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbMy, i => Console.WriteLine(i), default(Expression<Action<Exception>>), () => Console.WriteLine()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbMy, i => Console.WriteLine(i), ex => Console.WriteLine(ex.Message), default(Expression<Action>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbNull, Observer.Create<int>(i => Console.WriteLine(i))));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Do(_qbMy, default(IObserver<int>)));
        }

        [TestMethod]
        public void Do()
        {
            _qbMy.Do(i => Console.WriteLine(i));
            _qbMy.Do(i => Console.WriteLine(i), ex => Console.WriteLine(ex.Message));
            _qbMy.Do(i => Console.WriteLine(i), () => Console.WriteLine());
            _qbMy.Do(i => Console.WriteLine(i), ex => Console.WriteLine(ex.Message), () => Console.WriteLine());
            _qbMy.Do(Observer.Create<int>(i => Console.WriteLine(i)));
        }

        [TestMethod]
        public void DoWhile_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.DoWhile(_qbNull, () => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.DoWhile(_qbMy, null));
        }

        [TestMethod]
        public void DoWhile()
        {
            _qbMy.DoWhile(() => true);
        }

        [TestMethod]
        public void Empty_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Empty<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Empty<int>(_qbp, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Empty<int>(null, Scheduler.Immediate));
        }

        [TestMethod]
        public void Empty()
        {
            _qbp.Empty<int>();
            _qbp.Empty<int>(Scheduler.Immediate);
        }

        [TestMethod]
        public void Finally_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Finally(_qbNull, () => Console.WriteLine()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Finally(_qbMy, null));
        }

        [TestMethod]
        public void Finally()
        {
            _qbMy.Finally(() => Console.WriteLine());
        }

        [TestMethod]
        public void For_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.For(null, new[] { 1 }, i => _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.For(_qbp, default(IEnumerable<int>), i => _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.For(_qbp, new[] { 1 }, default(Expression<Func<int, IObservable<int>>>)));
        }

        [TestMethod]
        public void For()
        {
            _qbp.For(new[] { 1 }, i => _qbMy);
        }

        [TestMethod]
        public void FromEvent_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.FromEventPattern<EventArgs>(null, "", "Event"));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.FromEventPattern<EventArgs>(_qbp, null, "Event"));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.FromEventPattern<EventArgs>(_qbp, "", null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.FromEventPattern<EventArgs>(null, e => Console.WriteLine(), e => Console.WriteLine()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.FromEventPattern<EventArgs>(_qbp, null, e => Console.WriteLine()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.FromEventPattern<EventArgs>(_qbp, e => Console.WriteLine(), null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.FromEventPattern<Action, EventArgs>(null, e => () => Console.WriteLine(), e => Console.WriteLine(), e => Console.WriteLine()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.FromEventPattern<Action, EventArgs>(_qbp, null, e => Console.WriteLine(), e => Console.WriteLine()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.FromEventPattern<Action, EventArgs>(_qbp, e => () => Console.WriteLine(), null, e => Console.WriteLine()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.FromEventPattern<Action, EventArgs>(_qbp, e => () => Console.WriteLine(), e => Console.WriteLine(), null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.FromEventPattern(_qbp, default(Expression<Action<EventHandler>>), e => Console.WriteLine()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.FromEventPattern(_qbp, e => Console.WriteLine(), default(Expression<Action<EventHandler>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.FromEventPattern(null, e => Console.WriteLine(), e => Console.WriteLine()));
        }

        [TestMethod]
        public void FromEvent()
        {
            _qbp.FromEventPattern<EventArgs>("", "Event");
            _qbp.FromEventPattern<EventArgs>(e => Console.WriteLine(), e => Console.WriteLine());
            _qbp.FromEventPattern<Action, EventArgs>(e => () => Console.WriteLine(), a => Console.WriteLine(), a => Console.WriteLine());
            _qbp.FromEventPattern(e => Console.WriteLine(), e => Console.WriteLine());
        }

        [TestMethod]
        public void Generate_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(null, 1, i => true, i => i + 1, i => i));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, null, i => i + 1, i => i));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, i => i + 1, default(Expression<Func<int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, null, i => i));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(null, 1, i => true, i => i + 1, i => i, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, null, i => i + 1, i => i, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, i => i + 1, default(Expression<Func<int, int>>), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, null, i => i, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, i => i + 1, i => i, default(IScheduler)));
        }

        [TestMethod]
        public void Generate()
        {
            _qbp.Generate(1, i => true, i => i + 1, i => i);
            _qbp.Generate(1, i => true, i => i + 1, i => i, Scheduler.Immediate);
        }

        [TestMethod]
        public void GenerateWithTime_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(null, 1, i => true, i => i + 1, i => i, i => DateTimeOffset.Now));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, null, i => i + 1, i => i, i => DateTimeOffset.Now));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, i => i + 1, default(Expression<Func<int, int>>), i => DateTimeOffset.Now));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, null, i => i, i => DateTimeOffset.Now));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, i => i + 1, i => i, default(Expression<Func<int, DateTimeOffset>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(null, 1, i => true, i => i + 1, i => i, i => DateTimeOffset.Now, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, null, i => i + 1, i => i, i => DateTimeOffset.Now, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, i => i + 1, default(Expression<Func<int, int>>), i => DateTimeOffset.Now, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, i => i + 1, i => i, default(Expression<Func<int, DateTimeOffset>>), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, null, i => i, i => DateTimeOffset.Now, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, i => i + 1, i => i, i => DateTimeOffset.Now, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(null, 1, i => true, i => i + 1, i => i, i => TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, null, i => i + 1, i => i, i => TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, i => i + 1, default(Expression<Func<int, int>>), i => TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, null, i => i, i => TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, i => i + 1, i => i, default(Expression<Func<int, TimeSpan>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(null, 1, i => true, i => i + 1, i => i, i => TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, null, i => i + 1, i => i, i => TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, i => i + 1, default(Expression<Func<int, int>>), i => TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, i => i + 1, i => i, default(Expression<Func<int, TimeSpan>>), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, null, i => i, i => TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Generate(_qbp, 1, i => true, i => i + 1, i => i, i => TimeSpan.Zero, null));
        }

        [TestMethod]
        public void GenerateWithTime()
        {
            _qbp.Generate(1, i => true, i => i + 1, i => i, i => DateTimeOffset.Now);
            _qbp.Generate(1, i => true, i => i + 1, i => i, i => DateTimeOffset.Now, Scheduler.Immediate);
            _qbp.Generate(1, i => true, i => i + 1, i => i, i => TimeSpan.Zero);
            _qbp.Generate(1, i => true, i => i + 1, i => i, i => TimeSpan.Zero, Scheduler.Immediate);
        }

        [TestMethod]
        public void GroupBy_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.GroupBy(_qbNull, x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.GroupBy(_qbMy, default(Expression<Func<int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.GroupBy(_qbNull, x => x, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.GroupBy(_qbMy, default(Expression<Func<int, int>>), EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.GroupBy(_qbMy, x => x, default(IEqualityComparer<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.GroupBy(_qbNull, x => x, x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.GroupBy(_qbMy, default(Expression<Func<int, int>>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.GroupBy(_qbMy, x => x, default(Expression<Func<int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.GroupBy(_qbNull, x => x, x => x, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.GroupBy(_qbMy, default(Expression<Func<int, int>>), x => x, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.GroupBy(_qbMy, x => x, default(Expression<Func<int, int>>), EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.GroupBy(_qbMy, x => x, x => x, default(IEqualityComparer<int>)));
        }

        [TestMethod]
        public void GroupBy()
        {
            _qbMy.GroupBy(x => (double)x);
            _qbMy.GroupBy(x => (double)x, EqualityComparer<double>.Default);
            _qbMy.GroupBy(x => (double)x, x => x.ToString());
            _qbMy.GroupBy(x => (double)x, x => x.ToString(), EqualityComparer<double>.Default);
        }

        [TestMethod]
        public void If_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.If(null, () => true, _qbMy, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.If(_qbp, null, _qbMy, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.If(_qbp, () => true, _qbNull, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.If(_qbp, () => true, _qbMy, _qbNull));
        }

        [TestMethod]
        public void If()
        {
            _qbp.If(() => true, _qbMy, _qbMy);
        }

        [TestMethod]
        public void Interval_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Interval(null, TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Interval(null, TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Interval(_qbp, TimeSpan.Zero, default(IScheduler)));
        }

        [TestMethod]
        public void Interval()
        {
            _qbp.Interval(TimeSpan.Zero);
            _qbp.Interval(TimeSpan.Zero, Scheduler.Immediate);
        }

        [TestMethod]
        public void IsEmpty_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.IsEmpty(_qbNull));
        }

        [TestMethod]
        public void IsEmpty()
        {
            _qbMy.IsEmpty();
        }

        [TestMethod]
        public void Latest_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Latest(_qbNull));
        }

        [TestMethod]
        public void Latest()
        {
            ReactiveAssert.Throws<InvalidCastException>(() => _qbMy.Latest());
            new MyQbservableQueryable<int>().Latest();
        }

        [TestMethod]
        public void LongCount_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.LongCount(_qbNull));
        }

        [TestMethod]
        public void LongCount()
        {
            _qbMy.LongCount();
        }

        [TestMethod]
        public void Materialize_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Materialize(_qbNull));
        }

        [TestMethod]
        public void Materialize()
        {
            _qbMy.Materialize();
        }

        [TestMethod]
        public void Max_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Max<string>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Max<string>(null, Comparer<string>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Max<string>(new MyQbservable<string>(), default(IComparer<string>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Max(default(IQbservable<decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Max(default(IQbservable<decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Max(default(IQbservable<double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Max(default(IQbservable<double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Max(default(IQbservable<float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Max(default(IQbservable<float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Max(default(IQbservable<int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Max(default(IQbservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Max(default(IQbservable<long?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Max(default(IQbservable<long>)));
        }

        [TestMethod]
        public void Max()
        {
            new MyQbservable<string>().Max();
            new MyQbservable<string>().Max(Comparer<string>.Default);
            new MyQbservable<decimal?>().Max();
            new MyQbservable<decimal>().Max();
            new MyQbservable<double?>().Max();
            new MyQbservable<double>().Max();
            new MyQbservable<float?>().Max();
            new MyQbservable<float>().Max();
            new MyQbservable<int?>().Max();
            new MyQbservable<int>().Max();
            new MyQbservable<long?>().Max();
            new MyQbservable<long>().Max();
        }

        [TestMethod]
        public void MaxBy_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.MaxBy(default(IQbservable<string>), s => s.Length));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.MaxBy(new MyQbservable<string>(), default(Expression<Func<string, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.MaxBy(default(IQbservable<string>), s => s.Length, Comparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.MaxBy(new MyQbservable<string>(), default(Expression<Func<string, int>>), Comparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.MaxBy(new MyQbservable<string>(), s => s.Length, default(IComparer<int>)));
        }

        [TestMethod]
        public void MaxBy()
        {
            new MyQbservable<string>().MaxBy(s => s.Length);
            new MyQbservable<string>().MaxBy(s => s.Length, Comparer<int>.Default);
        }

        [TestMethod]
        public void Merge_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(_qbNull, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(_qbMy, _qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(_qbNull, _qbMy, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(_qbMy, _qbNull, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(_qbMy, _qbMy, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(default(IQbservable<IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(default(IQbservableProvider), _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(_qbp, default(IQbservable<int>[])));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(default(IQbservableProvider), Scheduler.Immediate, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(_qbp, default(IScheduler), _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(_qbp, Scheduler.Immediate, default(IQbservable<int>[])));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(default(IQbservableProvider), new MyQueryable<IObservable<int>>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(_qbp, default(IQueryable<IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(default(IQbservableProvider), new MyQueryable<IObservable<int>>(), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(_qbp, new MyQueryable<IObservable<int>>(), default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Merge(_qbp, default(IQueryable<IObservable<int>>), Scheduler.Immediate));
        }

        [TestMethod]
        public void Merge()
        {
            _qbMy.Merge(_qbMy);
            _qbMy.Merge(_qbMy, Scheduler.Immediate);
            new MyQbservable<IObservable<int>>().Merge();
            _qbp.Merge(_qbMy, _qbMy);
            _qbp.Merge(Scheduler.Immediate, _qbMy, _qbMy);
            _qbp.Merge(new MyQueryable<IObservable<int>>());
            _qbp.Merge(new MyQueryable<IObservable<int>>(), Scheduler.Immediate);
        }

        [TestMethod]
        public void Min_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Min<string>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Min<string>(null, Comparer<string>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Min<string>(new MyQbservable<string>(), default(IComparer<string>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Min(default(IQbservable<decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Min(default(IQbservable<decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Min(default(IQbservable<double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Min(default(IQbservable<double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Min(default(IQbservable<float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Min(default(IQbservable<float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Min(default(IQbservable<int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Min(default(IQbservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Min(default(IQbservable<long?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Min(default(IQbservable<long>)));
        }

        [TestMethod]
        public void Min()
        {
            new MyQbservable<string>().Min();
            new MyQbservable<string>().Min(Comparer<string>.Default);
            new MyQbservable<decimal?>().Min();
            new MyQbservable<decimal>().Min();
            new MyQbservable<double?>().Min();
            new MyQbservable<double>().Min();
            new MyQbservable<float?>().Min();
            new MyQbservable<float>().Min();
            new MyQbservable<int?>().Min();
            new MyQbservable<int>().Min();
            new MyQbservable<long?>().Min();
            new MyQbservable<long>().Min();
        }

        [TestMethod]
        public void MinBy_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.MinBy(default(IQbservable<string>), s => s.Length));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.MinBy(new MyQbservable<string>(), default(Expression<Func<string, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.MinBy(default(IQbservable<string>), s => s.Length, Comparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.MinBy(new MyQbservable<string>(), default(Expression<Func<string, int>>), Comparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.MinBy(new MyQbservable<string>(), s => s.Length, default(IComparer<int>)));
        }

        [TestMethod]
        public void MinBy()
        {
            new MyQbservable<string>().MinBy(s => s.Length);
            new MyQbservable<string>().MinBy(s => s.Length, Comparer<int>.Default);
        }

        [TestMethod]
        public void MostRecent_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.MostRecent(_qbNull, 1));
        }

        [TestMethod]
        public void MostRecent()
        {
            ReactiveAssert.Throws<InvalidCastException>(() => _qbMy.MostRecent(1));
            new MyQbservableQueryable<int>().MostRecent(1);
        }

        [TestMethod]
        public void Never_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Never<int>(null));
        }

        [TestMethod]
        public void Never()
        {
            _qbp.Never<int>();
        }

        [TestMethod]
        public void Next_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Next(_qbNull));
        }

        [TestMethod]
        public void Next()
        {
            ReactiveAssert.Throws<InvalidCastException>(() => _qbMy.Next());
            new MyQbservableQueryable<int>().Next();
        }

        [TestMethod]
        public void ObserveOn_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ObserveOn(_qbMy, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ObserveOn(_qbMy, default(SynchronizationContext)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ObserveOn(_qbMy, default(DispatcherScheduler)));
#if HAS_WINFORMS
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ObserveOn(_qbMy, default(ControlScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ObserveOn(_qbNull, new ControlScheduler(new System.Windows.Forms.Form())));
#endif
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ObserveOn(_qbNull, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ObserveOn(_qbNull, new SynchronizationContext()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ObserveOn(_qbNull, DispatcherScheduler.Instance));
        }

        [TestMethod]
        public void ObserveOn()
        {
            _qbMy.ObserveOn(Scheduler.Immediate);
            _qbMy.ObserveOn(new SynchronizationContext());
            Qbservable.ObserveOn(_qbMy, DispatcherScheduler.Instance);
        }

        [TestMethod]
        public void OfType_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.OfType<int>(default(MyQbservable<object>)));
        }

        [TestMethod]
        public void OfType()
        {
            Qbservable.OfType<int>(new MyQbservable<object>());
        }

        [TestMethod]
        public void OnErrorResumeNext_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.OnErrorResumeNext(_qbNull, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.OnErrorResumeNext(_qbMy, _qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.OnErrorResumeNext(default(IQbservableProvider), _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.OnErrorResumeNext(_qbp, default(IQbservable<int>[])));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.OnErrorResumeNext(default(IQbservableProvider), new MyQueryable<IObservable<int>>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.OnErrorResumeNext(_qbp, default(IQueryable<IObservable<int>>)));
        }

        [TestMethod]
        public void OnErrorResumeNext()
        {
            _qbMy.OnErrorResumeNext(_qbMy);
            _qbp.OnErrorResumeNext(_qbMy, _qbMy);
            _qbp.OnErrorResumeNext(new MyQueryable<IObservable<int>>());
        }


        [TestMethod]
        public void Range_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Range(null, 0, 10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Range(null, 0, 10, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Range(_qbp, 0, 10, default(IScheduler)));
        }

        [TestMethod]
        public void Range()
        {
            _qbp.Range(0, 10);
            _qbp.Range(0, 10, Scheduler.Immediate);
        }

        [TestMethod]
        public void RefCount_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.RefCount(null, Observable.Return(1).Multicast(new ReplaySubject<int>())));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.RefCount(_qbp, default(IConnectableObservable<int>)));
        }

        [TestMethod]
        public void RefCount()
        {
            _qbp.RefCount(Observable.Return(1).Multicast(new ReplaySubject<int>()));
        }

        [TestMethod]
        public void Repeat_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Repeat(null, 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Repeat(null, 0, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Repeat(_qbp, 0, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Repeat(null, 0, 10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Repeat(null, 0, 10, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Repeat(_qbp, 0, 10, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Repeat(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Repeat(_qbNull, 1));
        }

        [TestMethod]
        public void Repeat()
        {
            _qbMy.Repeat();
            _qbMy.Repeat(1);
            _qbp.Repeat(42);
            _qbp.Repeat(42, 1);
            _qbp.Repeat(42, Scheduler.Immediate);
            _qbp.Repeat(42, 1, Scheduler.Immediate);
        }
        [TestMethod]
        public void Retry_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Retry(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Retry(_qbNull, 1));
        }

        [TestMethod]
        public void Retry()
        {
            _qbMy.Retry();
            _qbMy.Retry(1);
        }

        [TestMethod]
        public void Return_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Return(null, 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Return(null, 1, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Return(_qbp, 1, default(IScheduler)));
        }

        [TestMethod]
        public void Return()
        {
            _qbp.Return(1);
            _qbp.Return(1, Scheduler.Immediate);
        }

        [TestMethod]
        public void Sample_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Sample(_qbNull, TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Sample(_qbNull, TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Sample(_qbMy, TimeSpan.Zero, default(IScheduler)));
        }

        [TestMethod]
        public void Sample()
        {
            _qbMy.Sample(TimeSpan.Zero);
            _qbMy.Sample(TimeSpan.Zero, Scheduler.Immediate);
        }

        [TestMethod]
        public void Scan_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Scan(_qbNull, (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Scan(_qbMy, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Scan(_qbNull, 1, (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Scan(_qbMy, 1, null));
        }

        [TestMethod]
        public void Scan()
        {
            _qbMy.Scan((a, b) => a + b);
            _qbMy.Scan("", (a, b) => a + b);
        }

        [TestMethod]
        public void Select_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Select(_qbNull, x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Select(_qbNull, (x, i) => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Select(_qbMy, default(Expression<Func<int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Select(_qbMy, default(Expression<Func<int, int, int>>)));
        }

        [TestMethod]
        public void Select()
        {
            _qbMy.Select(x => x + 1);
            _qbMy.Select((x, i) => x + i);
        }

        [TestMethod]
        public void SelectMany_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SelectMany(_qbNull, x => new[] { "" }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SelectMany(_qbMy, default(Expression<Func<int, IEnumerable<string>>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SelectMany(_qbNull, x => Observable.Return("")));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SelectMany(_qbMy, default(Expression<Func<int, IObservable<string>>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SelectMany(_qbNull, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SelectMany(_qbMy, _qbNull));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SelectMany(_qbNull, x => Observable.Return(""), (x, s) => 0.0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SelectMany(_qbMy, default(Expression<Func<int, IObservable<string>>>), (x, s) => 0.0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SelectMany(_qbMy, x => Observable.Return(""), default(Expression<Func<int, string, double>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SelectMany(_qbNull, x => Observable.Return(""), x => Observable.Return(""), () => Observable.Return("")));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SelectMany(_qbMy, default(Expression<Func<int, IObservable<string>>>), x => Observable.Return(""), () => Observable.Return("")));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SelectMany(_qbMy, x => Observable.Return(""), default(Expression<Func<Exception, IObservable<string>>>), () => Observable.Return("")));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SelectMany(_qbMy, x => Observable.Return(""), x => Observable.Return(""), default(Expression<Func<IObservable<string>>>)));
        }

        [TestMethod]
        public void SelectMany()
        {
            _qbMy.SelectMany(x => new[] { "" });
            _qbMy.SelectMany(x => Observable.Return(""));
            _qbMy.SelectMany(_qbMy);
            _qbMy.SelectMany(x => Observable.Return(""), (x, s) => 0.0);
            _qbMy.SelectMany(x => Observable.Return(""), x => Observable.Return(""), () => Observable.Return(""));
        }

        [TestMethod]
        public void Skip_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Skip(_qbNull, 1));
        }

        [TestMethod]
        public void Skip()
        {
            _qbMy.Skip(1);
        }

        [TestMethod]
        public void SkipLast_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SkipLast(_qbNull, 1));
        }

        [TestMethod]
        public void SkipLast()
        {
            _qbMy.SkipLast(1);
        }

        [TestMethod]
        public void SkipUntil_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SkipUntil(_qbNull, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SkipUntil(_qbMy, _qbNull));
        }

        [TestMethod]
        public void SkipUntil()
        {
            _qbMy.SkipUntil(_qbMy);
        }

        [TestMethod]
        public void SkipWhile_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SkipWhile(_qbNull, x => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SkipWhile(_qbMy, default(Expression<Func<int, bool>>)));
        }

        [TestMethod]
        public void SkipWhile()
        {
            _qbMy.SkipWhile(x => true);
        }

        [TestMethod]
        public void StartWith_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.StartWith(_qbNull, new[] { 1 }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.StartWith(_qbMy, default(int[])));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.StartWith(_qbNull, Scheduler.Immediate, new[] { 1 }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.StartWith(_qbMy, default(IScheduler), new[] { 1 }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.StartWith(_qbMy, Scheduler.Immediate, default(int[])));
        }

        [TestMethod]
        public void StartWith()
        {
            Ignore(_qbMy.StartWith(1, 2, 3));
            Ignore(_qbMy.StartWith(Scheduler.Immediate, 1, 2, 3));
        }

        [TestMethod]
        public void SubscribeOn_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SubscribeOn(_qbMy, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SubscribeOn(_qbMy, default(SynchronizationContext)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SubscribeOn(_qbMy, default(DispatcherScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SubscribeOn(_qbNull, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SubscribeOn(_qbNull, new SynchronizationContext()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SubscribeOn(_qbNull, DispatcherScheduler.Instance));
#if HAS_WINFORMS
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SubscribeOn(_qbMy, default(ControlScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.SubscribeOn(_qbNull, new ControlScheduler(new System.Windows.Forms.Form())));
#endif
        }

        [TestMethod]
        public void SubscribeOn()
        {
            _qbMy.SubscribeOn(Scheduler.Immediate);
            _qbMy.SubscribeOn(new SynchronizationContext());
            Qbservable.SubscribeOn(_qbMy, DispatcherScheduler.Instance);
#if HAS_WINFORMS
            _qbMy.SubscribeOn(new ControlScheduler(new System.Windows.Forms.Form()));
#endif
        }

        [TestMethod]
        public void Sum_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Sum(default(IQbservable<decimal?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Sum(default(IQbservable<decimal>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Sum(default(IQbservable<double?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Sum(default(IQbservable<double>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Sum(default(IQbservable<float?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Sum(default(IQbservable<float>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Sum(default(IQbservable<int?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Sum(default(IQbservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Sum(default(IQbservable<long?>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Sum(default(IQbservable<long>)));
        }

        [TestMethod]
        public void Sum()
        {
            new MyQbservable<decimal?>().Sum();
            new MyQbservable<decimal>().Sum();
            new MyQbservable<double?>().Sum();
            new MyQbservable<double>().Sum();
            new MyQbservable<float?>().Sum();
            new MyQbservable<float>().Sum();
            new MyQbservable<int?>().Sum();
            new MyQbservable<int>().Sum();
            new MyQbservable<long?>().Sum();
            new MyQbservable<long>().Sum();
        }

        [TestMethod]
        public void Switch_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Switch<int>(default(IQbservable<IObservable<int>>)));
        }

        [TestMethod]
        public void Switch()
        {
            new MyQbservable<IObservable<int>>().Switch();
        }

        [TestMethod]
        public void Synchronize_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Synchronize(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Synchronize(_qbNull, ""));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Synchronize(_qbMy, null));
        }

        [TestMethod]
        public void Synchronize()
        {
            _qbMy.Synchronize();
            _qbMy.Synchronize("");
        }

        [TestMethod]
        public void Take_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Take(_qbNull, 1));
        }

        [TestMethod]
        public void Take()
        {
            _qbMy.Take(1);
        }

        [TestMethod]
        public void TakeLast_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.TakeLast(_qbNull, 1));
        }

        [TestMethod]
        public void TakeLast()
        {
            _qbMy.TakeLast(1);
        }

        [TestMethod]
        public void TakeUntil_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.TakeUntil(_qbNull, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.TakeUntil(_qbMy, _qbNull));
        }

        [TestMethod]
        public void TakeUntil()
        {
            _qbMy.TakeUntil(_qbMy);
        }

        [TestMethod]
        public void TakeWhile_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.TakeWhile(_qbNull, x => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.TakeWhile(_qbMy, default(Expression<Func<int, bool>>)));
        }

        [TestMethod]
        public void TakeWhile()
        {
            _qbMy.TakeWhile(x => true);
        }

        [TestMethod]
        public void Throttle_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Throttle(_qbNull, TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Throttle(_qbNull, TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Throttle(_qbMy, TimeSpan.Zero, default(IScheduler)));
        }

        [TestMethod]
        public void Throttle()
        {
            _qbMy.Throttle(TimeSpan.Zero);
            _qbMy.Throttle(TimeSpan.Zero, Scheduler.Immediate);
        }

        [TestMethod]
        public void Throw_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Throw<int>(null, new Exception()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Throw<int>(_qbp, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Throw<int>(null, new Exception(), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Throw<int>(_qbp, null, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Throw<int>(_qbp, new Exception(), default(IScheduler)));
        }

        [TestMethod]
        public void Throw()
        {
            _qbp.Throw<int>(new Exception());
            _qbp.Throw<int>(new Exception(), Scheduler.Immediate);
        }

        [TestMethod]
        public void TimeInterval_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.TimeInterval(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.TimeInterval(_qbNull, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.TimeInterval(_qbMy, default(IScheduler)));
        }

        [TestMethod]
        public void TimeInterval()
        {
            _qbMy.TimeInterval();
            _qbMy.TimeInterval(Scheduler.Immediate);
        }

        [TestMethod]
        public void Timeout_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbNull, DateTimeOffset.Now));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbNull, TimeSpan.Zero));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbNull, DateTimeOffset.Now, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbNull, TimeSpan.Zero, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbMy, DateTimeOffset.Now, _qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbMy, TimeSpan.Zero, _qbNull));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbNull, DateTimeOffset.Now, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbNull, TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbMy, DateTimeOffset.Now, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbMy, TimeSpan.Zero, default(IScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbNull, DateTimeOffset.Now, _qbMy, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbNull, TimeSpan.Zero, _qbMy, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbMy, DateTimeOffset.Now, _qbNull, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbMy, TimeSpan.Zero, _qbNull, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbMy, DateTimeOffset.Now, _qbMy, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timeout(_qbMy, TimeSpan.Zero, _qbMy, default(IScheduler)));
        }

        [TestMethod]
        public void Timeout()
        {
            _qbMy.Timeout(DateTimeOffset.Now);
            _qbMy.Timeout(TimeSpan.Zero);
            _qbMy.Timeout(DateTimeOffset.Now, _qbMy);
            _qbMy.Timeout(TimeSpan.Zero, _qbMy);
            _qbMy.Timeout(DateTimeOffset.Now, Scheduler.Immediate);
            _qbMy.Timeout(TimeSpan.Zero, Scheduler.Immediate);
            _qbMy.Timeout(DateTimeOffset.Now, _qbMy, Scheduler.Immediate);
            _qbMy.Timeout(TimeSpan.Zero, _qbMy, Scheduler.Immediate);
        }

        [TestMethod]
        public void Timer_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timer(null, DateTimeOffset.Now));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timer(null, TimeSpan.Zero));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timer(null, DateTimeOffset.Now, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timer(_qbp, DateTimeOffset.Now, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timer(null, TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timer(_qbp, TimeSpan.Zero, default(IScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timer(null, DateTimeOffset.Now, TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timer(null, TimeSpan.Zero, TimeSpan.Zero));

            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timer(null, DateTimeOffset.Now, TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timer(_qbp, DateTimeOffset.Now, TimeSpan.Zero, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timer(null, TimeSpan.Zero, TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timer(_qbp, TimeSpan.Zero, TimeSpan.Zero, default(IScheduler)));
        }

        [TestMethod]
        public void Timer()
        {
            _qbp.Timer(DateTimeOffset.Now);
            _qbp.Timer(TimeSpan.Zero);
            _qbp.Timer(DateTimeOffset.Now, Scheduler.Immediate);
            _qbp.Timer(TimeSpan.Zero, Scheduler.Immediate);
            _qbp.Timer(DateTimeOffset.Now, TimeSpan.Zero);
            _qbp.Timer(TimeSpan.Zero, TimeSpan.Zero);
            _qbp.Timer(DateTimeOffset.Now, TimeSpan.Zero, Scheduler.Immediate);
            _qbp.Timer(TimeSpan.Zero, TimeSpan.Zero, Scheduler.Immediate);
        }

        [TestMethod]
        public void Timestamp_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timestamp(_qbNull));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timestamp(_qbNull, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Timestamp(_qbMy, default(IScheduler)));
        }

        [TestMethod]
        public void Timestamp()
        {
            _qbMy.Timestamp();
            _qbMy.Timestamp(Scheduler.Immediate);
        }

        [TestMethod]
        public void ToObservable_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ToObservable(null, new[] { 1 }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ToObservable(_qbp, default(IEnumerable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ToObservable(null, new[] { 1 }, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ToObservable(_qbp, default(IEnumerable<int>), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ToObservable(_qbp, new[] { 1 }, default(IScheduler)));
        }

        [TestMethod]
        public void ToObservable()
        {
            _qbp.ToObservable(new[] { 1 });
            _qbp.ToObservable(new[] { 1 }, Scheduler.Immediate);
        }

        [TestMethod]
        public void ToQueryable_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ToQueryable(_qbNull));
        }

        [TestMethod]
        public void ToQueryable()
        {
            ReactiveAssert.Throws<InvalidCastException>(() => _qbMy.ToQueryable());
            new MyQbservableQueryable<int>().ToQueryable();
        }

        [TestMethod]
        public void ToQbservable_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.ToQbservable(default(IQueryable<int>)));
        }

        [TestMethod]
        public void ToQbservable()
        {
            ReactiveAssert.Throws<InvalidCastException>(() => new[] { 1 }.AsQueryable().ToQbservable());
            new MyQueryable<int>().ToQbservable();
        }

        [TestMethod]
        public void Using_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Using(null, () => new MyDisposable(), x => Observable.Return(x.ToString())));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Using(_qbp, default(Expression<Func<MyDisposable>>), x => Observable.Return(x.ToString())));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Using(_qbp, () => new MyDisposable(), default(Expression<Func<MyDisposable, IObservable<int>>>)));
        }

        class MyDisposable : IDisposable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void Using()
        {
            _qbp.Using(() => new MyDisposable(), x => Observable.Return(x.ToString()));
        }

        [TestMethod]
        public void Where_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Where(_qbNull, x => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Where(_qbNull, (x, i) => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Where(_qbMy, default(Expression<Func<int, bool>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Where(_qbMy, default(Expression<Func<int, int, bool>>)));
        }

        [TestMethod]
        public void Where()
        {
            _qbMy.Where(x => true);
            _qbMy.Where((x, i) => true);
        }

        [TestMethod]
        public void While_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.While(null, () => true, _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.While(_qbp, default(Expression<Func<bool>>), _qbMy));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.While(_qbp, () => true, _qbNull));
        }

        [TestMethod]
        public void While()
        {
            _qbp.While(() => true, _qbMy);
        }

        [TestMethod]
        public void Zip_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Zip(_qbNull, _qbMy, (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Zip(_qbMy, _qbNull, (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Zip(_qbMy, _qbMy, default(Expression<Func<int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Zip(_qbNull, new[] { 1 }, (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Zip(_qbMy, default(IEnumerable<int>), (a, b) => a + b));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Zip(_qbMy, new[] { 1 }, default(Expression<Func<int, int, int>>)));
        }

        [TestMethod]
        public void Zip()
        {
            _qbMy.Zip(_qbMy, (a, b) => a + b);
            _qbMy.Zip(new[] { 1 }, (a, b) => a + b);
        }

        [TestMethod]
        public void AsObservable_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.AsObservable(_qbNull));
        }

        [TestMethod]
        public void AsObservable()
        {
            Assert.AreSame(_qbMy.AsObservable(), _qbMy);
        }

        [TestMethod]
        public void Join_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.When(null, _qbMy.Then(x => x)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.When(_qbp, default(QueryablePlan<int>[])));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.When(null, new MyQueryable<QueryablePlan<int>>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.When(_qbp, default(IQueryable<QueryablePlan<int>>)));
        }

        [TestMethod]
        public void Join()
        {
            _qbp.When(new MyQueryable<QueryablePlan<int>>());

            _qbp.When(
                _qbMy.Then((t0) => 1),
                _qbMy.And(_qbMy).Then((t0, t1) => 1),
                _qbMy.And(_qbMy).And(_qbMy).Then((t0, t1, t2) => 1),
                _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).Then((t0, t1, t2, t3) => 1)
#if !NO_LARGEARITY
                ,
                _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then((t0, t1, t2, t3, t4) => 1),
                _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then((t0, t1, t2, t3, t4, t5) => 1),
                _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then((t0, t1, t2, t3, t4, t5, t6) => 1),
                _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then((t0, t1, t2, t3, t4, t5, t6, t7) => 1),
                _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then((t0, t1, t2, t3, t4, t5, t6, t7, t8) => 1),
                _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then((t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) => 1),
                _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then((t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 1),
                _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then((t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 1),
                _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then((t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 1),
                _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then((t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 1),
                _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then((t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 1),
                _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then((t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 1)
#endif
);
        }

        [TestMethod]
        public void Then_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.Then(_qbNull, default(Expression<Func<int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.Then(default(Expression<Func<int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).Then(default(Expression<Func<int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int, int>>)));
#if !NO_LARGEARITY
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int, int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int, int, int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int, int, int, int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int, int, int, int, int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int, int, int, int, int, int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => _qbMy.And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).And(_qbMy).Then(default(Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>)));
#endif
        }

        [TestMethod]
        public void AsQbservable_ArgumentNullChecks()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Qbservable.AsQbservable<int>(null));
        }

        [TestMethod]
        public void AsQbservable_CreateQuery_ArgumentChecks()
        {
            var xs = Observable.Return(1).AsQbservable();
            ReactiveAssert.Throws<ArgumentNullException>(() => xs.Provider.CreateQuery<int>(null));
            ReactiveAssert.Throws<ArgumentException>(() => xs.Provider.CreateQuery<int>(Expression.Constant(1)));
        }

        [TestMethod]
        public void AsQbservable_ToString()
        {
            var xs = Observable.Return(1);
            var ys = xs.AsQbservable();
            Assert.AreEqual(ys.ToString(), xs.ToString());

            var ex = Expression.Constant(xs);
            var zs = ys.Provider.CreateQuery<int>(ex);
            Assert.AreEqual(zs.ToString(), ex.ToString());

            var ns = ys.Provider.CreateQuery<int>(Expression.Constant(null, typeof(IObservable<int>)));
            Assert.AreEqual(ns.ToString(), "null");

            var ws = ys.Where(x => true);
            Assert.AreEqual(ws.Expression.ToString(), ws.ToString());
        }

        [TestMethod]
        public void Qbservable_Subscribe_Source()
        {
            var xs = Observable.Return(1).AsQbservable();
            var _x = 0;
            xs.ForEach(x => _x = x);
            Assert.AreEqual(_x, 1);
        }

        [TestMethod]
        public void Qbservable_VariousOperators()
        {
            var xs = Observable.Return(1).AsQbservable();

            Assert.IsTrue(xs.Where(x => true).ToEnumerable().SequenceEqual(new[] { 1 }), "Where");
            Assert.IsTrue(xs.Select(x => x.ToString()).ToEnumerable().SequenceEqual(new[] { "1" }), "Select");
            Assert.IsTrue(xs.Take(1).ToEnumerable().SequenceEqual(new[] { 1 }), "Take");
            Assert.IsTrue(xs.Sum().ToEnumerable().SequenceEqual(new[] { 1 }), "Sum");
            Assert.IsTrue(xs.Amb(xs).ToEnumerable().SequenceEqual(new[] { 1 }), "Amb");
            Assert.IsTrue(xs.Concat(xs).ToEnumerable().SequenceEqual(new[] { 1, 1 }), "Concat");
            Assert.IsTrue(xs.Aggregate("", (s, i) => s + i).ToEnumerable().SequenceEqual(new[] { "1" }), "Aggregate");
            Assert.IsTrue(xs.Where(x => true).Concat(xs.Where(x => false)).ToEnumerable().SequenceEqual(new[] { 1 }), "Concat/Where");
            Assert.IsTrue(xs.SelectMany(x => xs).ToEnumerable().SequenceEqual(new[] { 1 }), "SelectMany");
            Assert.IsTrue(xs.GroupBy(x => x).SelectMany(g => g).ToEnumerable().SequenceEqual(new[] { 1 }), "GroupBy/SelectMany");
            Assert.IsTrue(xs.GroupBy(x => x, x => x).SelectMany(g => g).ToEnumerable().SequenceEqual(new[] { 1 }), "GroupBy/SelectMany (more generics)");

            // TODO: IQueryable ones
        }

        [TestMethod]
        public void Qbservable_ProviderOperators()
        {
            var xs = Observable.Return(1).AsQbservable();

            Assert.IsTrue(Qbservable.Provider.Amb(xs, xs, xs).ToEnumerable().SequenceEqual(new[] { 1 }), "Amb (n-ary)");
            Assert.IsTrue(Qbservable.Provider.Concat(xs, xs, xs).ToEnumerable().SequenceEqual(new[] { 1, 1, 1 }), "Concat (n-ary)");

            ReactiveAssert.Throws<MyException>(() => Qbservable.Provider.Throw<int>(new MyException()).ForEach(_ => { }));
        }

        class MyException : Exception
        {
        }

        [TestMethod]
        public void Qbservable_JoinPatterns()
        {
            var xs = Observable.Return(1).AsQbservable();
            var ys = Observable.Return(2).AsQbservable();
            var zs = Observable.Return(3).AsQbservable();

            Assert.IsTrue(Qbservable.Provider.When(xs.And(ys).Then((x, y) => x + y)).ToEnumerable().SequenceEqual(new[] { 3 }), "Join");
            Assert.IsTrue(Qbservable.Provider.When(xs.And(ys).And(zs).Then((x, y, z) => x + y + z)).ToEnumerable().SequenceEqual(new[] { 6 }), "Join");
        }

        [TestMethod]
        public void Qbservable_MoreProviderFun()
        {
            Assert.IsTrue(
                Qbservable.Provider.Concat(
                    Qbservable.Provider.Return(1).Where(x => x > 0).Select(x => x + 1),
                    Qbservable.Provider.Return(2).Where(x => x < 2),
                    Qbservable.Provider.Throw<int>(new Exception())
                )
                .Catch((Exception ex) => Qbservable.Provider.Return(3))
                .ToEnumerable()
                .SequenceEqual(new[] { 2, 3 })
            );
        }

        [TestMethod]
        public void Qbservable_AsQbservable_ToQueryable()
        {
            var xs = Observable.Range(0, 10).Where(x => x > 5).AsQbservable().Select(x => x + 1);
            var ys = xs.ToQueryable().OrderByDescending(x => x);

            Assert.IsTrue(ys.SequenceEqual(new[] { 10, 9, 8, 7 }));
        }

        [TestMethod]
        public void Qbservable_AsQbservable_ToQueryable_Errors()
        {
            var provider = (IQueryProvider)Qbservable.Provider;

            ReactiveAssert.Throws<NotImplementedException>(() => provider.Execute(Expression.Constant(1)));
            ReactiveAssert.Throws<NotImplementedException>(() => provider.Execute<int>(Expression.Constant(1)));

            ReactiveAssert.Throws<NotImplementedException>(() => provider.CreateQuery(Expression.Constant(1)));
            ReactiveAssert.Throws<ArgumentException>(() => provider.CreateQuery<int>(Expression.Constant(1)));
            ReactiveAssert.Throws<ArgumentException>(() => provider.CreateQuery<int>(new[] { 0 }.AsQueryable().Reverse().Expression));
            ReactiveAssert.Throws<ArgumentException>(() => provider.CreateQuery<int>(Qbservable.Provider.Return(1).Expression));
        }

        [TestMethod]
        public void Qbservable_TwoProviders_Amb()
        {
            var xs = Observable.Return(1).AsQbservable();
            var ys = new EmptyQbservable<int>().Where(x => true);

            xs.Amb(ys).ForEach(_ => { });
            ys.Amb(xs).ForEach(_ => { });
            xs.Concat(ys.Provider.Amb(xs)).ForEach(_ => { });
        }

        private void Ignore<T>(IQbservable<T> q)
        {
        }

        [TestMethod]
        public void Qbservable_Observable_Parity()
        {
            var obs = typeof(Observable).GetMethods(BindingFlags.Public | BindingFlags.Static).ToList();
            var qbs = typeof(Qbservable).GetMethods(BindingFlags.Public | BindingFlags.Static).ToList();

            var onlyInObs = obs.Select(m => m.Name).Except(qbs.Select(m => m.Name)).Except(new[] { "First", "FirstOrDefault", "Last", "LastOrDefault", "Single", "SingleOrDefault", "ForEach", "Subscribe", "GetEnumerator", "ToEnumerable", "Multicast", "GetAwaiter", "ToEvent", "ToEventPattern", "ForEachAsync", "Wait", "RunAsync", "ToListObservable" }).ToList();
            var onlyInQbs = qbs.Select(m => m.Name).Except(obs.Select(m => m.Name)).Except(new[] { "ToQueryable", "ToQbservable", "get_Provider", "AsQbservable" }).ToList();

            Assert.IsTrue(onlyInObs.Count == 0, "Missing Qbservable operator: " + string.Join(", ", onlyInObs.ToArray()));
            Assert.IsTrue(onlyInQbs.Count == 0, "Missing Observable operator: " + string.Join(", ", onlyInQbs.ToArray()));

            var obgs = obs.GroupBy(m => m.Name);
            var qbgs = qbs.GroupBy(m => m.Name);
            var mtch = (from o in obgs
                        where o.Key != "And" && o.Key != "Then" && o.Key != "When"
                        join q in qbgs on o.Key equals q.Key
                        select new { Name = o.Key, Observable = o.ToList(), Qbservable = q.ToList() })
                       .ToList();

            Func<Type, bool> filterReturn = t =>
            {
                if (t.IsGenericType)
                {
                    var gd = t.GetGenericTypeDefinition();
                    if (
                        gd == typeof(ListObservable<>) ||
                        gd == typeof(IConnectableObservable<>))
                        return false;
                }
                return true;
            };

            foreach (var group in mtch)
            {
                if (group.Name == "FromAsyncPattern" || group.Name == "ToAsync")
                {
                    Assert.AreEqual(group.Observable.Count, group.Qbservable.Count, "Mismatch overload count between Qbservable and Observable for " + group.Name);
                    continue;
                }

                var oss = group.Observable.Where(m => filterReturn(m.ReturnType)).Select(m => GetSignature(m, false)).OrderBy(x => x).ToList();
                var qss = group.Qbservable.Select(m => GetSignature(m, true)).OrderBy(x => x).ToList();
                
                Assert.IsTrue(oss.SequenceEqual(qss), "Mismatch between Qbservable and Observable for " + group.Name);
            }
        }

        public static string GetSignature(MethodInfo m, bool correct)
        {
            var ps = m.GetParameters();
            var pss = ps.AsEnumerable();
            if (correct && ps.Length > 0 && ps[0].ParameterType == typeof(IQbservableProvider))
                pss = pss.Skip(1);

            var gens = m.IsGenericMethod ? string.Format("<{0}>", string.Join(", ", m.GetGenericArguments().Select(a => GetTypeName(a, correct)).ToArray())) : "";

            var pars = string.Join(", ", pss.Select(p => (Attribute.IsDefined(p, typeof(ParamArrayAttribute)) ? "params " : "") + GetTypeName(p.ParameterType, correct) + " " + p.Name).ToArray());
            if (Attribute.IsDefined(m, typeof(ExtensionAttribute)))
            {
                if (pars.StartsWith("IQbservable") || pars.StartsWith("IOrderedQbservable") || pars.StartsWith("IQueryable"))
                    pars = "this " + pars;
            }

            return string.Format("{0} {1}{2}({3})", GetTypeName(m.ReturnType, correct), m.Name, gens, pars);
        }

        public static string GetTypeName(Type t, bool correct)
        {
            if (t.IsGenericType)
            {
                var gtd = t.GetGenericTypeDefinition();
                if (gtd == typeof(Expression<>))
                    return GetTypeName(t.GetGenericArguments()[0], false);

                var args = string.Join(", ", t.GetGenericArguments().Select(a => GetTypeName(a, false)).ToArray());
                
                var len = t.Name.IndexOf('`');
                var name = len >= 0 ? t.Name.Substring(0, len) : t.Name;
                if (correct && name == "IQbservable")
                    name = "IObservable";
                if (correct && name == "IOrderedQbservable")
                    name = "IOrderedObservable";
                if (correct && name == "IQueryable")
                    name = "IEnumerable";

                return string.Format("{0}<{1}>", name, args);
            }

            if (t.IsArray)
            {
                return GetTypeName(t.GetElementType(), correct) + "[]";
            }

            return t.Name;
        }

        [TestMethod]
        public void Qbservable_Extensibility_Combinator()
        {
            var res1 = Observable.Return(42).AsQbservable().Foo(x => x / 2).AsObservable().Single();
            Assert.AreEqual(21, res1);

            var res2 = Observable.Return(3).AsQbservable().Bar().AsObservable().Single();
            Assert.AreEqual("***", res2);
        }

        [TestMethod]
        public void Qbservable_Extensibility_Constructor()
        {
            var res1 = Qbservable.Provider.Qux(42).AsObservable().Single();
            Assert.AreEqual(42, res1);
        }

        [TestMethod]
        public void Qbservable_Extensibility_Missing()
        {
            try
            {
                Observable.Return(42).AsQbservable().Baz(x => x).AsObservable().Single();
            }
            catch (InvalidOperationException)
            {
                return;
            }

            Assert.Fail();
        }
    }

    public static class MyExt
    {
        public static IQbservable<R> Foo<T, R>(this IQbservable<T> source, Expression<Func<T, R>> f)
        {
            return source.Provider.CreateQuery<R>(
                Expression.Call(
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(R)),
                    source.Expression,
                    f
                )
            );
        }

        public static IObservable<R> Foo<T, R>(this IObservable<T> source, Func<T, R> f)
        {
            return source.Select(f);
        }

        public static IQbservable<string> Bar(this IQbservable<int> source)
        {
            return source.Provider.CreateQuery<string>(
                Expression.Call(
                    (MethodInfo)MethodInfo.GetCurrentMethod(),
                    source.Expression
                )
            );
        }

        public static IObservable<string> Bar(this IObservable<int> source)
        {
            return source.Select(x => new string('*', x));
        }

        public static IQbservable<T> Qux<T>(this IQbservableProvider provider, T value)
        {
            return provider.CreateQuery<T>(
                Expression.Call(
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    Expression.Constant(provider, typeof(IQbservableProvider)),
                    Expression.Constant(value, typeof(T))
                )
            );
        }

        public static IObservable<T> Qux<T>(T value)
        {
            return Observable.Return(value);
        }

        public static IQbservable<R> Baz<T, R>(this IQbservable<T> source, Expression<Func<T, R>> f)
        {
            return source.Provider.CreateQuery<R>(
                Expression.Call(
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(R)),
                    source.Expression,
                    f
                )
            );
        }
    }

    class MyQbservable<T> : IQbservable<T>
    {
        public MyQbservable()
        {
            Expression = Expression.Constant(this);
        }

        public MyQbservable(Expression expression)
        {
            Expression = expression;
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get;
            private set;
        }

        public IQbservableProvider Provider
        {
            get { return new MyQbservableProvider(); }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }

    class MyQbservableProvider : IQbservableProvider
    {
        public IQbservable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return new MyQbservable<TResult>(expression);
        }
    }

    class MyQbservableQueryable<T> : IQbservable<T>
    {
        public MyQbservableQueryable()
        {
            Expression = Expression.Constant(this);
        }

        public MyQbservableQueryable(Expression expression)
        {
            Expression = expression;
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get;
            private set;
        }

        public IQbservableProvider Provider
        {
            get { return new MyQbservableQueryableProvider(); }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }

    class MyQueryable<T> : IQueryable<T>
    {
        public MyQueryable()
        {
            Expression = Expression.Constant(this);
        }

        public MyQueryable(Expression expression)
        {
            Expression = expression;
        }

        public Expression Expression
        {
            get;
            private set;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new MyQbservableQueryableProvider(); }
        }
    }

    class MyQbservableQueryableProvider : IQbservableProvider, IQueryProvider
    {
        public IQbservable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return new MyQbservable<TResult>(expression);
        }

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            return new MyQueryable<TElement>(expression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }
    }

    class EmptyQbservable<T> : IQbservable<T>, IQbservableProvider
    {
        private Expression _expression;

        public EmptyQbservable()
        {
            _expression = Expression.Constant(this);
        }

        public EmptyQbservable(Expression expression)
        {
            _expression = expression;
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get { return _expression; }
        }

        public IQbservableProvider Provider
        {
            get { return this; }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            observer.OnCompleted();
            return new MyD();
        }

        class MyD : IDisposable
        {
            public void Dispose()
            {
            }
        }

        public IQbservable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return new EmptyQbservable<TResult>(expression);
        }
    }
}

#endif
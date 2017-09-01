using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class AsyncQueryableTests
    {
        [Fact]
        public void Buffer1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Buffer<int>(default(IAsyncQueryable<int>), 1), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.Buffer<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Buffer2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Buffer<int>(default(IAsyncQueryable<int>), 1, 1), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.Buffer<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, 1);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Catch1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int, Exception>(default(IAsyncQueryable<int>), (Exception arg0) => new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int, Exception>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<Exception, IAsyncEnumerable<int>>>)), ane => ane.ParamName == "handler");

            var res = AsyncQueryableEx.Catch<int, Exception>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (Exception arg0) => new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Catch2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int, Exception>(default(IAsyncQueryable<int>), (Exception arg0) => default(Task<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int, Exception>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<Exception, Task<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "handler");

            var res = AsyncQueryableEx.Catch<int, Exception>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (Exception arg0) => default(Task<IAsyncEnumerable<int>>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Catch3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>)), ane => ane.ParamName == "second");

            var res = AsyncQueryableEx.Catch<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Distinct1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Distinct2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(Task<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Distinct3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(IEqualityComparer<int>)), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Distinct4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(Task<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), default(IEqualityComparer<int>)), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void DistinctUntilChanged1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int>(default(IAsyncQueryable<int>)), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.DistinctUntilChanged<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void DistinctUntilChanged2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int>(default(IAsyncQueryable<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IEqualityComparer<int>)), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.DistinctUntilChanged<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void DistinctUntilChanged3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void DistinctUntilChanged4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(Task<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void DistinctUntilChanged5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(IEqualityComparer<int>)), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void DistinctUntilChanged6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(Task<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), default(IEqualityComparer<int>)), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0) => Console.WriteLine()), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Action<int>>)), ane => ane.ParamName == "onNext");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => Console.WriteLine());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0) => default(Task)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task>>)), ane => ane.ParamName == "onNext");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), new NopObserver<int>()), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IObserver<int>)), ane => ane.ParamName == "observer");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new NopObserver<int>());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0) => Console.WriteLine(), () => { }), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Action<int>>), () => { }), ane => ane.ParamName == "onNext");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => Console.WriteLine(), default(Action)), ane => ane.ParamName == "onCompleted");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => Console.WriteLine(), () => { });
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0) => Console.WriteLine(), (Exception arg0) => Console.WriteLine()), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Action<int>>), (Exception arg0) => Console.WriteLine()), ane => ane.ParamName == "onNext");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => Console.WriteLine(), default(Expression<Action<Exception>>)), ane => ane.ParamName == "onError");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => Console.WriteLine(), (Exception arg0) => Console.WriteLine());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0) => default(Task), () => default(Task)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task>>), () => default(Task)), ane => ane.ParamName == "onNext");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task), default(Expression<Func<Task>>)), ane => ane.ParamName == "onCompleted");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task), () => default(Task));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0) => default(Task), (Exception arg0) => default(Task)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task>>), (Exception arg0) => default(Task)), ane => ane.ParamName == "onNext");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task), default(Expression<Func<Exception, Task>>)), ane => ane.ParamName == "onError");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task), (Exception arg0) => default(Task));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0) => Console.WriteLine(), (Exception arg0) => Console.WriteLine(), () => { }), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Action<int>>), (Exception arg0) => Console.WriteLine(), () => { }), ane => ane.ParamName == "onNext");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => Console.WriteLine(), default(Expression<Action<Exception>>), () => { }), ane => ane.ParamName == "onError");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => Console.WriteLine(), (Exception arg0) => Console.WriteLine(), default(Action)), ane => ane.ParamName == "onCompleted");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => Console.WriteLine(), (Exception arg0) => Console.WriteLine(), () => { });
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0) => default(Task), (Exception arg0) => default(Task), () => default(Task)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task>>), (Exception arg0) => default(Task), () => default(Task)), ane => ane.ParamName == "onNext");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task), default(Expression<Func<Exception, Task>>), () => default(Task)), ane => ane.ParamName == "onError");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task), (Exception arg0) => default(Task), default(Expression<Func<Task>>)), ane => ane.ParamName == "onCompleted");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task), (Exception arg0) => default(Task), () => default(Task));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Expand1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Expand<int>(default(IAsyncQueryable<int>), (int arg0) => new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Expand<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, IAsyncEnumerable<int>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryableEx.Expand<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => new int[] { default(int) }.ToAsyncEnumerable());
            res = res.Take(5);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Expand2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Expand<int>(default(IAsyncQueryable<int>), (int arg0) => default(Task<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Expand<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryableEx.Expand<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<IAsyncEnumerable<int>>));
            res = res.Take(5);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Finally1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Finally<int>(default(IAsyncQueryable<int>), () => { }), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Finally<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Action)), ane => ane.ParamName == "finallyAction");

            var res = AsyncQueryableEx.Finally<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), () => { });
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Finally2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Finally<int>(default(IAsyncQueryable<int>), () => default(Task)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Finally<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<Task>>)), ane => ane.ParamName == "finallyAction");

            var res = AsyncQueryableEx.Finally<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), () => default(Task));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void IgnoreElements1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.IgnoreElements<int>(default(IAsyncQueryable<int>)), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.IgnoreElements<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void IsEmpty1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.IsEmpty<int>(default(IAsyncQueryable<int>)), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.IsEmpty<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable());
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void IsEmpty2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.IsEmpty<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.IsEmpty<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void Max1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Max<int>(default(IAsyncQueryable<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Max<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IComparer<int>)), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.Max<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), Comparer<int>.Default);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void Max2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Max<int>(default(IAsyncQueryable<int>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Max<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IComparer<int>), CancellationToken.None), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.Max<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), Comparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxBy1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int));
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxBy2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(Task<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>));
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxBy3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(Task<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxBy4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(Task<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), default(IComparer<int>)), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), Comparer<int>.Default);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxBy5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxBy6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(IComparer<int>)), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), Comparer<int>.Default);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxBy7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(Task<int>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task<int>>>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), default(IComparer<int>), CancellationToken.None), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), Comparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxBy8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(IComparer<int>), CancellationToken.None), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.MaxBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), Comparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void Min1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Min<int>(default(IAsyncQueryable<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Min<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IComparer<int>)), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.Min<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), Comparer<int>.Default);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void Min2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Min<int>(default(IAsyncQueryable<int>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Min<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IComparer<int>), CancellationToken.None), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.Min<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), Comparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinBy1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int));
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinBy2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(Task<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>));
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinBy3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinBy4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(IComparer<int>)), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), Comparer<int>.Default);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinBy5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(Task<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinBy6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(Task<int>), Comparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task<int>>>), Comparer<int>.Default), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), default(IComparer<int>)), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), Comparer<int>.Default);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinBy7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), default(IComparer<int>), CancellationToken.None), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), Comparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinBy8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(Task<int>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task<int>>>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), default(IComparer<int>), CancellationToken.None), ane => ane.ParamName == "comparer");

            var res = AsyncQueryableEx.MinBy<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task<int>), Comparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void OnErrorResumeNext1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.OnErrorResumeNext<int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.OnErrorResumeNext<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>)), ane => ane.ParamName == "second");

            var res = AsyncQueryableEx.OnErrorResumeNext<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Retry1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Retry<int>(default(IAsyncQueryable<int>)), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.Retry<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Retry2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Retry<int>(default(IAsyncQueryable<int>), 1), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.Retry<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Scan1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, int>>)), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryableEx.Scan<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Scan2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(Task<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, Task<int>>>)), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryableEx.Scan<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(Task<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Scan3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, int>>)), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryableEx.Scan<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Scan4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1) => default(Task<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, Task<int>>>)), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryableEx.Scan<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(Task<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void SelectMany1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.SelectMany<int, int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>)), ane => ane.ParamName == "other");

            var res = AsyncQueryableEx.SelectMany<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void StartWith1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.StartWith<int>(default(IAsyncQueryable<int>), new int[] { default(int) }), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.StartWith<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(int[])), ane => ane.ParamName == "values");

            var res = AsyncQueryableEx.StartWith<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) });
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

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
        public void Amb1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Amb<int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Amb<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>)), ane => ane.ParamName == "second");

            var res = AsyncQueryableEx.Amb<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int>(default(IAsyncQueryable<int>), new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "first");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IAsyncEnumerable<int>)), ane => ane.ParamName == "second");

            var res = AsyncQueryableEx.Catch<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Catch2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int, Exception>(default(IAsyncQueryable<int>), (Exception arg0) => new int[] { default(int) }.ToAsyncEnumerable()), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int, Exception>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<Exception, IAsyncEnumerable<int>>>)), ane => ane.ParamName == "handler");

            var res = AsyncQueryableEx.Catch<int, Exception>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (Exception arg0) => new int[] { default(int) }.ToAsyncEnumerable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Catch3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int, Exception>(default(IAsyncQueryable<int>), (Exception arg0) => default(ValueTask<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int, Exception>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<Exception, ValueTask<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "handler");

            var res = AsyncQueryableEx.Catch<int, Exception>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (Exception arg0) => default(ValueTask<IAsyncEnumerable<int>>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Catch4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int, Exception>(default(IAsyncQueryable<int>), (Exception arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Catch<int, Exception>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<Exception, CancellationToken, ValueTask<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "handler");

            var res = AsyncQueryableEx.Catch<int, Exception>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (Exception arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Concat1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Concat<int>(default(IAsyncQueryable<IAsyncEnumerable<int>>)), ane => ane.ParamName == "sources");

            var res = AsyncQueryableEx.Concat<int>(new IAsyncEnumerable<int>[] { default(IAsyncEnumerable<int>) }.ToAsyncEnumerable().AsAsyncQueryable());
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Distinct3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Distinct4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Distinct5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Distinct6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.Distinct<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void DistinctUntilChanged5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void DistinctUntilChanged6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void DistinctUntilChanged7()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void DistinctUntilChanged8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), EqualityComparer<int>.Default), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.DistinctUntilChanged<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), EqualityComparer<int>.Default);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), new NopObserver<int>()), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(IObserver<int>)), ane => ane.ParamName == "observer");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), new NopObserver<int>());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0) => Console.WriteLine()), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Action<int>>)), ane => ane.ParamName == "onNext");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => Console.WriteLine());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0) => default(Task)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task>>)), ane => ane.ParamName == "onNext");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(Task)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, Task>>)), ane => ane.ParamName == "onNext");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(Task));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0) => Console.WriteLine(), () => { }), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Action<int>>), () => { }), ane => ane.ParamName == "onNext");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => Console.WriteLine(), default(Action)), ane => ane.ParamName == "onCompleted");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => Console.WriteLine(), () => { });
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(Task), (CancellationToken arg0) => default(Task)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, Task>>), (CancellationToken arg0) => default(Task)), ane => ane.ParamName == "onNext");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(Task), default(Expression<Func<CancellationToken, Task>>)), ane => ane.ParamName == "onCompleted");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(Task), (CancellationToken arg0) => default(Task));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do8()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0) => Console.WriteLine(), (Exception arg0) => Console.WriteLine()), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Action<int>>), (Exception arg0) => Console.WriteLine()), ane => ane.ParamName == "onNext");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => Console.WriteLine(), default(Expression<Action<Exception>>)), ane => ane.ParamName == "onError");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => Console.WriteLine(), (Exception arg0) => Console.WriteLine());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do9()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0) => default(Task), (Exception arg0) => default(Task)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, Task>>), (Exception arg0) => default(Task)), ane => ane.ParamName == "onNext");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task), default(Expression<Func<Exception, Task>>)), ane => ane.ParamName == "onError");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(Task), (Exception arg0) => default(Task));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do10()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(Task), (Exception arg0, CancellationToken arg1) => default(Task)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, Task>>), (Exception arg0, CancellationToken arg1) => default(Task)), ane => ane.ParamName == "onNext");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(Task), default(Expression<Func<Exception, CancellationToken, Task>>)), ane => ane.ParamName == "onError");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(Task), (Exception arg0, CancellationToken arg1) => default(Task));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do11()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(Task), (Exception arg0, CancellationToken arg1) => default(Task), (CancellationToken arg0) => default(Task)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, Task>>), (Exception arg0, CancellationToken arg1) => default(Task), (CancellationToken arg0) => default(Task)), ane => ane.ParamName == "onNext");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(Task), default(Expression<Func<Exception, CancellationToken, Task>>), (CancellationToken arg0) => default(Task)), ane => ane.ParamName == "onError");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(Task), (Exception arg0, CancellationToken arg1) => default(Task), default(Expression<Func<CancellationToken, Task>>)), ane => ane.ParamName == "onCompleted");

            var res = AsyncQueryableEx.Do<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(Task), (Exception arg0, CancellationToken arg1) => default(Task), (CancellationToken arg0) => default(Task));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Do12()
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
        public void Do13()
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Expand<int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Expand<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryableEx.Expand<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<IAsyncEnumerable<int>>));
            res = res.Take(5);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Expand3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Expand<int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Expand<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<IAsyncEnumerable<int>>>>)), ane => ane.ParamName == "selector");

            var res = AsyncQueryableEx.Expand<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<IAsyncEnumerable<int>>));
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
        public void IsEmptyAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.IsEmptyAsync<int>(default(IAsyncQueryable<int>), CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.IsEmptyAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxAsync<int>(default(IAsyncQueryable<int>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.MaxAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), Comparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxByAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxByAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MaxByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxByAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxByAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MaxByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxByAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxByAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MaxByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxByAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxByAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MaxByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), Comparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxByAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxByAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MaxByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), Comparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MaxByAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxByAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MaxByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MaxByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void Merge1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Merge<int>(default(IAsyncQueryable<IAsyncEnumerable<int>>)), ane => ane.ParamName == "sources");

            var res = AsyncQueryableEx.Merge<int>(new IAsyncEnumerable<int>[] { default(IAsyncEnumerable<int>) }.ToAsyncEnumerable().AsAsyncQueryable());
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void MinAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinAsync<int>(default(IAsyncQueryable<int>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.MinAsync<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), Comparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinByAsync1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinByAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MinByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinByAsync2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinByAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MinByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinByAsync3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinByAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MinByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinByAsync4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinByAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(int), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int>>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MinByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(int), Comparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinByAsync5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinByAsync<int, int>(default(IAsyncQueryable<int>), (int arg0) => default(ValueTask<int>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, ValueTask<int>>>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MinByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0) => default(ValueTask<int>), Comparer<int>.Default, CancellationToken.None);
            AssertEx.SucceedOrFailProper(() => res.Wait());
        }

        [Fact]
        public void MinByAsync6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinByAsync<int, int>(default(IAsyncQueryable<int>), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.MinByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, CancellationToken, ValueTask<int>>>), Comparer<int>.Default, CancellationToken.None), ane => ane.ParamName == "keySelector");

            var res = AsyncQueryableEx.MinByAsync<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, CancellationToken arg1) => default(ValueTask<int>), Comparer<int>.Default, CancellationToken.None);
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
        public void Repeat1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Repeat<int>(default(IAsyncQueryable<int>)), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.Repeat<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable());
            res = res.Take(5);
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Repeat2()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Repeat<int>(default(IAsyncQueryable<int>), 1), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.Repeat<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1);
            res = res.Take(5);
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
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int>(default(IAsyncQueryable<int>), (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, ValueTask<int>>>)), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryableEx.Scan<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Scan3()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int>(default(IAsyncQueryable<int>), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryableEx.Scan<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Scan4()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1) => default(int)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, int>>)), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryableEx.Scan<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(int));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Scan5()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, ValueTask<int>>>)), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryableEx.Scan<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1) => default(ValueTask<int>));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

        [Fact]
        public void Scan6()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int, int>(default(IAsyncQueryable<int>), 1, (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>)), ane => ane.ParamName == "source");
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Scan<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, default(Expression<Func<int, int, CancellationToken, ValueTask<int>>>)), ane => ane.ParamName == "accumulator");

            var res = AsyncQueryableEx.Scan<int, int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), 1, (int arg0, int arg1, CancellationToken arg2) => default(ValueTask<int>));
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

        [Fact]
        public void Timeout1()
        {
            AssertEx.Throws<ArgumentNullException>(() => AsyncQueryableEx.Timeout<int>(default(IAsyncQueryable<int>), default(TimeSpan)), ane => ane.ParamName == "source");

            var res = AsyncQueryableEx.Timeout<int>(new int[] { default(int) }.ToAsyncEnumerable().AsAsyncQueryable(), default(TimeSpan));
            var task = res.ForEachAsync(_ => { });
            AssertEx.SucceedOrFailProper(() => task.Wait());
        }

    }
}
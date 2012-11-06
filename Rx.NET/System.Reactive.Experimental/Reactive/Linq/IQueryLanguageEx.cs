// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Internal interface describing the LINQ to Events query language.
    /// </summary>
    internal interface IQueryLanguageEx
    {
        IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, IEnumerable<IObservable<object>>> iteratorMethod);
        IObservable<Unit> Create(Func<IEnumerable<IObservable<object>>> iteratorMethod);

        IObservable<TSource> Expand<TSource>(IObservable<TSource> source, Func<TSource, IObservable<TSource>> selector);
        IObservable<TSource> Expand<TSource>(IObservable<TSource> source, Func<TSource, IObservable<TSource>> selector, IScheduler scheduler);

        IObservable<TResult> ForkJoin<TFirst, TSecond, TResult>(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector);
        IObservable<TSource[]> ForkJoin<TSource>(params IObservable<TSource>[] sources);
        IObservable<TSource[]> ForkJoin<TSource>(IEnumerable<IObservable<TSource>> sources);

        IObservable<TResult> Let<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> function);

        IObservable<TResult> ManySelect<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, TResult> selector);
        IObservable<TResult> ManySelect<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, TResult> selector, IScheduler scheduler);

        ListObservable<TSource> ToListObservable<TSource>(IObservable<TSource> source);
    }
}

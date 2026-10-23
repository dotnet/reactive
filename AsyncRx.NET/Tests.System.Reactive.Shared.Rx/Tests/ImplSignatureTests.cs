// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Pins that the real calls in <see cref="RxTarget"/> keep Rx.NET's genericity.</summary>
/// <remarks>
/// An <c>*Impl</c> less generic than the library call it makes compiles and passes every flat
/// scenario, and fails only when a nested one reaches it. This compares each with its call, and
/// turns that into an immediate failure.
/// </remarks>
[TestClass]
public sealed class ImplSignatureTests
{
    [TestMethod]
    public void Impl_methods_keep_the_librarys_genericity()
    {
        var problems = ImplSignatures.Check(
            typeof(RxTarget),
            typeof(IObservable<>),
            typeof(IGroupedObservable<,>),
            typeof(ITestableObservable<>),
            typeof(IConnectableObservable<>));

        Assert.IsEmpty(problems, string.Join(Environment.NewLine, problems));
    }

    [TestMethod]
    public void Check_reports_each_way_an_impl_can_be_less_generic_than_its_call()
    {
        var problems = ImplSignatures.Check(
            typeof(LessGenericThanTheirCalls),
            typeof(IObservable<>));

        // One problem per offending position: the closed source, predicate and return of the
        // first, the merged selector and return of the second, and one each for the rest.
        Assert.HasCount(7, problems, string.Join(Environment.NewLine, problems));
        Assert.Contains(
            p => p.StartsWith(
                "ClosedSourceImpl (Observable.Where): parameter 'source', argument 0 is " +
                "System.Int32 where the library has its type parameter TSource",
                StringComparison.Ordinal),
            problems);
        Assert.Contains(
            p => p.StartsWith(
                "MergedParametersImpl (Observable.Select): parameter 'selector', argument 1: T " +
                "stands for the library's TResult here and for its TSource elsewhere",
                StringComparison.Ordinal),
            problems);
        Assert.Contains(
            p => p.StartsWith(
                "ExtraParameterImpl (Observable.Take): 3 parameter(s) where the library call has 2",
                StringComparison.Ordinal),
            problems);
        Assert.Contains(
            p => p.StartsWith("NoCallImpl calls no library method", StringComparison.Ordinal),
            problems);
    }

    [TestMethod]
    public void Check_applies_the_observable_rule_to_an_impl_that_adapts_its_call()
    {
        var problems = ImplSignatures.Check(typeof(ClosedAdapters), typeof(IObservable<>));

        Assert.HasCount(1, problems, string.Join(Environment.NewLine, problems));
        Assert.StartsWith(
            "ClosedAdapterImpl: parameter 'source' is System.IObservable`1[System.Int32]",
            problems[0]);
    }

    // Each of these compiles and would pass every flat scenario.
    private static class LessGenericThanTheirCalls
    {
        private static IObservable<int> ClosedSourceImpl(
            IObservable<int> source,
            Func<int, bool> predicate) =>
            source.Where(predicate);

        private static IObservable<T> MergedParametersImpl<T>(
            IObservable<T> source,
            Func<T, T> selector) =>
            source.Select(selector);

        private static IObservable<T> ExtraParameterImpl<T>(
            IObservable<T> source,
            int count,
            string note) =>
            source.Take(count);

        private static int NoCallImpl<T>(IObservable<T>[] sources) => sources.Length;
    }

    private static class ClosedAdapters
    {
        [AdaptsLibraryCall]
        private static IDisposable ClosedAdapterImpl(
            IObservable<int> source,
            Func<int, ValueTask> onNext) =>
            source.Subscribe(x => onNext(x).GetAwaiter().GetResult());
    }
}

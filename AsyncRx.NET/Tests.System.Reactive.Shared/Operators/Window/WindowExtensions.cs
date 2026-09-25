// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

// Window as data. Every form returns a Nested<T>; the callbacks return neutral sequences
// (Seq<TClosing>), which the materializer materializes at the moment the target invokes
// the callback — so a closing selector that throws, throws inside the target's own call.

public static class WindowExtensions
{
    public static Nested<T> Window<T, TWindowClosing>(this Seq<T> source, Func<Seq<TWindowClosing>> windowClosingSelector, [CallerArgumentExpression(nameof(windowClosingSelector))] string text = "") =>
        new WindowClosingsSeq<T, TWindowClosing>(source, windowClosingSelector, text);

    public static Nested<T> Window<T, TWindowOpening, TWindowClosing>(this Seq<T> source, Seq<TWindowOpening> windowOpenings, Func<TWindowOpening, Seq<TWindowClosing>> windowClosingSelector, [CallerArgumentExpression(nameof(windowClosingSelector))] string text = "") =>
        new WindowOpeningsSeq<T, TWindowOpening, TWindowClosing>(source, windowOpenings, windowClosingSelector, text);

    public static Nested<T> Window<T, TWindowBoundary>(this Seq<T> source, Seq<TWindowBoundary> windowBoundaries) =>
        new WindowBoundariesSeq<T, TWindowBoundary>(source, windowBoundaries);

    public static Nested<T> Window<T>(this Seq<T> source, int count, int skip) => new WindowCountSeq<T>(source, count, skip);

    public static Nested<T> Window<T>(this Seq<T> source, TimeSpan timeSpan, SchedulerRef scheduler) => new WindowTimeSeq<T>(source, timeSpan, scheduler);

    public static Nested<T> Window<T>(this Seq<T> source, TimeSpan timeSpan, TimeSpan timeShift, SchedulerRef scheduler) => new WindowTimeShiftSeq<T>(source, timeSpan, timeShift, scheduler);

    public static Nested<T> Window<T>(this Seq<T> source, TimeSpan timeSpan, int count, SchedulerRef scheduler) => new WindowTimeOrCountSeq<T>(source, timeSpan, count, scheduler);
}

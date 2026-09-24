// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

// Window as data. Every form returns a Nested<T>; the callbacks return neutral sequences
// (Seq<TClosing>), which the materializer materializes at the moment the platform invokes
// the callback — so a closing selector that throws, throws inside the platform's own call.

public sealed class WindowClosingsSeq<T, TWindowClosing>(Seq<T> source, Func<Seq<TWindowClosing>> windowClosingSelector, string text) : Nested<T>
{
    public Seq<T> Source => source;

    public Func<Seq<TWindowClosing>> WindowClosingSelector => windowClosingSelector;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowClosings(this);

    public override string ToString() => $"{source}.Window({text})";
}

public sealed class WindowOpeningsSeq<T, TWindowOpening, TWindowClosing>(Seq<T> source, Seq<TWindowOpening> windowOpenings, Func<TWindowOpening, Seq<TWindowClosing>> windowClosingSelector, string text) : Nested<T>
{
    public Seq<T> Source => source;

    public Seq<TWindowOpening> WindowOpenings => windowOpenings;

    public Func<TWindowOpening, Seq<TWindowClosing>> WindowClosingSelector => windowClosingSelector;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowOpenings(this);

    public override string ToString() => $"{source}.Window({windowOpenings}, {text})";
}

public sealed class WindowBoundariesSeq<T, TWindowBoundary>(Seq<T> source, Seq<TWindowBoundary> windowBoundaries) : Nested<T>
{
    public Seq<T> Source => source;

    public Seq<TWindowBoundary> WindowBoundaries => windowBoundaries;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowBoundaries(this);

    public override string ToString() => $"{source}.Window({windowBoundaries})";
}

public sealed class WindowCountSeq<T>(Seq<T> source, int count, int skip) : Nested<T>
{
    public Seq<T> Source => source;

    public int Count => count;

    public int Skip => skip;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowCount(this);

    public override string ToString() => $"{source}.Window({count}, {skip})";
}

public sealed class WindowTimeSeq<T>(Seq<T> source, TimeSpan timeSpan, SchedulerRef scheduler) : Nested<T>
{
    public Seq<T> Source => source;

    public TimeSpan TimeSpan => timeSpan;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowTime(this);

    public override string ToString() => $"{source}.Window({timeSpan.Ticks} ticks, {scheduler})";
}

public sealed class WindowTimeShiftSeq<T>(Seq<T> source, TimeSpan timeSpan, TimeSpan timeShift, SchedulerRef scheduler) : Nested<T>
{
    public Seq<T> Source => source;

    public TimeSpan TimeSpan => timeSpan;

    public TimeSpan TimeShift => timeShift;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowTimeShift(this);

    public override string ToString() => $"{source}.Window({timeSpan.Ticks} ticks, {timeShift.Ticks} ticks, {scheduler})";
}

public sealed class WindowTimeOrCountSeq<T>(Seq<T> source, TimeSpan timeSpan, int count, SchedulerRef scheduler) : Nested<T>
{
    public Seq<T> Source => source;

    public TimeSpan TimeSpan => timeSpan;

    public int Count => count;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowTimeOrCount(this);

    public override string ToString() => $"{source}.Window({timeSpan.Ticks} ticks, {count}, {scheduler})";
}

public partial interface ISeqVisitor
{
    object WindowClosings<T, TWindowClosing>(WindowClosingsSeq<T, TWindowClosing> seq);

    object WindowOpenings<T, TWindowOpening, TWindowClosing>(WindowOpeningsSeq<T, TWindowOpening, TWindowClosing> seq);

    object WindowBoundaries<T, TWindowBoundary>(WindowBoundariesSeq<T, TWindowBoundary> seq);

    object WindowCount<T>(WindowCountSeq<T> seq);

    object WindowTime<T>(WindowTimeSeq<T> seq);

    object WindowTimeShift<T>(WindowTimeShiftSeq<T> seq);

    object WindowTimeOrCount<T>(WindowTimeOrCountSeq<T> seq);
}

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

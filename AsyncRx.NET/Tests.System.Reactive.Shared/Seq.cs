// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

// The query representation: a Seq<T> is a *description* of an observable
// sequence, not an observable. Its leaves are the platform's own objects (NativeSeq<T>: a
// testable source the test created through the scheduler, or an inner window/group handed to
// a callback) and the neutral creation operators; its interior nodes are operator applications,
// one node type per overload. Nothing here runs. A platform (IPlatform, in Harness.cs) is a
// visitor that turns a tree into its own pipeline at the moment the test's Start callback is
// invoked, and the result is native from end to end: xs.Window(...).Select((w, i) => ...).Merge()
// materializes as the platform's Window, Select and Merge with nothing in between — the inner
// window reaches the projection callback as a NativeSeq<T> *value*, and the callback's result
// is materialized in place. A runtime abstraction that dispatched each operator call to the
// platform at once could not do that: it would have to wrap every inner window with a Select and
// unwrap it with another, so the pipeline under test would not be the one written. Everything
// around the query — the scheduler, the sources, Start, the assertions — is in Harness.cs.

public interface ISeq
{
    object Accept(ISeqVisitor visitor);
}

/// <summary>A description of an observable sequence of <typeparamref name="T"/>.</summary>
public abstract class Seq<T> : ISeq
{
    public abstract object Accept(ISeqVisitor visitor);

    public abstract override string ToString();
}

/// <summary>
/// A description of an observable sequence of observable sequences of <typeparamref name="T"/>
/// (what <c>Window</c> and <c>GroupJoin</c> produce). The higher-kinded gap is confined to this
/// one type: it is not a <c>Seq&lt;Seq&lt;T&gt;&gt;</c>, because the neutral world never needs to
/// hold an inner sequence as an element value — only to describe what is done with one.
/// </summary>
public abstract class Nested<T> : ISeq
{
    public abstract object Accept(ISeqVisitor visitor);

    public abstract override string ToString();
}

/// <summary>
/// A platform is a visitor: one member per node type, each returning the platform's own
/// observable as <see cref="object"/>. The platform casts; nothing outside it does. The
/// interface is partial and each operator's file adds its members.
/// </summary>
public partial interface ISeqVisitor
{
    object Native<T>(NativeSeq<T> seq);

    object Timer(TimerSeq seq);

    object Return<T>(ReturnSeq<T> seq);

    object Range(RangeSeq seq);

    object Empty<T>(EmptySeq<T> seq);

    object Throw<T>(ThrowSeq<T> seq);

    object Select<TIn, TOut>(SelectSeq<TIn, TOut> seq);

    object SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq);

    object Where<T>(WhereSeq<T> seq);

    object SelectMany<TIn, TOut>(SelectManySeq<TIn, TOut> seq);

    object Concat<T>(ConcatSeq<T> seq);

    object Merge<T>(MergeSeq<T> seq);

    object SelectNested<TIn, TOut>(SelectNestedSeq<TIn, TOut> seq);
}

/// <summary>
/// A scheduler argument in a description: the platform's own scheduler, carried as
/// <see cref="object"/>. The test's <see cref="TestScheduler"/> is one; the result of its
/// <c>DisableOptimizations()</c> is another.
/// </summary>
public class SchedulerRef(object native, string description)
{
    public object Native => native;

    public override string ToString() => description;
}

// ---- Leaves ----

/// <summary>
/// The platform's own observable, as a leaf of a description: a testable source the test
/// created (see <see cref="TestableSeq{T}"/>), or an inner window or group handed to a
/// callback. Materializing it is the identity.
/// </summary>
public class NativeSeq<T>(object native, string description) : Seq<T>
{
    public object Native => native;

    public override object Accept(ISeqVisitor visitor) => visitor.Native(this);

    public override string ToString() => description;
}

public sealed class TimerSeq(TimeSpan dueTime, SchedulerRef scheduler) : Seq<long>
{
    public TimeSpan DueTime => dueTime;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.Timer(this);

    public override string ToString() => $"Observable.Timer({dueTime.Ticks} ticks, {scheduler})";
}

public sealed class ReturnSeq<T>(T value) : Seq<T>
{
    public T Value => value;

    public override object Accept(ISeqVisitor visitor) => visitor.Return(this);

    public override string ToString() => $"Observable.Return({value})";
}

public sealed class RangeSeq(int start, int count) : Seq<int>
{
    public int Start => start;

    public int Count => count;

    public override object Accept(ISeqVisitor visitor) => visitor.Range(this);

    public override string ToString() => $"Observable.Range({start}, {count})";
}

public sealed class EmptySeq<T> : Seq<T>
{
    public override object Accept(ISeqVisitor visitor) => visitor.Empty(this);

    public override string ToString() => $"Observable.Empty<{typeof(T).Name}>()";
}

public sealed class ThrowSeq<T>(Exception error, SchedulerRef? scheduler) : Seq<T>
{
    public Exception Error => error;

    public SchedulerRef? Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.Throw(this);

    public override string ToString() =>
        scheduler is null ? $"Observable.Throw<{typeof(T).Name}>({error.GetType().Name})" : $"Observable.Throw<{typeof(T).Name}>({error.GetType().Name}, {scheduler})";
}

/// <summary>
/// The creation operators, under the sync suite's own name so that <c>Observable.Timer(t, Scheduler)</c>
/// is the sync text with <c>scheduler</c> capitalised. Adapters that also import
/// <c>System.Reactive.Linq</c> alias one of the two.
/// </summary>
public static class Observable
{
    public static Seq<long> Timer(TimeSpan dueTime, SchedulerRef scheduler) => new TimerSeq(dueTime, scheduler);

    public static Seq<T> Return<T>(T value) => new ReturnSeq<T>(value);

    public static Seq<int> Range(int start, int count) => new RangeSeq(start, count);

    public static Seq<T> Empty<T>() => new EmptySeq<T>();

    public static Seq<T> Throw<T>(Exception error) => new ThrowSeq<T>(error, null);

    public static Seq<T> Throw<T>(Exception error, SchedulerRef scheduler) => new ThrowSeq<T>(error, scheduler);
}

// ---- Plumbing: the composition core the scenarios use around the operators under test ----

public sealed class SelectSeq<TIn, TOut>(Seq<TIn> source, Func<TIn, TOut> selector, string text) : Seq<TOut>
{
    public Seq<TIn> Source => source;

    public Func<TIn, TOut> Selector => selector;

    public override object Accept(ISeqVisitor visitor) => visitor.Select(this);

    public override string ToString() => $"{source}.Select({text})";
}

public sealed class SelectIndexedSeq<TIn, TOut>(Seq<TIn> source, Func<TIn, int, TOut> selector, string text) : Seq<TOut>
{
    public Seq<TIn> Source => source;

    public Func<TIn, int, TOut> Selector => selector;

    public override object Accept(ISeqVisitor visitor) => visitor.SelectIndexed(this);

    public override string ToString() => $"{source}.Select({text})";
}

public sealed class WhereSeq<T>(Seq<T> source, Func<T, bool> predicate, string text) : Seq<T>
{
    public Seq<T> Source => source;

    public Func<T, bool> Predicate => predicate;

    public override object Accept(ISeqVisitor visitor) => visitor.Where(this);

    public override string ToString() => $"{source}.Where({text})";
}

/// <summary>The <c>SelectMany(other)</c> form the scenarios use; AsyncRx.NET spells it <c>SelectMany(_ =&gt; other)</c>.</summary>
public sealed class SelectManySeq<TIn, TOut>(Seq<TIn> source, Seq<TOut> other) : Seq<TOut>
{
    public Seq<TIn> Source => source;

    public Seq<TOut> Other => other;

    public override object Accept(ISeqVisitor visitor) => visitor.SelectMany(this);

    public override string ToString() => $"{source}.SelectMany({other})";
}

public sealed class ConcatSeq<T>(Seq<T> first, Seq<T> second) : Seq<T>
{
    public Seq<T> First => first;

    public Seq<T> Second => second;

    public override object Accept(ISeqVisitor visitor) => visitor.Concat(this);

    public override string ToString() => $"{first}.Concat({second})";
}

public sealed class MergeSeq<T>(Nested<T> sources) : Seq<T>
{
    public Nested<T> Sources => sources;

    public override object Accept(ISeqVisitor visitor) => visitor.Merge(this);

    public override string ToString() => $"{sources}.Merge()";
}

/// <summary>
/// The flattening idiom's projection over a nested sequence. The callback receives each inner
/// sequence as a <see cref="NativeSeq{T}"/> and returns a description; the platform materializes
/// that description in place, over the real inner sequence.
/// </summary>
public sealed class SelectNestedSeq<TIn, TOut>(Nested<TIn> source, Func<Seq<TIn>, int, Seq<TOut>> selector, string text) : Nested<TOut>
{
    public Nested<TIn> Source => source;

    public Func<Seq<TIn>, int, Seq<TOut>> Selector => selector;

    public override object Accept(ISeqVisitor visitor) => visitor.SelectNested(this);

    public override string ToString() => $"{source}.Select({text})";
}

public static class SeqExtensions
{
    public static Seq<TOut> Select<TIn, TOut>(this Seq<TIn> source, Func<TIn, TOut> selector, [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SelectSeq<TIn, TOut>(source, selector, text);

    public static Seq<TOut> Select<TIn, TOut>(this Seq<TIn> source, Func<TIn, int, TOut> selector, [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SelectIndexedSeq<TIn, TOut>(source, selector, text);

    public static Seq<T> Where<T>(this Seq<T> source, Func<T, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string text = "") =>
        new WhereSeq<T>(source, predicate, text);

    public static Seq<TOut> SelectMany<TIn, TOut>(this Seq<TIn> source, Seq<TOut> other) => new SelectManySeq<TIn, TOut>(source, other);

    public static Seq<T> Concat<T>(this Seq<T> first, Seq<T> second) => new ConcatSeq<T>(first, second);

    public static Seq<T> Merge<T>(this Nested<T> sources) => new MergeSeq<T>(sources);

    public static Nested<TOut> Select<TIn, TOut>(this Nested<TIn> source, Func<Seq<TIn>, int, Seq<TOut>> selector, [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SelectNestedSeq<TIn, TOut>(source, selector, text);
}

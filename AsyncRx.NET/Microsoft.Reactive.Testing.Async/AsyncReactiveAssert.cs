// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Text;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Assertion helpers over recorded async traces.
/// </summary>
/// <remarks>
/// <para>
/// Although this does the same job as <see cref="ReactiveAssert"/>, and has superficially similar
/// usage, there are some differences driven by the needs of AsyncRx.NET. In particular, whereas
/// normal method invocation runs entirely within the space of a single tick, this is not
/// necessarily true for asynchronous methods: these might start at one tick, but then yield,
/// going on to complete during a later tick.
/// </para>
/// <para>
/// Where Rx.NET's <c>Microsoft.Reactive.Testing</c> library defines <see cref="Recorded{T}"/>,
/// which wraps a value in a single timestamp, this library defines <see cref="AsyncRecorded{T}"/>,
/// which wraps a value in a start and end timestamp. Similarly, where <see cref="Subscription"/>
/// has a pair of timestamps indicating when subscription and disposal occur, this library's'
/// <see cref="AsyncSubscription"/> has two <em>pairs</em> of timestamps, because both the
/// subscribe and dispose events might yield, meaning they might start and end on different
/// timestamps.
/// </para>
/// <para>
/// This class defines assertions as extension methods for collections of type
/// <see cref="AsyncRecorded{T}"/> and <see cref="AsyncSubscription"/>, but these offer overloads
/// enabling the expected values to be supplied in either sync or async forms. That means that
/// tests asserting equivalence with Rx.NET (in scenarios that do not exploit the potential for
/// async) can specify the expected values using the simple <see cref="Recorded{T}"/> or
/// <see cref="Subscription"/> types. (We refer to these as 'compact' overloads.) This reduces
/// verbosity in tests, and can enable sharing of code between tests for Rx.NET and AsyncRx.NET.
/// </para>
/// <para>
/// To enable these mixed assertions, in which actual asynchronous traces are compared with
/// expected synchronous traces, the overloads offered by this class are more specialized than
/// those offered by <see cref="ReactiveAssert"/>. <see cref="ReactiveAssert"/> does not
/// offer any overloads specific to <see cref="Recorded{T}"/> or <see cref="Subscription"/>,
/// instead offering more general collection comparisons that then rely on those types' support
/// for equality comparisons. That doesn't work for mixed comparisons: although we could make
/// <see cref="AsyncRecorded{T}"/> detect when it is being compared with <see cref="Recorded{T}"/>,
/// we can't do the converse, and so comparison would be asymmetric. Thus, we make these mixed
/// comparisons work by providing suitable overloads of the assertion methods.
/// </para>
/// <para>
/// The 'compact' overloads that enable a sequence of <see cref="AsyncRecorded{T}"/> events to be
/// compared with a sequence of <see cref="Recorded{T}"/> events are equivalent to assertions
/// comparing with a sequence of <see cref="AsyncRecorded{T}"/> events in which each event's
/// start and end timestamps are the same. So instead of writing something like this:
/// </para>
/// <code><![CDATA[
/// res.Messages.AssertEqual(
///   OnNext((210, 210), 9),
///   OnNext((230, 230), 13));
/// ]]></code>
/// <para>
/// we can write just this:
/// </para>
/// <code><![CDATA[
/// res.Messages.AssertEqual(
///   OnNext(210, 9),
///   OnNext(230, 13));
/// ]]></code>
/// <para>
/// Similarly, the 'compact' overloads that compare a sequence of <see cref="AsyncSubscription"/>
/// events with a sequence of <see cref="Subscription"/> events are equivalent to equality
/// assertions with a sequence of <see cref="AsyncSubscription"/> events in which subscription
/// starts and ends at the same timestamp, and also where the disposal starts and ends at the same
/// timestamp. The subscription and disposal timestamps can be different of course, because
/// <see cref="Subscription"/> can represent that. It's just that both events are required to be
/// effectively instantaneous in any such test - neither the call to
/// <see cref="IAsyncObservable{T}.SubscribeAsync(IAsyncObserver{T})"/> nor the eventual call to
/// <see cref="IAsyncDisposable.DisposeAsync"/> is allowed to yield in tests that use these simpler
/// overloads.)
/// </para>
/// </remarks>
public static class AsyncReactiveAssert
{
    /// <summary>
    /// Asserts that the sequence recorded no notifications.
    /// </summary>
    public static void AssertEmpty<T>(this IEnumerable<AsyncRecorded<Notification<T>>> actual) =>
        AssertEqual(actual, Array.Empty<Recorded<Notification<T>>>());

    /// <summary>
    /// Asserts that the recorded notifications match expectations.
    /// </summary>
    /// <remarks>
    /// This is the compact form of the assertion, which asserts that each notification's delivery
    /// started and completed at the same tick. If prolonged completion is intended, use the
    /// extended (start, end) form:
    /// <see cref="AssertEqual{T}(IEnumerable{AsyncRecorded{Notification{T}}}, AsyncRecorded{Notification{T}}[])"/>.
    /// </remarks>
    public static void AssertEqual<T>(
        this IEnumerable<AsyncRecorded<Notification<T>>> actual,
        params Recorded<Notification<T>>[] expected)
    {
        if (actual == null)
        {
            throw new ArgumentNullException(nameof(actual));
        }
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        var actualList = actual.ToList();

        AssertSameCount("notification", actualList.Count, expected.Length, () => Sequences(expected, actualList));

        for (var i = 0; i < expected.Length; i++)
        {
            var e = expected[i];
            var a = actualList[i];

            if (!a.Time.IsComplete)
            {
                Fail($"Notification #{i}: delivery of {a.Value} started at {a.Start} and never completed.", Sequences(expected, actualList));
            }

            if (a.Start != e.Time)
            {
                Fail($"Notification #{i}: delivery of {a.Value} started at {a.Start}, expected {e.Time}.", Sequences(expected, actualList));
            }

            if (a.End != a.Start)
            {
                Fail(
                    $"Notification #{i}: delivery started at {a.Start} but completed at {a.End}; the compact form asserts " +
                    "start and completion at the same tick. If prolonged completion is intended, use the extended (start, end) expectation.",
                    Sequences(expected, actualList));
            }

            // Expected is the receiver so predicate-based expectations (OnNext(ticks, v => ...)) work.
            if (!e.Value.Equals(a.Value))
            {
                Fail($"Notification #{i}: expected {e.Value}, but got {a.Value}.", Sequences(expected, actualList));
            }
        }
    }

    /// <summary>
    /// Asserts that the recorded notifications match expectations.
    /// </summary>
    /// <remarks>
    /// This compares delivery start and completion times independently. For scenarios where the
    /// operations are expected to complete instantly (in the same tick that they started) you can
    /// use the compact form:
    /// <see cref="AssertEqual{T}(IEnumerable{AsyncRecorded{Notification{T}}}, Recorded{Notification{T}}[])"/>.
    /// </remarks>
    public static void AssertEqual<T>(
        this IEnumerable<AsyncRecorded<Notification<T>>> actual,
        params AsyncRecorded<Notification<T>>[] expected)
    {
        if (actual == null)
        {
            throw new ArgumentNullException(nameof(actual));
        }
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        var actualList = actual.ToList();

        AssertSameCount("notification", actualList.Count, expected.Length, () => Sequences(expected, actualList));

        for (var i = 0; i < expected.Length; i++)
        {
            var e = expected[i];
            var a = actualList[i];

            if (a.Start != e.Start)
            {
                Fail($"Notification #{i}: delivery of {a.Value} started at {a.Start}, expected {e.Start}.", Sequences(expected, actualList));
            }

            if (a.End != e.End)
            {
                Fail($"Notification #{i}: delivery of {a.Value} completed at {FormatEnd(a.End)}, expected {FormatEnd(e.End)}.", Sequences(expected, actualList));
            }

            if (!e.Value.Equals(a.Value))
            {
                Fail($"Notification #{i}: expected {e.Value}, but got {a.Value}.", Sequences(expected, actualList));
            }
        }
    }

    /// <summary>
    /// Asserts that the sequence recorded no notifications.
    /// </summary>
    public static void AssertEmpty(this IEnumerable<AsyncSubscription> actual) =>
        AssertEqual(actual, Array.Empty<Subscription>());

    /// <summary>
    /// Asserts that the recorded subscription timings match expectations.
    /// </summary>
    /// <remarks>
    /// This is for scenarios where operations complete instantaneously. (This means that
    /// the subscription operation completes in the same tick that it started. Likewise,
    /// it means that the disposal of the subscription completes in the same tick that disposal
    /// was started). For scenarios where the operations are not instantaneous (they yield, and complete
    /// asynchronously in a later tick) you can use the full form:
    /// <see cref="AssertEqual{T}(IEnumerable{AsyncRecorded{Notification{T}}}, AsyncRecorded{Notification{T}}[])"/>.
    /// </remarks>
    public static void AssertEqual(this IEnumerable<AsyncSubscription> actual, params Subscription[] expected)
    {
        if (actual == null)
        {
            throw new ArgumentNullException(nameof(actual));
        }
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        var actualList = actual.ToList();

        AssertSameCount("subscription", actualList.Count, expected.Length, () => Sequences(expected, actualList));

        for (var i = 0; i < expected.Length; i++)
        {
            var e = expected[i];
            var a = actualList[i];

            AssertTimestamp(i, "SubscribeCalled", a.Subscribe.Start, e.Subscribe, expected, actualList);
            AssertTimestamp(i, "SubscribeCompleted", a.Subscribe.End, e.Subscribe, expected, actualList,
                compactNote: a.Subscribe.Start == e.Subscribe);
            AssertTimestamp(i, "DisposeCalled", a.Dispose.Start, e.Unsubscribe, expected, actualList);
            AssertTimestamp(i, "DisposeCompleted", a.Dispose.End, e.Unsubscribe, expected, actualList,
                compactNote: a.Dispose.Start == e.Unsubscribe);
        }
    }

    /// <summary>
    /// Asserts that the recorded subscription timings match expectations.
    /// </summary>
    /// <remarks>
    /// This compares delivery start and completion times independently (for both subscription and
    /// disposal, meaning tests specify 4 timestamps). For scenarios where the operations are
    /// expected to complete instantly (in the same tick that they started) you can use the compact
    /// form:
    /// <see cref="AssertEqual{T}(IEnumerable{AsyncRecorded{Notification{T}}}, Recorded{Notification{T}}[])"/>.
    /// </remarks>
    public static void AssertEqual(this IEnumerable<AsyncSubscription> actual, params AsyncSubscription[] expected)
    {
        if (actual == null)
        {
            throw new ArgumentNullException(nameof(actual));
        }
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        var actualList = actual.ToList();

        AssertSameCount("subscription", actualList.Count, expected.Length, () => Sequences(expected, actualList));

        for (var i = 0; i < expected.Length; i++)
        {
            var e = expected[i];
            var a = actualList[i];

            AssertTimestamp(i, "SubscribeCalled", a.Subscribe.Start, e.Subscribe.Start, expected, actualList);
            AssertTimestamp(i, "SubscribeCompleted", a.Subscribe.End, e.Subscribe.End, expected, actualList);
            AssertTimestamp(i, "DisposeCalled", a.Dispose.Start, e.Dispose.Start, expected, actualList);
            AssertTimestamp(i, "DisposeCompleted", a.Dispose.End, e.Dispose.End, expected, actualList);
        }
    }

    private static void AssertTimestamp<TExpected>(int index, string name, long actual, long expected, IEnumerable<TExpected> expectedSeq, IEnumerable<AsyncSubscription> actualSeq, bool compactNote = false)
    {
        if (actual != expected)
        {
            var note = compactNote
                ? " (the compact form asserts call and completion at the same tick; use the broken-down four-timestamp form if the divergence is intended)"
                : string.Empty;

            Fail($"Subscription #{index}: {name} was {FormatTime(actual)}, expected {FormatTime(expected)}{note}.", Sequences(expectedSeq, actualSeq));
        }
    }

    private static void AssertSameCount(string what, int actualCount, int expectedCount, Func<string> sequences)
    {
        if (actualCount != expectedCount)
        {
            Fail($"Expected {expectedCount} {what}(s) but got {actualCount}.", sequences());
        }
    }

    private static string FormatEnd(long time) => time == OperationTime.Infinite ? "(incomplete)" : time.ToString();

    private static string FormatTime(long time) => time == OperationTime.Infinite ? "Infinite" : time.ToString();

    private static string Sequences<TExpected, TActual>(IEnumerable<TExpected> expected, IEnumerable<TActual> actual)
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.Append("Expected: [").Append(string.Join(", ", expected)).AppendLine("]");
        sb.Append("Actual..: [").Append(string.Join(", ", actual)).Append(']');
        return sb.ToString();
    }

    private static void Fail(string message, string sequences) =>
        throw new AsyncReactiveAssertException(message + sequences);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Text;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>Thrown when an async trace assertion fails.</summary>
public class AsyncReactiveAssertException : Exception
{
    public AsyncReactiveAssertException(string message)
        : base(message)
    {
    }
}

/// <summary>
/// Assertion helpers over recorded async traces, mirroring the sync
/// <see cref="ReactiveAssert"/> conventions. The compact overloads take the shared (sync)
/// vocabulary — <see cref="Recorded{T}"/> and <see cref="Subscription"/> — and assert that
/// each async process's start and completion coincide; failures name exactly which
/// timestamp mismatched. The extended overloads take the four-timestamp /
/// two-timestamp async records for tests where the divergence is the point.
/// </summary>
public static class AsyncReactiveAssert
{
    /// <summary>Asserts the observer recorded no notifications.</summary>
    public static void AssertEqual<T>(this IEnumerable<AsyncRecorded<Notification<T>>> actual) =>
        AssertEqual(actual, Array.Empty<Recorded<Notification<T>>>());

    /// <summary>
    /// Asserts the recorded notifications match the compact expectations: for each, delivery
    /// started <em>and</em> completed at the expected tick, with the expected value.
    /// </summary>
    public static void AssertEqual<T>(this IEnumerable<AsyncRecorded<Notification<T>>> actual, params Recorded<Notification<T>>[] expected)
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
    /// Asserts the recorded notifications match extended expectations, comparing delivery
    /// start and completion times independently.
    /// </summary>
    public static void AssertEqual<T>(this IEnumerable<AsyncRecorded<Notification<T>>> actual, params AsyncRecorded<Notification<T>>[] expected)
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

    /// <summary>Asserts no subscriptions were recorded.</summary>
    public static void AssertEqual(this IEnumerable<AsyncSubscription> actual) =>
        AssertEqual(actual, Array.Empty<Subscription>());

    /// <summary>
    /// Asserts the recorded subscriptions match the compact expectations:
    /// <c>Subscribe(200, 590)</c> means subscribe was called <em>and</em> completed at 200,
    /// and dispose was called <em>and</em> completed at 590.
    /// </summary>
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
    /// Asserts the recorded subscriptions match broken-down four-timestamp expectations.
    /// </summary>
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

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

using Microsoft.Reactive.Testing.Async;

namespace Tests.Microsoft.Reactive.Testing.Async;

/// <summary>
/// Tests for the assertion helpers: the compact forms assert that each async process's
/// start and completion coincide, and every failure names the timestamp that mismatched.
/// </summary>
[TestClass]
public class AsyncReactiveAssertTest : AsyncReactiveTest
{
    private static AsyncRecorded<Notification<int>> Delivered(long start, long end, int value) =>
        new(start, end, Notification.CreateOnNext(value));

    [TestMethod]
    public void Compact_message_assertion_passes_when_start_and_end_coincide()
    {
        var actual = new[] { Delivered(210, 210, 2), Delivered(220, 220, 3) };

        actual.AssertEqual(OnNext(210, 2), OnNext(220, 3));
    }

    [TestMethod]
    public void Compact_message_assertion_supports_value_predicates()
    {
        var actual = new[] { Delivered(210, 210, 5) };

        actual.AssertEqual(OnNext<int>(210, x => x > 4));
    }

    [TestMethod]
    public void Compact_message_assertion_reports_incomplete_delivery()
    {
        var actual = new[] { new AsyncRecorded<Notification<int>>(OperationTime.StartingAt(210), Notification.CreateOnNext(2)) };

        var thrown = Assert.ThrowsExactly<AsyncReactiveAssertException>(() => actual.AssertEqual(OnNext(210, 2)));

        Assert.Contains("started at 210 and never completed", thrown.Message);
    }

    [TestMethod]
    public void Compact_message_assertion_reports_prolonged_completion_and_points_to_the_extended_form()
    {
        var actual = new[] { Delivered(210, 250, 2) };

        var thrown = Assert.ThrowsExactly<AsyncReactiveAssertException>(() => actual.AssertEqual(OnNext(210, 2)));

        Assert.Contains("started at 210 but completed at 250", thrown.Message);
        Assert.Contains("extended", thrown.Message);
    }

    [TestMethod]
    public void Compact_message_assertion_reports_wrong_start_time()
    {
        var actual = new[] { Delivered(215, 215, 2) };

        var thrown = Assert.ThrowsExactly<AsyncReactiveAssertException>(() => actual.AssertEqual(OnNext(210, 2)));

        Assert.Contains("started at 215, expected 210", thrown.Message);
    }

    [TestMethod]
    public void Extended_message_assertion_passes_for_prolonged_completion()
    {
        var actual = new[] { Delivered(210, 250, 2) };

        actual.AssertEqual(OnNext((210, 250), 2));
    }

    [TestMethod]
    public void Extended_message_assertion_reports_wrong_completion_time()
    {
        var actual = new[] { Delivered(210, 250, 2) };

        var thrown = Assert.ThrowsExactly<AsyncReactiveAssertException>(() => actual.AssertEqual(OnNext((210, 240), 2)));

        Assert.Contains("completed at 250, expected 240", thrown.Message);
    }

    [TestMethod]
    public void Message_count_mismatch_is_reported_with_both_sequences()
    {
        var actual = new[] { Delivered(210, 210, 2) };

        var thrown = Assert.ThrowsExactly<AsyncReactiveAssertException>(() => actual.AssertEqual(OnNext(210, 2), OnNext(220, 3)));

        Assert.Contains("Expected 2 notification(s) but got 1", thrown.Message);
        Assert.Contains("Expected:", thrown.Message);
        Assert.Contains("Actual..:", thrown.Message);
    }

    [TestMethod]
    public void Empty_message_assertion_passes_for_no_messages()
    {
        Array.Empty<AsyncRecorded<Notification<int>>>().AssertEmpty();
    }

    [TestMethod]
    public void Compact_subscription_assertion_passes_when_all_four_timestamps_pair_up()
    {
        var actual = new[] { new AsyncSubscription(200, 200, 590, 590) };

        actual.AssertEqual(Subscribe(200, 590));
    }

    [TestMethod]
    public void Compact_subscription_assertion_treats_a_single_time_as_never_disposed()
    {
        var actual = new[] { new AsyncSubscription(200, 200, OperationTime.Infinite, OperationTime.Infinite) };

        actual.AssertEqual(Subscribe(200));
    }

    [TestMethod]
    public void Compact_subscription_assertion_names_the_mismatched_timestamp()
    {
        // Subscribe completed one tick after it was called: a genuine discovery on the async
        // platform. The compact form must reject it and say exactly which timestamp diverged.
        var actual = new[] { new AsyncSubscription(200, 201, 590, 590) };

        var thrown = Assert.ThrowsExactly<AsyncReactiveAssertException>(() => actual.AssertEqual(Subscribe(200, 590)));

        Assert.Contains("SubscribeCompleted was 201, expected 200", thrown.Message);
        Assert.Contains("compact form", thrown.Message);
    }

    [TestMethod]
    public void Four_timestamp_subscription_assertion_passes_when_divergence_is_expected()
    {
        var actual = new[] { new AsyncSubscription(200, 201, 590, 592) };

        actual.AssertEqual(Subscribe(200, 201, 590, 592));
    }

    [TestMethod]
    public void Four_timestamp_subscription_assertion_names_the_mismatched_timestamp()
    {
        var actual = new[] { new AsyncSubscription(200, 201, 590, 592) };

        var thrown = Assert.ThrowsExactly<AsyncReactiveAssertException>(() => actual.AssertEqual(Subscribe(200, 201, 590, 591)));

        Assert.Contains("DisposeCompleted was 592, expected 591", thrown.Message);
    }

    [TestMethod]
    public void Subscription_records_print_compactly_when_possible()
    {
        Assert.AreEqual("Subscribe(200, 590)", new AsyncSubscription(200, 200, 590, 590).ToString());
        Assert.AreEqual("Subscribe(200)", new AsyncSubscription(200, 200, OperationTime.Infinite, OperationTime.Infinite).ToString());
        Assert.Contains("completed: 201", new AsyncSubscription(200, 201, 590, 590).ToString());
    }
}

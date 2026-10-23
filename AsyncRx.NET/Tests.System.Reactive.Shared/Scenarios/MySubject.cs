// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>A subject that counts its subscriptions and records that one was disposed.</summary>
/// <remarks>
/// <para>
/// Rx.NET's <c>MySubject</c> (<c>Tests/MySubject.cs</c>), which is shared by its
/// <c>RefCountTest</c> and <c>ConnectableObservableTest</c>, so it is a top-level type here
/// too rather than a nested one, unlike the doubles that one scenario class owns. It forwards
/// each notification to its current observers; counts every subscription in
/// <see cref="SubscribeCount"/>; sets <see cref="Disposed"/> when any subscription is disposed;
/// and, after forwarding a value registered through <see cref="DisposeOn"/>, disposes the
/// disposable registered for it, which is how a test disconnects a connectable from inside its
/// own notification. Each target builds the object that behaves this way over its own observer
/// type, recording into the <see cref="State"/>; this type is that object's wrapper, a
/// <see cref="SubjectSeq{T}"/> so that it goes wherever a subject does, as the original goes
/// wherever an <c>ISubject&lt;int&gt;</c> does.
/// </para>
/// <para>
/// Created by <see cref="SharedReactiveTest.CreateMySubject"/>, where the Rx.NET test writes
/// <c>new MySubject()</c>.
/// </para>
/// </remarks>
public sealed class MySubject(SubjectSeq<int> subject, MySubject.State state)
    : SubjectSeq<int>(subject)
{
    /// <summary>How many times the subject has been subscribed to.</summary>
    public int SubscribeCount => state.SubscribeCount;

    /// <summary>Whether any subscription to the subject has been disposed.</summary>
    public bool Disposed => state.Disposed;

    /// <summary>
    /// Disposes <paramref name="disposable"/> after <paramref name="value"/> is forwarded.
    /// </summary>
    /// <param name="value">The value whose arrival triggers the disposal.</param>
    /// <param name="disposable">What to dispose, typically a connection.</param>
    public void DisposeOn(int value, IAsyncDisposable disposable) =>
        state.DisposeOn[value] = disposable;

    /// <summary>What the double remembers, and what the target's object reads.</summary>
    public sealed class State
    {
        /// <summary>How many times the subject has been subscribed to.</summary>
        public int SubscribeCount { get; set; }

        /// <summary>Whether any subscription to the subject has been disposed.</summary>
        public bool Disposed { get; set; }

        /// <summary>What to dispose after forwarding each registered value.</summary>
        public Dictionary<int, IAsyncDisposable> DisposeOn { get; } = [];
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The awaiter of a sequence, which a target's <c>GetAwaiter</c> or <c>RunAsync</c> returns.</summary>
/// <typeparam name="T">The element type.</typeparam>
/// <remarks>
/// <para>
/// Rx.NET's <c>GetAwaiter</c> and <c>RunAsync</c> return an <c>AsyncSubject&lt;T&gt;</c>,
/// AsyncRx.NET's an <c>AsyncAsyncSubject&lt;T&gt;</c>. Both are awaiters in the C# sense: a
/// completion flag, a continuation to register, and a result that is the last element or
/// throws the sequence's error, and the scenarios use them as nothing else. Each target has
/// one adapter over its own subject, so a scenario holds the awaiter by this type and never
/// sees the subject.
/// </para>
/// <para>
/// <see cref="GetAwaiter"/> returns this, so a sequence's awaiter can also be awaited.
/// </para>
/// </remarks>
public abstract class SeqAwaiter<T> : INotifyCompletion
{
    /// <summary>Whether the sequence has terminated.</summary>
    public abstract bool IsCompleted { get; }

    /// <summary>Registers a continuation to run when the sequence terminates.</summary>
    /// <param name="continuation">The continuation.</param>
    /// <remarks>
    /// Runs it at once if the sequence has already terminated, and on the synchronization
    /// context current at registration if there is one, as both libraries do.
    /// </remarks>
    public abstract void OnCompleted(Action continuation);

    /// <summary>Gets the last element of the sequence.</summary>
    /// <exception cref="InvalidOperationException">The sequence completed without an element.</exception>
    /// <remarks>
    /// Throws the sequence's error if it failed, and blocks until the sequence terminates if
    /// it has not yet.
    /// </remarks>
    public abstract T GetResult();

    /// <summary>This awaiter.</summary>
    public SeqAwaiter<T> GetAwaiter() => this;
}

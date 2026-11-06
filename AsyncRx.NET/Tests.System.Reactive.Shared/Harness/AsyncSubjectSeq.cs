// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>An async subject the scenario drives by hand, and can await.</summary>
/// <param name="subject">The plain wrapper the target built for the subject.</param>
/// <param name="awaiter">The target's subject as its own awaiter.</param>
/// <remarks>
/// A <see cref="SubjectSeq{T}"/> that is awaitable, as Rx.NET's <c>AsyncSubject&lt;T&gt;</c>
/// is its own awaiter: the <c>AsyncSubject</c> scenarios read <see cref="IsCompleted"/>, call
/// <see cref="GetResult"/>, or <c>await</c> the subject where the Rx.NET test does so on the
/// subject itself. Created by <see cref="SharedReactiveTest.CreateAsyncSubject{T}"/>.
/// </remarks>
public sealed class AsyncSubjectSeq<T>(SubjectSeq<T> subject, SeqAwaiter<T> awaiter)
    : SubjectSeq<T>(subject)
{
    /// <summary>Whether the subject has terminated.</summary>
    /// <remarks>The target's own <c>IsCompleted</c>.</remarks>
    public bool IsCompleted => awaiter.IsCompleted;

    /// <summary>The subject's final value, once it has one.</summary>
    /// <remarks>
    /// The target's own <c>GetResult</c>: the last value before completion, the error if it
    /// failed, or <see cref="InvalidOperationException"/> if it completed without a value;
    /// blocks until it has terminated.
    /// </remarks>
    public T GetResult() => awaiter.GetResult();

    /// <summary>The subject as an awaiter, so that a scenario can <c>await</c> it.</summary>
    public SeqAwaiter<T> GetAwaiter() => awaiter;
}

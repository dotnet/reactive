// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Subjects;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

/// <summary>
/// AsyncRx.NET's <see cref="AsyncAsyncSubject{T}"/> as the awaiter a scenario holds.
/// </summary>
/// <param name="subject">The subject <c>GetAwaiter</c> or <c>RunAsync</c> returned.</param>
internal sealed class AsyncRxSeqAwaiter<T>(AsyncAsyncSubject<T> subject) : SeqAwaiter<T>
{
    public override bool IsCompleted => subject.IsCompleted;

    public override void OnCompleted(Action continuation) => subject.OnCompleted(continuation);

    public override T GetResult() => subject.GetResult();
}

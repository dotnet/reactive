// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A subject the scenario drives by hand.</summary>
/// <param name="native">The target's own subject.</param>
/// <param name="onNext">Pushes a value through the target's subject.</param>
/// <param name="onError">Pushes an error through the target's subject.</param>
/// <param name="onCompleted">Completes the target's subject.</param>
/// <remarks>
/// A leaf, so it composes like any other sequence, with the three observer methods a test calls
/// where the Rx.NET test calls them on a <c>Subject&lt;T&gt;</c>. They are async-shaped so that
/// the same scenario text drives AsyncRx.NET's subject, whose methods are awaited; on Rx.NET they
/// complete synchronously. Created by <see cref="SharedReactiveTest.CreateSubject{T}"/>.
/// </remarks>
public sealed class SubjectSeq<T>(
    Realized<Seq<T>> native,
    Func<T, ValueTask> onNext,
    Func<Exception, ValueTask> onError,
    Func<ValueTask> onCompleted) : NativeSeq<T>(native, "subject")
{
    /// <summary>Pushes <paramref name="value"/> to the subject's observers.</summary>
    /// <param name="value">The value to push.</param>
    public ValueTask OnNextAsync(T value) => onNext(value);

    /// <summary>Pushes <paramref name="error"/> to the subject's observers.</summary>
    /// <param name="error">The error to push.</param>
    public ValueTask OnErrorAsync(Exception error) => onError(error);

    /// <summary>Completes the subject.</summary>
    public ValueTask OnCompletedAsync() => onCompleted();
}

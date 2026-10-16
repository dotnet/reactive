// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A subject the scenario drives by hand.</summary>
/// <remarks>
/// A leaf, so it composes like any other sequence, with the three observer methods a test calls
/// where the Rx.NET test calls them on a <c>Subject&lt;T&gt;</c>. They are async-shaped so that
/// the same scenario text drives AsyncRx.NET's subject, whose methods are awaited; on Rx.NET they
/// complete synchronously. Created by <see cref="SharedReactiveTest.CreateSubject{T}"/>. A test
/// double that is a subject with more to it, such as <see cref="Scenarios.MySubject"/>, derives
/// from this through the copying constructor, over the plain wrapper its target built.
/// </remarks>
public class SubjectSeq<T> : NativeSeq<T>
{
    private readonly Func<T, ValueTask> _onNext;
    private readonly Func<Exception, ValueTask> _onError;
    private readonly Func<ValueTask> _onCompleted;

    /// <summary>Wraps a target's subject.</summary>
    /// <param name="native">The target's own subject.</param>
    /// <param name="onNext">Pushes a value through the target's subject.</param>
    /// <param name="onError">Pushes an error through the target's subject.</param>
    /// <param name="onCompleted">Completes the target's subject.</param>
    public SubjectSeq(
        Realized<Seq<T>> native,
        Func<T, ValueTask> onNext,
        Func<Exception, ValueTask> onError,
        Func<ValueTask> onCompleted)
        : base(native, "subject")
    {
        _onNext = onNext;
        _onError = onError;
        _onCompleted = onCompleted;
    }

    /// <summary>Stands for the same target subject as <paramref name="subject"/>.</summary>
    /// <param name="subject">The plain wrapper a target built for the subject.</param>
    /// <remarks>
    /// For a derived type that adds what a test double needs beyond the observer methods. The
    /// target builds the subject object and wraps it as it does any subject; the double is then
    /// built over that wrapper, so no target needs to know the double's constructor.
    /// </remarks>
    protected SubjectSeq(SubjectSeq<T> subject)
        : this(subject.Native, subject._onNext, subject._onError, subject._onCompleted)
    {
    }

    /// <summary>Pushes <paramref name="value"/> to the subject's observers.</summary>
    /// <param name="value">The value to push.</param>
    public ValueTask OnNextAsync(T value) => _onNext(value);

    /// <summary>Pushes <paramref name="error"/> to the subject's observers.</summary>
    /// <param name="error">The error to push.</param>
    public ValueTask OnErrorAsync(Exception error) => _onError(error);

    /// <summary>Completes the subject.</summary>
    public ValueTask OnCompletedAsync() => _onCompleted();
}

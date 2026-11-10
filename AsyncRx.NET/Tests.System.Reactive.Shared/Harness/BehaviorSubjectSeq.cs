// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A behavior subject the scenario drives by hand.</summary>
/// <param name="subject">The plain wrapper the target built for the subject.</param>
/// <param name="value">Reads the target's subject's current value.</param>
/// <param name="tryGetValue">Tries to read the target's subject's current value.</param>
/// <remarks>
/// A <see cref="SubjectSeq{T}"/> with the two members a behavior subject adds, which the
/// <c>BehaviorSubject</c> scenarios read where the Rx.NET test reads them on a
/// <c>BehaviorSubject&lt;T&gt;</c>. Created by
/// <see cref="SharedReactiveTest.CreateBehaviorSubject{T}"/>.
/// </remarks>
public sealed class BehaviorSubjectSeq<T>(
    SubjectSeq<T> subject,
    Func<T> value,
    BehaviorSubjectSeq<T>.TryGetValueFunc tryGetValue)
    : SubjectSeq<T>(subject)
{
    /// <summary>Tries to read a subject's current value.</summary>
    /// <param name="value">The current value, when there is one.</param>
    /// <returns>Whether there was a value to read.</returns>
    public delegate bool TryGetValueFunc(out T value);

    /// <summary>The subject's current value.</summary>
    /// <remarks>
    /// The target's own <c>Value</c>: the latest value pushed, or the initial one; throws the
    /// subject's error once it has one.
    /// </remarks>
    public T Value => value();

    /// <summary>Tries to read the subject's current value.</summary>
    /// <param name="value">The current value, when there is one.</param>
    /// <returns>Whether there was a value to read.</returns>
    /// <remarks>The target's own <c>TryGetValue</c>.</remarks>
    public bool TryGetValue(out T value) => tryGetValue(out value);
}

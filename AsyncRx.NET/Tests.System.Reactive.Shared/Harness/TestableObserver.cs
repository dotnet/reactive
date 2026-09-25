// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The recording observer returned by <see cref="TestScheduler.Start{T}(Func{Seq{T}})"/>, or created for the raw surface.</summary>
public sealed class TestableObserver<T>(IRxTarget target, object native, string query)
{
    public IRxTarget Target => target;

    public object Native => native;

    /// <summary>The query this observer was started over (its description), for diagnostics.</summary>
    public string Query => query;

    public MessageLog<T> Messages => new(target, native, query);
}

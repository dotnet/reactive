// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reflection;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// Rx.NET's test double that sets the running count inside an aggregate's sink before
/// subscribing, so that the next element overflows it.
/// </summary>
/// <remarks>
/// From Rx.NET's <c>CountTest.cs</c>, where it serves the <c>Average</c>, <c>Count</c> and
/// <c>LongCount</c> overflow-injection tests; the same three runners use it here. It reaches
/// into Rx.NET's sink by reflection on its private <c>_count</c> field, which is why those
/// tests are Rx.NET's own.
/// </remarks>
/// <typeparam name="T">The type of the elements in the sequence.</typeparam>
/// <param name="source">The sequence to subscribe to once the count is set.</param>
/// <param name="initialCount">The value to set the sink's count to.</param>
internal sealed class OverflowInjection<T>(IObservable<T> source, object initialCount)
    : IObservable<T>
{
    /// <inheritdoc/>
    public IDisposable Subscribe(IObserver<T> observer)
    {
        var f = observer.GetType().GetField(
            "_count",
            BindingFlags.Instance | BindingFlags.NonPublic);
        f!.SetValue(observer, initialCount);

        return source.Subscribe(observer);
    }
}

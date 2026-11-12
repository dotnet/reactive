// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>A comparer that throws on every comparison.</summary>
/// <typeparam name="T">The type of the objects to compare.</typeparam>
/// <param name="exception">The exception to throw.</param>
/// <remarks>
/// Rx.NET's <c>ThrowingComparer</c>, shared by its <c>Min</c> and <c>Max</c> tests, so a
/// top-level type here.
/// </remarks>
public sealed class ThrowingComparer<T>(Exception exception) : IComparer<T>
{
    /// <inheritdoc/>
    public int Compare(T? x, T? y) => throw exception;
}

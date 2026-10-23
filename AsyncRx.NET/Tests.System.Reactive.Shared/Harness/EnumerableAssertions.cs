// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;

namespace Tests.System.Reactive.Shared;

/// <summary>The harness's element-wise <c>AssertEqual</c>, for the scenarios.</summary>
/// <remarks>
/// Rx.NET's tests assert over the collections an operator yields with
/// <c>ReactiveAssert</c>'s <c>AssertEqual</c> over plain enumerables (<c>ToLookup</c>'s
/// groups, for instance). The harness has the same pair on
/// <see cref="AsyncReactiveAssert"/>; the scenarios reach it through this class, as they
/// reach the rest of the harness through the kit, without importing its namespace.
/// </remarks>
public static class EnumerableAssertions
{
    /// <summary>
    /// Asserts that an enumerable sequence holds the expected elements, in order.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="actual">The sequence to check.</param>
    /// <param name="expected">The elements it should hold.</param>
    public static void AssertEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected) =>
        AsyncReactiveAssert.AssertEqual(actual, expected);

    /// <summary>
    /// Asserts that an enumerable sequence holds the expected elements, in order.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="actual">The sequence to check.</param>
    /// <param name="expected">The elements it should hold.</param>
    public static void AssertEqual<T>(this IEnumerable<T> actual, params T[] expected) =>
        AsyncReactiveAssert.AssertEqual(actual, expected);
}

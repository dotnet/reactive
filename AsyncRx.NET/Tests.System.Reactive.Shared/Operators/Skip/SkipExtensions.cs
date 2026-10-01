// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Skip</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
public static class SkipExtensions
{
    /// <summary>Describes <c>source.Skip(count)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence to skip elements from.</param>
    /// <param name="count">
    /// The number of elements to skip before returning the remaining elements.
    /// </param>
    public static Seq<T> Skip<T>(this Seq<T> source, int count) => new SkipSeq<T>(source, count);
}

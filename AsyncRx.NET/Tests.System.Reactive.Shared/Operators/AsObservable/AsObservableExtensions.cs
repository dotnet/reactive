// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>AsObservable</c> operator, which hides a sequence's identity.</summary>
/// <remarks>
/// Rx.NET's <c>AsObservable</c>; AsyncRx.NET's is <c>AsAsyncObservable</c>, and the shared name
/// is Rx.NET's, as for the rest of the model.
/// </remarks>
public static class AsObservableExtensions
{
    /// <summary>Describes <c>source.AsObservable()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence to hide identity of.</param>
    /// <remarks>
    /// Builds an <see cref="AsObservableSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AsObservable{T}(AsObservableSeq{T})"/>.
    /// </remarks>
    public static Seq<T> AsObservable<T>(this Seq<T> source) => new AsObservableSeq<T>(source);
}

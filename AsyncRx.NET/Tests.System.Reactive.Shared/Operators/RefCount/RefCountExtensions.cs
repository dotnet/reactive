// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>RefCount</c> overloads, as extension methods on <see cref="ConnectableSeq{T}"/>.
/// </summary>
/// <remarks>
/// The first operator whose input is a connectable: the bridge maps a
/// <see cref="ConnectableSeq{T}"/> argument to the target's own connectable type, as it maps a
/// <see cref="Seq{T}"/> to its observable type. Each method builds one node of the query
/// description (one node type per overload, in this folder), and each target turns that node
/// into its own <c>RefCount</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.RefCount.cs</c>. The delayed-disconnect overloads are still to come.
/// </remarks>
public static class RefCountExtensions
{
    /// <summary>Describes <c>source.RefCount()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Connectable observable sequence.</param>
    /// <remarks>
    /// Builds a <see cref="RefCountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.RefCount{T}(RefCountSeq{T})"/>.
    /// </remarks>
    public static Seq<T> RefCount<T>(this ConnectableSeq<T> source) => new RefCountSeq<T>(source);

    /// <summary>Describes <c>source.RefCount(minObservers)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Connectable observable sequence.</param>
    /// <param name="minObservers">The minimum number of observers required to connect.</param>
    /// <remarks>
    /// Builds a <see cref="RefCountMinObserversSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.RefCountMinObservers{T}(RefCountMinObserversSeq{T})"/>.
    /// </remarks>
    public static Seq<T> RefCount<T>(this ConnectableSeq<T> source, int minObservers) =>
        new RefCountMinObserversSeq<T>(source, minObservers);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>DefaultIfEmpty</c> overloads, as extension methods on <see cref="Seq{T}"/>.
/// </summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.DefaultIfEmpty()</c> or
/// <c>xs.DefaultIfEmpty(-1)</c> exactly as the Rx.NET test it was migrated from does. Each
/// method builds one node of the query description (one node type per overload, in this
/// folder), which each target turns into its own <c>DefaultIfEmpty</c> call through the
/// matching <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.DefaultIfEmpty.cs</c>.
/// </remarks>
public static class DefaultIfEmptyExtensions
{
    /// <summary>Describes <c>source.DefaultIfEmpty()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence to return a default value for if it is empty.</param>
    /// <remarks>
    /// Builds a <see cref="DefaultIfEmptySeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.DefaultIfEmpty{T}(DefaultIfEmptySeq{T})"/>.
    /// </remarks>
    public static Seq<T> DefaultIfEmpty<T>(this Seq<T> source) => new DefaultIfEmptySeq<T>(source);

    /// <summary>Describes <c>source.DefaultIfEmpty(defaultValue)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence to return a default value for if it is empty.</param>
    /// <param name="defaultValue">The value to return if the sequence is empty.</param>
    /// <remarks>
    /// Builds a <see cref="DefaultIfEmptyValueSeq{T}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.DefaultIfEmptyValue{T}(DefaultIfEmptyValueSeq{T})"/>.
    /// </remarks>
    public static Seq<T> DefaultIfEmpty<T>(this Seq<T> source, T defaultValue) =>
        new DefaultIfEmptyValueSeq<T>(source, defaultValue);
}

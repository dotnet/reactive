// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Retry</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Retry()</c> or <c>xs.Retry(3)</c> exactly
/// as the Rx.NET test it was migrated from does. Each method builds one node of the query
/// description (one node type per overload, in this folder), which each target turns into its
/// own <c>Retry</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.Retry.cs</c>.
/// </remarks>
public static class RetryExtensions
{
    /// <summary>Describes <c>source.Retry()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Observable sequence to repeat until it successfully terminates.</param>
    /// <remarks>
    /// Builds a <see cref="RetrySeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Retry{T}(RetrySeq{T})"/>.
    /// </remarks>
    public static Seq<T> Retry<T>(this Seq<T> source) => new RetrySeq<T>(source);

    /// <summary>Describes <c>source.Retry(retryCount)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Observable sequence to repeat until it successfully terminates.</param>
    /// <param name="retryCount">Number of times to repeat the sequence.</param>
    /// <remarks>
    /// Builds a <see cref="RetryCountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.RetryCount{T}(RetryCountSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Retry<T>(this Seq<T> source, int retryCount) =>
        new RetryCountSeq<T>(source, retryCount);
}

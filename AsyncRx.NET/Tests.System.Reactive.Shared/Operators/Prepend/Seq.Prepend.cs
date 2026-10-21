// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Prepend</c> overloads, as extension methods on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.Prepend(3)</c>, a shared scenario writes the same over a
/// <see cref="Seq{T}"/>; the overload set is Rx.NET's (one value, with or without a
/// scheduler), so the same call resolves to the same overload on both sides. One node type per
/// overload, in this folder, each materialized by a target through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Prepend.cs</c>.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>source.Prepend(value)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to prepend the value to.</param>
    /// <param name="value">Value to prepend to the specified sequence.</param>
    /// <remarks>
    /// Builds an <see cref="PrependSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Prepend{T}(PrependSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Prepend<T>(this Seq<T> source, T value) =>
        new PrependSeq<T>(source, value);

    /// <summary>Describes <c>source.Prepend(value, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to prepend the value to.</param>
    /// <param name="value">Value to prepend to the specified sequence.</param>
    /// <param name="scheduler">Scheduler to emit the prepend values on.</param>
    /// <remarks>
    /// Builds an <see cref="PrependSchedulerSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.PrependScheduler{T}(PrependSchedulerSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Prepend<T>(this Seq<T> source, T value, SchedulerRef scheduler) =>
        new PrependSchedulerSeq<T>(source, value, scheduler);
}

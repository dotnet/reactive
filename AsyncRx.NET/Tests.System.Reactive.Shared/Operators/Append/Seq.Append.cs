// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Append</c> overloads, as extension methods on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.Append(3)</c>, a shared scenario writes the same over a
/// <see cref="Seq{T}"/>; the overload set is Rx.NET's (one value, with or without a
/// scheduler), so the same call resolves to the same overload on both sides. One node type per
/// overload, in this folder, each materialized by a target through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Append.cs</c>.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>source.Append(value)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to append the value to.</param>
    /// <param name="value">Value to append to the specified sequence.</param>
    /// <remarks>
    /// Builds an <see cref="AppendSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Append{T}(AppendSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Append<T>(this Seq<T> source, T value) =>
        new AppendSeq<T>(source, value);

    /// <summary>Describes <c>source.Append(value, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to append the value to.</param>
    /// <param name="value">Value to append to the specified sequence.</param>
    /// <param name="scheduler">Scheduler to emit the append values on.</param>
    /// <remarks>
    /// Builds an <see cref="AppendSchedulerSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AppendScheduler{T}(AppendSchedulerSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Append<T>(this Seq<T> source, T value, SchedulerRef scheduler) =>
        new AppendSchedulerSeq<T>(source, value, scheduler);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Sample</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Sample(...)</c> exactly as the Rx.NET
/// test it was migrated from does: the time-based form, with and without the scheduler, and the
/// sampler form. Each method builds one node of the query description (one node type per
/// overload, in this folder), and each target turns that node into its own <c>Sample</c> call
/// through the matching <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Sample.cs</c>.
/// </remarks>
public static class SampleExtensions
{
    /// <summary>Describes <c>source.Sample(interval, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to sample.</param>
    /// <param name="interval">
    /// Interval at which to sample. If this value is equal to TimeSpan.Zero, the scheduler will
    /// continuously sample the stream.
    /// </param>
    /// <param name="scheduler">Scheduler to run the sampling timer on.</param>
    /// <remarks>
    /// Builds a <see cref="SampleTimeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SampleTime{T}(SampleTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Sample<T>(this Seq<T> source, TimeSpan interval, SchedulerRef scheduler) =>
        new SampleTimeSeq<T>(source, interval, scheduler);

    /// <summary>
    /// Describes <c>source.Sample(interval)</c>, on the target's default scheduler.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to sample.</param>
    /// <param name="interval">
    /// Interval at which to sample. If this value is equal to TimeSpan.Zero, the scheduler will
    /// continuously sample the stream.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SampleTimeSeq{T}"/> with no scheduler, which each target
    /// materializes through <see cref="ISeqVisitor.SampleTime{T}(SampleTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Sample<T>(this Seq<T> source, TimeSpan interval) =>
        new SampleTimeSeq<T>(source, interval, null);

    /// <summary>Describes <c>source.Sample(sampler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TSample">The type of the elements in the sampling sequence.</typeparam>
    /// <param name="source">Source sequence to sample.</param>
    /// <param name="sampler">Sampling tick sequence.</param>
    /// <remarks>
    /// Builds a <see cref="SampleSamplerSeq{T, TSample}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.SampleSampler{T, TSample}(SampleSamplerSeq{T, TSample})"/>.
    /// </remarks>
    public static Seq<T> Sample<T, TSample>(this Seq<T> source, Seq<TSample> sampler) =>
        new SampleSamplerSeq<T, TSample>(source, sampler);
}

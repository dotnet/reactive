// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="SampleTimeSeq{T}"/> as the target's own
    /// <c>Sample(interval[, scheduler])</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SampleExtensions.Sample{T}(Seq{T}, TimeSpan, SchedulerRef)"/> or
    /// <see cref="SampleExtensions.Sample{T}(Seq{T}, TimeSpan)"/>.
    /// </remarks>
    Realized<Seq<T>> SampleTime<T>(SampleTimeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SampleSamplerSeq{T, TSample}"/> as the target's own
    /// <c>Sample(sampler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SampleExtensions.Sample{T, TSample}(Seq{T}, Seq{TSample})"/>.
    /// </remarks>
    Realized<Seq<T>> SampleSampler<T, TSample>(SampleSamplerSeq<T, TSample> seq);
}

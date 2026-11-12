// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="OnErrorResumeNextSeq{T}"/> as the target's own <c>OnErrorResumeNext(second)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.OnErrorResumeNext{T}(Seq{T}, Seq{T})"/>.</remarks>
    Realized<Seq<T>> OnErrorResumeNext<T>(OnErrorResumeNextSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="OnErrorResumeNextParamsSeq{T}"/> as the target's own
    /// <c>OnErrorResumeNext(params)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.OnErrorResumeNext{T}(Seq{T}[])"/>.</remarks>
    Realized<Seq<T>> OnErrorResumeNextParams<T>(OnErrorResumeNextParamsSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="OnErrorResumeNextEnumerableSeq{T}"/> as the target's own
    /// <c>OnErrorResumeNext(IEnumerable)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.OnErrorResumeNext{T}(IEnumerable{Seq{T}})"/>.</remarks>
    Realized<Seq<T>> OnErrorResumeNextEnumerable<T>(OnErrorResumeNextEnumerableSeq<T> seq);
}

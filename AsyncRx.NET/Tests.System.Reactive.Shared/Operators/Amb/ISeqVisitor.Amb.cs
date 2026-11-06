// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="AmbSeq{T}"/> as the target's own <c>Amb(second)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Amb{T}(Seq{T}, Seq{T})"/>.</remarks>
    Realized<Seq<T>> Amb<T>(AmbSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="AmbParamsSeq{T}"/> as the target's own
    /// <c>Amb(params)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Amb{T}(Seq{T}[])"/>.</remarks>
    Realized<Seq<T>> AmbParams<T>(AmbParamsSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="AmbEnumerableSeq{T}"/> as the target's own
    /// <c>Amb(IEnumerable)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Amb{T}(IEnumerable{Seq{T}})"/>.</remarks>
    Realized<Seq<T>> AmbEnumerable<T>(AmbEnumerableSeq<T> seq);
}

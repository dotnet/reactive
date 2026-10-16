// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="CatchSeq{T}"/> as the target's own <c>Catch(second)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Catch{T}(Seq{T}, Seq{T})"/>.</remarks>
    Realized<Seq<T>> Catch<T>(CatchSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="CatchParamsSeq{T}"/> as the target's own <c>Catch(params)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Catch{T}(Seq{T}[])"/>.</remarks>
    Realized<Seq<T>> CatchParams<T>(CatchParamsSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="CatchEnumerableSeq{T}"/> as the target's own
    /// <c>Catch(IEnumerable)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Catch{T}(IEnumerable{Seq{T}})"/>.</remarks>
    Realized<Seq<T>> CatchEnumerable<T>(CatchEnumerableSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="CatchHandlerSeq{T, TException}"/> as the target's own
    /// <c>Catch(handler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.Catch{T, TException}(Seq{T}, Func{TException, Seq{T}}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> CatchHandler<T, TException>(CatchHandlerSeq<T, TException> seq)
        where TException : Exception;
}

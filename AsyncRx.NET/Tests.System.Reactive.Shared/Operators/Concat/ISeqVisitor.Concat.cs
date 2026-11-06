// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="ConcatSeq{T}"/> as the target's own <c>Concat(second)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Concat{T}(Seq{T}, Seq{T})"/>.</remarks>
    Realized<Seq<T>> Concat<T>(ConcatSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ConcatNestedSeq{T}"/> as the target's own <c>Concat()</c> over a
    /// nested sequence.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Concat{T}(Seq{Seq{T}})"/>.</remarks>
    Realized<Seq<T>> ConcatNested<T>(ConcatNestedSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ConcatParamsSeq{T}"/> as the target's own <c>Concat(params)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Concat{T}(Seq{T}[])"/>.</remarks>
    Realized<Seq<T>> ConcatParams<T>(ConcatParamsSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ConcatEnumerableSeq{T}"/> as the target's own
    /// <c>Concat(IEnumerable)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Concat{T}(IEnumerable{Seq{T}})"/>.</remarks>
    Realized<Seq<T>> ConcatEnumerable<T>(ConcatEnumerableSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ConcatTasksSeq{T}"/> as the target's own <c>Concat()</c> over
    /// a sequence of tasks.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Concat{T}(Seq{Task{T}})"/>.</remarks>
    Realized<Seq<T>> ConcatTasks<T>(ConcatTasksSeq<T> seq);
}

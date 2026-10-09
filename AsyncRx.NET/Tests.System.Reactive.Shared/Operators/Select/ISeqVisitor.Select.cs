// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="SelectSeq{TIn, TOut}"/> as the target's own
    /// <c>Select(selector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SelectExtensions.Select{TIn, TOut}(Seq{TIn}, Func{TIn, TOut}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> Select<TIn, TOut>(SelectSeq<TIn, TOut> seq);

    /// <summary>
    /// Materializes a <see cref="SelectIndexedSeq{TIn, TOut}"/> as the target's own
    /// <c>Select((x, i) =&gt; ...)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SelectExtensions.Select{TIn, TOut}(Seq{TIn}, Func{TIn, int, TOut}, string)"/>.
    /// </remarks>
    Realized<Seq<TOut>> SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq);
}

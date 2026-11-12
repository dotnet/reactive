// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="DefaultIfEmptySeq{T}"/> as the target's own
    /// <c>DefaultIfEmpty()</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="DefaultIfEmptyExtensions.DefaultIfEmpty{T}(Seq{T})"/>.
    /// </remarks>
    Realized<Seq<T>> DefaultIfEmpty<T>(DefaultIfEmptySeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DefaultIfEmptyValueSeq{T}"/> as the target's own
    /// <c>DefaultIfEmpty(defaultValue)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="DefaultIfEmptyExtensions.DefaultIfEmpty{T}(Seq{T}, T)"/>.
    /// </remarks>
    Realized<Seq<T>> DefaultIfEmptyValue<T>(DefaultIfEmptyValueSeq<T> seq);
}

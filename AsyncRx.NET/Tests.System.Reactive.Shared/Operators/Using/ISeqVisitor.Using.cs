// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="UsingSeq{T, TResource}"/> as the target's own
    /// <c>Using(resourceFactory, observableFactory)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.Using{T, TResource}(Func{TResource}, Func{TResource, Seq{T}}, string, string)"/>.
    /// </remarks>
    Realized<Seq<T>> Using<T, TResource>(UsingSeq<T, TResource> seq)
        where TResource : IDisposable;
}

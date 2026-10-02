// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A description of any shape.</summary>
/// <remarks>
/// What every <see cref="Seq{T}"/> is, seen without its element type: something a target can
/// materialize. The <see cref="DescriptionBridge"/> works at this level, because it learns a
/// description's shape at run time.
/// </remarks>
public interface ISeq
{
    /// <summary>Materializes this description on <paramref name="visitor"/>, a target.</summary>
    /// <param name="visitor">The target materializing the query.</param>
    Realized Accept(ISeqVisitor visitor);
}

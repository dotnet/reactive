// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A description of an observable sequence, seen without its element type.</summary>
/// <remarks>
/// What every <see cref="Seq{T}"/> is. Materializing a description needs only
/// <see cref="IDescription"/>; the <see cref="DescriptionBridge"/> asks for this where it matters
/// that the description stands for the target's observable: when it maps a description type to
/// the real type an <c>*Impl</c> is instantiated at, and when it wraps a real sequence handed to a
/// callback as a <see cref="NativeSeq{T}"/>. A pattern or a plan is neither.
/// </remarks>
public interface ISeq : IDescription
{
}

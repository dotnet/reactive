// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The target's own observable, as a leaf of a description: a testable source the test
/// created (see <see cref="TestableSeq{T}"/>), or an inner window or group handed to a
/// callback. Materializing it is the identity.
/// </summary>
/// <remarks>
/// Throughout the shared library, <c>Native</c> means "the target's own object that this
/// target-neutral object stands for", typed as <see cref="object"/> because the shared code does
/// not know the target's types;
/// only the target casts it. Here it is the target's observable: an <c>IObservable&lt;T&gt;</c>
/// on Rx.NET, an <c>IAsyncObservable&lt;T&gt;</c> on AsyncRx.NET.
/// </remarks>
public class NativeSeq<T>(object native, string description) : Seq<T>
{
    /// <summary>The target's own observable (see the remarks on this type).</summary>
    public object Native => native;

    public override object Accept(ISeqVisitor visitor) => visitor.Native(this);

    public override string ToString() => description;
}

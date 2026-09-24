// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The platform's own observable, as a leaf of a description: a testable source the test
/// created (see <see cref="TestableSeq{T}"/>), or an inner window or group handed to a
/// callback. Materializing it is the identity.
/// </summary>
public class NativeSeq<T>(object native, string description) : Seq<T>
{
    public object Native => native;

    public override object Accept(ISeqVisitor visitor) => visitor.Native(this);

    public override string ToString() => description;
}

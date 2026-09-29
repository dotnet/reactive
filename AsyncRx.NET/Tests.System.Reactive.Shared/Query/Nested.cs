// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// A description of an observable sequence of observable sequences of <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the inner sequences.</typeparam>
/// <remarks>
/// What <c>Window</c> and <c>GroupJoin</c> produce. The higher-kinded gap is confined to this one
/// type, and the slight misalignment that results from this shows up in the asymmetry between
/// the <c>ISeq&lt;ISeq&lt;T&gt;&gt;</c>, and the return type of <see cref="Accept(ISeqVisitor)"/>.
/// The use of <see cref="ISeq{T}"/> as the type argument for the <see cref="RSeq{T}"/> that
/// <see cref="Accept(ISeqVisitor)"/> returns is somewhat arbitrary, in that it isn't really used
/// for anything. We could define a marker type to denote a nested sequence, but using
/// <see cref="ISeq{T}"/> seems more logical.
/// </remarks>
public abstract class Nested<T> : ISeq<ISeq<T>>
{
    /// <inheritdoc/>
    public abstract RSeq<ISeq<T>> Accept(ISeqVisitor visitor);

    /// <summary>
    /// The description as the query was written in the scenario, for diagnostics.
    /// </summary>
    public abstract override string ToString();

    protected class RSeqResult : RSeq<ISeq<T>>
    {
        private readonly RSeq<RSeq<T>> _result;
        public RSeqResult(RSeq<RSeq<T>> result) => _result = result;

        public override TS Get<TS>() => _result.Get<TS>();
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A group handed to a callback, carrying its key.</summary>
/// <remarks>
/// A leaf that holds the target's own grouped observable. What <c>GroupBy</c> produces is a
/// <c>Seq&lt;Group&lt;TKey, T&gt;&gt;</c>. The bridge maps a <see cref="Group{TKey, T}"/> to the
/// target's grouped observable type, and wraps each real group in one of these when it reaches a
/// scenario's selector.
/// </remarks>
public sealed class Group<TKey, T>(Realized<Seq<T>> native, TKey key, string description)
    : NativeSeq<T>(native, description)
{
    /// <summary>The group's key.</summary>
    public TKey Key => key;
}

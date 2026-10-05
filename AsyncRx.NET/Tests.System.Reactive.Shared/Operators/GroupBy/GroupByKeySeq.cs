// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.GroupBy(keySelector)</c>.</summary>
/// <remarks>
/// Built by <see cref="GroupByExtensions.GroupBy{T, TKey}(Seq{T}, Func{T, TKey}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.GroupByKey{T, TKey}(GroupByKeySeq{T, TKey})"/>. The result is a sequence
/// of <see cref="Group{TKey, T}"/> descriptions, each with its key.
/// </remarks>
public sealed class GroupByKeySeq<T, TKey>(
    Seq<T> source,
    Func<T,
    TKey> keySelector,
    string text) : Seq<Group<TKey, T>>
{
    /// <summary>An observable sequence whose elements to group.</summary>
    public Seq<T> Source => source;

    /// <summary>A function to extract the key for each element.</summary>
    public Func<T, TKey> KeySelector => keySelector;

    /// <inheritdoc/>
    protected override Realized<Seq<Group<TKey, T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.GroupByKey(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.GroupBy({text})";
}

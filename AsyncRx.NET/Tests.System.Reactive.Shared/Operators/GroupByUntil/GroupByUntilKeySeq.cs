// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.GroupByUntil(keySelector, durationSelector)</c>.</summary>
/// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
/// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
/// <typeparam name="TDuration">The type of the elements in the duration sequences.</typeparam>
/// <remarks>
/// Built by the <c>GroupByUntil</c> overload of <see cref="GroupByUntilExtensions"/> with these
/// parameters; materialized by each target through
/// <see cref="ISeqVisitor.GroupByUntilKey{T, TKey, TDuration}(GroupByUntilKeySeq{T, TKey, TDuration})"/>, with the duration selector adapted to take
/// the target's own group and return its own sequence.
/// </remarks>
public sealed class GroupByUntilKeySeq<T, TKey, TDuration>(
    Seq<T> source,
    Func<T, TKey> keySelector,
    Func<Group<TKey, T>, Seq<TDuration>> durationSelector,
    string text)
    : Seq<Group<TKey, T>>
{
    /// <summary>An observable sequence whose elements to group.</summary>
    public Seq<T> Source => source;

    /// <summary>A function to extract the key for each element.</summary>
    public Func<T, TKey> KeySelector => keySelector;

    /// <summary>A function to signal the expiration of a group.</summary>
    public Func<Group<TKey, T>, Seq<TDuration>> DurationSelector => durationSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<Group<TKey, T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.GroupByUntilKey(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.GroupByUntil({text})";
}

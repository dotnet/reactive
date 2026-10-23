// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// What a description does: materialize on a target, once, and print as the scenario wrote it.
/// </summary>
/// <typeparam name="TDescription">
/// The description's own type, which its realization is held under.
/// </typeparam>
/// <remarks>
/// The base of <see cref="Seq{T}"/>, of the patterns (<c>SeqPattern2</c> and up) and of the
/// plans (<see cref="SeqPlan{TResult}"/>). Nothing here runs: a description is a tree a target
/// walks.
/// </remarks>
public abstract class Description<TDescription> : IDescription
    where TDescription : Description<TDescription>
{
    private ISeqVisitor? _visitor;
    private Realized<TDescription>? _realized;

    /// <summary>Materializes this description on a target, once.</summary>
    /// <param name="visitor">The target materializing the query.</param>
    /// <remarks>
    /// The first call dispatches to the visitor member for this node type; later calls return
    /// the same realization, so a description names one real object however many times it is
    /// used. Operators with construction-time state need that (<c>Publish</c> makes its subject
    /// when it is built, and a scenario that connects a published sequence at one tick and
    /// subscribes to it at another must reach the same subject; <c>When</c> keys its
    /// subscriptions by source, so a source joined with itself must be one object); the rest do
    /// not notice. A description belongs to one target: a second target asking is an error. The
    /// result is the target's own object, held under the description it realizes; only the
    /// target's own code looks inside.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// The description was first materialized by a different target.
    /// </exception>
    public Realized<TDescription> Accept(ISeqVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        if (_realized is null)
        {
            _visitor = visitor;
            _realized = AcceptCore(visitor);
        }
        else if (!ReferenceEquals(_visitor, visitor))
        {
            throw new InvalidOperationException(
                $"{this} was materialized by {_visitor}; a description belongs to one target.");
        }

        return _realized;
    }

    /// <summary>Dispatches to the visitor member for this node type.</summary>
    /// <param name="visitor">The target materializing the query.</param>
    protected abstract Realized<TDescription> AcceptCore(ISeqVisitor visitor);

    Realized IDescription.Accept(ISeqVisitor visitor) => Accept(visitor);

    /// <summary>
    /// The description as the query was written in the scenario (for example
    /// <c>Hot(3 messages).Take(2)</c>), for diagnostics.
    /// </summary>
    public abstract override string ToString();
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Catch(handler)</c> for exceptions of one type.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.Catch{T, TException}(Seq{T}, Func{TException, Seq{T}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.CatchHandler{T, TException}(CatchHandlerSeq{T, TException})"/>. The
/// handler returns a description, which the target materializes at the moment it invokes the
/// handler, so a handler that throws, throws inside the target's own call, as in the original
/// test.
/// </remarks>
public sealed class CatchHandlerSeq<T, TException>(
    Seq<T> source,
    Func<TException, Seq<T>> handler,
    string text)
    : Seq<T>
    where TException : Exception
{
    /// <summary>Source sequence.</summary>
    public Seq<T> Source => source;

    /// <summary>
    /// Exception handler function, producing another observable sequence.
    /// </summary>
    public Func<TException, Seq<T>> Handler => handler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CatchHandler(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Catch({text})";
}

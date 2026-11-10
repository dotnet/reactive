// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="FromEventConversionSeq{TDelegate, TEventArgs}"/> as the target's own
    /// <c>FromEvent(conversion, addHandler, removeHandler[, scheduler])</c> with a conversion from
    /// the handler to the event's delegate type.
    /// </summary>
    /// <remarks>Built by the <c>Seq.FromEvent</c> overloads in <c>Seq.FromEvent.cs</c>.</remarks>
    Realized<Seq<TEventArgs>> FromEventConversion<TDelegate, TEventArgs>(FromEventConversionSeq<
            TDelegate, TEventArgs> seq);

    /// <summary>
    /// Materializes a <see cref="FromEventDelegateSeq{TDelegate, TEventArgs}"/> as the target's own
    /// <c>FromEvent(addHandler, removeHandler[, scheduler])</c> over an event of any delegate type
    /// with one argument.
    /// </summary>
    /// <remarks>Built by the <c>Seq.FromEvent</c> overloads in <c>Seq.FromEvent.cs</c>.</remarks>
    Realized<Seq<TEventArgs>> FromEventDelegate<TDelegate, TEventArgs>(FromEventDelegateSeq<
            TDelegate, TEventArgs> seq);

    /// <summary>
    /// Materializes a <see cref="FromEventSeq{TEventArgs}"/> as the target's own
    /// <c>FromEvent(addHandler, removeHandler[, scheduler])</c> over an
    /// <c>Action&lt;TEventArgs&gt;</c> event.
    /// </summary>
    /// <remarks>Built by the <c>Seq.FromEvent</c> overloads in <c>Seq.FromEvent.cs</c>.</remarks>
    Realized<Seq<TEventArgs>> FromEvent<TEventArgs>(FromEventSeq<TEventArgs> seq);

    /// <summary>
    /// Materializes a <see cref="FromEventUnitSeq"/> as the target's own
    /// <c>FromEvent(addHandler, removeHandler[, scheduler])</c> over an <c>Action</c> event,
    /// producing <c>Unit</c>.
    /// </summary>
    /// <remarks>Built by the <c>Seq.FromEvent</c> overloads in <c>Seq.FromEvent.cs</c>.</remarks>
    Realized<Seq<Unit>> FromEventUnit(FromEventUnitSeq seq);
}

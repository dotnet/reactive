// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="FromEventPatternHandlerSeq"/> as the target's own
    /// <c>FromEventPattern(addHandler, removeHandler[, scheduler])</c> over an <c>EventHandler</c>
    /// event.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>.
    /// </remarks>
    Realized<Seq<EventPattern<object>>> FromEventPatternHandler(FromEventPatternHandlerSeq seq);

    /// <summary>
    /// Materializes a <see cref="FromEventPatternDelegateSeq{TDelegate, TEventArgs}"/> as the target's own
    /// <c>FromEventPattern(addHandler, removeHandler[, scheduler])</c> over an event of any
    /// delegate type following the event pattern.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>.
    /// </remarks>
    Realized<Seq<EventPattern<TEventArgs>>> FromEventPatternDelegate<TDelegate, TEventArgs>(
            FromEventPatternDelegateSeq<TDelegate, TEventArgs> seq);

    /// <summary>
    /// Materializes a <see cref="FromEventPatternConversionSeq{TDelegate, TEventArgs}"/> as the target's own
    /// <c>FromEventPattern(conversion, addHandler, removeHandler[, scheduler])</c> with a
    /// conversion from the handler to the event's delegate type.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>.
    /// </remarks>
    Realized<Seq<EventPattern<TEventArgs>>> FromEventPatternConversion<TDelegate, TEventArgs>(
            FromEventPatternConversionSeq<TDelegate, TEventArgs> seq);

    /// <summary>
    /// Materializes a <see cref="FromEventPatternSenderSeq{TDelegate, TSender, TEventArgs}"/> as the target's own
    /// <c>FromEventPattern(addHandler, removeHandler[, scheduler])</c> over an event of any
    /// delegate type, keeping the sender's type.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>.
    /// </remarks>
    Realized<Seq<EventPattern<TSender, TEventArgs>>> FromEventPatternSender<TDelegate, TSender,
            TEventArgs>(FromEventPatternSenderSeq<TDelegate, TSender, TEventArgs> seq);

    /// <summary>
    /// Materializes a <see cref="FromEventPatternSeq{TEventArgs}"/> as the target's own
    /// <c>FromEventPattern(addHandler, removeHandler[, scheduler])</c> over an
    /// <c>EventHandler&lt;TEventArgs&gt;</c> event.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>.
    /// </remarks>
    Realized<Seq<EventPattern<TEventArgs>>> FromEventPattern<TEventArgs>(FromEventPatternSeq<
            TEventArgs> seq);

    /// <summary>
    /// Materializes a <see cref="FromEventPatternInstanceObjectSeq"/> as the target's own
    /// <c>FromEventPattern(target, eventName[, scheduler])</c> over an instance event found by
    /// name.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>.
    /// </remarks>
    Realized<Seq<EventPattern<object>>> FromEventPatternInstanceObject(
            FromEventPatternInstanceObjectSeq seq);

    /// <summary>
    /// Materializes a <see cref="FromEventPatternInstanceSeq{TEventArgs}"/> as the target's own
    /// <c>FromEventPattern(target, eventName[, scheduler])</c> over an instance event found by
    /// name, with typed event arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>.
    /// </remarks>
    Realized<Seq<EventPattern<TEventArgs>>> FromEventPatternInstance<TEventArgs>(
            FromEventPatternInstanceSeq<TEventArgs> seq);

    /// <summary>
    /// Materializes a <see cref="FromEventPatternInstanceSenderSeq{TSender, TEventArgs}"/> as the target's own
    /// <c>FromEventPattern(target, eventName[, scheduler])</c> over an instance event found by
    /// name, with typed sender and event arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>.
    /// </remarks>
    Realized<Seq<EventPattern<TSender, TEventArgs>>> FromEventPatternInstanceSender<TSender,
            TEventArgs>(FromEventPatternInstanceSenderSeq<TSender, TEventArgs> seq);

    /// <summary>
    /// Materializes a <see cref="FromEventPatternStaticObjectSeq"/> as the target's own
    /// <c>FromEventPattern(type, eventName[, scheduler])</c> over a static event found by name.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>.
    /// </remarks>
    Realized<Seq<EventPattern<object>>> FromEventPatternStaticObject(
            FromEventPatternStaticObjectSeq seq);

    /// <summary>
    /// Materializes a <see cref="FromEventPatternStaticSeq{TEventArgs}"/> as the target's own
    /// <c>FromEventPattern(type, eventName[, scheduler])</c> over a static event found by name,
    /// with typed event arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>.
    /// </remarks>
    Realized<Seq<EventPattern<TEventArgs>>> FromEventPatternStatic<TEventArgs>(
            FromEventPatternStaticSeq<TEventArgs> seq);

    /// <summary>
    /// Materializes a <see cref="FromEventPatternStaticSenderSeq{TSender, TEventArgs}"/> as the target's own
    /// <c>FromEventPattern(type, eventName[, scheduler])</c> over a static event found by name,
    /// with typed sender and event arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>.
    /// </remarks>
    Realized<Seq<EventPattern<TSender, TEventArgs>>> FromEventPatternStaticSender<TSender,
            TEventArgs>(FromEventPatternStaticSenderSeq<TSender, TEventArgs> seq);
}

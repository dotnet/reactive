// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A target's own object, held as the realization of a description.</summary>
/// <remarks>
/// <para>
/// What a target returns from its <see cref="ISeqVisitor"/> members, and what every other
/// target-made object in the harness is held as: a test scheduler, a scheduler double, a
/// testable observer. The shared library never names a target's types, so the object is held as
/// <see cref="object"/> and handed back through <see cref="Get{TS}"/>, which checks the shape
/// and names both types when it is wrong. The typed form, <see cref="Realized{TDescription}"/>,
/// says which description the object realizes, and is the type a factory on
/// <see cref="IRxTarget"/> returns; <see cref="object"/> appears nowhere on that interface.
/// </para>
/// <para>
/// Every description type maps to one real type on a given target, at any depth of nesting:
/// <c>Seq&lt;int&gt;</c> to <c>IObservable&lt;int&gt;</c> on Rx.NET,
/// <c>Seq&lt;Seq&lt;int&gt;&gt;</c> to <c>IObservable&lt;IObservable&lt;int&gt;&gt;</c>, and so on.
/// The <see cref="DescriptionBridge"/> owns that mapping.
/// </para>
/// </remarks>
public abstract class Realized
{
    /// <summary>The target's own object.</summary>
    public abstract object Raw { get; }

    /// <summary>The description the object realizes.</summary>
    public abstract Type Description { get; }

    /// <summary>Gets the target's own object as <paramref name="realType"/>.</summary>
    /// <param name="realType">A type the object is expected to be assignable to.</param>
    /// <exception cref="InvalidOperationException">
    /// The object is not a <paramref name="realType"/>.
    /// </exception>
    public object GetAs(Type realType)
    {
        if (!realType.IsInstanceOfType(Raw))
        {
            throw new InvalidOperationException(
                $"The realization of {Description} is a {Raw.GetType()}, "
                + $"which is not a {realType}.");
        }

        return Raw;
    }

    /// <summary>Gets the target's own object.</summary>
    /// <typeparam name="TS">The type the object is expected to be.</typeparam>
    /// <exception cref="InvalidOperationException">
    /// The object is not a <typeparamref name="TS"/>.
    /// </exception>
    public TS Get<TS>() => (TS)GetAs(typeof(TS));

    /// <summary>Holds a target's own object as the realization of a description.</summary>
    /// <typeparam name="TDescription">The description the object realizes.</typeparam>
    /// <param name="real">The target's own object.</param>
    public static Realized<TDescription> Of<TDescription>(object real) =>
        new Holder<TDescription>(real);

    /// <summary>Holds a target's own object, for a description type known at run time.</summary>
    /// <param name="descriptionType">The description the object realizes.</param>
    /// <param name="real">The target's own object.</param>
    /// <remarks>As <see cref="Of{TDescription}(object)"/>, for the bridge's use.</remarks>
    public static Realized Of(Type descriptionType, object real) =>
        (Realized)Activator.CreateInstance(
            typeof(Holder<>).MakeGenericType(descriptionType), real)!;

    private sealed class Holder<TDescription>(object real) : Realized<TDescription>
    {
        public override object Raw => real;

        public override Type Description => typeof(TDescription);
    }
}

/// <summary>The realization of a <typeparamref name="TDescription"/>.</summary>
/// <typeparam name="TDescription">
/// The description this object realizes: <c>Seq&lt;int&gt;</c>, <c>Seq&lt;Seq&lt;int&gt;&gt;</c>,
/// <c>Seq&lt;Group&lt;string, int&gt;&gt;</c>, and so on; or a harness handle such as
/// <see cref="TestSchedulerRef"/> or <see cref="SchedulerDouble"/>, for what the target made
/// behind it.
/// </typeparam>
/// <remarks>
/// This is the type an <see cref="ISeqVisitor"/> member or an <see cref="IRxTarget"/> factory
/// returns, and it lets the interface state the shape of what each member realizes without
/// naming any target's types.
/// </remarks>
public abstract class Realized<TDescription> : Realized
{
}

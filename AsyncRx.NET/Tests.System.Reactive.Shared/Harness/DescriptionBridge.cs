// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;
using System.Linq.Expressions;
using System.Reactive;
using System.Reflection;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The reflection that bridges the describe and build worlds for one target.
/// </summary>
/// <remarks>
/// <para>
/// A target writes each operator once, as an ordinary generic method over its own real types
/// (<c>IObservable&lt;T&gt;</c>, <c>Func&lt;T, bool&gt;</c>, ...). A visitor member hands the
/// bridge that method, instantiated at the description's type arguments, together with the
/// node's descriptions and delegates. The bridge rewrites every description type to the target's
/// real type (<c>Seq&lt;Seq&lt;int&gt;&gt;</c> to <c>IObservable&lt;IObservable&lt;int&gt;&gt;</c>,
/// at any depth), re-instantiates the method at the real types, converts each argument
/// (materializing descriptions and adapting delegates), invokes it, and returns the result under
/// the description type the member promised.
/// </para>
/// <para>
/// This is the only place where types are rewritten, so it is the only place where a mismatch
/// surfaces at run time rather than compile time. Every failure message names the description
/// type, the real type and the method involved.
/// </para>
/// </remarks>
public sealed class DescriptionBridge(ISeqVisitor target, DescriptionBridge.TargetTypes types)
{
    /// <summary>The type constructors the bridge uses to map descriptions to real types.</summary>
    /// <param name="ObservableOf">
    /// Builds the target's observable type for an element type. On Rx.NET,
    /// <c>ObservableOf(typeof(int))</c> returns <c>typeof(IObservable&lt;int&gt;)</c>; on
    /// AsyncRx.NET, <c>typeof(IAsyncObservable&lt;int&gt;)</c>. The bridge calls it for
    /// <c>Seq&lt;int&gt;</c>, and again for <c>Seq&lt;Seq&lt;int&gt;&gt;</c> with the first result as the
    /// element type.
    /// </param>
    /// <param name="GroupedOf">
    /// Builds the target's grouped-observable type for a key type and an element type. On Rx.NET,
    /// <c>GroupedOf(typeof(string), typeof(int))</c> returns
    /// <c>typeof(IGroupedObservable&lt;string, int&gt;)</c>; on AsyncRx.NET,
    /// <c>typeof(IGroupedAsyncObservable&lt;string, int&gt;)</c>. The bridge calls it for
    /// <c>Group&lt;string, int&gt;</c>.
    /// </param>
    /// <param name="KeyOf">
    /// Reads the key of one of the target's grouped observables, so the bridge can set
    /// <see cref="Group{TKey, T}.Key"/> when it hands a real group to a scenario's callback.
    /// </param>
    /// <param name="SubjectOf">
    /// Builds the target's subject type for an element type: on Rx.NET,
    /// <c>SubjectOf(typeof(int))</c> returns <c>typeof(ISubject&lt;int&gt;)</c>; on AsyncRx.NET,
    /// <c>typeof(IAsyncSubject&lt;int&gt;)</c>. The bridge calls it for a
    /// <c>SubjectSeq&lt;int&gt;</c> parameter, the input of <c>Multicast</c>, and for the result
    /// of a subject factory.
    /// </param>
    /// <param name="ConnectableOf">
    /// Builds the target's connectable type for an element type: on Rx.NET,
    /// <c>ConnectableOf(typeof(int))</c> returns <c>typeof(IConnectableObservable&lt;int&gt;)</c>;
    /// on AsyncRx.NET, <c>typeof(IConnectableAsyncObservable&lt;int&gt;)</c>. The bridge calls it
    /// for a <c>ConnectableSeq&lt;int&gt;</c> parameter, the input of <c>RefCount</c>.
    /// </param>
    /// <param name="ObserverOf">
    /// Builds the target's observer type for an element type: on Rx.NET,
    /// <c>ObserverOf(typeof(int))</c> returns <c>typeof(IObserver&lt;int&gt;)</c>; on
    /// AsyncRx.NET, <c>typeof(IAsyncObserver&lt;int&gt;)</c>. The bridge calls it for
    /// <c>ObserverRef&lt;int&gt;</c>, the observer a <c>Create</c> callback drives.
    /// </param>
    /// <param name="WrapObserver">
    /// Wraps one of the target's observers as the <see cref="ObserverRef{T}"/> a scenario's
    /// callback drives, so the bridge can hand a real observer to the callback.
    /// </param>
    public sealed record TargetTypes(
        Func<Type, Type> ObservableOf,
        Func<Type, Type, Type> GroupedOf,
        Func<object, object?> KeyOf,
        Func<Type, Type> ObserverOf,
        Func<object, object> WrapObserver,
        Func<Type, Type> ConnectableOf,
        Func<Type, Type> SubjectOf);

    /// <summary>The target's real type for a description type, at any depth of nesting.</summary>
    /// <param name="descriptionType">
    /// A type as the query model states it, which may be or contain descriptions.
    /// </param>
    /// <remarks>
    /// <para>
    /// A target writes <c>WindowCountImpl&lt;T&gt;(IObservable&lt;T&gt; source, int count, int skip)</c>
    /// once, and the visitor member for <c>WindowCountSeq&lt;T&gt;</c> passes it as
    /// <c>WindowCountImpl&lt;T&gt;</c> at whatever <c>T</c> the node has. For a flat scenario that
    /// is <c>int</c>, and nothing needs rewriting. For <c>xs.Window(2, 2).Window(1, 1)</c> the
    /// outer node is a <c>WindowCountSeq&lt;Seq&lt;int&gt;&gt;</c>, so the member passes
    /// <c>WindowCountImpl&lt;Seq&lt;int&gt;&gt;</c>, whose <c>source</c> parameter is an
    /// <c>IObservable&lt;Seq&lt;int&gt;&gt;</c>. No such object exists. The real source is an
    /// <c>IObservable&lt;IObservable&lt;int&gt;&gt;</c>, so the bridge calls this method on each of
    /// the method's type arguments, gets <c>IObservable&lt;int&gt;</c> for <c>Seq&lt;int&gt;</c>,
    /// and re-instantiates the method as <c>WindowCountImpl&lt;IObservable&lt;int&gt;&gt;</c> before
    /// invoking it.
    /// </para>
    /// <para>
    /// The rewriting is structural, so it applies at any depth and inside any other type: on
    /// Rx.NET, <c>Seq&lt;int&gt;</c> becomes <c>IObservable&lt;int&gt;</c>,
    /// <c>Seq&lt;Seq&lt;int&gt;&gt;</c> becomes <c>IObservable&lt;IObservable&lt;int&gt;&gt;</c>,
    /// <c>Group&lt;string, int&gt;</c> becomes <c>IGroupedObservable&lt;string, int&gt;</c>,
    /// <c>Func&lt;int, Seq&lt;long&gt;&gt;</c> becomes <c>Func&lt;int, IObservable&lt;long&gt;&gt;</c>, and
    /// <c>Recorded&lt;Notification&lt;Seq&lt;int&gt;&gt;&gt;[]</c> becomes
    /// <c>Recorded&lt;Notification&lt;IObservable&lt;int&gt;&gt;&gt;[]</c>. A type that contains no
    /// description (<c>int</c>, <c>TimeSpan</c>, <c>Func&lt;int, bool&gt;</c>) comes back unchanged.
    /// </para>
    /// </remarks>
    private Type Real(Type descriptionType)
    {
        if (descriptionType.IsArray)
        {
            return Real(descriptionType.GetElementType()!).MakeArrayType();
        }

        if (!descriptionType.IsGenericType)
        {
            return descriptionType;
        }

        var definition = descriptionType.GetGenericTypeDefinition();
        var args = descriptionType.GetGenericArguments();
        if (definition == typeof(Group<,>))
        {
            return types.GroupedOf(args[0], Real(args[1]));
        }

        if (definition == typeof(ObserverRef<>))
        {
            return types.ObserverOf(Real(args[0]));
        }

        if (definition == typeof(ConnectableSeq<>))
        {
            return types.ConnectableOf(Real(args[0]));
        }

        if (definition == typeof(SubjectSeq<>))
        {
            return types.SubjectOf(Real(args[0]));
        }

        if (typeof(ISeq).IsAssignableFrom(descriptionType))
        {
            // Seq<X>, NativeSeq<X>, TestableSeq<X>, ... all stand for an observable of X.
            return types.ObservableOf(Real(SeqElementType(descriptionType)));
        }

        var realArgs = args.Select(Real).ToArray();
        return realArgs.SequenceEqual(args)
            ? descriptionType
            : definition.MakeGenericType(realArgs);
    }

    /// <summary>Runs one of the target's methods and wraps the sequence it returns.</summary>
    /// <typeparam name="TDescription">The description the result realizes.</typeparam>
    /// <param name="impl">
    /// A generic method the target wrote over its real types, instantiated at the description's
    /// types (for example <c>TakeImpl&lt;Seq&lt;int&gt;&gt;</c>).
    /// </param>
    /// <param name="args">
    /// The node's descriptions, delegates and values, in parameter order.
    /// </param>
    /// <remarks>
    /// <para>
    /// The bridge re-instantiates <paramref name="impl"/> at the real types, converts each
    /// argument with <see cref="ToReal"/>, invokes it, and wraps the result as a
    /// <see cref="Realized{TDescription}"/>.
    /// </para>
    /// <para>
    /// The wrapper's type argument is the description, not the real element type. A
    /// <c>Realized&lt;Seq&lt;int&gt;&gt;</c> holds the target's observable of <c>int</c>, but a
    /// <c>Realized&lt;Seq&lt;Seq&lt;int&gt;&gt;&gt;</c> holds an observable whose elements are the
    /// target's observables of <c>int</c> (<c>IObservable&lt;IObservable&lt;int&gt;&gt;</c> on
    /// Rx.NET), never an observable of <c>Seq&lt;int&gt;</c>. The real type is always
    /// <see cref="Real"/> of the type argument, and that is what <see cref="ToReal"/> expects when
    /// the object is passed to another <c>*Impl</c>, and what a target must ask for through
    /// <see cref="Realized{TDescription}.Get{TS}"/>, for example when <c>Start</c> subscribes the
    /// observer.
    /// </para>
    /// </remarks>
    [StackTraceHidden]
    public Realized<TDescription> Run<TDescription>(Delegate impl, params object?[] args)
        where TDescription : ISeq => Realized.Of<TDescription>(Call<object>(impl, args));

    /// <summary>Runs one of the target's methods and returns its result.</summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="impl">A generic method the target wrote over its real types.</param>
    /// <param name="args">
    /// The node's descriptions, delegates and values, in parameter order.
    /// </param>
    /// <remarks>As <see cref="Run{TDescription}"/>, for a result that is not a sequence.</remarks>
    [StackTraceHidden]
    public TResult Call<TResult>(Delegate impl, params object?[] args)
    {
        ArgumentNullException.ThrowIfNull(impl);

        var described = impl.Method;
        var real = described.IsGenericMethod
            ? described.GetGenericMethodDefinition().MakeGenericMethod(
                described.GetGenericArguments().Select(Real).ToArray())
            : described;

        var describedParameters = described.GetParameters();
        var realParameters = real.GetParameters();
        if (describedParameters.Length != args.Length)
        {
            throw new InvalidOperationException(
                $"{described.Name} takes {describedParameters.Length} arguments but {args.Length} were supplied.");
        }

        var realArgs = new object?[args.Length];
        for (var i = 0; i < args.Length; i++)
        {
            try
            {
                realArgs[i] = ToReal(args[i], realParameters[i].ParameterType);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(
                    $"Argument {i} ({describedParameters[i].Name}) of {described.Name}: {ex.Message}",
                    ex);
            }
        }

        return (TResult)real.Invoke(
            impl.Target, BindingFlags.DoNotWrapExceptions, null, realArgs, null)!;
    }

    /// <summary>Converts a description-world value to the target's real value.</summary>
    /// <param name="value">The value as the describe world holds it.</param>
    /// <param name="realType">The type the target's method takes.</param>
    /// <remarks>
    /// The value's own type says what it is in the describe world (a description, a realized
    /// sequence, a delegate over descriptions, a message whose value is a description); the real
    /// parameter type says what the target's method needs.
    /// </remarks>
    private object? ToReal(object? value, Type realType)
    {
        if (value is null)
        {
            return null;
        }

        var describedType = value.GetType();
        switch (value)
        {
            case Realized realized:
                return realized.GetAs(realType);

            case ISeq description:
                return description.Accept(target).GetAs(realType);

            case Delegate callback:
                return realType.IsInstanceOfType(callback) ? callback : Adapt(callback, describedType, realType);

            case Array array when realType.IsArray && !realType.IsInstanceOfType(array):
            {
                var elementType = realType.GetElementType()!;
                var result = Array.CreateInstance(elementType, array.Length);
                for (var i = 0; i < array.Length; i++)
                {
                    result.SetValue(ToReal(array.GetValue(i), elementType), i);
                }

                return result;
            }

            default:
                if (realType.IsInstanceOfType(value))
                {
                    return value;
                }

                if (value is global::System.Collections.IEnumerable items
                    && Instantiation(realType, typeof(IEnumerable<>)) is { } enumerable)
                {
                    // An enumerable of descriptions (a LINQ query over Seq<T>s, say), materialized
                    // element by element into a list of the target's own sequences.
                    var elementType = enumerable.GetGenericArguments()[0];
                    var list = (global::System.Collections.IList)Activator.CreateInstance(
                        typeof(List<>).MakeGenericType(elementType))!;
                    foreach (var item in items)
                    {
                        list.Add(ToReal(item, elementType));
                    }

                    return list;
                }

                if (Instantiation(describedType, typeof(Recorded<>)) is { } recorded)
                {
                    describedType = recorded;
                    var time = (long)describedType.GetProperty("Time")!.GetValue(value)!;
                    var inner = describedType.GetProperty("Value")!.GetValue(value);
                    var innerReal = ToReal(inner, realType.GetGenericArguments()[0]);
                    return Activator.CreateInstance(realType, time, innerReal);
                }

                if (Instantiation(describedType, typeof(Notification<>)) is { } notification)
                {
                    describedType = notification;
                    var kind = (NotificationKind)describedType.GetProperty("Kind")!.GetValue(value)!;
                    var realElement = realType.GetGenericArguments()[0];
                    object? Create(string factory, params object?[] arguments) =>
                        typeof(Notification).GetMethod(factory)!
                            .MakeGenericMethod(realElement)
                            .Invoke(null, arguments);
                    return kind switch
                    {
                        NotificationKind.OnNext => Create(
                            nameof(Notification.CreateOnNext),
                            ToReal(describedType.GetProperty("Value")!.GetValue(value), realElement)),
                        NotificationKind.OnError => Create(
                            nameof(Notification.CreateOnError),
                            describedType.GetProperty("Exception")!.GetValue(value)),
                        _ => Create(nameof(Notification.CreateOnCompleted)),
                    };
                }

                throw new InvalidOperationException(
                    $"the bridge does not know how to convert a {describedType} to a {realType}.");
        }
    }

    /// <summary>Wraps a target's real value as the description a callback expects.</summary>
    /// <param name="real">The value the target produced.</param>
    /// <param name="describedType">The type the scenario's callback takes.</param>
    private object? ToDescription(object? real, Type describedType)
    {
        if (real is null || describedType.IsInstanceOfType(real))
        {
            return real;
        }

        if (describedType.IsGenericType
            && describedType.GetGenericTypeDefinition() == typeof(ObserverRef<>))
        {
            return types.WrapObserver(real);
        }

        if (describedType.IsGenericType
            && describedType.GetGenericTypeDefinition() == typeof(Group<,>))
        {
            var args = describedType.GetGenericArguments();
            var native = Realized.Of(typeof(Seq<>).MakeGenericType(args[1]), real);
            return Activator.CreateInstance(describedType, native, types.KeyOf(real), "group");
        }

        if (typeof(ISeq).IsAssignableFrom(describedType))
        {
            var element = SeqElementType(describedType);
            var native = Realized.Of(typeof(Seq<>).MakeGenericType(element), real);
            return Activator.CreateInstance(
                typeof(NativeSeq<>).MakeGenericType(element), native, "window");
        }

        throw new InvalidOperationException(
            $"the bridge does not know how to present a {real.GetType()} as a {describedType}.");
    }

    private static readonly MethodInfo ToRealMethod = typeof(DescriptionBridge)
        .GetMethod(nameof(ToReal), BindingFlags.NonPublic | BindingFlags.Instance)!;

    private static readonly MethodInfo ToDescriptionMethod = typeof(DescriptionBridge)
        .GetMethod(nameof(ToDescription), BindingFlags.NonPublic | BindingFlags.Instance)!;

    // A delegate written over descriptions, re-expressed over the target's real types: each
    // parameter is wrapped on the way in and the result materialized on the way out.
    private Delegate Adapt(Delegate callback, Type describedType, Type realType)
    {
        var describedInvoke = describedType.GetMethod("Invoke")!;
        var realInvoke = realType.GetMethod("Invoke")!;
        var describedParameters = describedInvoke.GetParameters();
        var parameters = realInvoke.GetParameters()
            .Select(p => Expression.Parameter(p.ParameterType, p.Name))
            .ToArray();
        var self = Expression.Constant(this);
        var arguments = parameters.Select((p, i) =>
            p.Type == describedParameters[i].ParameterType
                ? (Expression)p
                : Expression.Convert(
                    Expression.Call(
                        self,
                        ToDescriptionMethod,
                        Expression.Convert(p, typeof(object)),
                        Expression.Constant(describedParameters[i].ParameterType)),
                    describedParameters[i].ParameterType));
        Expression body = Expression.Invoke(Expression.Constant(callback), arguments);
        if (realInvoke.ReturnType != typeof(void)
            && realInvoke.ReturnType != describedInvoke.ReturnType)
        {
            body = Expression.Convert(
                Expression.Call(
                    self,
                    ToRealMethod,
                    Expression.Convert(body, typeof(object)),
                    Expression.Constant(realInvoke.ReturnType)),
                realInvoke.ReturnType);
        }

        return Expression.Lambda(realType, body, parameters).Compile();
    }

    // The closed form of an open generic definition in a type's base chain or interfaces, if any
    // (a Notification<T> value is an instance of a nested subclass, for example).
    private static Type? Instantiation(Type type, Type definition)
    {
        for (var t = type; t is not null; t = t.BaseType)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == definition)
            {
                return t;
            }
        }

        if (definition.IsInterface)
        {
            return type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == definition);
        }

        return null;
    }

    private static Type SeqElementType(Type descriptionType)
    {
        for (var t = descriptionType; t is not null; t = t.BaseType)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Seq<>))
            {
                return t.GetGenericArguments()[0];
            }
        }

        throw new InvalidOperationException($"{descriptionType} is not a Seq<T>.");
    }
}

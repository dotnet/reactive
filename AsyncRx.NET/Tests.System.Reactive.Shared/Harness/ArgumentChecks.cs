// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reflection;

namespace Tests.System.Reactive.Shared;

/// <summary>Checks every overload of an operator for argument validation, by reflection.</summary>
/// <remarks>
/// <para>
/// The shared replacement for Rx.NET's hand-written <c>*_ArgumentChecking</c> tests, derived
/// from the API surface itself. For each public static method of the surface's classes with the
/// operator's name, instantiated at <see cref="int"/> where generic: every non-nullable reference
/// parameter passed as null must raise <see cref="ArgumentNullException"/> naming that parameter;
/// every parameter whose name says it is a count or a duration (<c>count</c>, <c>skip</c>,
/// <c>capacity</c>, <c>bufferSize</c>, <c>maxConcurrent</c>, <c>duration</c>, <c>timeSpan</c>,
/// <c>timeShift</c>, <c>dueTime</c>, <c>period</c>, <c>window</c>, <c>interval</c>,
/// <c>index</c>, <c>repeatCount</c>, <c>retryCount</c>, <c>minObservers</c>) passed as minus one
/// must raise <see cref="ArgumentOutOfRangeException"/> naming it, except where Rx.NET
/// deliberately accepts the value (<c>Timer</c>\'s due time, where negative means now, and
/// <c>AutoConnect</c>\'s minimum, where non-positive means connect at once); and, where calling the
/// overload with plausible arguments returns an observable, subscribing a null observer to that
/// observable must raise <see cref="ArgumentNullException"/>. The other arguments of each
/// probing call come from <see cref="Dummies"/>.
/// </para>
/// <para>
/// The rules are Rx.NET's, and the Rx.NET runner is their oracle: a rule that does not match
/// Rx.NET's validation fails there first. A parameter whose annotation allows null is not
/// probed. An exception is reported by the probe that raised it, with the overload's signature,
/// so one failing test lists every defect of one operator.
/// </para>
/// </remarks>
public static class ArgumentChecks
{
    private static readonly HashSet<string> RangeChecked = new(StringComparer.Ordinal)
    {
        "count", "skip", "capacity", "bufferSize", "maxConcurrent", "duration", "timeSpan",
        "timeShift", "dueTime", "period", "window", "interval", "index", "repeatCount",
        "retryCount", "minObservers",
    };

    private static readonly HashSet<(string Operator, string Parameter)> NotRangeChecked =
    [
        ("Timer", "dueTime"),
        ("AutoConnect", "minObservers"),
    ];

    /// <summary>The overloads of <paramref name="operatorName"/> on the surface.</summary>
    /// <param name="surface">The target's operator surface.</param>
    /// <param name="operatorName">The operator's method name.</param>
    /// <param name="scope">Which of the surface's classes to look at.</param>
    public static IReadOnlyList<MethodInfo> Overloads(
        IApiSurface surface,
        string operatorName,
        SurfaceClass scope = SurfaceClass.Any)
    {
        ArgumentNullException.ThrowIfNull(surface);

        return Classes(surface, scope)
            .SelectMany(c => c.GetMethods(BindingFlags.Public | BindingFlags.Static))
            .Where(m => m.Name == operatorName)
            .ToArray();
    }

    /// <summary>The surface's classes that <paramref name="scope"/> names.</summary>
    /// <param name="surface">The target's operator surface.</param>
    /// <param name="scope">Which of the surface's classes to name.</param>
    public static IReadOnlyList<Type> Classes(IApiSurface surface, SurfaceClass scope)
    {
        ArgumentNullException.ThrowIfNull(surface);

        return scope switch
        {
            SurfaceClass.Operators => [surface.Operators],
            SurfaceClass.Extensions => [surface.Extensions],
            _ => [surface.Operators, surface.Extensions],
        };
    }

    /// <summary>Runs the checks over every overload and describes each defect.</summary>
    /// <param name="surface">The target's operator surface.</param>
    /// <param name="operatorName">The operator's method name.</param>
    /// <param name="scope">Which of the surface's classes to look at.</param>
    public static IReadOnlyList<string> Check(
        IApiSurface surface,
        string operatorName,
        SurfaceClass scope = SurfaceClass.Any)
    {
        ArgumentNullException.ThrowIfNull(surface);

        var problems = new List<string>();
        var nullability = new NullabilityInfoContext();
        foreach (var overload in Overloads(surface, operatorName, scope))
        {
            var method = overload;
            if (method.IsGenericMethodDefinition)
            {
                try
                {
                    method = method.MakeGenericMethod(method.GetGenericArguments().Select(TypeArgumentFor).ToArray());
                }
                catch (ArgumentException ex)
                {
                    problems.Add($"{Describe(overload)}: could not instantiate the type parameters ({ex.Message}).");
                    continue;
                }
            }

            var parameters = method.GetParameters();
            var valid = new object?[parameters.Length];
            var buildable = true;
            for (var i = 0; i < parameters.Length; i++)
            {
                if (!Dummies.TryCreate(parameters[i].ParameterType, out valid[i]))
                {
                    problems.Add($"{Describe(method)}: no dummy for parameter '{parameters[i].Name}' of type {parameters[i].ParameterType}.");
                    buildable = false;
                }
            }

            if (!buildable)
            {
                continue;
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var type = parameter.ParameterType;
                if (!type.IsValueType && nullability.Create(parameter).WriteState != NullabilityState.Nullable)
                {
                    Probe(problems, method, valid, i, null, typeof(ArgumentNullException), "null");
                }

                if (RangeChecked.Contains(parameter.Name!)
                    && !NotRangeChecked.Contains((operatorName, parameter.Name!)))
                {
                    object? negative = type == typeof(int) ? -1
                        : type == typeof(long) ? -1L
                        : type == typeof(TimeSpan) ? TimeSpan.FromTicks(-1)
                        : null;
                    if (negative is not null)
                    {
                        Probe(problems, method, valid, i, negative, typeof(ArgumentOutOfRangeException), "negative");
                    }
                }
            }

            ProbeSubscribe(problems, surface, method, valid);
        }

        return problems;
    }

    private static void Probe(List<string> problems, MethodInfo method, object?[] valid, int index, object? bad, Type expected, string what)
    {
        var args = (object?[])valid.Clone();
        args[index] = bad;
        var name = method.GetParameters()[index].Name;
        var thrown = Invoke(method, args);
        if (thrown is null)
        {
            problems.Add($"{Describe(method)}: {what} '{name}' raised nothing; expected {expected.Name}.");
        }
        else if (thrown.GetType() != expected)
        {
            problems.Add($"{Describe(method)}: {what} '{name}' raised {thrown.GetType().Name} ({thrown.Message}); expected {expected.Name}.");
        }
        else if (((ArgumentException)thrown).ParamName != name)
        {
            problems.Add($"{Describe(method)}: {what} '{name}' raised {expected.Name} naming '{((ArgumentException)thrown).ParamName}'; expected '{name}'.");
        }
    }

    private static void ProbeSubscribe(List<string> problems, IApiSurface surface, MethodInfo method, object?[] valid)
    {
        object? result;
        try
        {
            result = method.Invoke(null, valid);
        }
        catch (TargetInvocationException)
        {
            // An eager operator (a blocking one, say) ran into a dummy; that is not what this
            // check is about, and there is no observable to subscribe to.
            return;
        }

        var observable = result is null ? null : Instantiation(result.GetType(), surface.ObservableDefinition);
        if (observable is null)
        {
            return;
        }

        var subscribe = observable.GetMethod(surface.SubscribeMethodName);
        if (subscribe is null)
        {
            problems.Add($"{Describe(method)}: the result's {observable} has no {surface.SubscribeMethodName} method.");
            return;
        }

        var thrown = Invoke(subscribe, [null], result);
        if (thrown is not ArgumentNullException)
        {
            problems.Add($"{Describe(method)}: subscribing a null observer to the result raised {thrown?.GetType().Name ?? "nothing"}; expected ArgumentNullException.");
        }
    }

    // Invokes, and returns the exception the call raised, whether synchronously or as a
    // faulted task (an async method's argument check), or null if it raised none.
    private static Exception? Invoke(MethodInfo method, object?[] args, object? instance = null)
    {
        object? result;
        try
        {
            result = method.Invoke(instance, args);
        }
        catch (TargetInvocationException ex)
        {
            return ex.InnerException;
        }

        var task = result switch
        {
            Task t => t,
            ValueTask vt => vt.AsTask(),
            _ when result is not null && result.GetType() is { IsGenericType: true } rt && rt.GetGenericTypeDefinition() == typeof(ValueTask<>) =>
                (Task)rt.GetMethod("AsTask")!.Invoke(result, null)!,
            _ => null,
        };
        return task is { IsFaulted: true } ? task.Exception!.InnerException : null;
    }

    // A type that satisfies the parameter's constraints: the constraining class or interface
    // itself where there is one, otherwise int (or string under a class constraint).
    private static Type TypeArgumentFor(Type parameter)
    {
        foreach (var constraint in parameter.GetGenericParameterConstraints())
        {
            if (constraint.IsClass || constraint.IsInterface)
            {
                return constraint;
            }
        }

        return (parameter.GenericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) != 0 ? typeof(string) : typeof(int);
    }

    private static Type? Instantiation(Type type, Type definition) =>
        type.GetInterfaces().Prepend(type).FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == definition);

    private static string Describe(MethodInfo method) =>
        $"{method.Name}({string.Join(", ", method.GetParameters().Select(p => $"{Pretty(p.ParameterType)} {p.Name}"))})";

    private static string Pretty(Type type) =>
        type.IsGenericType
            ? $"{type.Name[..type.Name.IndexOf('`')]}<{string.Join(", ", type.GetGenericArguments().Select(Pretty))}>"
            : type.Name;
}

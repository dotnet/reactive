// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Linq.Expressions;
using System.Reflection;

namespace Tests.System.Reactive.Shared;

/// <summary>Builds a plausible argument of any parameter type, for the argument checks.</summary>
/// <remarks>
/// <para>
/// An argument check calls an operator with one argument deliberately wrong and every other
/// argument plausible enough to get past the operator's validation of it. Nothing here is ever
/// run as a sequence: an interface is a proxy whose every member throws
/// <see cref="NotImplementedException"/> (so an operator that subscribed to its source eagerly
/// would fail loudly, as with Rx.NET's own <c>DummyObservable</c>), a delegate returns its
/// result type's default, a number is one (zero is out of range for some parameters), a time
/// span is one tick, a collection is empty, a task is completed, a class is built through its
/// simplest public constructor, and a type parameter is <see cref="int"/>. The one thing a
/// dummy does do is validate: subscribing a null observer to a dummy observable raises
/// <see cref="ArgumentNullException"/>, as a real source would, so that an operator which
/// forwards its subscriptions to its source unchecked still meets the rule through the source.
/// </para>
/// <para>
/// A type this cannot build is reported by the check rather than guessed at.
/// </para>
/// </remarks>
public static class Dummies
{
    /// <summary>Builds a dummy of <paramref name="type"/>, or returns false if it cannot.</summary>
    /// <param name="type">The parameter type.</param>
    /// <param name="dummy">The dummy.</param>
    public static bool TryCreate(Type type, out object? dummy)
    {
        ArgumentNullException.ThrowIfNull(type);

        dummy = null;
        if (type == typeof(string))
        {
            dummy = "";
            return true;
        }

        if (type == typeof(Type))
        {
            dummy = typeof(object);
            return true;
        }

        if (type == typeof(object))
        {
            dummy = new object();
            return true;
        }

        if (type == typeof(TimeSpan))
        {
            dummy = TimeSpan.FromTicks(1);
            return true;
        }

        if (type == typeof(int) || type == typeof(long) || type == typeof(double) || type == typeof(float) || type == typeof(decimal))
        {
            dummy = Convert.ChangeType(1, type, global::System.Globalization.CultureInfo.InvariantCulture);
            return true;
        }

        if (type.IsValueType)
        {
            dummy = Activator.CreateInstance(type);
            return true;
        }

        if (type.IsArray)
        {
            dummy = Array.CreateInstance(type.GetElementType()!, 0);
            return true;
        }

        if (type == typeof(Task))
        {
            dummy = Task.CompletedTask;
            return true;
        }

        if (type.IsGenericType)
        {
            var definition = type.GetGenericTypeDefinition();
            var args = type.GetGenericArguments();
            if (definition == typeof(Task<>))
            {
                dummy = typeof(Task).GetMethod(nameof(Task.FromResult))!.MakeGenericMethod(args[0]).Invoke(null, [Default(args[0])]);
                return true;
            }

            if (definition == typeof(IEqualityComparer<>))
            {
                dummy = typeof(EqualityComparer<>).MakeGenericType(args[0]).GetProperty("Default")!.GetValue(null);
                return true;
            }

            if (definition == typeof(IComparer<>))
            {
                dummy = typeof(Comparer<>).MakeGenericType(args[0]).GetProperty("Default")!.GetValue(null);
                return true;
            }

            if (definition == typeof(IEnumerable<>) || definition == typeof(IReadOnlyCollection<>) || definition == typeof(IReadOnlyList<>) || definition == typeof(IList<>) || definition == typeof(ICollection<>))
            {
                dummy = Array.CreateInstance(args[0], 0);
                return true;
            }
        }

        if (typeof(Delegate).IsAssignableFrom(type))
        {
            var invoke = type.GetMethod("Invoke")!;
            var parameters = invoke.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();
            Expression body = invoke.ReturnType == typeof(void) ? Expression.Empty() : Expression.Default(invoke.ReturnType);
            dummy = Expression.Lambda(type, body, parameters).Compile();
            return true;
        }

        if (type.IsInterface)
        {
            dummy = typeof(DispatchProxy).GetMethod(nameof(DispatchProxy.Create), 2, [])!
                .MakeGenericMethod(type, typeof(ThrowingProxy)).Invoke(null, null);
            return true;
        }

        if (type.IsClass && !type.IsAbstract)
        {
            foreach (var constructor in type.GetConstructors().OrderBy(c => c.GetParameters().Length))
            {
                var parameters = constructor.GetParameters();
                var args = new object?[parameters.Length];
                var buildable = true;
                for (var i = 0; i < parameters.Length && buildable; i++)
                {
                    buildable = TryCreate(parameters[i].ParameterType, out args[i]);
                }

                if (buildable)
                {
                    dummy = constructor.Invoke(args);
                    return true;
                }
            }
        }

        return false;
    }

    private static object? Default(Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;

    /// <summary>A proxy whose every member throws, standing in for any interface.</summary>
    public class ThrowingProxy : DispatchProxy
    {
        /// <inheritdoc/>
        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            if (targetMethod is { } method && method.Name.StartsWith("Subscribe", StringComparison.Ordinal) && args is [null])
            {
                throw new ArgumentNullException(method.GetParameters()[0].Name);
            }

            throw new NotImplementedException($"A dummy {targetMethod?.DeclaringType} must not be used; it only stands in as an argument.");
        }
    }
}

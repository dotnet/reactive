// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reflection;

namespace Tests.System.Reactive.Shared;

/// <summary>Checks that a target's <c>*Impl</c> methods keep the library's genericity.</summary>
/// <remarks>
/// <para>
/// The <see cref="DescriptionBridge"/> reaches a target's real operator calls through reflection,
/// and it relies on a convention the compiler cannot see: each <c>*Impl</c> method must be as
/// generic as the library call it makes. Within an <c>*Impl</c> the compiler checks everything, so
/// a wrong operator, a missing argument or a mistyped parameter is an ordinary compile error. What
/// it cannot check is how the bridge will later instantiate the method. The bridge instantiates an
/// <c>*Impl</c> at real element types that may themselves be observables, so every observable in
/// the signature must be written over the method's own type parameters, as the library's
/// signatures are. An <c>*Impl</c> written over a closed type such as
/// <c>IObservable&lt;int&gt;</c> is legal C#, compiles, passes every flat scenario, and fails only
/// when a nested one reaches it, with a message that reads as a cast failure.
/// </para>
/// <para>
/// This class enforces the convention instead. A target's test project calls <see cref="Check"/>
/// once, so a signature that breaks the rule fails on every run, by name, whether or not a nested
/// scenario for that operator exists yet.
/// </para>
/// </remarks>
public static class ImplSignatures
{
    /// <summary>
    /// Returns a description of every <c>*Impl</c> method on <paramref name="target"/> that names
    /// one of <paramref name="observableDefinitions"/> over anything but a type parameter.
    /// </summary>
    /// <param name="target">
    /// The target whose private static <c>*Impl</c> methods to inspect.
    /// </param>
    /// <param name="observableDefinitions">
    /// The target's observable interfaces, as open generic definitions.
    /// </param>
    public static IReadOnlyList<string> Check(Type target, params Type[] observableDefinitions)
    {
        ArgumentNullException.ThrowIfNull(target);

        var problems = new List<string>();
        var impls = target.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(m => m.Name.EndsWith("Impl", StringComparison.Ordinal));
        foreach (var method in impls)
        {
            foreach (var parameter in method.GetParameters())
            {
                Inspect(parameter.ParameterType, $"{method.Name}: parameter '{parameter.Name}'");
            }

            Inspect(method.ReturnType, $"{method.Name}: return type");
        }

        return problems;

        void Inspect(Type type, string where)
        {
            if (type.IsArray)
            {
                Inspect(type.GetElementType()!, where);
                return;
            }

            if (!type.IsGenericType)
            {
                return;
            }

            var isObservable = observableDefinitions.Contains(type.GetGenericTypeDefinition());
            foreach (var argument in type.GetGenericArguments())
            {
                if (isObservable && !argument.ContainsGenericParameters)
                {
                    problems.Add(
                        $"{where} is {type}; the element type must be a type parameter so the " +
                        "bridge can instantiate it for nested elements.");
                }

                Inspect(argument, where);
            }
        }
    }
}

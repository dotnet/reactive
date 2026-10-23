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
/// <c>*Impl</c> at real element types that may themselves be observables, so every type parameter
/// of the library call must be a type parameter of the <c>*Impl</c>, in the same place. An
/// <c>*Impl</c> written over a closed type such as <c>IObservable&lt;int&gt;</c> where the library
/// has <c>IObservable&lt;TSource&gt;</c> is legal C#, compiles, passes every flat scenario, and
/// fails only when a nested one reaches it, with a message that reads as a cast failure; one that
/// merges two of the library's type parameters into one fails the same way.
/// </para>
/// <para>
/// This class enforces the convention instead, by comparison with the library call itself. For
/// each <c>*Impl</c> it finds the library method the body calls (<see cref="ImplCalls"/>, which
/// reads the source), lines up that method's parameters (the receiver first, for an instance
/// method) and return type with the <c>*Impl</c>'s, and walks each pair. Where the library has one
/// of its type parameters, the <c>*Impl</c> must have one of its own: the same one wherever that
/// library parameter recurs, and a different one for each. Where the library has a generic type
/// or an array, the <c>*Impl</c> must have one of the same arity, compared argument by argument.
/// Where the library has a closed type, the <c>*Impl</c> may have what it likes, since the
/// library's own signatures have closed types: <c>Sum(source, Func&lt;TSource, int&gt;)</c>
/// returns <c>IObservable&lt;int&gt;</c>.
/// </para>
/// <para>
/// An <c>*Impl</c> that adapts rather than mirrors a call says so with
/// <see cref="AdaptsLibraryCallAttribute"/>; for those the older rule applies, that no observable
/// of the target's own kinds (<c>observableDefinitions</c>) is closed over anything but a type
/// parameter. A target's test project calls <see cref="Check"/> once, so a signature that breaks
/// either rule fails on every run, by name, whether or not a nested scenario for that operator
/// exists yet.
/// </para>
/// </remarks>
public static class ImplSignatures
{
    /// <summary>
    /// Returns a description of every <c>*Impl</c> method on <paramref name="target"/> that is
    /// less generic than the library call it makes.
    /// </summary>
    /// <param name="target">
    /// The target whose private static <c>*Impl</c> methods to inspect.
    /// </param>
    /// <param name="observableDefinitions">
    /// The target's observable interfaces, as open generic definitions, for the <c>*Impl</c>s
    /// marked <see cref="AdaptsLibraryCallAttribute"/>.
    /// </param>
    public static IReadOnlyList<string> Check(Type target, params Type[] observableDefinitions)
    {
        ArgumentNullException.ThrowIfNull(target);

        var problems = new List<string>();

        var impls = target.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(m => m.Name.EndsWith("Impl", StringComparison.Ordinal));

        foreach (var impl in impls)
        {
            if (impl.IsDefined(typeof(AdaptsLibraryCallAttribute), inherit: false))
            {
                CheckObservables(impl, observableDefinitions, problems);
                continue;
            }

            MethodInfo? library;
            try
            {
                library = ImplCalls.Resolve(impl);
            }
            catch (InvalidOperationException ex)
            {
                problems.Add($"{impl.Name}: {ex.Message}");
                continue;
            }

            if (library is null)
            {
                problems.Add(
                    $"{impl.Name} calls no library method; if it adapts rather than mirrors a " +
                    "call, mark it [AdaptsLibraryCall].");
                continue;
            }

            CompareWithLibrary(impl, library, problems);
        }

        return problems;
    }

    private static void CompareWithLibrary(
        MethodInfo impl,
        MethodInfo library,
        List<string> problems)
    {
        var name = $"{impl.Name} ({library.DeclaringType!.Name}.{library.Name})";

        var libraryTypes = new List<(string Where, Type Type)>();
        if (!library.IsStatic)
        {
            libraryTypes.Add(("the receiver", library.DeclaringType));
        }

        libraryTypes.AddRange(
            library.GetParameters().Select(p => ($"parameter '{p.Name}'", p.ParameterType)));

        var implTypes = impl.GetParameters()
            .Select(p => (Where: $"parameter '{p.Name}'", Type: p.ParameterType))
            .ToList();

        if (implTypes.Count != libraryTypes.Count)
        {
            problems.Add(
                $"{name}: {implTypes.Count} parameter(s) where the library call has " +
                $"{libraryTypes.Count} (the receiver counts as one); if it adapts rather than " +
                "mirrors the call, mark it [AdaptsLibraryCall].");
            return;
        }

        var map = new Dictionary<Type, Type>();
        for (var i = 0; i < implTypes.Count; i++)
        {
            Match(
                implTypes[i].Type,
                libraryTypes[i].Type,
                map,
                $"{name}: {implTypes[i].Where}",
                problems);
        }

        Match(impl.ReturnType, library.ReturnType, map, $"{name}: return type", problems);
    }

    // Walks an *Impl type and the library type it stands for, in parallel.
    private static void Match(
        Type impl,
        Type library,
        Dictionary<Type, Type> map,
        string where,
        List<string> problems)
    {
        if (library.IsGenericParameter)
        {
            if (!impl.IsGenericParameter)
            {
                problems.Add(
                    $"{where} is {impl} where the library has its type parameter {library.Name}; " +
                    "it must be a type parameter so the bridge can instantiate it for nested " +
                    "elements.");
            }
            else if (map.TryGetValue(library, out var seen))
            {
                if (seen != impl)
                {
                    problems.Add(
                        $"{where}: the library's {library.Name} appears as {impl.Name} here and " +
                        $"as {seen.Name} elsewhere.");
                }
            }
            else if (map.ContainsValue(impl))
            {
                var other = map.First(kv => kv.Value == impl).Key.Name;
                problems.Add(
                    $"{where}: {impl.Name} stands for the library's {library.Name} here and for " +
                    $"its {other} elsewhere; one type parameter per library type parameter.");
            }
            else
            {
                map[library] = impl;
            }

            return;
        }

        if (library.IsArray)
        {
            if (!impl.IsArray)
            {
                problems.Add($"{where} is {impl} where the library has the array {library}.");
                return;
            }

            Match(
                impl.GetElementType()!,
                library.GetElementType()!,
                map,
                $"{where}, element",
                problems);
            return;
        }

        if (library.IsGenericType)
        {
            var libraryArguments = library.GetGenericArguments();
            if (!impl.IsGenericType || impl.GetGenericArguments().Length != libraryArguments.Length)
            {
                problems.Add($"{where} is {impl} where the library has {library}.");
                return;
            }

            var implArguments = impl.GetGenericArguments();
            for (var i = 0; i < libraryArguments.Length; i++)
            {
                Match(
                    implArguments[i],
                    libraryArguments[i],
                    map,
                    $"{where}, argument {i}",
                    problems);
            }
        }

        // A closed type in the library's signature constrains nothing.
    }

    // The rule for an *Impl that adapts a call: no observable closed over anything but a type
    // parameter.
    private static void CheckObservables(
        MethodInfo impl,
        Type[] observableDefinitions,
        List<string> problems)
    {
        foreach (var parameter in impl.GetParameters())
        {
            Inspect(parameter.ParameterType, $"{impl.Name}: parameter '{parameter.Name}'");
        }

        Inspect(impl.ReturnType, $"{impl.Name}: return type");

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

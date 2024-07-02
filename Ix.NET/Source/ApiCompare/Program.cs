// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ApiCompare
{
    internal class Program
    {
        private static readonly Type AsyncInterfaceType = typeof(IAsyncEnumerable<>);
        private static readonly Type SyncInterfaceType = typeof(IEnumerable<>);

        private static readonly Type AsyncOrderedInterfaceType = typeof(IOrderedAsyncEnumerable<>);
        private static readonly Type SyncOrderedInterfaceType = typeof(IOrderedEnumerable<>);

        private static readonly string[] Exceptions =
        [
            "SkipLast",  // In .NET Core 2.0
            "TakeLast",  // In .NET Core 2.0

            "ToHashSet",  // In .NET Core 2.0

            "Cast",    // Non-generic methods
            "OfType",  // Non-generic methods

            "AsEnumerable",       // Trivially renamed
            "AsAsyncEnumerable",  // Trivially renamed

            "ForEachAsync",  // "foreach await" language substitute for the time being

            "ToAsyncEnumerable",  // First-class conversions
            "ToEnumerable",       // First-class conversions
            "ToObservable",       // First-class conversions
        ];

        private static readonly TypeSubstitutor Subst = new(new Dictionary<Type, Type>
        {
            { AsyncInterfaceType, SyncInterfaceType },
            { AsyncOrderedInterfaceType, SyncOrderedInterfaceType },
            { typeof(IAsyncGrouping<,>), typeof(IGrouping<,>) },
        });

        private static void Main()
        {
            var asyncOperatorsType = typeof(AsyncEnumerable);
            var syncOperatorsType = typeof(Enumerable);

            Compare(syncOperatorsType, asyncOperatorsType);
        }

        private static void Compare(Type syncOperatorsType, Type asyncOperatorsType)
        {
            var syncOperators = GetQueryOperators([SyncInterfaceType, SyncOrderedInterfaceType], syncOperatorsType, Exceptions);
            var asyncOperators = GetQueryOperators([AsyncInterfaceType, AsyncOrderedInterfaceType], asyncOperatorsType, Exceptions);

            CompareFactories(syncOperators.Factories, asyncOperators.Factories);
            CompareQueryOperators(syncOperators.QueryOperators, asyncOperators.QueryOperators);
            CompareAggregates(syncOperators.Aggregates, asyncOperators.Aggregates);
        }

        private static void CompareFactories(ILookup<string, MethodInfo> syncFactories, ILookup<string, MethodInfo> asyncFactories)
        {
            CompareSets(syncFactories, asyncFactories, CompareFactoryOverloads);
        }

        private static void CompareFactoryOverloads(string name, IEnumerable<MethodInfo> syncMethods, IEnumerable<MethodInfo> asyncMethods)
        {
            var sync = GetSignatures(syncMethods).ToArray();
            var async = GetRewrittenSignatures(asyncMethods).ToArray();

            //
            // Ensure that async is a superset of sync.
            //

            var notInAsync = sync.Except(async);

            if (notInAsync.Any())
            {
                foreach (var signature in notInAsync)
                {
                    Console.WriteLine("MISSING " + ToString(signature.Method));
                }
            }

            //
            // Check for excess overloads.
            //

            var notInSync = async.Except(sync);

            if (notInSync.Any())
            {
                foreach (var signature in notInSync)
                {
                    Console.WriteLine("EXCESS " + ToString(signature.Method));
                }
            }
        }

        private static void CompareQueryOperators(ILookup<string, MethodInfo> syncOperators, ILookup<string, MethodInfo> asyncOperators)
        {
            CompareSets(syncOperators, asyncOperators, CompareQueryOperatorsOverloads);
        }

        private static void CompareQueryOperatorsOverloads(string name, IEnumerable<MethodInfo> syncMethods, IEnumerable<MethodInfo> asyncMethods)
        {
            var sync = GetSignatures(syncMethods).ToArray();
            var async = GetRewrittenSignatures(asyncMethods).ToArray();

            //
            // Ensure that async is a superset of sync.
            //

            var notInAsync = sync.Except(async);

            if (notInAsync.Any())
            {
                foreach (var signature in notInAsync)
                {
                    Console.WriteLine("MISSING " + ToString(signature.Method));
                }
            }

            //
            // Find Task-based overloads.
            //

            var taskBasedSignatures = new List<Signature>();

            foreach (var signature in sync)
            {
                if (signature.ParameterTypes.Any(IsFuncOrActionType))
                {
                    taskBasedSignatures.Add(GetAsyncVariant(signature));
                }
            }

            if (taskBasedSignatures.Count > 0)
            {
                var notInAsyncTaskBased = taskBasedSignatures.Except(async);

                if (notInAsyncTaskBased.Any())
                {
                    foreach (var signature in notInAsyncTaskBased)
                    {
                        Console.WriteLine("MISSING " + name + " :: " + signature);
                    }
                }
            }

            //
            // Excess overloads that are neither carbon copies of sync nor Task-based variants of sync.
            //

            var notInSync = async.Except(sync.Union(taskBasedSignatures));

            if (notInSync.Any())
            {
                foreach (var signature in notInSync)
                {
                    Console.WriteLine("EXCESS " + ToString(signature.Method));
                }
            }
        }

        private static void CompareAggregates(ILookup<string, MethodInfo> syncAggregates, ILookup<string, MethodInfo> asyncAggregates)
        {
            CompareSets(syncAggregates, asyncAggregates, CompareAggregateOverloads);
        }

        private static void CompareAggregateOverloads(string name, IEnumerable<MethodInfo> syncMethods, IEnumerable<MethodInfo> asyncMethods)
        {
            var sync = GetSignatures(syncMethods).Select(GetAsyncAggregateSignature).ToArray();
            var async = GetRewrittenSignatures(asyncMethods).ToArray();

            //
            // Ensure that async is a superset of sync.
            //

            var notInAsync = sync.Except(async);

            if (notInAsync.Any())
            {
                foreach (var signature in notInAsync)
                {
                    Console.WriteLine("MISSING " + ToString(signature.Method));
                }
            }

            //
            // Find Task-based overloads.
            //

            var taskBasedSignatures = new List<Signature>();

            foreach (var signature in sync)
            {
                if (signature.ParameterTypes.Any(IsFuncOrActionType))
                {
                    taskBasedSignatures.Add(GetAsyncVariant(signature));
                }
            }

            if (taskBasedSignatures.Count > 0)
            {
                var notInAsyncTaskBased = taskBasedSignatures.Except(async);

                if (notInAsyncTaskBased.Any())
                {
                    foreach (var signature in notInAsyncTaskBased)
                    {
                        Console.WriteLine("MISSING " + name + " :: " + signature);
                    }
                }
            }

            //
            // Check for overloads with CancellationToken.
            //

            var withCancellationToken = new List<Signature>();

            foreach (var signature in sync)
            {
                withCancellationToken.Add(AppendCancellationToken(signature));
            }

            foreach (var signature in taskBasedSignatures)
            {
                withCancellationToken.Add(AppendCancellationToken(signature));
            }

            var notInAsyncWithCancellationToken = withCancellationToken.Except(async);

            if (notInAsyncWithCancellationToken.Any())
            {
                foreach (var signature in notInAsyncWithCancellationToken)
                {
                    Console.WriteLine("MISSING " + name + " :: " + signature);
                }
            }

            //
            // Excess overloads that are neither carbon copies of sync nor Task-based variants of sync.
            //

            var notInSync = async.Except(sync.Union(taskBasedSignatures).Union(withCancellationToken));

            if (notInSync.Any())
            {
                foreach (var signature in notInSync)
                {
                    Console.WriteLine("EXCESS " + ToString(signature.Method));
                }
            }
        }

        private static bool IsFuncOrActionType(Type type)
        {
            if (type.IsConstructedGenericType)
            {
                var defName = type.GetGenericTypeDefinition().Name;
                return defName.StartsWith("Func`") || defName.StartsWith("Action`");
            }

            if (type == typeof(Action))
            {
                return true;
            }

            return false;
        }

        private static Signature GetAsyncVariant(Signature signature)
        {
            return new Signature(
                parameterTypes: signature.ParameterTypes.Select(GetAsyncVariant).ToArray(),
                returnType: signature.ReturnType);
        }

        private static Signature AppendCancellationToken(Signature signature)
        {
            return new Signature(
                parameterTypes: [.. signature.ParameterTypes, typeof(CancellationToken)],
                returnType: signature.ReturnType);
        }

        private static Type GetAsyncVariant(Type type)
        {
            if (IsFuncOrActionType(type))
            {
                if (type == typeof(Action))
                {
                    return typeof(Func<Task>);
                }
                else
                {
                    var args = type.GetGenericArguments();

                    var defName = type.GetGenericTypeDefinition().Name;
                    if (defName.StartsWith("Func`"))
                    {
                        var ret = typeof(Task<>).MakeGenericType(args.Last());

                        return Expression.GetFuncType(Enumerable.SkipLast(args, 1).Append(ret).ToArray());
                    }
                    else
                    {
                        return Expression.GetFuncType([.. args, typeof(Task)]);
                    }
                }
            }

            return type;
        }

        private static void CompareSets(ILookup<string, MethodInfo> sync, ILookup<string, MethodInfo> async, Action<string, IEnumerable<MethodInfo>, IEnumerable<MethodInfo>> compareCore)
        {
            var syncNames = sync.Select(g => g.Key).ToArray();
            var asyncNames = async.Select(g => g.Key).ToArray();

            //
            // Analyze that async is a superset of sync.
            //

            var notInAsync = syncNames.Except(asyncNames);

            foreach (var n in notInAsync)
            {
                foreach (var o in sync[n])
                {
                    Console.WriteLine("MISSING " + ToString(o));
                }
            }

            //
            // Need to find the same overloads.
            //

            var inBoth = syncNames.Intersect(asyncNames);

            foreach (var n in inBoth)
            {
                var s = sync[n];
                var a = async[n];

                compareCore(n, s, a);
            }

            //
            // Report excessive API surface.
            //

            var onlyInAsync = asyncNames.Except(syncNames);

            foreach (var n in onlyInAsync)
            {
                foreach (var o in async[n])
                {
                    Console.WriteLine("EXCESS " + ToString(o));
                }
            }
        }

        private static Operators GetQueryOperators(Type[] interfaceTypes, Type operatorsType, string[] exclude)
        {
            //
            // Get all the static methods.
            //

            var methods = operatorsType.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => !exclude.Contains(m.Name));

            //
            // Get extension methods. These can be either operators or aggregates.
            //

            var extensionMethods = methods.Where(m => m.IsDefined(typeof(ExtensionAttribute))).ToArray();

            //
            // Static methods that aren't extension methods can be factories.
            //

            var factories = methods.Except(extensionMethods).Where(m => m.ReturnType.IsConstructedGenericType && interfaceTypes.Contains(m.ReturnType.GetGenericTypeDefinition())).ToArray();

            //
            // Extension methods that return the interface type are operators.
            //

            var queryOperators = extensionMethods.Where(m => m.ReturnType.IsConstructedGenericType && interfaceTypes.Contains(m.ReturnType.GetGenericTypeDefinition())).ToArray();

            //
            // Extension methods that return another type are aggregates.
            //

            var aggregates = extensionMethods.Except(queryOperators).ToArray();

            //
            // Return operators.
            //

            return new Operators(
                Factories: factories.ToLookup(m => m.Name, m => m),
                QueryOperators: queryOperators.ToLookup(m => m.Name, m => m),
                Aggregates: aggregates.ToLookup(m => m.Name, m => m));
        }

        private static IEnumerable<Signature> GetSignatures(IEnumerable<MethodInfo> methods)
        {
            return methods.Select(m => GetSignature(m));
        }

        private static IEnumerable<Signature> GetRewrittenSignatures(IEnumerable<MethodInfo> methods)
        {
            return GetSignatures(methods).Select(s => RewriteSignature(s));
        }

        private static Signature GetSignature(MethodInfo method)
        {
            if (method.IsGenericMethodDefinition)
            {
                var newArgs = method.GetGenericArguments().Select((t, i) => Wildcards[i]).ToArray();
                method = method.MakeGenericMethod(newArgs);
            }

            return new Signature(
                returnType: method.ReturnType,
                parameterTypes: method.GetParameters().Select(p => p.ParameterType).ToArray(),
                method: method);
        }

        private static Signature RewriteSignature(Signature signature)
        {
            return new Signature(
                returnType: Subst.Visit(signature.ReturnType),
                parameterTypes: Subst.Visit(signature.ParameterTypes),
                method: signature.Method);
        }

        private static Signature GetAsyncAggregateSignature(Signature signature)
        {
            var retType = signature.ReturnType == typeof(void) ? typeof(Task) : typeof(Task<>).MakeGenericType(signature.ReturnType);

            return new Signature(
                returnType: retType,
                parameterTypes: signature.ParameterTypes,
                method: signature.Method);
        }

        private static string ToString(MethodInfo? method)
        {
            if (method == null)
            {
                return "UNKNOWN";
            }

            if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
            {
                method = method.GetGenericMethodDefinition();
            }

            return method.ToString() ?? "UNKNOWN";
        }

        private record Operators(
            ILookup<string, MethodInfo> Factories,
            ILookup<string, MethodInfo> QueryOperators,
            ILookup<string, MethodInfo> Aggregates);

        private class Signature(
            Type returnType,
            Type[] parameterTypes,
            MethodInfo? method = null) : IEquatable<Signature>
        {
            public MethodInfo? Method { get; } = method;
            public Type ReturnType { get; } = returnType;
            public Type[] ParameterTypes { get; } = parameterTypes;

            public static bool operator ==(Signature s1, Signature s2)
            {
                if (s1 is null && s2 is null)
                {
                    return true;
                }

                if (s1 is null || s2 is null)
                {
                    return false;
                }

                return s1.Equals(s2);
            }

            public static bool operator !=(Signature s1, Signature s2)
            {
                return !(s1 == s2);
            }

            public bool Equals(Signature? s)
            {
                return s is not null && ReturnType.Equals(s.ReturnType) && ParameterTypes.SequenceEqual(s.ParameterTypes);
            }

            public override bool Equals(object? obj) => obj is Signature s && Equals(s);

            public override int GetHashCode()
            {
                return ParameterTypes.Concat([ReturnType]).Aggregate(0, (a, t) => a * 17 + t.GetHashCode());
            }

            public override string ToString()
            {
                return "(" + string.Join(", ", ParameterTypes.Select(t => t.ToCSharp())) + ") -> " + ReturnType.ToCSharp();
            }
        }

        private class TypeVisitor
        {
            public virtual Type Visit(Type type)
            {
                if (type.IsArray)
                {
                    if (type.GetElementType()!.MakeArrayType() == type)
                    {
                        return VisitArray(type);
                    }
                    else
                    {
                        return VisitMultidimensionalArray(type);
                    }
                }
                else if (type.GetTypeInfo().IsGenericTypeDefinition)
                {
                    return VisitGenericTypeDefinition(type);
                }
                else if (type.IsConstructedGenericType)
                {
                    return VisitGeneric(type);
                }
                else if (type.IsByRef)
                {
                    return VisitByRef(type);
                }
                else if (type.IsPointer)
                {
                    return VisitPointer(type);
                }
                else
                {
                    return VisitSimple(type);
                }
            }

            protected virtual Type VisitArray(Type type)
            {
                return Visit(type.GetElementType() ?? throw new ArgumentException($"{type} does not have an element type")).MakeArrayType();
            }

            protected virtual Type VisitMultidimensionalArray(Type type)
            {
                return Visit(type.GetElementType() ?? throw new ArgumentException($"{type} does not have an element type")).MakeArrayType(type.GetArrayRank());
            }

            protected virtual Type VisitGenericTypeDefinition(Type type)
            {
                return type;
            }

            protected virtual Type VisitGeneric(Type type)
            {
                return Visit(type.GetGenericTypeDefinition()).MakeGenericType(Visit(type.GenericTypeArguments));
            }

            protected virtual Type VisitByRef(Type type)
            {
                return Visit(type.GetElementType() ?? throw new ArgumentException($"{type} does not have an element type")).MakeByRefType();
            }

            protected virtual Type VisitPointer(Type type)
            {
                return Visit(type.GetElementType() ?? throw new ArgumentException($"{type} does not have an element type")).MakePointerType();
            }

            protected virtual Type VisitSimple(Type type)
            {
                return type;
            }

            public Type[] Visit(Type[] types)
            {
                return types.Select(Visit).ToArray();
            }
        }

        private class TypeSubstitutor(Dictionary<Type, Type> map) : TypeVisitor
        {
            public override Type Visit(Type type)
            {
                if (map.TryGetValue(type, out var subst))
                {
                    return subst;
                }

                return base.Visit(type);
            }
        }

        private static readonly Type[] Wildcards = [typeof(T1), typeof(T2), typeof(T3), typeof(T4)];

        private class T1 { }

        private class T2 { }

        private class T3 { }

        private class T4 { }
    }

    internal static class TypeExtensions
    {
        public static string ToCSharp(this Type type)
        {
            if (type.IsArray)
            {
                var elementType = type.GetElementType()!;
                if (elementType.MakeArrayType() == type)
                {
                    return elementType.ToCSharp() + "[]";
                }
                else
                {
                    return elementType.ToCSharp() + "[" + new string(',', type.GetArrayRank() - 1) + "]";
                }
            }
            else if (type.IsConstructedGenericType)
            {
                var def = type.GetGenericTypeDefinition();
                var defName = def.Name[..def.Name.IndexOf('`')];

                return defName + "<" + string.Join(", ", type.GetGenericArguments().Select(ToCSharp)) + ">";
            }
            else
            {
                return type.Name;
            }
        }
    }
}

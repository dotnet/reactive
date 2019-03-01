// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
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
    class Program
    {
        private static readonly Type asyncInterfaceType = typeof(IAsyncEnumerable<>);
        private static readonly Type syncInterfaceType = typeof(IEnumerable<>);

        private static readonly Type asyncOrderedInterfaceType = typeof(IOrderedAsyncEnumerable<>);
        private static readonly Type syncOrderedInterfaceType = typeof(IOrderedEnumerable<>);

        private static readonly string[] exceptions = new[]
        {
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
        };

        private static readonly TypeSubstitutor subst = new TypeSubstitutor(new Dictionary<Type, Type>
        {
            { asyncInterfaceType, syncInterfaceType },
            { asyncOrderedInterfaceType, syncOrderedInterfaceType },
            { typeof(IAsyncGrouping<,>), typeof(IGrouping<,>) },
        });

        static void Main()
        {
            var asyncOperatorsType = typeof(AsyncEnumerable);
            var syncOperatorsType = typeof(Enumerable);

            Compare(syncOperatorsType, asyncOperatorsType);
        }

        static void Compare(Type syncOperatorsType, Type asyncOperatorsType)
        {
            var syncOperators = GetQueryOperators(new[] { syncInterfaceType, syncOrderedInterfaceType }, syncOperatorsType, exceptions);
            var asyncOperators = GetQueryOperators(new[] { asyncInterfaceType, asyncOrderedInterfaceType }, asyncOperatorsType, exceptions);

            CompareFactories(syncOperators.Factories, asyncOperators.Factories);
            CompareQueryOperators(syncOperators.QueryOperators, asyncOperators.QueryOperators);
            CompareAggregates(syncOperators.Aggregates, asyncOperators.Aggregates);
        }

        static void CompareFactories(ILookup<string, MethodInfo> syncFactories, ILookup<string, MethodInfo> asyncFactories)
        {
            CompareSets(syncFactories, asyncFactories, CompareFactoryOverloads);
        }

        static void CompareFactoryOverloads(string name, IEnumerable<MethodInfo> syncMethods, IEnumerable<MethodInfo> asyncMethods)
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

        static void CompareQueryOperators(ILookup<string, MethodInfo> syncOperators, ILookup<string, MethodInfo> asyncOperators)
        {
            CompareSets(syncOperators, asyncOperators, CompareQueryOperatorsOverloads);
        }

        static void CompareQueryOperatorsOverloads(string name, IEnumerable<MethodInfo> syncMethods, IEnumerable<MethodInfo> asyncMethods)
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

        static void CompareAggregates(ILookup<string, MethodInfo> syncAggregates, ILookup<string, MethodInfo> asyncAggregates)
        {
            CompareSets(syncAggregates, asyncAggregates, CompareAggregateOverloads);
        }

        static void CompareAggregateOverloads(string name, IEnumerable<MethodInfo> syncMethods, IEnumerable<MethodInfo> asyncMethods)
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
            return new Signature
            {
                ParameterTypes = signature.ParameterTypes.Select(GetAsyncVariant).ToArray(),
                ReturnType = signature.ReturnType
            };
        }

        private static Signature AppendCancellationToken(Signature signature)
        {
            return new Signature
            {
                ParameterTypes = signature.ParameterTypes.Concat(new[] { typeof(CancellationToken) }).ToArray(),
                ReturnType = signature.ReturnType
            };
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
                        return Expression.GetFuncType(args.Append(typeof(Task)).ToArray());
                    }
                }
            }

            return type;
        }

        static void CompareSets(ILookup<string, MethodInfo> sync, ILookup<string, MethodInfo> async, Action<string, IEnumerable<MethodInfo>, IEnumerable<MethodInfo>> compareCore)
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

        static Operators GetQueryOperators(Type[] interfaceTypes, Type operatorsType, string[] exclude)
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

            return new Operators
            {
                Factories = factories.ToLookup(m => m.Name, m => m),
                QueryOperators = queryOperators.ToLookup(m => m.Name, m => m),
                Aggregates = aggregates.ToLookup(m => m.Name, m => m),
            };
        }

        static IEnumerable<Signature> GetSignatures(IEnumerable<MethodInfo> methods)
        {
            return methods.Select(m => GetSignature(m));
        }

        static IEnumerable<Signature> GetRewrittenSignatures(IEnumerable<MethodInfo> methods)
        {
            return GetSignatures(methods).Select(s => RewriteSignature(s));
        }

        static Signature GetSignature(MethodInfo method)
        {
            if (method.IsGenericMethodDefinition)
            {
                var newArgs = method.GetGenericArguments().Select((t, i) => Wildcards[i]).ToArray();
                method = method.MakeGenericMethod(newArgs);
            }

            return new Signature
            {
                Method = method,
                ReturnType = method.ReturnType,
                ParameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray()
            };
        }

        static Signature RewriteSignature(Signature signature)
        {
            return new Signature
            {
                Method = signature.Method,
                ReturnType = subst.Visit(signature.ReturnType),
                ParameterTypes = subst.Visit(signature.ParameterTypes)
            };
        }

        static Signature GetAsyncAggregateSignature(Signature signature)
        {
            var retType = signature.ReturnType == typeof(void) ? typeof(Task) : typeof(Task<>).MakeGenericType(signature.ReturnType);

            return new Signature
            {
                Method = signature.Method,
                ReturnType = retType,
                ParameterTypes = signature.ParameterTypes
            };
        }

        static string ToString(MethodInfo method)
        {
            if (method == null)
            {
                return "UNKNOWN";
            }

            if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
            {
                method = method.GetGenericMethodDefinition();
            }

            return method.ToString();
        }

        class Operators
        {
            public ILookup<string, MethodInfo> Factories;
            public ILookup<string, MethodInfo> QueryOperators;
            public ILookup<string, MethodInfo> Aggregates;
        }

        class Signature : IEquatable<Signature>
        {
            public MethodInfo Method;
            public Type ReturnType;
            public Type[] ParameterTypes;

            public static bool operator ==(Signature s1, Signature s2)
            {
                if ((object)s1 == null && (object)s2 == null)
                {
                    return true;
                }

                if ((object)s1 == null || (object)s2 == null)
                {
                    return false;
                }

                return s1.Equals(s2);
            }

            public static bool operator !=(Signature s1, Signature s2)
            {
                return !(s1 == s2);
            }

            public bool Equals(Signature s)
            {
                return (object)s != null && ReturnType.Equals(s.ReturnType) && ParameterTypes.SequenceEqual(s.ParameterTypes);
            }

            public override bool Equals(object obj)
            {
                if (obj is Signature s)
                {
                    return Equals(s);
                }

                return false;
            }

            public override int GetHashCode()
            {
                return ParameterTypes.Concat(new[] { ReturnType }).Aggregate(0, (a, t) => a * 17 + t.GetHashCode());
            }

            public override string ToString()
            {
                return "(" + string.Join(", ", ParameterTypes.Select(t => t.ToCSharp())) + ") -> " + ReturnType.ToCSharp();
            }
        }

        class TypeVisitor
        {
            public virtual Type Visit(Type type)
            {
                if (type.IsArray)
                {
                    if (type.GetElementType().MakeArrayType() == type)
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
                return Visit(type.GetElementType()).MakeArrayType();
            }

            protected virtual Type VisitMultidimensionalArray(Type type)
            {
                return Visit(type.GetElementType()).MakeArrayType(type.GetArrayRank());
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
                return Visit(type.GetElementType()).MakeByRefType();
            }

            protected virtual Type VisitPointer(Type type)
            {
                return Visit(type.GetElementType()).MakePointerType();
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

        class TypeSubstitutor : TypeVisitor
        {
            private readonly Dictionary<Type, Type> map;

            public TypeSubstitutor(Dictionary<Type, Type> map)
            {
                this.map = map;
            }

            public override Type Visit(Type type)
            {
                if (map.TryGetValue(type, out var subst))
                {
                    return subst;
                }

                return base.Visit(type);
            }
        }

        private static readonly Type[] Wildcards = new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) };

        class T1 { }
        class T2 { }
        class T3 { }
        class T4 { }
    }

    static class TypeExtensions
    {
        public static string ToCSharp(this Type type)
        {
            if (type.IsArray)
            {
                if (type.GetElementType().MakeArrayType() == type)
                {
                    return type.GetElementType().ToCSharp() + "[]";
                }
                else
                {
                    return type.GetElementType().ToCSharp() + "[" + new string(',', type.GetArrayRank() - 1) + "]";
                }
            }
            else if (type.IsConstructedGenericType)
            {
                var def = type.GetGenericTypeDefinition();
                var defName = def.Name.Substring(0, def.Name.IndexOf('`'));

                return defName + "<" + string.Join(", ", type.GetGenericArguments().Select(ToCSharp)) + ">";
            }
            else
            {
                return type.Name;
            }
        }
    }
}

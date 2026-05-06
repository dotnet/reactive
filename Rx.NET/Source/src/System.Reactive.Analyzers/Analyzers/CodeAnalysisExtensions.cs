// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace System.Reactive.Analyzers
{
    internal static class CodeAnalysisExtensions
    {
        public static IEnumerable<ITypeSymbol> GetBaseTypesAndThis(this ITypeSymbol? type)
        {
            var current = type;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }

        public static bool InheritsFromOrEquals(
                    this ITypeSymbol type, ITypeSymbol baseType, bool includeInterfaces)
        {
            if (!includeInterfaces)
            {
                return InheritsFromOrEquals(type, baseType);
            }

            return type.GetBaseTypesAndThis().Concat(type.AllInterfaces).Any(t => SymbolEqualityComparer.Default.Equals(t, baseType));
        }

        public static bool InheritsFromOrEquals(
            this ITypeSymbol type, ITypeSymbol baseType)
        {
            return type.GetBaseTypesAndThis().Any(t => SymbolEqualityComparer.Default.Equals(t, baseType));
        }

        public static bool IsIObservable(this ITypeSymbol typeSymbol)
        {
            return typeSymbol is INamedTypeSymbol { Name: "IObservable", Arity: 1, ContainingNamespace.MetadataName: "System" }
                || typeSymbol.AllInterfaces.Any(IsIObservable);
        }

        public static bool IsIObservableOfEventPattern(this ITypeSymbol typeSymbol)
        {
            return (typeSymbol is INamedTypeSymbol { Name: "IObservable", Arity: 1, ContainingNamespace.MetadataName: "System" } nts &&
                nts.TypeArguments.Length == 1 && nts.TypeArguments[0] is INamedTypeSymbol { Name: "EventPattern", Arity: 2, ContainingNamespace: { MetadataName: "Reactive", ContainingNamespace.MetadataName: "System" } })
                || typeSymbol.AllInterfaces.Any(IsIObservableOfEventPattern);
        }

        public static bool IsIAsyncAction(this ITypeSymbol typeSymbol)
        {
            return typeSymbol is INamedTypeSymbol { Name: "IAsyncAction", Arity: 0, ContainingNamespace: { MetadataName: "Foundation", ContainingNamespace.MetadataName: "Windows" } }
                || typeSymbol.AllInterfaces.Any(IsIAsyncAction);
        }

        public static bool IsIAsyncActionWithProgress(this ITypeSymbol typeSymbol)
        {
            // Note: we are deliberately ignore what the actual type parameters are, because we want to match on IAsyncActionWithProgress<TProgress> for any TProgress.
            return typeSymbol is INamedTypeSymbol { Name: "IAsyncActionWithProgress", Arity: 1, ContainingNamespace: { MetadataName: "Foundation", ContainingNamespace.MetadataName: "Windows" } }
                || typeSymbol.AllInterfaces.Any(IsIAsyncActionWithProgress);
        }

        public static bool IsIAsyncOperation(this ITypeSymbol typeSymbol)
        {
            // Note: we are deliberately ignore what the actual type parameters are, because we want to match on IAsyncOperation<TProgress> for any TProgress.
            return typeSymbol is INamedTypeSymbol { Name: "IAsyncOperation", Arity: 1, ContainingNamespace: { MetadataName: "Foundation", ContainingNamespace.MetadataName: "Windows" } }
                || typeSymbol.AllInterfaces.Any(IsIAsyncOperation);
        }

        public static bool IsIAsyncOperationWithProgress(this ITypeSymbol typeSymbol)
        {
            // Note: we are deliberately ignore what the actual type parameters are, because we want to match on IAsyncOperationWithProgress<TProgress> for any TProgress.
            return typeSymbol is INamedTypeSymbol { Name: "IAsyncOperationWithProgress", Arity: 2, ContainingNamespace: { MetadataName: "Foundation", ContainingNamespace.MetadataName: "Windows" } }
                || typeSymbol.AllInterfaces.Any(IsIAsyncOperationWithProgress);
        }
    }
}

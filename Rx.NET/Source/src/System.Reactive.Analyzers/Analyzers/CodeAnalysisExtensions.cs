// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}

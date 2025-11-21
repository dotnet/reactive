// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Linq.Async.SourceGenerator
{
    /// <summary>
    /// Builds a facade class containing duplicate definitions of the various deprecated AsyncEnumerable methods
    /// as members of <c>AsyncEnumerableDeprecated</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is necessary because of complicated backwards compatibility issues.
    /// </para>
    /// <para>
    /// The scenario this addresses is when an application has been using System.Linq.Async v6, and was using
    /// methods that have been marked as obsolete in System.Linq.Async v7. For example, code might be using
    /// the <c>WhereAwaitWithCancellation</c> extension method.
    /// </para>
    /// <para>
    /// When the developer upgrades such a project to System.Linq.Async v7, we want to ensure that they get
    /// deprecation warnings telling them which method they should use instead (e.g., <c>WhereAwaitWithCancellation</c>
    /// should be replaced with one of the overloads of <c>Where</c> that .NET 10's <c>System.Linq.AsyncEnumerable</c>
    /// defines. However, we don't want to break their build immediately; if they are OK with the deprecation warnings,
    /// we want their existing code to continue to work without changes.
    /// </para>
    /// <para>
    /// This is complicated by the fact that in .NET 10, <c>System.Linq.AsyncEnumerable</c> now defines its own
    /// <c>AsyncEnumerable</c> type, meaning that <c>System.Linq.Async</c>'s public API must no longer contain a type
    /// of that name. (Otherwise, code such as <c>AsyncEnumerable.Range(1, 10)</c> would fail to compile due to the
    /// type name being ambiguous.) Thus, the public-facing API of <c>System.Linq.Async</c> in v7 has moved all of the
    /// extension methods that it continues to define into a type named <c>AsyncEnumerableDeprecated</c>. (We've
    /// called it this because the only methods we need to retain from the public API are the ones that are deprecated.
    /// In cases where there are direct replacements in .NET 10's <c>System.Linq.AsyncEnumerable</c>, we've removed
    /// the corresponding methods from <c>System.Linq.Async</c> entirely. And in other cases we've moved functionality
    /// into <c>System.Interactive.Async</c>. The goal is for everyone to stop using <c>System.Linq.Async</c>, so by
    /// definition, if you are still using a method it defines, that method is obsolete.)
    /// </para>
    /// <para>
    /// But now the problem is that code that continues to use these deprecated methods will expect them to live in
    /// <c>AsyncEnumerableDeprecated</c>, because that's where the compiler will find the relevant methods. So
    /// the runtime assembly will need to make these methods available in a type of that name. But for binary backwards
    /// compatibility, we need every method that our <c>AsyncEnumerable</c> defined in V6 still to be available on
    /// a type still called <c>AsyncEnumerable</c>.
    /// </para>
    /// <para>
    /// In other words, we need to make our API available twice, on two different types. One for code that hasn't been
    /// recompiled against v7, and therefore expects all the methods to be in <c>AsyncEnumerable</c>, and one for code
    /// that has been recompiled against v7 but which has chosen to continue using deprecated method, and which will
    /// expect those to be in <c>AsyncEnumerableDeprecated</c>.
    /// </para>
    /// <para>
    /// So this generator duplicates public static methods defined by <c>AsyncEnumerable</c> into <c>AsyncEnumerableDeprecated</c>.
    /// It also strips off their extension method 'this' modifier from the first parameter, so that they are normal static methods,
    /// because otherwise, we get ambiguity errors in our unit tests.
    /// </para>
    /// </remarks>
    internal class DeprecatedDuplicateBuilder
    {
        private readonly GeneratorExecutionContext _context;
        private readonly GenerationOptions _options;
        private readonly INamedTypeSymbol _generateAsyncOverloadAttributeAttributeSymbol;
        private readonly INamedTypeSymbol _attributeSymbol;
        private readonly SyntaxReceiver _syntaxReceiver;

        public DeprecatedDuplicateBuilder(
            GeneratorExecutionContext context,
            GenerationOptions options,
            INamedTypeSymbol generateAsyncOverloadAttributeAttributeSymbol,
            INamedTypeSymbol duplicateAsyncEnumerableAsAsyncEnumerableDeprecatedAttributeSymbol,
            SyntaxReceiver syntaxReceiver)
        {
            _context = context;
            _options = options;
            _generateAsyncOverloadAttributeAttributeSymbol = generateAsyncOverloadAttributeAttributeSymbol;
            _attributeSymbol = duplicateAsyncEnumerableAsAsyncEnumerableDeprecatedAttributeSymbol;
            _syntaxReceiver = syntaxReceiver;
        }

        internal void BuildDuplicatesIfRequired()
        {
            if (!_syntaxReceiver.CandidateGenerateDeprecatedDuplicatesAttributes.Any(a =>
            {
                var sm = _context.Compilation.GetSemanticModel(a.SyntaxTree);
                var am = sm.GetSymbolInfo(a.Name).Symbol?.ContainingType;

                return SymbolEqualityComparer.Default.Equals(am, _attributeSymbol);
            }))
            {
                // The assembly does not have the attribute, so we mustn't run.
                return;
            }

            foreach (var classDeclaration in _syntaxReceiver.CandidateAsyncEnumerableClasses)
            {
                var sm = _context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                var classSymbol = sm.GetDeclaredSymbol(classDeclaration);
                if (classSymbol == null || classSymbol.DeclaredAccessibility != Accessibility.Public)
                {
                    continue;
                }

                // We aren't a general purpose generator, so we only handle the case where the class is nested in a namespace.
                if (classDeclaration.Parent is not NamespaceDeclarationSyntax nsDecl)
                {
                    _context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                        "IXNETG001",
                        "AsyncEnumerable class must be in a namespace declaration",
                        "AsyncEnumerable class must be declared within a namespace to generate deprecated duplicates",
                        "Usage",
                        DiagnosticSeverity.Warning,
                        isEnabledByDefault: true),
                        classDeclaration.Identifier.GetLocation()));

                    continue;
                }

                if (nsDecl.Parent is not CompilationUnitSyntax file)
                {     _context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                        "IXNETG002",
                        "Namespace must be in compilation unit",
                        "Namespace containing AsyncEnumerable class must be declared within a compilation unit to generate deprecated duplicates",
                        "Usage",
                        DiagnosticSeverity.Warning,
                        isEnabledByDefault: true),
                        nsDecl.Name.GetLocation()));
                    continue;
                }

                var facadeClass = SyntaxFactory.ClassDeclaration("AsyncEnumerableDeprecated")
                    .WithModifiers(SyntaxFactory.TokenList(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.StaticKeyword),
                        SyntaxFactory.Token(SyntaxKind.PartialKeyword)));
                bool atLeastOneMethod = false;
                foreach (var method in classDeclaration.Members.OfType<MethodDeclarationSyntax>())
                {
                    var facadeMethod = method;
                    string? methodName = null;
                    if (facadeMethod.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)))
                    {
                        // We're only generating duplicates for obsolete methods. (The non-obsolete
                        // methods have been removed from AsyncEnumerable's public API entirely,
                        // because either they are now available in .NET 10's System.Linq.AsyncEnumerable
                        // or we've moved them to System.Interactive.Async.)
                        if (!facadeMethod.AttributeLists.SelectMany(a => a.Attributes)
                            .Any(a =>
                            {
                                var asym = sm.GetSymbolInfo(a.Name);
                                return asym.Symbol?.Name == "ObsoleteAttribute" && asym.Symbol.ContainingNamespace?.Name == "System.Runtime.InteropServices";
                            }))
                        {
                            continue;
                        }

                        methodName = facadeMethod.Identifier.Text;
                    }
                    else
                    {
                        // We also need to emit the facades corresponding to the public methods generated with [GenerateAsyncOverload].
                        // (Since this code runs as part of the same generator that expands those, we don't get to see the expanded versions
                        // as input, so we end up slightly duplicating a little of the logic here.)
                        if (facadeMethod.AttributeLists.SelectMany(a => a.Attributes).Any(a =>
                            SymbolEqualityComparer.Default.Equals(sm.GetSymbolInfo(a.Name).Symbol?.ContainingType, _generateAsyncOverloadAttributeAttributeSymbol)))
                        {
                            var originalMethodSymbol = sm.GetDeclaredSymbol(method)!;
                            methodName = AsyncOverloadsGenerator.GetMethodNameForGeneratedAsyncMethod(originalMethodSymbol, _options);

                            facadeMethod = facadeMethod.WithIdentifier(SyntaxFactory.Identifier(methodName));
                        }
                    }

                    if (methodName is not null)
                    {
                        // Strip off 'this' from first parameter if it's an extension method.
                        if (facadeMethod.ParameterList.Parameters.Count > 0)
                        {
                            var firstParam = facadeMethod.ParameterList.Parameters[0];
                            if (firstParam.Modifiers.Any(SyntaxKind.ThisKeyword))
                            {
                                var newFirstParam = firstParam.WithModifiers(SyntaxFactory.TokenList());
                                var newParamList = facadeMethod.ParameterList.WithParameters(
                                    facadeMethod.ParameterList.Parameters.Replace(firstParam, newFirstParam));
                                facadeMethod = facadeMethod.WithParameterList(newParamList);
                            }
                        }

                        // We don't want to duplicate the implementation. We just generate a simple
                        // facade that defers to the original method.
                        var invokeOriginalMethod = SyntaxFactory.InvocationExpression(
                            SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName("AsyncEnumerable"), SyntaxFactory.IdentifierName(methodName)),
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    facadeMethod.ParameterList.Parameters.Select(p =>
                                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName(p.Identifier))))));
                        facadeMethod = facadeMethod
                            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword)))
                            .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>())
                            .WithBody(null)
                            .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(invokeOriginalMethod))
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                            .NormalizeWhitespace();
                        facadeClass = facadeClass.AddMembers(facadeMethod);

                        atLeastOneMethod = true;
                    }
                }

                if (atLeastOneMethod)
                {
                    var namespaceWithClassDup = nsDecl.WithMembers(SyntaxFactory.SingletonList<MemberDeclarationSyntax>(facadeClass));
                    var fileWithDup = file
                        .WithLeadingTrivia(file.GetLeadingTrivia().Add(SyntaxFactory.Trivia(SyntaxFactory.NullableDirectiveTrivia(SyntaxFactory.Token(SyntaxKind.EnableKeyword), true))))
                        .WithMembers(SyntaxFactory.SingletonList<MemberDeclarationSyntax>(namespaceWithClassDup));

                    var source = fileWithDup
                        .NormalizeWhitespace()
                        .ToFullString();
                    if (!string.IsNullOrEmpty(source))
                    {
                        string existingFileName = System.IO.Path.GetFileNameWithoutExtension(classDeclaration.SyntaxTree.FilePath);
                        _context.AddSource($"{existingFileName}.DeprecatedDuplicates.g.cs", source);
                    }

                }
            }
        }
    }
}

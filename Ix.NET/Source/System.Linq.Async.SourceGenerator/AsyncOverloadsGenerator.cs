using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace System.Linq.Async.SourceGenerator
{
    [Generator]
    public sealed class AsyncOverloadsGenerator : ISourceGenerator
    {
        private const string AttributeSource =
            "using System;\n" +
            "using System.Diagnostics;\n" +
            "namespace System.Linq\n" +
            "{\n" +
            "    [AttributeUsage(AttributeTargets.Method)]\n" +
            "    [Conditional(\"COMPILE_TIME_ONLY\")]\n" +
            "    internal sealed class GenerateAsyncOverloadAttribute : Attribute { }\n" +
            "}\n";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
            context.RegisterForPostInitialization(c => c.AddSource("GenerateAsyncOverloadAttribute", AttributeSource));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not SyntaxReceiver syntaxReceiver) return;

            var options = GetGenerationOptions(context);
            var attributeSymbol = GetAsyncOverloadAttributeSymbol(context);
            var methodsBySyntaxTree = GetMethodsGroupedBySyntaxTree(context, syntaxReceiver);

            foreach (var grouping in methodsBySyntaxTree)
                context.AddSource(
                    $"{Path.GetFileNameWithoutExtension(grouping.SyntaxTree.FilePath)}.AsyncOverloads",
                    GenerateOverloads(grouping, options, context, attributeSymbol));
        }

        private static GenerationOptions GetGenerationOptions(GeneratorExecutionContext context)
            => new(SupportFlatAsyncApi: context.ParseOptions.PreprocessorSymbolNames.Contains("SUPPORT_FLAT_ASYNC_API"));

        private static IEnumerable<AsyncMethodGrouping> GetMethodsGroupedBySyntaxTree(GeneratorExecutionContext context, SyntaxReceiver syntaxReceiver)
            => GetMethodsGroupedBySyntaxTree(
                context,
                syntaxReceiver,
                GetAsyncOverloadAttributeSymbol(context));

        private static string GenerateOverloads(AsyncMethodGrouping grouping, GenerationOptions options, GeneratorExecutionContext context, INamedTypeSymbol attributeSymbol)
        {
            var usings = grouping.SyntaxTree.GetRoot() is CompilationUnitSyntax compilationUnit
                ? compilationUnit.Usings.ToString()
                : string.Empty;

            // This source generator gets used not just in System.Linq.Async, but also for code that has migrated from
            // System.Linq.Async to System.Interactive.Async. (E.g., we define overloads of AverageAsync that accept
            // selector callbacks. The .NET runtime library implementation offers no equivalents. We want to continue
            // to offer these even though we're decprecating System.Linq.Async, so they migrate into
            // System.Interactive.Async.) In those cases, the containing type is typically AsyncEnumerableEx,
            // but in System.Linq.Async it is AsyncEnumerable. So we need to discover the containing type name.
            var containingTypeName = grouping.Methods.First().Symbol.ContainingType.Name;

            var overloads = new StringBuilder();
            overloads.AppendLine("#nullable enable");
            overloads.AppendLine(usings);
            overloads.AppendLine("namespace System.Linq");
            overloads.AppendLine("{");
            overloads.AppendLine($"    partial class {containingTypeName}");
            overloads.AppendLine("    {");

            foreach (var method in grouping.Methods)
            {
                var model = context.Compilation.GetSemanticModel(method.Syntax.SyntaxTree);
                overloads.AppendLine(GenerateOverload(method, options, model, attributeSymbol));
            }

            overloads.AppendLine("    }");
            overloads.AppendLine("}");

            return overloads.ToString();
        }

        //var allLeadingTrivia = method.Syntax.GetLeadingTrivia();
        //var allLeadingTriviaStructure = method.Syntax.GetLeadingTrivia().Select(t => t.GetStructure());
        //var leadingTrivia = method.Syntax.GetLeadingTrivia().Where(t => !t.IsKind(SyntaxKind.DisabledTextTrivia) && t.GetStructure() is not DirectiveTriviaSyntax);

        private static string GenerateOverload(AsyncMethod method, GenerationOptions options, SemanticModel model, INamedTypeSymbol attributeSymbol)
        {
            var attributeListsWithGenerateAsyncOverloadRemoved = SyntaxFactory.List(method.Syntax.AttributeLists
                .Select(list => AttributeList(SeparatedList(
                        (from a in list.Attributes
                         let am = model.GetSymbolInfo(a.Name).Symbol?.ContainingType
                         where !SymbolEqualityComparer.Default.Equals(am, attributeSymbol)
                         select a))))
                .Where(list => list.Attributes.Count > 0));

            return MethodDeclaration(method.Syntax.ReturnType, GetMethodName(method.Symbol, options))
                .WithAttributeLists(attributeListsWithGenerateAsyncOverloadRemoved)
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
                .WithTypeParameterList(method.Syntax.TypeParameterList)
                .WithParameterList(method.Syntax.ParameterList)
                .WithConstraintClauses(method.Syntax.ConstraintClauses)
                .WithExpressionBody(ArrowExpressionClause(
                    InvocationExpression(
                        IdentifierName(method.Symbol.Name),
                        ArgumentList(
                            SeparatedList(
                                method.Syntax.ParameterList.Parameters
                                    .Select(p => Argument(IdentifierName(p.Identifier))))))))
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                .WithLeadingTrivia(method.Syntax.GetLeadingTrivia().Where(t => !t.IsKind(SyntaxKind.DisabledTextTrivia) && t.GetStructure() is not DirectiveTriviaSyntax))
                .NormalizeWhitespace()
                .ToFullString();
        }

        private static INamedTypeSymbol GetAsyncOverloadAttributeSymbol(GeneratorExecutionContext context)
            => context.Compilation.GetTypeByMetadataName("System.Linq.GenerateAsyncOverloadAttribute") ?? throw new InvalidOperationException();

        private static IEnumerable<AsyncMethodGrouping> GetMethodsGroupedBySyntaxTree(GeneratorExecutionContext context, SyntaxReceiver syntaxReceiver, INamedTypeSymbol attributeSymbol)
            => from candidate in syntaxReceiver.Candidates
               group candidate by candidate.SyntaxTree into grouping
               let model = context.Compilation.GetSemanticModel(grouping.Key)
               select new AsyncMethodGrouping(
                  grouping.Key,
                  from methodSyntax in grouping
                  let methodSymbol = model.GetDeclaredSymbol(methodSyntax) ?? throw new InvalidOperationException()
                  where methodSymbol.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass!, attributeSymbol))
                  select new AsyncMethod(methodSymbol, methodSyntax));

        private static string GetMethodName(IMethodSymbol methodSymbol, GenerationOptions options)
        {
            var methodName = methodSymbol.Name.Replace("Core", "");
            return options.SupportFlatAsyncApi
                ? methodName.Replace("Await", "").Replace("WithCancellation", "")
                : methodName;
        }
    }
}

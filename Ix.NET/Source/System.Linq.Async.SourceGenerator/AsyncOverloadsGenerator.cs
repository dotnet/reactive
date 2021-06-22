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
            "    public sealed class GenerateAsyncOverloadAttribute : Attribute { }\n" +
            "}\n";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
            context.RegisterForPostInitialization(c => c.AddSource("Attribute.cs", AttributeSource));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not SyntaxReceiver syntaxReceiver) return;

            var supportFlatAsyncApi = context.ParseOptions.PreprocessorSymbolNames.Contains("SUPPORT_FLAT_ASYNC_API");
            var attributeSymbol = context.Compilation.GetTypeByMetadataName("System.Linq.GenerateAsyncOverloadAttribute");

            foreach (var grouping in syntaxReceiver.Candidates.GroupBy(c => c.SyntaxTree))
            {
                var model = context.Compilation.GetSemanticModel(grouping.Key);
                var methodsBuilder = new StringBuilder();

                foreach (var candidate in grouping)
                {
                    var methodSymbol = model.GetDeclaredSymbol(candidate) ?? throw new NullReferenceException();

                    if (!methodSymbol.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass!, attributeSymbol))) continue;

                    var shortName = methodSymbol.Name.Replace("Core", "");
                    if (supportFlatAsyncApi)
                    {
                        shortName = shortName.Replace("Await", "").Replace("WithCancellation", "");
                    }

                    var publicMethod = MethodDeclaration(candidate.ReturnType, shortName)
                        .WithModifiers(TokenList(Token(TriviaList(), SyntaxKind.PublicKeyword, TriviaList(Space)), Token(TriviaList(), SyntaxKind.StaticKeyword, TriviaList(Space))))
                        .WithTypeParameterList(candidate.TypeParameterList)
                        .WithParameterList(candidate.ParameterList)
                        .WithConstraintClauses(candidate.ConstraintClauses)
                        .WithExpressionBody(ArrowExpressionClause(InvocationExpression(IdentifierName(methodSymbol.Name), ArgumentList(SeparatedList(candidate.ParameterList.Parameters.Select(p => Argument(IdentifierName(p.Identifier))))))))
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                        .WithLeadingTrivia(candidate.GetLeadingTrivia().Where(t => t.GetStructure() is not DirectiveTriviaSyntax));

                    methodsBuilder.AppendLine(publicMethod.ToFullString());
                }

                if (methodsBuilder.Length == 0) continue;

                var usings = grouping.Key.GetRoot() is CompilationUnitSyntax compilationUnit
                    ? compilationUnit.Usings
                    : List<UsingDirectiveSyntax>();

                var overloads = new StringBuilder();
                overloads.AppendLine("#nullable enable");
                overloads.AppendLine(usings.ToString());
                overloads.AppendLine("namespace System.Linq");
                overloads.AppendLine("{");
                overloads.AppendLine("    partial class AsyncEnumerable");
                overloads.AppendLine("    {");
                overloads.AppendLine(methodsBuilder.ToString());
                overloads.AppendLine("    }");
                overloads.AppendLine("}");

                context.AddSource($"{Path.GetFileNameWithoutExtension(grouping.Key.FilePath)}.AsyncOverloads.cs", overloads.ToString());
            }
        }

        private sealed class SyntaxReceiver : ISyntaxReceiver
        {
            public IList<MethodDeclarationSyntax> Candidates { get; } = new List<MethodDeclarationSyntax>();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is MethodDeclarationSyntax { AttributeLists: { Count: >0 } } methodDeclarationSyntax)
                {
                    Candidates.Add(methodDeclarationSyntax);
                }
            }
        }
    }
}

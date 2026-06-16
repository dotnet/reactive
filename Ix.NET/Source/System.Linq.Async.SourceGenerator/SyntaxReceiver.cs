using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Linq.Async.SourceGenerator
{
    internal sealed class SyntaxReceiver : ISyntaxReceiver
    {
        public List<MethodDeclarationSyntax> CandidateMethods { get; } = new();
        public List<ClassDeclarationSyntax> CandidateAsyncEnumerableClasses { get; } = new();
        public List<AttributeSyntax> CandidateGenerateDeprecatedDuplicatesAttributes { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is MethodDeclarationSyntax { AttributeLists: { Count: >0 } } methodDeclarationSyntax)
            {
                CandidateMethods.Add(methodDeclarationSyntax);
            }
            else if (syntaxNode is ClassDeclarationSyntax { Identifier.Text: "AsyncEnumerable" }  classDeclarationSyntax)
            {
                CandidateAsyncEnumerableClasses.Add(classDeclarationSyntax);
            }
            else if (syntaxNode is AttributeSyntax generateDeprecatedAttribute &&
                (generateDeprecatedAttribute.Parent is AttributeListSyntax { Target.Identifier.Text: "assembly" }) &&
                (generateDeprecatedAttribute.Name is IdentifierNameSyntax { Identifier.Text: "DuplicateAsyncEnumerableAsAsyncEnumerableDeprecated" } ||
                 generateDeprecatedAttribute.Name is QualifiedNameSyntax { Right: IdentifierNameSyntax { Identifier.Text: "DuplicateAsyncEnumerableAsAsyncEnumerableDeprecated" } }))
            {
                CandidateGenerateDeprecatedDuplicatesAttributes.Add(generateDeprecatedAttribute);
            }
        }
    }
}

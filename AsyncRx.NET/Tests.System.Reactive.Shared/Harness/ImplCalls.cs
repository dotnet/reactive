// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Concurrent;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyModel;

namespace Tests.System.Reactive.Shared;

/// <summary>Finds the library method an <c>*Impl</c> method calls, by reading its source.</summary>
/// <remarks>
/// <para>
/// A target project imports <c>CaptureCompilation.targets</c>, which preserves its
/// compilation context (<c>PreserveCompilationContext</c>: the references and the options its
/// compiler was given, read back through <see cref="DependencyContext"/>) and writes the one
/// input that does not record, the source list, beside the assembly.
/// <see cref="Resolve"/> rebuilds the compilation from the two with Roslyn, binds the
/// <c>*Impl</c>'s body, and takes the outermost call whose target is defined in metadata rather
/// than in the test's own source; that is the library call. The symbol comes back as a
/// <see cref="MethodInfo"/> through its documentation-comment id, resolved against the assemblies
/// loaded in this process (the compilation itself binds against reference assemblies, whose
/// metadata tokens are not the runtime's).
/// </para>
/// <para>
/// One compilation per assembly, built on first use and kept.
/// </para>
/// </remarks>
public static class ImplCalls
{
    private static readonly ConcurrentDictionary<Assembly, Lazy<CSharpCompilation>> Compilations =
        new();

    private static readonly Lazy<CSharpCompilation> Runtime = new(() =>
        CSharpCompilation.Create(
            "runtime",
            references: AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
                .Select(a => MetadataReference.CreateFromFile(a.Location))));

    /// <summary>
    /// Returns the library method definition that <paramref name="impl"/> calls, or null if it
    /// calls none.
    /// </summary>
    /// <param name="impl">A target's <c>*Impl</c> method.</param>
    /// <remarks>
    /// For a generic library method, the open definition (as <c>Observable.Sum&lt;TSource&gt;</c>,
    /// not an instantiation); for an extension method, the static method itself. Null for an
    /// <c>*Impl</c> that reads a property or builds an object and invokes nothing from outside
    /// the test's own source.
    /// </remarks>
    public static MethodInfo? Resolve(MethodInfo impl)
    {
        ArgumentNullException.ThrowIfNull(impl);

        var declaringType = impl.DeclaringType
            ?? throw new ArgumentException("No declaring type.", nameof(impl));
        var compilation = Compilations
            .GetOrAdd(declaringType.Assembly, a => new Lazy<CSharpCompilation>(() => Compile(a)))
            .Value;

        var type = compilation.GetTypeByMetadataName(declaringType.FullName!)
            ?? throw new InvalidOperationException($"{declaringType} is not in the compilation.");

        var candidates = type.GetMembers(impl.Name)
            .OfType<IMethodSymbol>()
            .Where(m =>
                m.Arity == impl.GetGenericArguments().Length
                && m.Parameters.Length == impl.GetParameters().Length)
            .ToList();

        if (candidates.Count != 1)
        {
            throw new InvalidOperationException(
                $"{declaringType.Name}.{impl.Name}: {candidates.Count} source methods match its " +
                "arity and parameter count.");
        }

        var syntax =
            (MethodDeclarationSyntax)candidates[0].DeclaringSyntaxReferences.Single().GetSyntax();
        var model = compilation.GetSemanticModel(syntax.SyntaxTree);

        // Document order puts the outermost invocation first.
        var call = syntax.DescendantNodes()
            .OfType<InvocationExpressionSyntax>()
            .Select(invocation => model.GetSymbolInfo(invocation).Symbol as IMethodSymbol)
            .FirstOrDefault(m => m is not null && m.Locations.All(l => l.IsInMetadata));

        return call is null ? null : ToMethodInfo((call.ReducedFrom ?? call).OriginalDefinition);
    }

    private static MethodInfo ToMethodInfo(IMethodSymbol symbol)
    {
        var id = symbol.GetDocumentationCommentId()
            ?? throw new InvalidOperationException($"{symbol} has no documentation id.");

        var runtimeSymbol = DocumentationCommentId.GetFirstSymbolForDeclarationId(id, Runtime.Value)
            ?? throw new InvalidOperationException($"{id} is not in a loaded assembly.");

        var assembly = Assembly.Load(runtimeSymbol.ContainingAssembly.Name);

        return (MethodInfo)assembly.ManifestModule.ResolveMethod(runtimeSymbol.MetadataToken)!;
    }

    private static CSharpCompilation Compile(Assembly assembly)
    {
        var context = DependencyContext.Load(assembly)
            ?? throw new InvalidOperationException($"{assembly.GetName().Name} has no deps.json.");
        var options = context.CompilationOptions;

        var languageVersion = LanguageVersionFacts.TryParse(options.LanguageVersion, out var v)
            ? v
            : LanguageVersion.Default;
        var parseOptions = new CSharpParseOptions(
            languageVersion,
            preprocessorSymbols: options.Defines.OfType<string>());

        var sources = Path.Combine(
            Path.GetDirectoryName(assembly.Location)!,
            assembly.GetName().Name + ".sources.txt");
        var trees = File.ReadAllLines(sources).Select(path =>
            CSharpSyntaxTree.ParseText(File.ReadAllText(path), parseOptions, path));

        var references = context.CompileLibraries
            .SelectMany(library => library.ResolveReferencePaths())
            .Select(path => MetadataReference.CreateFromFile(path));

        var compilation = CSharpCompilation.Create(
            assembly.GetName().Name,
            trees,
            references,
            new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                allowUnsafe: options.AllowUnsafe ?? false,
                nullableContextOptions: NullableContextOptions.Enable));

        var errors = compilation.GetDiagnostics()
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ToList();
        if (errors.Count > 0)
        {
            throw new InvalidOperationException(
                $"The compilation of {assembly.GetName().Name} has {errors.Count} error(s):" +
                Environment.NewLine +
                string.Join(Environment.NewLine, errors.Take(10)));
        }

        return compilation;
    }
}

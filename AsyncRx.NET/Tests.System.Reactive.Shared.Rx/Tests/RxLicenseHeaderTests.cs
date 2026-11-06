// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 


namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// Rx.NET's own license-header scan, from its <c>LicenseHeaderTest.cs</c>, over the AsyncRx.NET
/// tree.
/// </summary>
/// <remarks>
/// The file's one test, pointed at the <c>AsyncRx.NET</c> source tree this suite lives in rather
/// than <c>Rx.NET/Source</c>, with the same exclusions (build output, generated and designer
/// files) and without the original's option of rewriting the files it finds: a test reports, it
/// does not edit sources. Every <c>.cs</c> file under the tree must start with the .NET Foundation
/// header, which is the rule this suite's own files follow.
/// </remarks>
[TestClass]
public sealed class RxLicenseHeaderTests
{
    private static readonly string FirstLine =
        "// Licensed to the .NET Foundation under one or more agreements.";

    [TestMethod]
    public void ScanFiles()
    {
        var dir = Directory.GetCurrentDirectory();
        var idx = dir.LastIndexOf("AsyncRx.NET", StringComparison.Ordinal);
        Assert.IsFalse(idx < 0, $"Could not locate sources directory: {dir}");

        var root = Path.Combine(dir.Substring(0, idx), "AsyncRx.NET");

        var missing = new List<string>();
        foreach (var file in Directory.EnumerateFiles(root, "*.cs", SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(root, file).Replace('\\', '/');
            if (relative.Contains("/obj/")
                || relative.Contains("/bin/")
                || relative.EndsWith("AssemblyInfo.cs", StringComparison.Ordinal)
                || relative.EndsWith(".Designer.cs", StringComparison.Ordinal)
                || relative.EndsWith(".Generated.cs", StringComparison.Ordinal))
            {
                continue;
            }

            var content = File.ReadAllText(file);
            if (!content.StartsWith(FirstLine, StringComparison.Ordinal))
            {
                missing.Add(relative);
            }
        }

        Assert.IsEmpty(
            missing,
            $"Files with no license header: {missing.Count}{Environment.NewLine}" +
            string.Join(Environment.NewLine, missing));
    }
}

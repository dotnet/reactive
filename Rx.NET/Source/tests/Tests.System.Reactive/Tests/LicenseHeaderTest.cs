// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Text;
using Xunit;

namespace Tests.System.Reactive.Tests
{
    /// <summary>
    /// Verify that main classes and unit tests have a license header
    /// in the source files.
    /// </summary>
    public class LicenseHeaderTest
    {
        private static readonly bool FixHeaders = true;
        private static readonly string[] Lines = {
            "// Licensed to the .NET Foundation under one or more agreements.",
            "// The .NET Foundation licenses this file to you under the Apache 2.0 License.",
            "// See the LICENSE file in the project root for more information.",
            ""
        };

        [Fact]
        public void ScanFiles()
        {
            var dir = Directory.GetCurrentDirectory();
            var idx = dir.LastIndexOf("Rx.NET");

            Assert.False(idx < 0, $"Could not locate sources directory: {dir}");

            var newDir = Path.Combine(dir.Substring(0, idx), "Rx.NET", "Source");

            var error = new StringBuilder();

            var count = ScanPath(newDir, error);

            if (error.Length != 0)
            {
                Assert.False(true, $"Files with no license header: {count}\r\n{error.ToString()}");
            }
        }

        private int ScanPath(string path, StringBuilder error)
        {
            var count = 0;
            foreach (var file in Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories))
            {
                // exclusions
                if (file.Contains("/obj/")
                    || file.Contains(@"\obj\")
                    || file.Contains("AssemblyInfo.cs")
                    || file.Contains(".Designer.cs")
                    || file.Contains(".Generated.cs")
                    || file.Contains(".approved.cs")
                    || file.Contains("Uwp.DeviceRunner")
                )
                {
                    continue;
                }

                // analysis
                var content = File.ReadAllText(file);

                if (!content.StartsWith(Lines[0]))
                {
                    count++;
                    error.Append(file).Append("\r\n");

                    if (FixHeaders)
                    {
                        var newContent = new StringBuilder();
                        var separator = content.Contains("\r\n") ? "\r\n" : "\n";

                        foreach (var s in Lines)
                        {
                            newContent.Append(s).Append(separator);
                        }
                        newContent.Append(content);

                        File.WriteAllText(file, newContent.ToString());
                    }
                }
            }
            return count;
        }
    }
}

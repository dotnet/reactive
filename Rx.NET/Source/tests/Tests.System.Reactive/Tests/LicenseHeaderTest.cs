// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.System.Reactive.Tests
{
    /// <summary>
    /// Verify that main classes and unit tests have a license header
    /// in the source files.
    /// </summary>
    public class LicenseHeaderTest
    {
        static readonly bool fixHeaders = true;

        static readonly string[] lines = {
            "// Licensed to the .NET Foundation under one or more agreements.",
            "// The .NET Foundation licenses this file to you under the Apache 2.0 License.",
            "// See the LICENSE file in the project root for more information.",
            ""
        };

        [Fact]
        public void ScanFiles()
        {
            var dir = Directory.GetCurrentDirectory();
            var idx = dir.IndexOf("Rx.NET");
            if (idx < 0)
            {
                Console.WriteLine($"Could not locate sources directory: {dir}");
            }
            else
            {
                var newDir = dir.Substring(0, idx) + "Rx.NET/Source";

                var error = new StringBuilder();

                var count = ScanPath(newDir, error);

                if (error.Length != 0)
                {

                    Assert.False(true, $"Files with no license header: {count}\r\n{error.ToString()}");
                }
            }
        }

        int ScanPath(string path, StringBuilder error)
        {
            var count = 0;
            foreach (var file in Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories))
            {
                // exclusions
                if (file.Contains("obj/Debug") 
                    || file.Contains(@"obj\Debug")
                    || file.Contains("AssemblyInfo.cs")
                    || file.Contains(".Designer.cs")
                    || file.Contains(".Generated.cs")
                    || file.Contains("Uwp.DeviceRunner")
                    || file.Contains(@"obj\Release")
                    || file.Contains("obj/Release")
                )
                {
                    continue;
                }

                // analysis
                string content = File.ReadAllText(file);

                if (!content.StartsWith(lines[0]))
                {
                    count++;
                    error.Append(file).Append("\r\n");

                    if (fixHeaders)
                    {
                        StringBuilder newContent = new StringBuilder();
                        string separator = content.Contains("\r\n") ? "\r\n" : "\n";

                        foreach (var s in lines)
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

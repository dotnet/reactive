// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace ReactiveTests.Tests
{
    public class DiffPlexReporter
    {
        public void Report(string approvedText, string receivedText)
        {
            #if(!DEBUG)
            var diffBuilder = new InlineDiffBuilder(new Differ());
            var diff = diffBuilder.BuildDiffModel(approvedText, receivedText);

            foreach (var line in diff.Lines)
            {
                if (line.Type == ChangeType.Unchanged) continue;

                var prefix = "  ";
                switch (line.Type)
                {
                    case ChangeType.Inserted:
                        prefix = "+ ";
                        break;
                    case ChangeType.Deleted:
                        prefix = "- ";
                        break;
                }

                Console.WriteLine("{0}{1}", prefix, line.Text);
            }
            #endif
        }
    }
}

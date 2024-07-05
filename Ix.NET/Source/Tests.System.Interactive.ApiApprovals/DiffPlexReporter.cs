// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Tests.System.Interactive.ApiApprovals;

internal static class DiffPlexReporter
{
    public static Task Report(string receivedFile, string verifiedFile, string? message)
    {
#if (!DEBUG)
        var receivedText = File.ReadAllText(receivedFile);
        var verifiedText = File.ReadAllText(verifiedFile);
        var diffBuilder = new InlineDiffBuilder(new Differ());
        var diff = diffBuilder.BuildDiffModel(verifiedText, receivedText);

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

        return Task.CompletedTask;
    }
}

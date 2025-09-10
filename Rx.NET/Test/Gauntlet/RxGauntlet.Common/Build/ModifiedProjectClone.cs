using System.Diagnostics;

namespace RxGauntlet.Build;

public sealed class ModifiedProjectClone : IDisposable
{
    private readonly string copyPath;

    private ModifiedProjectClone(string copyPath)
    {
        this.copyPath = copyPath;
    }

    public string ClonedProjectFolderPath => copyPath;

    public static ModifiedProjectClone Create(
        string sourceProjectFolder,
        string copyParentFolderName,
        Action<ProjectFileRewriter> modifyProjectFile,
        (string FeedName, string FeedLocation)[]? additionalPackageSources)
    {
        string copyPath = Path.Combine(
            Path.GetTempPath(),
            "RxGauntlet",
            copyParentFolderName,
            DateTime.Now.ToString("yyyyMMdd-HHmmss"));

        Directory.CreateDirectory(copyPath);

        ModifiedProjectClone? clone = new(copyPath);
        try
        {
            foreach (string file in Directory.GetFiles(sourceProjectFolder))
            {
                string extension = Path.GetExtension(file).ToLowerInvariant();
                string relativePath = Path.GetRelativePath(sourceProjectFolder, file);
                string destinationPath = Path.Combine(copyPath, relativePath);

                switch (extension)
                {
                    case ".cs":
                        File.Copy(file, destinationPath, true);
                        break;

                    case ".csproj":
                        ProjectFileRewriter projectFileRewriter = ProjectFileRewriter.CreateForCsProj(file);
                        modifyProjectFile(projectFileRewriter);
                        projectFileRewriter.WriteModified(destinationPath);
                        break;
                }
            }

            if (additionalPackageSources is not null && additionalPackageSources.Length > 0)
            {
                // We need to emit a NuGet.config file, because the arguments specified one or more custom package sources
                string sources = string.Join(Environment.NewLine, additionalPackageSources.Select(
                    p => $"""    <add key="{p.FeedName}" value="{p.FeedLocation}" />"""));
                string nuGetConfigContent = $"""
                            <?xml version="1.0" encoding="utf-8"?>
                            <configuration>
                              <packageSources>
                                <clear />
                                <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
                            {sources}
                              </packageSources>
                            </configuration>
                            """;

                File.WriteAllText(
                    Path.Combine(copyPath, "NuGet.config"),
                    nuGetConfigContent);
            }

            // We're now going to return without error, so we no longer want the finally block
            // to delete the directory. That will now happen when the caller calls Dispose on
            // the ModifiedProjectClone we return..
            ModifiedProjectClone result = clone;
            clone = null;
            return result; 
        }
        finally
        {
            if (clone is not null)
            {
                // If we reach here, it means an error occurred during the copy process
                // and we need to clean up the directory we created.
                if (Directory.Exists(copyPath))
                {
                    Directory.Delete(copyPath, true);
                }
            }
        }
    }

    public void Dispose()
    {
        if (Directory.Exists(copyPath))
        {
            Directory.Delete(copyPath, true);
        }
    }

    public async Task<BuildOutput> RunDotnetBuild(string csProjName)
    {
        return await RunDotnetCommonBuild("build", csProjName);
    }

    public async Task<BuildOutput> RunDotnetPack(string csProjName)
    {
        return await RunDotnetCommonBuild("pack", csProjName);
    }

    public async Task<BuildOutput> RunDotnetPublish(string csProjName)
    {
        return await RunDotnetCommonBuild("publish", csProjName);
    }

    private async Task<BuildOutput> RunDotnetCommonBuild(string command, string csProjName)
    {
        string args = $"{command} -c Release {csProjName}";
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            UseShellExecute = false,
            RedirectStandardOutput = true,

            // Comment this out to see the output in the console window
            //CreateNoWindow = true,
            Arguments = args,
            WorkingDirectory = copyPath,
        };

        using var process = new Process { StartInfo = startInfo };
        process.Start();
        Task<string> stdOutTask = Task.Run(process.StandardOutput.ReadToEndAsync);
        Task processTask = process.WaitForExitAsync();
        Task firstToFinish = await Task.WhenAny(processTask, stdOutTask);

        if (!stdOutTask.IsCompleted)
        {
            // The process finished, but the standard output task is still running. It's possible that
            // it is nearly done, so give it some time.
            await Task.WhenAny(stdOutTask, Task.Delay(2000));
        }

        if (!stdOutTask.IsCompleted)
        {
            throw new InvalidOperationException("Did not get output from program");
        }
        string stdOut = await stdOutTask;

        await processTask;
        string outputFolder = Path.Combine(ClonedProjectFolderPath, "bin", "Release");
        return new BuildOutput(process.ExitCode, outputFolder, stdOut);
    }
}

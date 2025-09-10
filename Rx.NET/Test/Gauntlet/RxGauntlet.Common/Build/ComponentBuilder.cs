namespace RxGauntlet.Build;

public sealed class ComponentBuilder(string appBuildTempFolderName) : IDisposable
{
    private const string PackageTempFolderName = "PackageBuild";
    private const string LocalNuGetSourcePackageTempFolderName = "LocalNuGet";
    private readonly List<ModifiedProjectClone> _projectClones = [];

    public string LocalNuGetPackageFolderPath { get; } =
        Path.Combine(Path.GetTempPath(), "RxGauntlet", LocalNuGetSourcePackageTempFolderName, DateTime.Now.ToString("yyyyMMdd-HHmmss"));

    public async Task<BuildOutput> BuildLocalNuGetPackageAsync(
        string templateCsProj,
        Action<ProjectFileRewriter> modifyProjectFile,
        (string FeedName, string FeedLocation)[]? additionalPackageSources)
    {
        (string projectTemplateFileName, ModifiedProjectClone projectClone) = CreateModifiedProjectClone(
            PackageTempFolderName, templateCsProj, modifyProjectFile, additionalPackageSources);

        BuildOutput packResults = await projectClone.RunDotnetPack(projectTemplateFileName);

        if (!Directory.Exists(LocalNuGetPackageFolderPath))
        {
            Directory.CreateDirectory(LocalNuGetPackageFolderPath);
        }

        string nupkgPath = Directory.GetFiles(
            packResults.OutputFolder,
            "*.nupkg",
            SearchOption.AllDirectories) switch
        {
            [] => throw new InvalidOperationException("No .nupkg file found after packing the project"),
            [string nupkgFile] => nupkgFile,
            _ => throw new InvalidOperationException("Multiple .nupkg files found after packing the project")
        };

        string destinationNupkgPath = Path.Combine(LocalNuGetPackageFolderPath, Path.GetFileName(nupkgPath));
        File.Copy(nupkgPath, destinationNupkgPath);

        return packResults;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="templateCsProj"></param>
    /// <param name="modifyProjectFile"></param>
    /// <param name="additionalPackageSources"></param>
    /// <returns>
    /// A task that produces the path to the <c>bin\Release</c> folder of the built application.
    /// </returns>
    public async Task<BuildOutput> BuildAppAsync(
        string templateCsProj,
        Action<ProjectFileRewriter> modifyProjectFile,
        (string FeedName, string FeedLocation)[]? additionalPackageSources)
    {
        (string FeedName, string FeedLocation)[]? packageSourcesIncludingDynamicallyBuiltPackages =
            [
                ("DynamicallyBuiltPackages", LocalNuGetPackageFolderPath),
                ..(additionalPackageSources ?? [])
            ];

        (string projectTemplateFileName, ModifiedProjectClone project) = CreateModifiedProjectClone(
            appBuildTempFolderName, templateCsProj, modifyProjectFile, packageSourcesIncludingDynamicallyBuiltPackages);

        return await project.RunDotnetBuild(projectTemplateFileName);
    }

    private (string ProjectTemplateFileName, ModifiedProjectClone ProjectClone) CreateModifiedProjectClone(
        string tempParentFolderName,
        string templateCsProj,
        Action<ProjectFileRewriter> modifyProjectFile,
        (string FeedName, string FeedLocation)[]? additionalPackageSources)
    {
        string projectTemplateFolder = Path.GetDirectoryName(templateCsProj)
            ?? throw new ArgumentException("Template csproj path should be absolute", nameof(templateCsProj));
        string projectTemplateFileName = Path.GetFileName(templateCsProj)
            ?? throw new ArgumentException("Template csproj path should refer to a file, not a directory", nameof(templateCsProj));
        var projectClone = ModifiedProjectClone.Create(
            projectTemplateFolder,
            tempParentFolderName,
            modifyProjectFile,
            additionalPackageSources);
        _projectClones.Add(projectClone);

        return (projectTemplateFileName, projectClone);
    }

    public void Dispose()
    {
        foreach (ModifiedProjectClone clone in _projectClones)
        {
            clone.Dispose();
        }

        _projectClones.Clear();

        if (Directory.Exists(LocalNuGetPackageFolderPath))
        {
            Directory.Delete(LocalNuGetPackageFolderPath, true);
        }
    }

    public async Task DeleteBuiltAppNowAsync(BuildAndRunOutput beforeBuildResult)
    {
        var p = _projectClones.Single(c => beforeBuildResult.OutputFolder.StartsWith(c.ClonedProjectFolderPath));
        if (Directory.Exists(p.ClonedProjectFolderPath))
        {
            await Task.Run(() => Directory.Delete(p.ClonedProjectFolderPath, true));
        }
    }
}

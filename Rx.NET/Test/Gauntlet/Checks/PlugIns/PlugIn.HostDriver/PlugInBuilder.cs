// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using RxGauntlet.Build;

namespace PlugIn.HostDriver;

/// <summary>
/// Builds variations of the test plug-in for different target frameworks and Rx versions.
/// </summary>
/// <remarks>
/// <para>
/// Each distict <see cref="PlugInDescriptor"/> passed to <see cref="GetPlugInDllPathAsync(PlugIn.HostDriver.PlugInDescriptor)"/>
/// will result in a separate project being built. The <see cref="PlugInBuilder"/> puts these in a temporary folder,
/// and deletes them when it is disposed.
/// </para>
/// </remarks>
public sealed class PlugInBuilder : IDisposable
{
    private const string PlugInTempFolderName = "PlugInHost";
    private static readonly string PlugInTemplateProjectFolder = 
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../PlugIns/PlugIn"));

    // Note: I don't think the default comparison behaviour of records handles array-typed
    // properties correctly. Two PlugInDescriptors where the values are all equal, but
    // the RxPackages arrays are different instances, will not be considered equal even
    // if all the elements are the same. (I think.) So it's possible we'll end up generating
    // the same project multiple times. This is a performance issue, not a correctness issue,
    // and I'm not sure we even hit it in practice, but if we do, we should implement a
    // custom equality comparer for PlugInDescriptor that compares the RxPackages arrays
    // element-by-element.
    private readonly Dictionary<PlugInDescriptor, GeneratedProject> _plugInProjects = [];

    /// <summary>
    /// Creates a project that builds the plug-in with the settings specified in <paramref name="plugInDescriptor"/>
    /// (or locates a project created earlier with the same settings), and returns the path to the plug-in DLL.
    /// </summary>
    /// <param name="plugInDescriptor">
    /// Describes the build choices for the plug-in, including the target framework and Rx version.
    /// </param>
    /// <returns>
    /// The path to the plug-in DLL that was built, or that already exists from a previous build.
    /// </returns>
    /// <remarks>
    /// The file that this returns the path of will exist until the <see cref="PlugInBuilder"/> is disposed.
    /// </remarks>
    public async Task<string> GetPlugInDllPathAsync(PlugInDescriptor plugInDescriptor)
    {
        if (!_plugInProjects.TryGetValue(plugInDescriptor, out var project))
        {
            project = await CreateProjectForPlugIn(plugInDescriptor);
            _plugInProjects.Add(plugInDescriptor, project);
        }

        return Path.Combine(
            project.Project.ClonedProjectFolderPath,
            "bin",
            "Release",
            plugInDescriptor.TargetFrameworkMoniker,
            $"{project.AssemblyName}.dll");
    }

    private record GeneratedProject(ModifiedProjectClone Project, string AssemblyName);

    private static async Task<GeneratedProject> CreateProjectForPlugIn(PlugInDescriptor plugInDescriptor)
    {
        // Give each distinct framework/rx version a different assembly name, because the
        // .NET Fx plug-in host will only ever load the first assembly with any particular name.
        var simplifiedRxVersion = plugInDescriptor.RxPackages[0].Version.Replace(".", "")[..2];
        var assemblyName = $"PlugIn.{plugInDescriptor.TargetFrameworkMoniker}.Rx{simplifiedRxVersion}";
        var projectClone = ModifiedProjectClone.Create(
            PlugInTemplateProjectFolder,
            PlugInTempFolderName,
            (project) =>
            {
                project.SetTargetFramework(plugInDescriptor.TargetFrameworkMoniker);
                project.AddAssemblyNameProperty(assemblyName);
                project.ReplacePackageReference("System.Reactive", plugInDescriptor.RxPackages);
                project.FixUpProjectReferences(PlugInTemplateProjectFolder);
            },
            plugInDescriptor.PackageSource is string packageSource ? [("loc", packageSource)] : null);

        await projectClone.RunDotnetBuild("PlugIn.csproj");
        return new GeneratedProject(projectClone, assemblyName);
    }

    public void Dispose()
    {
        foreach (var projectClone in _plugInProjects.Values)
        {
            projectClone.Project.Dispose();
        }
    }
}
